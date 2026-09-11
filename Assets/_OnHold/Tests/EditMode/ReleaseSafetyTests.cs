using System;
using NUnit.Framework;
using UnityEngine;
using OnHold.Core;
using OnHold.Gameplay;
using Object = UnityEngine.Object;

namespace OnHold.Tests
{
    public sealed class ReleaseSafetyTests
    {
        FoundationConfig config;
        GameObject cargoGo, floor, wall;
        Rigidbody body;
        ItemDefinition def;

        [SetUp] public void Setup()
        {
            config = new FoundationConfig();

            // Create cargo with a Rigidbody — no full world needed for static ReleaseSafety tests.
            cargoGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cargoGo.layer = 10;
            cargoGo.transform.position = new Vector3(0, 5, 0);
            body = cargoGo.AddComponent<Rigidbody>();
            body.useGravity = false;

            def = ScriptableObject.CreateInstance<ItemDefinition>();
            def.DefinitionId = "testcargo";
            def.DisplayNameKey = "testcargo";
            def.MassKg = 10;
            def.BoundsCenter = Vector3.zero;
            def.BoundsSize = Vector3.one;

            Physics.SyncTransforms();
        }

        [TearDown] public void Teardown()
        {
            if (cargoGo) Object.DestroyImmediate(cargoGo);
            if (floor) Object.DestroyImmediate(floor);
            if (wall) Object.DestroyImmediate(wall);
            if (def) Object.DestroyImmediate(def);
        }

        [Test] public void S020_E01_SafeCurrentPoseClassifiedSafe()
        {
            Assert.IsTrue(ReleaseSafety.IsPoseSafe(body, def.BoundsCenter, def.BoundsSize,
                new Vector3(0, 5, 0), Quaternion.identity, config.ReleasePenetrationTolerance));
        }

        [Test] public void S020_E02_BlockingPenetrationClassifiedUnsafe()
        {
            floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.layer = 8;
            floor.transform.position = new Vector3(0, 5, 0);
            floor.transform.localScale = new Vector3(10, 1, 10);
            Physics.SyncTransforms();
            Assert.IsFalse(ReleaseSafety.IsPoseSafe(body, def.BoundsCenter, def.BoundsSize,
                new Vector3(0, 5, 0), Quaternion.identity, config.ReleasePenetrationTolerance));
        }

        [Test] public void S020_E03_OrdinarySupportContactRemainsSafe()
        {
            floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.layer = 8;
            floor.transform.position = new Vector3(0, 4f, 0);
            floor.transform.localScale = new Vector3(10, 1, 10);
            Physics.SyncTransforms();
            Assert.IsTrue(ReleaseSafety.IsPoseSafe(body, def.BoundsCenter, def.BoundsSize,
                new Vector3(0, 5, 0), Quaternion.identity, config.ReleasePenetrationTolerance));
        }

        [Test] public void S020_E04_SelfColliderFilteredOut()
        {
            Assert.IsTrue(ReleaseSafety.IsPoseSafe(body, def.BoundsCenter, def.BoundsSize,
                new Vector3(0, 5, 0), Quaternion.identity, config.ReleasePenetrationTolerance));
        }

        [Test] public void S020_E05_TriggerDoesNotBlockRelease()
        {
            var trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trigger.layer = 8;
            trigger.transform.position = new Vector3(0, 5, 0);
            trigger.GetComponent<Collider>().isTrigger = true;
            Physics.SyncTransforms();
            Assert.IsTrue(ReleaseSafety.IsPoseSafe(body, def.BoundsCenter, def.BoundsSize,
                new Vector3(0, 5, 0), Quaternion.identity, config.ReleasePenetrationTolerance));
            Object.DestroyImmediate(trigger);
        }

