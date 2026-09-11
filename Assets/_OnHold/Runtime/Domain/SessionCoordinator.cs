using System;
using System.Collections.Generic;
using System.Linq;
using OnHold.Core;

namespace OnHold.Domain
{
    public enum SessionPhase { Lobby, Loading, Briefing, Active, Settling, Results, Hub }
    public enum SessionMode { Solo, Coop }
    public sealed class SessionMember
    {
        public Actor Actor { get; internal set; }
        public string Ticket { get; internal set; }
        public bool Connected { get; internal set; }
        public bool Ready { get; internal set; }
        public bool Loaded { get; internal set; }
        public double ReservedUntil { get; internal set; }
    }
    public sealed class SessionCoordinator
    {
        readonly List<SessionMember> members = new List<SessionMember>(4);
        readonly HashSet<int> startRoster = new HashSet<int>();
        int nextActor;
        uint generation;
        public SessionPhase Phase { get; private set; } = SessionPhase.Lobby;
        public SessionMode Mode { get; }
        public ulong Epoch { get; }
        public uint Revision { get; private set; }
        public int HostId { get; private set; }
        public IReadOnlyList<SessionMember> Members => members;
        public SessionCoordinator(ulong epoch, SessionMode mode) { if (epoch == 0) throw new ArgumentException("epoch"); Epoch = epoch; Mode = mode; }
        public Reason Join(string ticket, bool host, double now, out SessionMember member)
        {
            member = null;
            if (!string.IsNullOrEmpty(ticket))
            {
                member = members.FirstOrDefault(m => m.Ticket == ticket);
                if (member == null) return Reason.AuthFailed;
                if (member.Connected) return Reason.Busy;
                if (now >= member.ReservedUntil) { member = null; return Reason.ReservationExpired; }
                member.Actor = new Actor(member.Actor.Id, ++generation); member.Connected = true; member.Ready = false; member.Loaded = false; return Reason.Accepted;
            }
            if (Phase != SessionPhase.Lobby && Phase != SessionPhase.Hub) return Reason.SessionInProgress;
            members.RemoveAll(m => !m.Connected && now >= m.ReservedUntil);
            if (members.Count >= 4) return Reason.LobbyFull;
            // Host-issued unguessable capability, transported only in the recipient's handshake.
            member = new SessionMember { Actor = new Actor(++nextActor, ++generation), Ticket = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"), Connected = true };
            members.Add(member); if (host) HostId = member.Actor.Id;
            return Reason.Accepted;
        }
        public void Disconnect(Actor actor, double now, double grace, Action<Actor> cleanup)
        {
            var m = members.FirstOrDefault(x => x.Actor.Equals(actor) && x.Connected);
            if (m == null) return;
            cleanup(actor); m.Connected = false; m.Ready = false; m.Loaded = false; m.ReservedUntil = now + grace;
        }
        public Reason Ready(Actor actor, bool loaded)
        {
            var m = members.FirstOrDefault(x => x.Actor.Equals(actor) && x.Connected);
            if (m == null) return Reason.AuthFailed;
            m.Loaded |= loaded; m.Ready = true; return Reason.Accepted;
        }
        public Reason Transition(SessionPhase next, int requester)
        {
            if (requester != HostId) return Reason.AuthFailed;
            if (next == Phase) return Reason.AlreadyApplied;
            bool legal = (Phase == SessionPhase.Lobby || Phase == SessionPhase.Hub) && next == SessionPhase.Loading ||
                Phase == SessionPhase.Loading && (next == SessionPhase.Briefing || next == SessionPhase.Lobby) ||
                Phase == SessionPhase.Briefing && (next == SessionPhase.Active || next == SessionPhase.Lobby) ||
                Phase == SessionPhase.Active && next == SessionPhase.Settling || Phase == SessionPhase.Settling && next == SessionPhase.Results ||
                Phase == SessionPhase.Results && next == SessionPhase.Hub;
            if (!legal) return Reason.InvalidState;
            int minPlayers = Mode == SessionMode.Solo ? 1 : 2;
            if (next == SessionPhase.Loading)
            {
                if (members.Count(m => m.Connected) < minPlayers) return Reason.NotReady;
                startRoster.Clear(); foreach (var m in members.Where(m => m.Connected)) { startRoster.Add(m.Actor.Id); m.Loaded = false; m.Ready = false; }
            }
            if (next == SessionPhase.Briefing || next == SessionPhase.Active)
                if (startRoster.Count < minPlayers || startRoster.Any(id => !members.Any(m => m.Actor.Id == id && m.Connected && m.Loaded && m.Ready))) return Reason.NotReady;
            Phase = next; Revision++; return Reason.Accepted;
        }
    }
    public sealed class DriverLease
    {
        public Actor? Driver { get; private set; }
        public float Intent { get; private set; }
        double renewed;
        public Reason Acquire(Actor actor, bool reachable, double now)
        {
            if (!reachable) return Reason.TooFar;
            if (Driver.HasValue && !Driver.Value.Equals(actor)) return Reason.Busy;
            Driver = actor; Intent = 0; renewed = now; return Reason.Accepted;
        }
        public Reason Control(Actor actor, float intent, bool reachable, double now)
        {
            if (!Driver.HasValue || !Driver.Value.Equals(actor)) return Reason.AuthFailed;
            if (!reachable || !Numbers.Finite(intent) || Math.Abs(intent) > 1) { Release(actor); return Reason.Blocked; }
            Intent = intent; renewed = now; return Reason.Accepted;
        }
        public void Watchdog(double now, double timeout) { if (Driver.HasValue && now - renewed >= timeout) Release(Driver.Value); }
        public void Release(Actor actor) { if (Driver.HasValue && Driver.Value.Equals(actor)) { Driver = null; Intent = 0; } }
        public void Stop() { Driver = null; Intent = 0; }
    }
}
