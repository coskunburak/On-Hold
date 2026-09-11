using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using OnHold.Core;
using OnHold.Domain;

namespace OnHold.Gameplay
{
    [Serializable] public sealed class BodyFrame { public int Id; public Vector3 Position; public Quaternion Rotation; public int Lifecycle, Handling, Integrity, Holders, Anchor = -1; public uint Revision; }
    [Serializable] public sealed class WorldFrame
    {
        public ulong Epoch, Tick; public double Time, Remaining; public int Phase, Delivered, Driver, Connections;
        public string RunId; public long Provisional, CampaignRevision, Balance; public bool Saved, MandatoryLost;
        public Vector3 PlatformPosition; public Quaternion PlatformRotation; public float PlatformSpeed, CentreX, CentreZ; public double SupportMass;
        public BodyFrame[] Items, Players;
    }
    public sealed class FoundationWorld : MonoBehaviour
    {
        public TextAsset Configuration;
        public string ContentHash;
        public CarryableBody[] Items;
        public ControlledPlatform Platform;
        public BoxCollider DeliveryZone;
        public Transform[] RecoveryPoints, PlayerSpawns;
        public GameObject PlayerPrefab;
        public float KillY = -5;
        public FoundationConfig Config { get; private set; }
        public bool IsAuthority { get; private set; }
        public SessionCoordinator Session { get; private set; }
        public ContractRun Run { get; private set; }
        public Dictionary<int, PlayerBody> Players { get; } = new Dictionary<int, PlayerBody>();
        public double Now { get; private set; }
        public ulong Tick { get; private set; }
        public double PhysicsMilliseconds { get; private set; }
        public long Balance, CampaignRevision;
        public bool Saved;
        public event Action<RunResult> SettlementRequested;
        public event Action<WorldFrame> SnapshotReady;
        public event Action<int> Pinged;
        readonly Queue<Action> commands = new Queue<Action>(128);
        readonly Dictionary<(int, UnityEngine.Object), (CarryableBody item, float speed)> impacts = new Dictionary<(int, UnityEngine.Object), (CarryableBody, float)>();
        readonly Dictionary<(int, UnityEngine.Object), double> impactCooldown = new Dictionary<(int, UnityEngine.Object), double>();
        readonly Stopwatch physicsTimer = new Stopwatch();
        readonly List<(int, UnityEngine.Object)> expiredImpacts = new List<(int, UnityEngine.Object)>(128);
        SimulationMode previousSimulation;
        bool initialized;
        ulong lastSnapshotTick;
        public int QueuedCommands => commands.Count;
        public int ActiveHandles => Items.Sum(i => i ? i.HandleCount : 0);
        public void Initialize()
        {
            if (initialized) return; initialized = true;
            Config = new FoundationConfig(); JsonUtility.FromJsonOverwrite(Configuration.text, Config); Config.Validate();
            Time.fixedDeltaTime = 1f / Config.PhysicsHz; Time.maximumDeltaTime = .1f;
            Platform.Initialize(this); foreach (var item in Items) item.Initialize(this);
        }
        public void StartAuthority(ulong epoch, OnHold.Domain.SessionMode mode)
        {
            Initialize(); IsAuthority = true; previousSimulation = Physics.simulationMode; Physics.simulationMode = SimulationMode.Script;
            Session = new SessionCoordinator(epoch, mode); Now = 0; Tick = lastSnapshotTick = 0;
            foreach (var item in Items) if (item && item.Body) item.ResetState(); Platform.ResetState();
        }
        public void StartGuest() { Initialize(); IsAuthority = false; foreach (var item in Items) if (item && item.Body) item.Body.isKinematic = true; }
        public void StopSession()
        {
            commands.Clear(); impacts.Clear(); impactCooldown.Clear();
            foreach (var item in Items) if (item) { item.State?.Dispose(); if (item.Body) item.Body.isKinematic = true; }
            Platform.Lease.Stop();
            foreach (var player in Players.Values) if (player) Destroy(player.gameObject); Players.Clear();
            if (IsAuthority) Physics.simulationMode = previousSimulation;
            IsAuthority = false; Session = null; Run = null; Saved = false; remoteFrames.Clear(); LatestFrame = null;
        }
        public bool Enqueue(Action action) { if (commands.Count >= 128) return false; commands.Enqueue(action); return true; }
        public Vector3 SafePlayerPoint(int id)
        {
            Physics.SyncTransforms();
            for (int i = 0; i < PlayerSpawns.Length; i++)
            {
                var point = PlayerSpawns[(Mathf.Max(1, id) - 1 + i) % PlayerSpawns.Length].position;
                if (!Physics.CheckCapsule(point + Vector3.up * Config.PlayerRadius,
                    point + Vector3.up * (Config.PlayerHeight - Config.PlayerRadius), Config.PlayerRadius - Config.SkinWidth,
                    (1 << 8) | (1 << 10) | (1 << 11), QueryTriggerInteraction.Ignore) &&
                    Physics.Raycast(point + Vector3.up * .1f, Vector3.down, .5f, (1 << 8) | (1 << 11), QueryTriggerInteraction.Ignore)) return point;
            }
            throw new InvalidOperationException("No unobstructed grounded player spawn available");
        }
        public PlayerBody AddPlayer(Actor actor)
        {
            if (Players.TryGetValue(actor.Id, out var old))
            {
                if (old.Actor.Equals(actor)) return old;
                CleanupActor(old.Actor); old.gameObject.SetActive(false); Destroy(old.gameObject); Players.Remove(actor.Id);
            }
            var go = Instantiate(PlayerPrefab, SafePlayerPoint(actor.Id), Quaternion.identity, transform);
            var player = go.GetComponent<PlayerBody>(); player.Initialize(actor, IsAuthority, Config); Players.Add(actor.Id, player); return player;
        }
        public void CleanupActor(Actor actor)
        {
            foreach (var item in Items) if (item) item.State.Release(actor); Platform.Lease.Release(actor);
        }
        public void RemovePlayer(Actor actor)
        {
            CleanupActor(actor); Platform.Support.Remove("player." + actor.Id);
            if (Players.TryGetValue(actor.Id, out var p) && p.Actor.Equals(actor)) { p.gameObject.SetActive(false); Destroy(p.gameObject); Players.Remove(actor.Id); }
        }
        public bool Reach(Actor actor, Vector3 point, CarryableBody target = null)
        {
            if (!Players.TryGetValue(actor.Id, out var player) || !player.Actor.Equals(actor)) return false;
            Vector3 delta = point - player.Eye;
            if (delta.magnitude > Config.Reach) return false;
            if (Physics.Raycast(player.Eye, delta.normalized, out var hit, delta.magnitude, Config.InteractionMask, (QueryTriggerInteraction)Config.InteractionTriggers))
                return target != null && hit.rigidbody == target.Body;
            return true;
        }
        // Read-only gameplay eligibility, also used by the transaction. Preview grants no authority.
        public Reason CanGrab(Actor actor, CarryableBody item) => EvaluateGrab(actor, item, out _);
        public GrabCandidate QueryGrabCandidate(Actor actor, CarryableBody item)
        {
            var reason = EvaluateGrab(actor, item, out int index);
            if (reason != Reason.Accepted) return new GrabCandidate(reason);
            var pose = item.GripWorldPose(index);
            return new GrabCandidate(item.NetworkId, index, pose, Vector3.Distance(Players[actor.Id].Eye, pose.position));
        }
        Reason EvaluateGrab(Actor actor, CarryableBody item, out int index)
        {
            index = -1;
            if (!IsAuthority || Session == null || !Players.TryGetValue(actor.Id, out var player) || !player || !player.Actor.Equals(actor)) return Reason.AuthFailed;
            if (Session.Phase != SessionPhase.Active || Run == null || Run.Result != null || Now >= Run.Deadline) return Reason.InvalidState;
            if (!item || !item.isActiveAndEnabled || !item.Body || item.World != this || item.State == null) return Reason.InvalidContent;
            if (item.RecoveryPending) return Reason.InvalidState;
            if (!Reach(actor, item.Body.worldCenterOfMass, item)) return Reason.TooFar;
            foreach (var other in Items)
                if (other && other != item && other.State.IsHeldBy(actor)) return Reason.Busy;
            var reason = item.State.CanGrab(actor);
            if (reason != Reason.Accepted) return reason;
            index = item.SelectGrip(actor);
            return index >= 0 ? Reason.Accepted : Reason.AttachmentFailed;
        }
        public Reason Apply(CommandEnvelope command, Actor actor)
        {
            if (!IsAuthority || Session == null || !Players.TryGetValue(actor.Id, out var player) || !player.Actor.Equals(actor)) return Reason.AuthFailed;
            var type = command.CommandType;
            if (type == CommandType.StopInput) { player.ClearInput(); return Reason.Accepted; }
            if (type == CommandType.Ready) return Session.Ready(actor, true);
            if (type == CommandType.Start)
            {
                if (actor.Id != Session.HostId) return Reason.AuthFailed;
                if (Session.Phase == SessionPhase.Lobby || Session.Phase == SessionPhase.Hub) return Session.Transition(SessionPhase.Loading, actor.Id);
                if (Session.Phase == SessionPhase.Loading) return Session.Transition(SessionPhase.Briefing, actor.Id);
                if (Session.Phase == SessionPhase.Briefing)
                {
                    var r = Session.Transition(SessionPhase.Active, actor.Id);
                    if (r == Reason.Accepted)
                    {
                        Run = new ContractRun(Guid.NewGuid().ToString("N"), Items.Select(i => i.State), new[] { Items[0].InstanceId }, 2, 100, Now, 600, Config); Saved = false;
                    }
                    return r;
                }
                return Reason.InvalidState;
            }
            if (type == CommandType.Hub) return Session.Transition(SessionPhase.Hub, actor.Id);
            if (type == CommandType.NewRun)
            {
                if (actor.Id != Session.HostId) return Reason.AuthFailed;
                if (Session.Phase != SessionPhase.Hub) return Reason.InvalidState;
                foreach (var i in Items) if (i && i.Body) i.ResetState(); Platform.ResetState(); Run = null;
                return Session.Transition(SessionPhase.Loading, actor.Id);
            }
            if (type == CommandType.Release) return HandleSafeRelease(actor);
            if (type == CommandType.ReleaseLease) { Platform.Lease.Release(actor); return Reason.Accepted; }
            if (Session.Phase != SessionPhase.Active || Run == null || Run.Result != null || Now >= Run.Deadline) return Reason.InvalidState;
            if (type == CommandType.Move) { player.Input(command, Now); return Reason.Accepted; }
            if (type == CommandType.Jump) return player.RequestJump() ? Reason.Accepted : Reason.InvalidState;
            if (type == CommandType.Lease) return Platform.Lease.Acquire(actor, Reach(actor, Platform.Panel.position), Now);
            if (type == CommandType.Winch) return Platform.Lease.Control(actor, command.Y, Reach(actor, Platform.Panel.position), Now);
            if (type == CommandType.VoteFinish) return Run.Vote(actor.Id, Session.HostId, Session.Members.Where(m => m.Connected).Select(m => m.Actor.Id), Now);
            if (type == CommandType.Abort) { if (actor.Id != Session.HostId) return Reason.AuthFailed; Run.Finish(FinishReason.Abort); return Reason.Accepted; }
            if (type == CommandType.Ping) { Pinged?.Invoke(actor.Id); return Reason.Accepted; }
            var item = Items.FirstOrDefault(i => i && i.NetworkId == command.TargetId);
            if (!item) return Reason.InvalidContent;
            if (item.RecoveryPending) return Reason.InvalidState;
            bool reachable = Reach(actor, item.Body.worldCenterOfMass, item);
            if (type == CommandType.Grab)
            {
                var eligibility = CanGrab(actor, item);
                return eligibility == Reason.Accepted ? item.State.Grab(actor, () => item.CreateGrip(actor)) : eligibility;
            }
            if (type == CommandType.Secure)
            {
                var slot = Platform.Slots.FirstOrDefault(s => s.ItemId == null);
                return item.State.Secure(slot, command.ExpectedRevision, Platform.ValidPlacement(item), reachable, Platform.Speed, Config, () => Platform.Attach(item));
            }
            if (type == CommandType.Unsecure) return item.State.Unsecure(command.ExpectedRevision, reachable, !OverlapsOther(item, item.Body.position, item.Body.rotation), Platform.Speed, Config);
            return Reason.UnsupportedAction;
        }

