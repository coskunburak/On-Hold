using System;
using NUnit.Framework;
using OnHold.Core;
using OnHold.Gameplay;
using UnityEngine;

namespace OnHold.Tests
{
    public sealed class HeldOrientationTests
    {
        readonly FoundationConfig config = new FoundationConfig();
        [Test] public void S019_InitializationMatchesActualAndDoesNotEngageCorrection()
        {
            var state = new HeldOrientation(); var actual = Quaternion.Euler(23, 177, -34);
            state.Initialize(actual);
            Assert.IsTrue(state.Initialized); Assert.IsFalse(state.Engaged); Assert.IsFalse(state.Active);
            Assert.Less(Quaternion.Angle(actual, state.Target), .01f);
        }
        [TestCase(1, 0)] [TestCase(-1, 0)] [TestCase(0, 1)] [TestCase(0, -1)]
        public void S019_InputUsesPredictableCameraAxes(float x, float y)
        {
            var state = new HeldOrientation(); state.Initialize(Quaternion.identity);
            var view = Quaternion.Euler(25, 180, 0);
            Assert.IsTrue(state.Update(true, new Vector2(x, y), view, Quaternion.identity, .02f, config));
            var expected = Quaternion.AngleAxis(1, view * new Vector3(-y, x, 0));
            Assert.Less(Quaternion.Angle(expected, state.Target), .01f);
        }
        [Test] public void S019_UninitializedAndInvalidCargoDoNotCaptureInput()
        {
            var state = new HeldOrientation();
            Assert.IsFalse(state.Update(true, Vector2.one, Quaternion.identity, Quaternion.identity, .02f, config));
            var go = new GameObject("unowned cargo"); var cargo = go.AddComponent<CarryableBody>();
            try { Assert.IsFalse(cargo.UpdateManipulation(new Actor(1, 1), true, Vector2.one, Quaternion.identity, .02f)); }
            finally { UnityEngine.Object.DestroyImmediate(go); }
            Assert.IsFalse(cargo.CanManipulate(new Actor(1, 1)));
        }
        [Test] public void S019_FirstEntryCapturesActualThenExitKeepsTarget()
        {
            var state = new HeldOrientation(); state.Initialize(Quaternion.identity);
            var actual = Quaternion.Euler(12, 43, -20);
            state.Update(true, Vector2.zero, Quaternion.identity, actual, .02f, config);
            Assert.Less(Quaternion.Angle(actual, state.Target), .01f);
            state.Update(true, Vector2.one, Quaternion.identity, actual, .02f, config);
            var requested = state.Target; state.EndInput();
            Assert.IsFalse(state.Active); Assert.IsTrue(state.Engaged);
            Assert.AreEqual(requested, state.Target);
            state.Update(true, Vector2.zero, Quaternion.identity, actual, .02f, config);
            Assert.Less(Quaternion.Angle(requested, state.Target), .01f);
            state.Reset(); Assert.IsFalse(state.Initialized); Assert.IsFalse(state.Active); Assert.IsFalse(state.Engaged);
            state.Initialize(actual); Assert.Less(Quaternion.Angle(actual, state.Target), .01f);
        }
        [Test] public void S019_ExtremeInputAndRepeatedObstructionBoundTargetLead()
        {
            var state = new HeldOrientation(); state.Initialize(Quaternion.identity);
            state.Update(true, Vector2.one * float.MaxValue, Quaternion.identity, Quaternion.identity, .02f, config);
            Assert.That(Quaternion.Angle(Quaternion.identity, state.Target), Is.InRange(2.3f, 2.41f));
            for (int i = 0; i < 1000; i++) state.Update(true, Vector2.one * float.MaxValue, Quaternion.identity, Quaternion.identity, .02f, config);
            Assert.LessOrEqual(Quaternion.Angle(Quaternion.identity, state.Target), config.ManipulationLeadMax + .01f);
            Assert.IsTrue(SafeMath.Finite(state.Target)); Assert.That(Quaternion.Dot(state.Target, state.Target), Is.EqualTo(1).Within(.0001));
        }
        [Test] public void S019_TargetSpeedIsConsistentAcrossRenderRates()
        {
            Quaternion Target(int hz)
            {
                var state = new HeldOrientation(); state.Initialize(Quaternion.identity);
                for (int i = 0; i < hz / 2; i++) state.Update(true, new Vector2(120f / hz, 0), Quaternion.identity, state.Target, 1f / hz, config);
                return state.Target;
            }
            Assert.Less(Quaternion.Angle(Target(30), Target(120)), .02f);
        }
        [Test] public void S019_InvalidInputAndQuaternionSafelyNoOp()
        {
            var state = new HeldOrientation(); state.Initialize(default); Assert.IsFalse(state.Initialized);
            state.Initialize(Quaternion.identity);
            Assert.IsFalse(state.Update(true, new Vector2(float.NaN, 0), Quaternion.identity, Quaternion.identity, .02f, config));
            Assert.IsFalse(state.Update(true, Vector2.one, default, Quaternion.identity, .02f, config));
            Assert.IsFalse(state.Update(true, Vector2.one, Quaternion.identity, Quaternion.identity, float.PositiveInfinity, config));
            Assert.AreEqual(Quaternion.identity, state.Target);
            Assert.AreEqual(Vector3.zero, HeldOrientation.Correction(default, Quaternion.identity, Vector3.zero, Vector3.one, Quaternion.identity, .02f, config));
        }
        [Test] public void S019_PhysicalTorqueIsBoundedAndTakesShortestArc()
        {
            var target = Quaternion.Euler(0, 359, 0);
            var torque = HeldOrientation.Correction(Quaternion.identity, target, Vector3.zero, Vector3.one, Quaternion.identity, .02f, config);
            Assert.Less(torque.y, 0); Assert.Less(torque.magnitude, 1);
            var equivalent = new Quaternion(-target.x, -target.y, -target.z, -target.w);
            Assert.Less(Vector3.Distance(torque, HeldOrientation.Correction(Quaternion.identity, equivalent, Vector3.zero, Vector3.one, Quaternion.identity, .02f, config)), .001f);
            torque = HeldOrientation.Correction(Quaternion.identity, Quaternion.Euler(0, 180, 0), Vector3.one * 10000, Vector3.one * 10000, Quaternion.identity, .02f, config);
            Assert.LessOrEqual(torque.magnitude, config.ManipulationTorqueMax + .001f); Assert.IsTrue(SafeMath.Finite(torque));
        }
        [Test] public void S019_InertiaLimitsPreserveHeavyCargoResponse()
        {
            var target = Quaternion.Euler(0, 70, 0);
            var small = HeldOrientation.Correction(Quaternion.identity, target, Vector3.zero, Vector3.one, Quaternion.identity, .02f, config);
            var large = HeldOrientation.Correction(Quaternion.identity, target, Vector3.zero, Vector3.one * 100, Quaternion.identity, .02f, config);
            Assert.Greater(small.y, large.y / 100, "Torque saturation slows high inertia rather than bypassing it");
        }
        [Test] public void S019_ThousandWarmedTargetAndCorrectionIterationsAllocateZero()
        {
            var state = new HeldOrientation(); state.Initialize(Quaternion.identity);
            for (int i = 0; i < 50; i++) Iterate(ref state);
            long start = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < 1000; i++) Iterate(ref state);
            long bytes = GC.GetAllocatedBytesForCurrentThread() - start;
            TestContext.WriteLine("1000 warmed target + correction iterations: " + bytes + " bytes"); Assert.AreEqual(0, bytes);
        }
        void Iterate(ref HeldOrientation state)
        {
            state.Update(true, Vector2.one, Quaternion.identity, Quaternion.identity, .02f, config);
            HeldOrientation.Correction(Quaternion.identity, state.Target, Vector3.zero, Vector3.one, Quaternion.identity, .02f, config);
        }
        [Test] public void S019_InvalidConfigurationRejectsUnsafeLimits()
        {
            var c = new FoundationConfig { ManipulationTorqueMax = 10000 }; Assert.Throws<ArgumentException>(() => c.Validate());
            c.ManipulationTorqueMax = 400; c.ManipulationResponse = float.NaN; Assert.Throws<ArgumentException>(() => c.Validate());
        }
    }
}
