using System.Collections.Generic;
using UnityEngine;
using OnHold.Core;
using OnHold.Domain;

namespace OnHold.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class CarryableBody : MonoBehaviour
    {
        public int NetworkId;
        public string InstanceId;
        public ItemDefinition Definition;
        public ItemState State { get; private set; }
        public Rigidbody Body { get; private set; }
        public FoundationWorld World { get; private set; }
        readonly Dictionary<Actor, GripHandle> grips = new Dictionary<Actor, GripHandle>();
        public int HandleCount => grips.Count;
        public Vector3 LastForce { get; private set; }
        public Vector3 LastTorque { get; private set; }
        public Vector3 LastManipulationTorque { get; private set; }
        readonly List<Actor> releases = new List<Actor>(2);
        public bool RecoveryPending;
        public double RecoveryUntil, SuppressDamageUntil;
        public ulong RecoveryEvent;
        Vector3 initialPosition;
        Quaternion initialRotation;
        public void Initialize(FoundationWorld world)
        {
            World = world; Body = GetComponent<Rigidbody>(); initialPosition = transform.position; initialRotation = transform.rotation;
            ResetState();
        }
        public void ResetState()
        {
            State?.Dispose(); LastForce = LastTorque = LastManipulationTorque = Vector3.zero; State = new ItemState(InstanceId, Definition.DefinitionId, Definition.MassKg, Definition.BaseValue);
            Body.mass = Definition.MassKg; Body.isKinematic = !World.IsAuthority; Body.position = initialPosition; Body.rotation = initialRotation;
            if (!Body.isKinematic) { Body.linearVelocity = Vector3.zero; Body.angularVelocity = Vector3.zero; }
            Body.maxLinearVelocity = World.Config.BodySpeedMax; Body.maxAngularVelocity = World.Config.AngularSpeedMax;
            Body.interpolation = RigidbodyInterpolation.None; Body.collisionDetectionMode = World.IsAuthority ? CollisionDetectionMode.ContinuousDynamic : CollisionDetectionMode.Discrete;
            RecoveryPending = false; RecoveryEvent = 0; SuppressDamageUntil = 0;
            foreach (var collider in GetComponentsInChildren<Collider>()) collider.enabled = true;
            foreach (var renderer in GetComponentsInChildren<Renderer>()) renderer.enabled = true;
        }
        // Existing rule: nearest unoccupied authored grip to the actor eye; lower index breaks exact ties.
        // Camera rotation never enters this score, so aiming jitter cannot switch candidates.
        internal int SelectGrip(Actor actor)
        {
            if (!World.IsAuthority || Body.isKinematic || !World.Players.TryGetValue(actor.Id, out var player) || !player.Actor.Equals(actor)) return -1;
            int choice = -1; float best = float.MaxValue;
            for (int i = 0; i < Definition.Grips.Length; i++)
            {
                bool used = false; foreach (var g in grips.Values) if (g.Index == i) used = true;
                float d = Vector3.Distance(Body.position + Body.rotation * Definition.Grips[i], player.Eye);
                if (!used && d < best) { choice = i; best = d; }
            }
            return best <= World.Config.Reach ? choice : -1;
        }
        public Pose GripWorldPose(int index) => new Pose(Body.position + Body.rotation * Definition.Grips[index], Body.rotation);
        public int HeldGripIndex(Actor actor) => grips.TryGetValue(actor, out var grip) ? grip.Index : -1;
        public bool CanManipulate(Actor actor)
        {
            return this && isActiveAndEnabled && World && World.IsAuthority && World.Session != null &&
                World.Session.Phase == SessionPhase.Active && World.Run != null && World.Run.Result == null && World.Now < World.Run.Deadline &&
                Body && !Body.isKinematic && !RecoveryPending && State != null && State.Lifecycle == Lifecycle.Available &&
                State.IsHeldBy(actor) && grips.TryGetValue(actor, out var grip) && ValidGrip(grip.Index) &&
                World.Players.TryGetValue(actor.Id, out var player) && player && player.isActiveAndEnabled && player.Actor.Equals(actor) &&
                (!World.Platform.Lease.Driver.HasValue || !World.Platform.Lease.Driver.Value.Equals(actor));
        }
        bool ValidGrip(int index) => Definition && Definition.Grips != null && index >= 0 && index < Definition.Grips.Length && SafeMath.Finite(Definition.Grips[index]);
        public bool TryHeldOrientation(Actor actor, out HeldOrientation orientation)
        {
            orientation = default;
            if (!CanManipulate(actor)) return false;
            orientation = grips[actor].Orientation; return true;
        }
        public bool UpdateManipulation(Actor actor, bool active, Vector2 degrees, Quaternion view, float dt)
        {
            if (!CanManipulate(actor)) { EndManipulation(actor); return false; }
            return grips[actor].Orientation.Update(active, degrees, view, Body.rotation, dt, World.Config);
        }
        public void EndManipulation(Actor actor)
        {
            if (grips.TryGetValue(actor, out var grip)) grip.Orientation.EndInput();
        }
        public IAttachment CreateGrip(Actor actor)
        {
            int choice = SelectGrip(actor);
            if (choice < 0) return null;
            var handle = new GripHandle(this, actor, choice); grips.Add(actor, handle); return handle;
        }
        sealed class GripHandle : IAttachment
        {
            readonly CarryableBody owner;
            public readonly Actor Actor;
            public readonly int Index;
            public HeldOrientation Orientation;
            bool disposed;
            public GripHandle(CarryableBody owner, Actor actor, int index) { this.owner = owner; Actor = actor; Index = index; Orientation.Initialize(owner.Body.rotation); }
            public void Dispose() { if (disposed) return; disposed = true; Orientation.Reset(); owner.grips.Remove(Actor);
                if (owner.grips.Count == 0) owner.LastForce = owner.LastTorque = owner.LastManipulationTorque = Vector3.zero; }
        }
        public static bool TryGripForce(FoundationConfig config, Vector3 error, Vector3 pointVelocity,
            Vector3 playerVelocity, float massShare, out Vector3 force)
        {
            force = Vector3.zero;
            if (!SafeMath.Finite(error) || !SafeMath.Finite(pointVelocity) || !SafeMath.Finite(playerVelocity) ||
                !Numbers.Finite(massShare) || massShare <= 0) return false;
            force = Vector3.ClampMagnitude(error, config.CarryErrorMax) * config.CarrySpring
                - (pointVelocity - playerVelocity) * config.CarryDamping - Physics.gravity * massShare;
            if (!SafeMath.Finite(force)) { force = Vector3.zero; return false; }
            force = Vector3.ClampMagnitude(force, config.HandForceMax); return true;
        }
        public void Drive() => Drive(1f / World.Config.PhysicsHz);
        public void Drive(float dt)
        {
            LastForce = LastTorque = LastManipulationTorque = Vector3.zero;
            if (!isActiveAndEnabled || !Body) { State?.ClearAttachments(); return; }
            if (!World.IsAuthority) return;
            if (State.Lifecycle != Lifecycle.Available || Body.isKinematic || RecoveryPending) { State.ClearAttachments(); return; }
            if (!SafeMath.Finite(Body.position) || !SafeMath.Finite(Body.rotation) ||
                !SafeMath.Finite(Body.linearVelocity) || !SafeMath.Finite(Body.angularVelocity))
            {
                State.MarkLost(); Body.isKinematic = true;
                Debug.LogError("Invalid carry body quarantined: " + InstanceId); return;
            }
            Vector3 force = Vector3.zero, torque = Vector3.zero, orientationTorque = Vector3.zero; int oriented = 0; releases.Clear();
            foreach (var handle in grips.Values)
            {
                if (!World.Players.TryGetValue(handle.Actor.Id, out var p) || !p || !p.isActiveAndEnabled || !p.Actor.Equals(handle.Actor) || !ValidGrip(handle.Index))
                { releases.Add(handle.Actor); continue; }
                var point = Body.position + Body.rotation * Definition.Grips[handle.Index];
                var target = p.Eye + p.Forward * World.Config.CarryDistance;
                if (!SafeMath.Finite(target) || Vector3.Distance(point, p.Eye) > World.Config.CarryBreakDistance)
                { releases.Add(handle.Actor); continue; }
                if (Physics.Raycast(p.Eye, p.Forward, out var hit, World.Config.CarryDistance, (1 << 8) | (1 << 11), QueryTriggerInteraction.Ignore))
                    target = hit.point - p.Forward * .2f;
                if (!TryGripForce(World.Config, target - point, Body.GetPointVelocity(point), p.Velocity,
                    Body.mass / Mathf.Max(1, grips.Count), out var f)) { releases.Add(handle.Actor); continue; }
                force += f; torque += Vector3.Cross(point - Body.worldCenterOfMass, f);
                if (handle.Orientation.Engaged && CanManipulate(handle.Actor))
                {
                    orientationTorque += HeldOrientation.Correction(Body.rotation, handle.Orientation.Target,
                        Body.angularVelocity, Body.inertiaTensor, Body.inertiaTensorRotation, dt, World.Config); oriented++;
                }
            }
            foreach (var actor in releases) State.Release(actor);
            force = Vector3.ClampMagnitude(force, World.Config.TotalForceMax);
            LastManipulationTorque = oriented > 0 ? orientationTorque / oriented : Vector3.zero;
            torque = Vector3.ClampMagnitude(torque + (oriented > 0 ? LastManipulationTorque :
                -Body.angularVelocity * World.Config.CarryAngularDamping), World.Config.TotalTorqueMax);
            if (grips.Count > 0 && SafeMath.Finite(force) && SafeMath.Finite(torque))
            { LastForce = force; LastTorque = torque; Body.AddForce(force); Body.AddTorque(torque); }
        }
        public void BoundPostSimulationVelocity()
        {
            if (!Body || Body.isKinematic) return;
            // PhysX max velocities apply before simulation; contacts may exceed them during a step.
            if (SafeMath.Finite(Body.linearVelocity)) Body.linearVelocity = Vector3.ClampMagnitude(Body.linearVelocity, World.Config.BodySpeedMax);
            if (SafeMath.Finite(Body.angularVelocity)) Body.angularVelocity = Vector3.ClampMagnitude(Body.angularVelocity, World.Config.AngularSpeedMax);
        }
        void OnCollisionEnter(Collision collision)
        {
            if (World == null || !World.IsAuthority || World.Now < SuppressDamageUntil || State.Lifecycle != Lifecycle.Available) return;
            UnityEngine.Object other = collision.rigidbody ? (UnityEngine.Object)collision.rigidbody : collision.collider.transform.root;
            World.QueueImpact(this, other, collision.relativeVelocity.magnitude);
        }
        public void TerminalPresentation()
        {
            if (State.Lifecycle == Lifecycle.Available) return;
            Body.isKinematic = true;
            foreach (var c in GetComponentsInChildren<Collider>()) c.enabled = false;
            foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        }
        void OnDisable() => State?.ClearAttachments();
        void OnDestroy() { State?.MarkLost(); State?.Dispose(); if (World && World.Platform) World.Platform.Support.Remove(InstanceId); }
    }
}