        [Test] public void S020_E06_DeterministicCandidateOrdering()
        {
            floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.layer = 8; floor.transform.position = new Vector3(0, 4.45f, 0);
            floor.transform.localScale = new Vector3(10, .2f, 10);
            Physics.SyncTransforms();
            Assert.IsFalse(ReleaseSafety.IsPoseSafe(body, Vector3.zero, Vector3.one, body.position, body.rotation, config.ReleasePenetrationTolerance));
            Assert.AreEqual(ReleaseResult.Released, ReleaseSafety.Evaluate(body, Vector3.zero, Vector3.one, config, out var first));
            Assert.AreEqual(new Vector3(0, 5.15f, 0), first, "First safe candidate in fixed order");
            for (int i = 0; i < 20; i++)
            {
                Assert.AreEqual(ReleaseResult.Released, ReleaseSafety.Evaluate(body, Vector3.zero, Vector3.one, config, out var next));
                Assert.AreEqual(first, next);
            }
        }

        [Test] public void S020_E07_BoundedSearchNeverExceedsConfiguredDistance()
        {
            floor = GameObject.CreatePrimitive(PrimitiveType.Cube); floor.layer = 8;
            floor.transform.position = new Vector3(0, 4.5f, 0); floor.transform.localScale = new Vector3(10, .2f, 10);
            Physics.SyncTransforms();
            foreach (float radius in new[] { .1f, .25f, .5f })
            {
                config.SafeReleaseSearchDistance = radius;
                var origin = body.position;
                var result = ReleaseSafety.Evaluate(body, Vector3.zero, Vector3.one, config, out var safe);
                Assert.LessOrEqual(Vector3.Distance(origin, safe), radius);
                Assert.AreEqual(origin, body.position, "Evaluation must not move the body");
                if (result == ReleaseResult.Released)
                    Assert.IsTrue(ReleaseSafety.IsPoseSafe(body, Vector3.zero, Vector3.one, safe, body.rotation, config.ReleasePenetrationTolerance));
                else Assert.AreEqual(ReleaseResult.NoSafePose, result);
            }
        }

        [Test] public void S020_E08_FullyEnclosedPoseIsUnsafe()
        {
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube); wall.layer = 8;
            wall.transform.position = body.position; wall.transform.localScale = Vector3.one * 10;
            Physics.SyncTransforms();
            Assert.AreEqual(ReleaseResult.NoSafePose, ReleaseSafety.Evaluate(body, Vector3.zero, Vector3.one, config, out var safe));
            Assert.AreEqual(body.position, safe);
        }

        [Test] public void S020_E09_LinearVelocityBound()
        {
            var linear = new Vector3(100, 0, 0);
            var angular = Vector3.zero;
            ReleaseSafety.SanitizeVelocity(ref linear, ref angular, config);
            Assert.LessOrEqual(linear.magnitude, config.MaxReleaseLinearSpeed + .001f);
        }

        [Test] public void S020_E10_AngularVelocityBound()
        {
            var linear = Vector3.zero;
            var angular = new Vector3(0, 50, 0);
            ReleaseSafety.SanitizeVelocity(ref linear, ref angular, config);
            Assert.LessOrEqual(angular.magnitude, config.MaxReleaseAngularSpeed + .001f);
        }

        [Test] public void S020_E11_ReasonableVelocityPreserved()
        {
            var linear = new Vector3(1, 0, 0);
            var angular = new Vector3(0, 1, 0);
            ReleaseSafety.SanitizeVelocity(ref linear, ref angular, config);
            Assert.AreEqual(1, linear.x, .001f);
            Assert.AreEqual(1, angular.y, .001f);
        }

        [Test] public void S020_E12_NaNInfinityHandledSafely()
        {
            var linear = new Vector3(float.NaN, 0, 0);
            var angular = new Vector3(0, float.PositiveInfinity, 0);
            Assert.IsFalse(ReleaseSafety.SanitizeVelocity(ref linear, ref angular, config));
            Assert.AreEqual(Vector3.zero, linear);
            Assert.AreEqual(Vector3.zero, angular);
        }

        [Test] public void S020_E13_InvalidStateDoesNotMoveBody()
        {
            var origin = body.position;
            Assert.AreEqual(ReleaseResult.InvalidState, ReleaseSafety.Evaluate(body, Vector3.zero, new Vector3(float.NaN, 1, 1), config, out _));
            Assert.AreEqual(origin, body.position);
        }

