using System;
using System.Collections.Generic;
using System.Linq;
using OnHold.Core;

namespace OnHold.Domain
{
    public enum Lifecycle { Available, Delivered, Lost }
    public enum Handling { Free, Held, Secured }
    public interface IAttachment : IDisposable { }
    public sealed class AnchorSlot
    {
        public readonly string Id, PlatformId;
        public readonly double Capacity;
        public string ItemId { get; internal set; }
        public AnchorSlot(string id, string platformId, double capacity)
        {
            if (!Numbers.Id(id) || !Numbers.Id(platformId) || !Numbers.Finite(capacity) || capacity <= 0) throw new ArgumentException("anchor");
            Id = id; PlatformId = platformId; Capacity = capacity;
        }
    }
    public sealed class ItemState : IDisposable
    {
        readonly Dictionary<Actor, IAttachment> holds = new Dictionary<Actor, IAttachment>();
        IAttachment anchorHandle;
        ulong lastRecoveryEvent;
        public string Id { get; }
        public string DefinitionId { get; }
        public double Mass { get; }
        public long BaseValue { get; }
        public Lifecycle Lifecycle { get; private set; } = Lifecycle.Available;
        public Handling Handling { get; private set; } = Handling.Free;
        public double Integrity { get; private set; } = 100;
        public uint Revision { get; private set; }
        public int RecoveryCount { get; private set; }
        public AnchorSlot Anchor { get; private set; }
        public IReadOnlyCollection<Actor> Holders => holds.Keys;
        public ItemState(string id, string definition, double mass, long value)
        {
            if (!Numbers.Id(id) || !Numbers.Id(definition) || !Numbers.Finite(mass) || mass <= 0 || mass > 1000 || value < 0 || value > 1000000000) throw new ArgumentException("Invalid item definition");
            Id = id; DefinitionId = definition; Mass = mass; BaseValue = value;
        }
        public bool IsHeldBy(Actor actor) => holds.ContainsKey(actor);
        public Reason CanGrab(Actor actor)
        {
            if (actor.Id <= 0 || actor.Generation == 0) return Reason.AuthFailed;
            if (Lifecycle != Lifecycle.Available || Handling == Handling.Secured) return Reason.InvalidState;
            if (holds.ContainsKey(actor)) return Reason.AlreadyApplied;
            if (holds.Count == 2) return Reason.Busy;
            foreach (var holder in holds.Keys) if (holder.Id == actor.Id) return Reason.Busy;
            return Reason.Accepted;
        }
        public Reason Grab(Actor actor, Func<IAttachment> create)
        {
            var eligibility = CanGrab(actor);
            if (eligibility != Reason.Accepted) return eligibility;
            IAttachment handle;
            try { handle = create(); } catch { return Reason.AttachmentFailed; }
            if (handle == null) return Reason.AttachmentFailed;
            holds.Add(actor, handle); Handling = Handling.Held; Revision++; AssertInvariant(); return Reason.Accepted;
        }
        // No revision requirement: only this connection generation's hold may be removed.
        public Reason Release(Actor actor)
        {
            if (!holds.TryGetValue(actor, out var h)) return Reason.AlreadyApplied;
            h.Dispose(); holds.Remove(actor); if (holds.Count == 0) Handling = Handling.Free;
            Revision++; AssertInvariant(); return Reason.Accepted;
        }
        public Reason Secure(AnchorSlot slot, uint expected, bool supported, bool reachable, double speed, FoundationConfig c, Func<IAttachment> create)
        {
            if (Lifecycle != Lifecycle.Available || Handling != Handling.Free) return Reason.InvalidState;
            if (expected != Revision) return Reason.StaleRevision;
            if (!reachable) return Reason.TooFar;
            if (!supported || slot == null || Mass > slot.Capacity) return Reason.Blocked;
            if (!Numbers.Finite(speed) || Math.Abs(speed) >= c.WinchSecureSpeed) return Reason.UnsafeSpeed;
            if (slot.ItemId != null) return Reason.Busy;
            slot.ItemId = Id;
            IAttachment handle = null;
            try { handle = create(); } catch { /* Rollback the reservation below. */ }
            if (handle == null) { slot.ItemId = null; return Reason.AttachmentFailed; }
            Anchor = slot; anchorHandle = handle; Handling = Handling.Secured; Revision++; AssertInvariant(); return Reason.Accepted;
        }
        public Reason Unsecure(uint expected, bool reachable, bool safePlacement, double speed, FoundationConfig c)
        {
            if (Lifecycle != Lifecycle.Available || Handling != Handling.Secured) return Reason.InvalidState;
            if (expected != Revision) return Reason.StaleRevision;
            if (!reachable) return Reason.TooFar;
            if (!safePlacement) return Reason.Blocked;
            if (!Numbers.Finite(speed) || Math.Abs(speed) >= c.WinchSecureSpeed) return Reason.UnsafeSpeed;
            ClearAttachments(); Revision++; AssertInvariant(); return Reason.Accepted;
        }
        public void Damage(double amount)
        {
            if (!Numbers.Finite(amount) || amount < 0) throw new ArgumentException("damage");
            if (Lifecycle != Lifecycle.Available || amount == 0) return;
            Integrity = Math.Max(0, Integrity - amount); Revision++;
            if (Integrity == 0) { ClearAttachments(); Lifecycle = Lifecycle.Lost; }
            AssertInvariant();
        }
        public Reason Recover(ulong eventId, FoundationConfig c, out double penalty)
        {
            penalty = 0;
            if (Lifecycle != Lifecycle.Available) return Reason.InvalidState;
            if (eventId == 0 || eventId <= lastRecoveryEvent) return Reason.AlreadyApplied;
            lastRecoveryEvent = eventId; ClearAttachments(); Revision++;
            if (RecoveryCount >= c.RecoveryMax) { Lifecycle = Lifecycle.Lost; return Reason.InvalidState; }
            RecoveryCount++; penalty = c.RecoveryPenalty; AssertInvariant(); return Reason.Accepted;
        }
        public void MarkLost() { if (Lifecycle != Lifecycle.Available) return; ClearAttachments(); Lifecycle = Lifecycle.Lost; Revision++; AssertInvariant(); }
        internal void Deliver() { ClearAttachments(); Lifecycle = Lifecycle.Delivered; Revision++; AssertInvariant(); }
        public void ClearAttachments()
        {
            foreach (var h in holds.Values) h.Dispose(); holds.Clear();
            anchorHandle?.Dispose(); anchorHandle = null;
            if (Anchor != null) Anchor.ItemId = null;
            Anchor = null; Handling = Handling.Free;
        }
        public void AssertInvariant()
        {
            if (!Numbers.Finite(Integrity) || Integrity < 0 || Integrity > 100 || (Integrity == 0 && Lifecycle != Lifecycle.Lost) ||
                (Lifecycle != Lifecycle.Available && Handling != Handling.Free) ||
                (Handling == Handling.Held && (holds.Count < 1 || holds.Count > 2 || Anchor != null)) ||
                (Handling == Handling.Free && (holds.Count != 0 || Anchor != null || anchorHandle != null)) ||
                (Handling == Handling.Secured && (holds.Count != 0 || Anchor == null || Anchor.ItemId != Id || anchorHandle == null)))
                throw new InvalidOperationException("Item invariant: " + Id);
        }
        public void Dispose() => ClearAttachments();
    }
}
