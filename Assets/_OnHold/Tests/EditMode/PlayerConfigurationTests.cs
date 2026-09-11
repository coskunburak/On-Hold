using System;
using NUnit.Framework;
using OnHold.Core;
using OnHold.Domain;

namespace OnHold.Tests
{
    public sealed class PlayerConfigurationTests
    {
        [Test] public void S011_DefaultConfigurationIsValid() => new FoundationConfig().Validate();
        [TestCase("PlayerSpeed", -1f)] [TestCase("PlayerRadius", 0f)]
        [TestCase("Gravity", float.NaN)] [TestCase("JumpHeight", -1f)]
        [TestCase("AirControl", 1.1f)] [TestCase("EyeHeight", 2f)]
        [TestCase("StepHeight", 1.8f)] [TestCase("PitchMin", 86f)]
        [TestCase("PitchMax", -86f)] [TestCase("Reach", 0f)]
        [TestCase("HandForceMax", -1f)] [TestCase("TotalTorqueMax", -1f)]
        [TestCase("CarryBreakDistance", 1f)] [TestCase("CarryAngularDamping", float.PositiveInfinity)]
        public void S011_InvalidTuningFailsEarly(string field, float value)
        {
            var config = new FoundationConfig(); typeof(FoundationConfig).GetField(field).SetValue(config, value);
            Assert.Throws<ArgumentException>(config.Validate);
        }
        [Test] public void S008_JumpUsesExistingCommandValidationAndDeduplication()
        {
            var gate = new CommandGate(); var actor = new Actor(1, 1); int jumps = 0;
            var command = new CommandEnvelope { ProtocolVersion = CommandGate.Protocol, SessionEpoch = 1, ConnectionGeneration = 1,
                Sequence = 1, RequestId = 1, CommandType = CommandType.Jump };
            Reason Apply(CommandEnvelope e, Actor a) { jumps++; return Reason.Accepted; }
            Assert.AreEqual(Reason.Accepted, gate.Execute(command, 88, actor, 1, 0, Apply));
            Assert.AreEqual(Reason.Accepted, gate.Execute(command, 88, actor, 1, .1, Apply)); Assert.AreEqual(1, jumps);
            command.Sequence = command.RequestId = 2; command.Pitch = float.NaN;
            Assert.AreEqual(Reason.InvalidPayload, gate.Execute(command, 88, actor, 1, .2, Apply)); Assert.AreEqual(1, jumps);
        }
    }
}