        [TestCase(8)] [TestCase(9)] [TestCase(10)] [TestCase(11)]
        public void S020_AllSolidGameplayLayersBlock(int layer)
        {
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube); wall.layer = layer;
            wall.transform.position = body.position; Physics.SyncTransforms();
            Assert.IsFalse(ReleaseSafety.IsPoseSafe(body, Vector3.zero, Vector3.one, body.position, body.rotation, config.ReleasePenetrationTolerance));
        }

        [Test] public void S020_InvalidNumericPoseRejectedBeforePhysicsQuery()
        {
            foreach (float bad in new[] { float.NaN, float.PositiveInfinity, float.NegativeInfinity })
            {
                Assert.IsFalse(ReleaseSafety.IsPoseSafe(body, Vector3.zero, Vector3.one, new Vector3(bad, 5, 0), Quaternion.identity, .03f));
                Assert.IsFalse(ReleaseSafety.IsPoseSafe(body, Vector3.zero, Vector3.one, body.position, new Quaternion(0, 0, 0, bad), .03f));
                Assert.IsFalse(ReleaseSafety.IsPoseSafe(body, Vector3.zero, Vector3.one, body.position, Quaternion.identity, bad));
            }
            Assert.IsFalse(ReleaseSafety.IsPoseSafe(body, Vector3.zero, Vector3.one, body.position, default, .03f));
        }

        [Test] public void S020_FullQueryBufferIsConservativelyUnsafe()
        {
            wall = new GameObject("Dense self colliders"); wall.transform.SetParent(cargoGo.transform, false);
            for (int i = 0; i < 40; i++) wall.AddComponent<BoxCollider>();
            wall.layer = 10; Physics.SyncTransforms();
            Assert.IsFalse(ReleaseSafety.IsPoseSafe(body, Vector3.zero, Vector3.one, body.position, body.rotation, .03f));
            Assert.AreEqual(ReleaseResult.NoSafePose, ReleaseSafety.Evaluate(body, Vector3.zero, Vector3.one, config, out _));
        }

        [TestCase(.02f, true)] [TestCase(.04f, false)]
        public void S020_PenetrationToleranceIsDistancePerFace(float depth, bool safe)
        {
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube); wall.layer = 8;
            wall.transform.position = new Vector3(1.5f - depth, 5, 0); Physics.SyncTransforms();
            Assert.AreEqual(safe, ReleaseSafety.IsPoseSafe(body, Vector3.zero, new Vector3(2, .6f, .7f), body.position, body.rotation, .03f));
        }

        [Test] public void S020_ConfigurationRejectsInvalidAndExcessiveLimits()
        {
            config.SafeReleaseSearchDistance = 2; Assert.Throws<ArgumentException>(() => config.Validate());
            config = new FoundationConfig { ReleasePenetrationTolerance = float.NaN }; Assert.Throws<ArgumentException>(() => config.Validate());
            config = new FoundationConfig { MaxReleaseLinearSpeed = 9 }; Assert.Throws<ArgumentException>(() => config.Validate());
            config = new FoundationConfig { MaxReleaseAngularSpeed = 7 }; Assert.Throws<ArgumentException>(() => config.Validate());
        }

        [Test] public void S020_E14_WarmedReleaseSafetyChecksAllocateZero()
        {
            // Warm up.
            for (int i = 0; i < 50; i++)
            {
                ReleaseSafety.IsPoseSafe(body, def.BoundsCenter, def.BoundsSize,
                    new Vector3(0, 5, 0), Quaternion.identity, config.ReleasePenetrationTolerance);
                var l = Vector3.one; var a = Vector3.one;
                ReleaseSafety.SanitizeVelocity(ref l, ref a, config);
            }
            long start = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < 1000; i++)
            {
                ReleaseSafety.IsPoseSafe(body, def.BoundsCenter, def.BoundsSize,
                    new Vector3(0, 5, 0), Quaternion.identity, config.ReleasePenetrationTolerance);
                var l = new Vector3(100, 0, 0); var a = new Vector3(0, 50, 0);
                ReleaseSafety.SanitizeVelocity(ref l, ref a, config);
            }
            long bytes = GC.GetAllocatedBytesForCurrentThread() - start;
            TestContext.WriteLine("1000 warmed release safety checks: " + bytes + " bytes");
            Assert.AreEqual(0, bytes);
        }
    }
}
