using NUnit.Framework;
using OnHold.Core;
using OnHold.Domain;
using OnHold.Gameplay;
namespace OnHold.Tests
{
    public sealed class InteractionFeedbackTests
    {
        [Test] public void S017_NoTargetAndSuppressedNeverExposeAction()
        {
            Assert.IsFalse(default(InteractionFeedback).Visible);
            var neutral = new InteractionFeedback(true);
            Assert.IsTrue(neutral.Visible); Assert.IsFalse(neutral.Actionable); Assert.IsNull(neutral.Action); Assert.AreEqual(0, neutral.TargetId);
        }
        [TestCase(Reason.TooFar, InteractionNotice.MoveCloser)]
        [TestCase(Reason.Busy, InteractionNotice.NoAvailableGrip)]
        [TestCase(Reason.StaleRevision, InteractionNotice.Unavailable)]
        [TestCase(Reason.InvalidPayload, InteractionNotice.Unavailable)]
        public void S017_OnlyPlayerFacingReasons(Reason reason, InteractionNotice expected)
        { Assert.AreEqual(expected, InteractionFeedback.Explain(reason, Handling.Free)); }
        [Test] public void S017_SecuredHasDeliberateMessage()
        { Assert.AreEqual(InteractionNotice.Secured, InteractionFeedback.Explain(Reason.InvalidState, Handling.Secured)); }
        [Test] public void S017_EligibilityDoesNotAcquireAttachment()
        {
            var item = new ItemState("test", "cargo", 20, 100); var actor = new Actor(1, 1);
            for (int i = 0; i < 1000; i++) Assert.AreEqual(Reason.Accepted, item.CanGrab(actor));
            Assert.AreEqual(Handling.Free, item.Handling); Assert.AreEqual(0, item.Holders.Count); Assert.AreEqual(0, item.Revision);
        }

        [Test] public void S018_DefaultAndRejectedCandidatesCannotBePresentedAsValid()
        {
            Assert.IsFalse(default(GrabCandidate).Success);
            Assert.IsFalse(new GrabCandidate(Reason.Busy).Success);
            Assert.IsFalse(new InteractionFeedback(true, action: CommandType.Release, availability: InteractionAvailability.Available).HasGrip);
        }
        [Test] public void S018_DomainPreviewRejectsSecuredTerminalAndInvalidActors()
        {
            var item = new ItemState("test", "cargo", 20, 100);
            Assert.AreEqual(Reason.AuthFailed, item.CanGrab(new Actor(1, 0)));
            item.MarkLost(); Assert.AreEqual(Reason.InvalidState, item.CanGrab(new Actor(1, 1)));
            Assert.AreEqual(0, item.Holders.Count);
        }
    }
}