        Reason HandleSafeRelease(Actor actor)
        {
            CarryableBody held = null;
            foreach (var i in Items)
                if (i && i.State != null && i.State.IsHeldBy(actor)) { held = i; break; }
            if (held == null) return Reason.Accepted; // Idempotent release, including destroyed cargo.
            if (!held.Body || held.Body.isKinematic || held.State.Lifecycle != Lifecycle.Available)
            return Reason.InvalidState;
            var result = ReleaseSafety.Evaluate(held, Config, out var safePos);
            if (result == ReleaseResult.Released)
            {
                if (safePos != held.Body.position) { held.Body.position = safePos; Physics.SyncTransforms(); }
                var linear = held.Body.linearVelocity; var angular = held.Body.angularVelocity;
                ReleaseSafety.SanitizeVelocity(ref linear, ref angular, Config);
                held.Body.linearVelocity = linear; held.Body.angularVelocity = angular;
                held.State.Release(actor); return Reason.Accepted;
            }
            return result == ReleaseResult.InvalidState ? Reason.InvalidState : Reason.Blocked;
        }
        void FixedUpdate()
        {
            if (!IsAuthority || Session == null) return;
            Step(1f / Config.PhysicsHz);
        }
        public void Step(float dt)
        {
            Tick++; Now += dt;
            // Tick-start timeout wins before commands and before this tick's physics callbacks.
            bool active = Run != null && Session.Phase == SessionPhase.Active && Run.BeginTick(Now);
            int count = commands.Count; for (int i = 0; i < count; i++) commands.Dequeue()();
            active = Session.Phase == SessionPhase.Active && Run != null && Run.Result == null && Now < Run.Deadline;
            if (active)
            {
                physicsTimer.Restart();
                Platform.Drive(dt); foreach (var p in Players.Values) if (p && p.isActiveAndEnabled) p.Drive(this, dt);
                foreach (var item in Items) if (item) item.Drive(dt);
                Physics.SyncTransforms(); Physics.Simulate(dt);
                foreach (var item in Items) if (item) item.BoundPostSimulationVelocity();
                Platform.DriveAttachments();
                physicsTimer.Stop(); PhysicsMilliseconds = physicsTimer.Elapsed.TotalMilliseconds;
                foreach (var impact in impacts)
                {
                    if (impactCooldown.TryGetValue(impact.Key, out var until) && Now < until) continue;
                    var hit = impact.Value; if (!hit.item || !hit.item.isActiveAndEnabled) continue;
                    hit.item.State.Damage(Mathf.Clamp((hit.speed - Config.DamageThreshold) * Config.DamageScale, 0, Config.DamageMax));
                    impactCooldown[impact.Key] = Now + Config.DamageCooldown;
                }
                impacts.Clear();
                expiredImpacts.Clear();
                foreach (var pair in impactCooldown) if (pair.Value < Now - 1) expiredImpacts.Add(pair.Key);
                foreach (var key in expiredImpacts) impactCooldown.Remove(key);
                foreach (var item in Items)
                {
                    if (!item || !item.Body || !item.isActiveAndEnabled) continue;
                    if (!SafeMath.Finite(item.Body.position) || !SafeMath.Finite(item.Body.rotation)) { item.State.MarkLost(); UnityEngine.Debug.LogError("Non-finite body quarantined: " + item.InstanceId); }
                    HandleRecovery(item); item.TerminalPresentation();
                    Run.EvaluateDelivery(item.State, InsideZone(item), item.Body.linearVelocity.magnitude, item.Body.angularVelocity.magnitude * Mathf.Rad2Deg, Now);
                    item.TerminalPresentation(); item.State.AssertInvariant();
                }
                if (Now >= Run.Deadline) Run.Finish(FinishReason.Timeout);
            }
            if (Run?.Result != null && Session.Phase == SessionPhase.Active)
            {
                Session.Transition(SessionPhase.Settling, Session.HostId); Platform.Lease.Stop(); SettlementRequested?.Invoke(Run.Result);
            }
            if ((Tick * (ulong)Config.SnapshotHz) / (ulong)Config.PhysicsHz != (lastSnapshotTick * (ulong)Config.SnapshotHz) / (ulong)Config.PhysicsHz)
                SnapshotReady?.Invoke(Capture());
            lastSnapshotTick = Tick;
        }
        public void CompleteSettlement(long balance, long revision) { Balance = balance; CampaignRevision = revision; Saved = true; Session.Transition(SessionPhase.Results, Session.HostId); }
        public void QueueImpact(CarryableBody item, UnityEngine.Object other, float speed)
        {
            if (!Numbers.Finite(speed) || impacts.Count >= 128) return;
            var key = (item.NetworkId, other);
            if (!impacts.TryGetValue(key, out var old) || old.speed < speed) impacts[key] = (item, speed);
        }
        void HandleRecovery(CarryableBody item)
        {
            if (item.State.Lifecycle != Lifecycle.Available) return;
            if (!item.RecoveryPending && item.Body.position.y < KillY)
            {
                item.RecoveryPending = true; item.RecoveryUntil = Now + Config.RecoveryWait;
                var result = item.State.Recover(++item.RecoveryEvent, Config, out var penalty); Run.RecoveryPenalty(penalty);
                item.Body.isKinematic = true;
                if (result != Reason.Accepted) return;
            }
            if (!item.RecoveryPending) return;
            foreach (var point in RecoveryPoints)
            {
                if (OverlapsOther(item, point.position, point.rotation)) continue;
                item.Body.position = point.position; item.Body.rotation = point.rotation; item.Body.isKinematic = false;
                item.Body.linearVelocity = item.Body.angularVelocity = Vector3.zero; item.SuppressDamageUntil = Now + Config.DamageCooldown;
                item.RecoveryPending = false; return;
            }
            if (Now >= item.RecoveryUntil) { item.State.MarkLost(); item.RecoveryPending = false; }
        }
        bool OverlapsOther(CarryableBody item, Vector3 position, Quaternion rotation)
        {
            var colliders = Physics.OverlapBox(position + rotation * item.Definition.BoundsCenter, item.Definition.BoundsSize * .5f * .98f, rotation, (1 << 8) | (1 << 9) | (1 << 10), QueryTriggerInteraction.Ignore);
            return colliders.Any(c => c.attachedRigidbody != item.Body);
        }
        public bool InsideZone(CarryableBody item)
        {
            if (item.RecoveryPending) return false;
            var half = DeliveryZone.size * .5f + Vector3.one * Config.DeliveryTolerance;
            for (int i = 0; i < 8; i++)
            {
                var local = DeliveryZone.transform.InverseTransformPoint(item.transform.TransformPoint(item.Definition.Corner(i))) - DeliveryZone.center;
                if (Mathf.Abs(local.x) > half.x || Mathf.Abs(local.y) > half.y || Mathf.Abs(local.z) > half.z) return false;
            }
            return true;
        }
        public WorldFrame Capture() => new WorldFrame { Epoch = Session.Epoch, Tick = Tick, Time = Now, Remaining = Run == null ? 600 : Math.Max(0, Run.Deadline - Now), Phase = (int)Session.Phase,
            RunId = Run?.Id ?? "", Delivered = Run?.DeliveredCount ?? 0, Provisional = Run?.ProvisionalValue ?? 0, CampaignRevision = CampaignRevision, Balance = Balance, Saved = Saved,
            MandatoryLost = Run?.MandatoryLost ?? false, Connections = Players.Count, Driver = Platform.Lease.Driver?.Id ?? 0,
            PlatformPosition = Platform.Body.position, PlatformRotation = Platform.Body.rotation, PlatformSpeed = Platform.Speed,
            CentreX = (float)Platform.Support.CentreX, CentreZ = (float)Platform.Support.CentreZ, SupportMass = Platform.Support.Mass,
            Items = Items.Select(CaptureItem).ToArray(),
            Players = Players.Values.Select(p => p ? new BodyFrame { Id = p.Actor.Id, Position = p.transform.position, Rotation = p.transform.rotation } :
                new BodyFrame { Id = p?.Actor.Id ?? 0, Rotation = Quaternion.identity }).ToArray() };
        BodyFrame CaptureItem(CarryableBody item)
        {
            // Keep the authored snapshot slot and identity when a Unity object has been destroyed.
            if (!item || !item.Body) return new BodyFrame { Id = item?.NetworkId ?? 0, Rotation = Quaternion.identity, Lifecycle = (int)Lifecycle.Lost };
            return new BodyFrame { Id = item.NetworkId, Position = item.Body.position, Rotation = item.Body.rotation,
                Lifecycle = (int)item.State.Lifecycle, Handling = (int)item.State.Handling, Integrity = (int)item.State.Integrity,
                Holders = item.State.Holders.Count, Anchor = item.State.Anchor == null ? -1 : Array.IndexOf(Platform.Slots, item.State.Anchor), Revision = item.State.Revision };
        }
        readonly List<WorldFrame> remoteFrames = new List<WorldFrame>(32);
        double receivedAt;
        public WorldFrame LatestFrame { get; private set; }
        public double RenderSampleTime { get; private set; }
        public float TimeAlignedRelativeError { get; private set; }
        public float LastCorrection { get; private set; }
        public void Receive(WorldFrame frame)
        {
            if (IsAuthority || frame == null || frame.Items == null || frame.Items.Length != Items.Length || frame.Players == null || frame.Players.Length > 4 ||
                !Numbers.Finite(frame.Time) || !SafeMath.Finite(frame.PlatformPosition) || !SafeMath.Finite(frame.PlatformRotation) ||
                frame.Items.Concat(frame.Players).Any(b => b == null || !SafeMath.Finite(b.Position) || !SafeMath.Finite(b.Rotation)) ||
                (LatestFrame != null && frame.Tick <= LatestFrame.Tick)) return;
            LatestFrame = frame; receivedAt = Time.realtimeSinceStartupAsDouble;
            remoteFrames.Add(frame); if (remoteFrames.Count > 32) remoteFrames.RemoveAt(0);
            foreach (var p in frame.Players) if (!Players.ContainsKey(p.Id)) AddPlayer(new Actor(p.Id, 1));
            foreach (var id in Players.Keys.Where(id => !frame.Players.Any(p => p.Id == id)).ToArray()) { Destroy(Players[id].gameObject); Players.Remove(id); }
        }
        void LateUpdate()
        {
            if (IsAuthority || remoteFrames.Count == 0) return;
            double target = LatestFrame.Time + Math.Min(.05, Time.realtimeSinceStartupAsDouble - receivedAt) - .1;
            var a = remoteFrames[0]; var b = a;
            foreach (var frame in remoteFrames) { b = frame; if (frame.Time >= target) break; a = frame; }
            float t = b.Time > a.Time ? Mathf.Clamp01((float)((target - a.Time) / (b.Time - a.Time))) : 1;
            RenderSampleTime = a.Time + (b.Time - a.Time) * t;
            Platform.transform.SetPositionAndRotation(Vector3.Lerp(a.PlatformPosition, b.PlatformPosition, t), Quaternion.Slerp(a.PlatformRotation, b.PlatformRotation, t));
            for (int i = 0; i < Items.Length; i++)
            {
                var x = a.Items.First(v => v.Id == Items[i].NetworkId); var y = b.Items.First(v => v.Id == Items[i].NetworkId);
                var desired = Vector3.Lerp(x.Position, y.Position, t); LastCorrection = Vector3.Distance(Items[i].transform.position, desired);
                Items[i].transform.SetPositionAndRotation(desired, Quaternion.Slerp(x.Rotation, y.Rotation, t));
                // This measures application error at the SAME sampled time, not latency/feel.
                var expectedLocal = Quaternion.Inverse(Platform.transform.rotation) * (desired - Platform.transform.position);
                TimeAlignedRelativeError = Vector3.Distance(expectedLocal, Platform.transform.InverseTransformPoint(Items[i].transform.position));
                foreach (var r in Items[i].GetComponentsInChildren<Renderer>()) r.enabled = y.Lifecycle == 0;
                foreach (var c in Items[i].GetComponentsInChildren<Collider>()) c.enabled = y.Lifecycle == 0;
            }
            foreach (var p in b.Players)
            {
                var x = a.Players.FirstOrDefault(v => v.Id == p.Id) ?? p;
                Players[p.Id].transform.SetPositionAndRotation(Vector3.Lerp(x.Position, p.Position, t), Quaternion.Slerp(x.Rotation, p.Rotation, t));
            }
        }
        void OnDestroy() { if (initialized) StopSession(); }
    }
}
