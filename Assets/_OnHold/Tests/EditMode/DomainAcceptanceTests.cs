using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OnHold.Core;
using OnHold.Domain;

namespace OnHold.Tests
{
    public sealed class DomainAcceptanceTests
    {
        readonly FoundationConfig config = new FoundationConfig();
        static readonly Actor A = new Actor(1, 1), B = new Actor(2, 2), C = new Actor(3, 3);
        sealed class Handle : IAttachment { public bool Disposed; public void Dispose() { Disposed = true; } }
        ItemState Item(string id = "cargo.a") => new ItemState(id, "item.a", 100, 101);
        ContractRun Run(ItemState item, double duration = 600) => new ContractRun("run.a", new[] { item }, new[] { item.Id }, 1, 100, 0, duration, config);
        [Test] public void TC009_TwoAxesAndFiftyGrabReleaseCycles()
        {
            var item = Item();
            for (int i = 0; i < 50; i++)
            {
                var first = new Handle(); var second = new Handle();
                Assert.AreEqual(Reason.Accepted, item.Grab(A, () => first));
                Assert.AreEqual(Reason.Accepted, item.Grab(B, () => second));
                Assert.AreEqual(Reason.Busy, item.Grab(C, () => new Handle()));
                Assert.AreEqual(Reason.Accepted, item.Release(A)); Assert.IsTrue(first.Disposed); Assert.IsFalse(second.Disposed);
                CollectionAssert.AreEqual(new[] { B }, item.Holders); Assert.AreEqual(Handling.Held, item.Handling);
                item.Release(B); item.Release(B); item.AssertInvariant(); Assert.AreEqual(Handling.Free, item.Handling);
            }
        }
        [Test] public void TC009_FailedAttachmentCannotCreatePhantomHolder()
        {
            var item = Item(); Assert.AreEqual(Reason.AttachmentFailed, item.Grab(A, () => throw new InvalidOperationException()));
            Assert.AreEqual(0, item.Holders.Count); Assert.AreEqual(0, item.Revision); item.AssertInvariant();
        }
        [Test] public void TC010_AnchorReservationRollbackAndHundredCycles()
        {
            var item = Item(); var rival = Item("cargo.b"); var slot = new AnchorSlot("anchor.a", "platform.a", 200);
            Assert.AreEqual(Reason.AttachmentFailed, item.Secure(slot, 0, true, true, 0, config, () => null)); Assert.IsNull(slot.ItemId);
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(Reason.Accepted, item.Secure(slot, item.Revision, true, true, 0, config, () => new Handle()));
                Assert.AreEqual(Reason.Busy, rival.Secure(slot, rival.Revision, true, true, 0, config, () => new Handle()));
                Assert.AreEqual(Reason.UnsafeSpeed, item.Unsecure(item.Revision, true, true, .1, config));
                Assert.AreEqual(Reason.Accepted, item.Unsecure(item.Revision, true, true, 0, config)); Assert.IsNull(slot.ItemId); item.AssertInvariant();
            }
        }
        [Test] public void TC010_SecureCannotStealHeldItemAndStaleExclusiveRejected()
        {
            var i = Item(); var slot = new AnchorSlot("anchor.a", "platform.a", 100);
            i.Grab(A, () => new Handle()); Assert.AreEqual(Reason.InvalidState, i.Secure(slot, i.Revision, true, true, 0, config, () => new Handle()));
            i.Release(A); Assert.AreEqual(Reason.StaleRevision, i.Secure(slot, 0, true, true, 0, config, () => new Handle()));
        }
        [TestCase(19, false)] [TestCase(20, true)]
        public void TC011_IntegrityThresholdDwellAndFloorRounding(int integrity, bool expected)
        {
            var item = Item(); item.Damage(100 - integrity); var run = Run(item);
            Assert.IsFalse(run.EvaluateDelivery(item, true, 0, 0, 0));
            Assert.AreEqual(expected, run.EvaluateDelivery(item, true, 0, 0, 1));
            Assert.AreEqual(expected ? 20 : 0, run.ProvisionalValue);
        }
        [Test] public void TC011_InterruptedDwellResetsAndRepeatedEventsDoNotPay()
        {
            var item = Item(); var run = Run(item);
            run.EvaluateDelivery(item, true, 0, 0, 0); run.EvaluateDelivery(item, false, 0, 0, .9);
            Assert.IsFalse(run.EvaluateDelivery(item, true, 0, 0, 1)); Assert.IsFalse(run.EvaluateDelivery(item, true, 0, 0, 1.9));
            Assert.IsTrue(run.EvaluateDelivery(item, true, 0, 0, 2));
            for (int j = 0; j < 100; j++) Assert.IsFalse(run.EvaluateDelivery(item, true, 0, 0, 3 + j));
            Assert.AreEqual(1, run.Ledger.Count); Assert.AreEqual(101, run.ProvisionalValue);
            Assert.AreEqual(Reason.InvalidState, item.Grab(A, () => new Handle()));
        }
        [Test] public void TC011_HeldSecuredAndAngularThresholdCannotDeliver()
        {
            var item = Item(); var run = Run(item); item.Grab(A, () => new Handle());
            Assert.IsFalse(run.EvaluateDelivery(item, true, 0, 0, 0)); Assert.IsFalse(run.EvaluateDelivery(item, true, 0, 0, 2));
            item.Release(A); Assert.IsFalse(run.EvaluateDelivery(item, true, 0, 5, 3)); Assert.IsFalse(run.EvaluateDelivery(item, true, 0, 0, 4));
            Assert.IsTrue(run.EvaluateDelivery(item, true, 0, 0, 5));
        }
        [Test] public void TC013_TimeoutAndDamageWinBeforeNewDelivery()
        {
            var i = Item(); var r = Run(i, 1); r.EvaluateDelivery(i, true, 0, 0, 0);
            Assert.IsFalse(r.BeginTick(1)); Assert.IsFalse(r.EvaluateDelivery(i, true, 0, 0, 1)); Assert.AreEqual(0, r.Result.Payout);
            i = Item(); r = Run(i); r.EvaluateDelivery(i, true, 0, 0, 0); i.Damage(100);
            Assert.IsFalse(r.EvaluateDelivery(i, true, 0, 0, 1)); Assert.IsTrue(r.MandatoryLost);
        }
        [Test] public void TC013_NaturalTimeoutPaysPartialButAbortPaysZero()
        {
            foreach (var reason in new[] { FinishReason.Timeout, FinishReason.Abort })
            {
                var i = Item(); var j = Item("cargo.b"); var r = new ContractRun("run.a", new[] { i, j }, new[] { j.Id }, 2, 100, 0, 600, config);
                r.EvaluateDelivery(i, true, 0, 0, 0); r.EvaluateDelivery(i, true, 0, 0, 1);
                Assert.AreEqual(reason == FinishReason.Timeout ? 101 : 0, r.Finish(reason).Payout);
            }
        }
        [Test] public void TC013_FinishVotePinsRosterAndRequiresHostMajority()
        {
            var i = Item(); var r = Run(i); r.EvaluateDelivery(i, true, 0, 0, 0); r.EvaluateDelivery(i, true, 0, 0, 1);
            Assert.AreEqual(Reason.VoteRequired, r.Vote(2, 1, new[] { 1, 2, 3, 4 }, 2));
            Assert.AreEqual(Reason.VoteRequired, r.Vote(1, 1, new[] { 1, 2 }, 3)); Assert.IsNull(r.Result);
            Assert.AreEqual(Reason.Accepted, r.Vote(3, 1, new[] { 1, 2, 3 }, 4)); Assert.AreEqual(201, r.Result.Payout);
        }
        [Test] public void TC014_TwoRecoveriesThirdLostPreservesDamageAndDeduplicates()
        {
            var i = Item(); i.Damage(30); i.Grab(A, () => new Handle());
            Assert.AreEqual(Reason.Accepted, i.Recover(1, config, out var p)); Assert.AreEqual(15, p); Assert.AreEqual(70, i.Integrity);
            Assert.AreEqual(Reason.AlreadyApplied, i.Recover(1, config, out p)); Assert.AreEqual(0, p);
            i.Recover(2, config, out p); Assert.AreEqual(15, p); i.Recover(3, config, out p);
            Assert.AreEqual(Lifecycle.Lost, i.Lifecycle); Assert.AreEqual(2, i.RecoveryCount); Assert.AreEqual(0, p); Assert.AreEqual(0, i.Holders.Count);
            Assert.AreEqual(Reason.InvalidState, i.Recover(4, config, out _));
        }
        [Test] public void TC009_ZeroIntegrityClearsAnchorAndTerminalNeverResurrects()
        {
            var i = Item(); var slot = new AnchorSlot("anchor.a", "platform.a", 200); var h = new Handle(); i.Secure(slot, 0, true, true, 0, config, () => h);
            i.Damage(100); Assert.IsTrue(h.Disposed); Assert.IsNull(slot.ItemId); i.AssertInvariant();
            i.Recover(1, config, out _); Assert.AreEqual(Lifecycle.Lost, i.Lifecycle);
        }
        [Test] public void TC005_SupportIdentityHysteresisAndSymmetry()
        {
            var s = new SupportedSet();
            s.Observe("a", 80, -1, 0, true, false, 0, config); s.Observe("b", 80, 1, 0, true, false, 0, config); s.Calculate(config.BaseMass); Assert.AreEqual(0, s.Count);
            s.Observe("a", 80, -1, 0, true, false, .1, config); s.Observe("b", 80, 1, 0, true, true, .1, config);
            s.Observe("b", 80, 1, 0, true, true, .1, config); s.Calculate(config.BaseMass); Assert.AreEqual(280, s.Mass); Assert.AreEqual(0, s.CentreX);
            s.Observe("a", 80, -1, 0, false, false, .2, config); s.Calculate(config.BaseMass); Assert.AreEqual(2, s.Count);
            s.Observe("a", 80, -1, 0, false, false, .4, config); s.Calculate(config.BaseMass); Assert.AreEqual(1, s.Count); Assert.Greater(s.CentreX, 0);
        }
        [Test] public void TC016_ReconnectCleansImmediatelyAndChangesGeneration()
        {
            var s = new SessionCoordinator(1, SessionMode.Coop); s.Join("", true, 0, out var host); s.Join("", false, 0, out var guest);
            var original = guest.Actor; int cleanup = 0; s.Disconnect(original, 1, 60, a => cleanup++); Assert.AreEqual(1, cleanup); Assert.IsFalse(guest.Connected);
            Assert.AreEqual(Reason.Accepted, s.Join(guest.Ticket, false, 2, out var rejoined)); Assert.AreEqual(original.Id, rejoined.Actor.Id); Assert.AreNotEqual(original.Generation, rejoined.Actor.Generation);
            var item = Item(); item.Grab(rejoined.Actor, () => new Handle()); item.Release(original); Assert.AreEqual(1, item.Holders.Count);
        }
        [Test] public void TC015_LoadingCannotSilentlyDropPlayerAndLateJoinRejected()
        {
            var s = new SessionCoordinator(1, SessionMode.Coop); s.Join("", true, 0, out var host);
            Assert.AreEqual(Reason.NotReady, s.Transition(SessionPhase.Loading, host.Actor.Id)); s.Join("", false, 0, out var guest);
            Assert.AreEqual(Reason.Accepted, s.Transition(SessionPhase.Loading, host.Actor.Id)); s.Ready(host.Actor, true);
            Assert.AreEqual(Reason.NotReady, s.Transition(SessionPhase.Briefing, host.Actor.Id)); s.Ready(guest.Actor, true);
            s.Transition(SessionPhase.Briefing, host.Actor.Id); s.Transition(SessionPhase.Active, host.Actor.Id);
            Assert.AreEqual(Reason.SessionInProgress, s.Join("", false, 0, out _));
        }
        [Test] public void TC015_FullLobbyAndExpiredReservation()
        {
            var s = new SessionCoordinator(1, SessionMode.Coop); for (int i = 0; i < 4; i++) Assert.AreEqual(Reason.Accepted, s.Join("", i == 0, 0, out _));
            Assert.AreEqual(Reason.LobbyFull, s.Join("", false, 0, out _)); var member = s.Members[1]; s.Disconnect(member.Actor, 0, 60, _ => { });
            Assert.AreEqual(Reason.ReservationExpired, s.Join(member.Ticket, false, 61, out _));
        }
        [Test] public void TC016_DriverWatchdogSafeStopsAndDoesNotTransferIntent()
        {
            var lease = new DriverLease(); lease.Acquire(A, true, 0); lease.Control(A, 1, true, .1);
            Assert.AreEqual(Reason.Busy, lease.Acquire(B, true, .2)); lease.Watchdog(.7, .5); Assert.IsNull(lease.Driver); Assert.AreEqual(0, lease.Intent);
            lease.Acquire(B, true, 1); Assert.AreEqual(0, lease.Intent);
        }
        CommandEnvelope Command(ulong seq = 1) => new CommandEnvelope { ProtocolVersion = 1, SessionEpoch = 1, ConnectionGeneration = 1, RequestId = seq, Sequence = seq, CommandType = CommandType.Grab, TargetId = 1 };
        [Test] public void TC022_FuzzCannotReachMutation()
        {
            var invalid = new List<CommandEnvelope>(); var e = Command(); e.X = float.NaN; invalid.Add(e); e = Command(); e.Y = float.PositiveInfinity; invalid.Add(e);
            e = Command(); e.CommandType = (CommandType)255; invalid.Add(e); e = Command(); e.TargetId = -1; invalid.Add(e);
            e = Command(); e.ConnectionGeneration = 99; invalid.Add(e); e = Command(); e.SessionEpoch = 99; invalid.Add(e);
            int effects = 0;
            foreach (var input in invalid) { var g = new CommandGate(); Assert.AreNotEqual(Reason.Accepted, g.Execute(input, 80, A, 1, 0, (_, __) => { effects++; return Reason.Accepted; })); }
            Assert.AreEqual(Reason.InvalidPayload, new CommandGate().Execute(Command(), 1000000, A, 1, 0, (_, __) => { effects++; return Reason.Accepted; })); Assert.AreEqual(0, effects);
        }
        [Test] public void TC022_DedupEvictionStillRejectsOldSequenceAndSpamIsBounded()
        {
            var g = new CommandGate(); int effects = 0;
            Reason Apply(CommandEnvelope e, Actor a) { effects++; return Reason.Accepted; }
            for (ulong i = 1; i <= 300; i++) Assert.AreEqual(Reason.Accepted, g.Execute(Command(i), 80, A, 1, i * .1, Apply));
            Assert.AreEqual(256, g.CacheCount); Assert.AreEqual(Reason.OldSequence, g.Execute(Command(1), 80, A, 1, 31, Apply));
            for (ulong i = 301; i < 10301; i++) g.Execute(Command(i), 80, A, 1, 31, Apply);
            Assert.LessOrEqual(effects, 316); Assert.LessOrEqual(g.CacheCount, 256); Assert.IsTrue(g.ShouldDisconnect);
        }
        [Test] public void TC003_ActorAlwaysComesFromConnectionAndRevisionSafeRelease()
        {
            var g = new CommandGate(); Actor resolved = default;
            g.Execute(Command(), 80, A, 1, 0, (e, a) => { resolved = a; return Reason.Accepted; }); Assert.AreEqual(A, resolved);
            Assert.IsNull(typeof(CommandEnvelope).GetField("ActorId")); Assert.IsNull(typeof(CommandEnvelope).GetField("Price"));
        }
        [Test] public void ConfigRejectsInvalidNumerics() { var c = new FoundationConfig { CarrySpring = float.NaN }; Assert.Throws<ArgumentException>(c.Validate); }
    }
}
