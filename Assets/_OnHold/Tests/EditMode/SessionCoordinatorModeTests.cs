using NUnit.Framework;
using OnHold.Domain;
using OnHold.Core;

namespace OnHold.Tests
{
    public class SessionCoordinatorModeTests
    {
        [Test]
        public void SoloMode_OnePlayer_CanTransitionToLoading()
        {
            var session = new SessionCoordinator(100, SessionMode.Solo);
            
            // Join one host player
            var joinResult = session.Join("", true, 0, out var hostMember);
            Assert.AreEqual(Reason.Accepted, joinResult);

            // Attempt to transition to Loading
            var transitionResult = session.Transition(SessionPhase.Loading, hostMember.Actor.Id);
            Assert.AreEqual(Reason.Accepted, transitionResult, "Solo mode should allow 1 connected player to transition to Loading.");
        }

        [Test]
        public void CoopMode_OnePlayer_CannotTransitionToLoading()
        {
            var session = new SessionCoordinator(100, SessionMode.Coop);
            
            // Join one host player
            var joinResult = session.Join("", true, 0, out var hostMember);
            Assert.AreEqual(Reason.Accepted, joinResult);

            // Attempt to transition to Loading
            var transitionResult = session.Transition(SessionPhase.Loading, hostMember.Actor.Id);
            Assert.AreEqual(Reason.NotReady, transitionResult, "Coop mode should reject transition to Loading if there's only 1 connected player.");
        }

        [Test]
        public void CoopMode_TwoPlayers_CanTransitionToLoading()
        {
            var session = new SessionCoordinator(100, SessionMode.Coop);
            
            // Join two players
            session.Join("", true, 0, out var hostMember);
            session.Join("", false, 0, out _);

            // Attempt to transition to Loading
            var transitionResult = session.Transition(SessionPhase.Loading, hostMember.Actor.Id);
            Assert.AreEqual(Reason.Accepted, transitionResult, "Coop mode should allow 2 connected players to transition to Loading.");
        }
        
        [Test]
        public void CoopMode_ModeSpecificRejectionDoesNotMutateSessionIncorrectly()
        {
            var session = new SessionCoordinator(100, SessionMode.Coop);
            
            // Join one host player
            session.Join("", true, 0, out var hostMember);

            Assert.AreEqual(SessionPhase.Lobby, session.Phase);

            // Attempt to transition to Loading
            var transitionResult = session.Transition(SessionPhase.Loading, hostMember.Actor.Id);
            
            // Ensure rejection didn't change phase
            Assert.AreEqual(Reason.NotReady, transitionResult);
            Assert.AreEqual(SessionPhase.Lobby, session.Phase, "Rejection should not mutate session phase.");
        }

        [Test]
        public void SoloMode_UsesNormalLifecycle()
        {
            var session = new SessionCoordinator(100, SessionMode.Solo);
            
            // Join one host player
            session.Join("", true, 0, out var hostMember);

            // Loading
            Assert.AreEqual(Reason.Accepted, session.Transition(SessionPhase.Loading, hostMember.Actor.Id));
            
            // Need to set loaded and ready to proceed to Briefing
            
            session.Ready(hostMember.Actor, true);

            // Briefing
            Assert.AreEqual(Reason.Accepted, session.Transition(SessionPhase.Briefing, hostMember.Actor.Id));

            // Active
            Assert.AreEqual(Reason.Accepted, session.Transition(SessionPhase.Active, hostMember.Actor.Id));
            Assert.AreEqual(SessionPhase.Active, session.Phase, "Solo mode should transition to Active normally.");
        }
    }
}
