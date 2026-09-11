using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using OnHold.Core;
using OnHold.Domain;
using OnHold.Gameplay;
using OnHold.Presentation;
using Object = UnityEngine.Object;

namespace OnHold.Tests
{
    public sealed class CarryReleaseSafetyTests
    {
        FoundationWorld world;
        PlayerBody player;
        CarryableBody cargo;
        readonly List<GameObject> fixtures = new List<GameObject>();

        [UnitySetUp] public IEnumerator Setup()
        {
            yield return SceneManager.LoadSceneAsync("Assets/_OnHold/Scenes/Bootstrap.unity");
            Object.FindAnyObjectByType<FoundationFrontend>().enabled = false;
            world = Object.FindAnyObjectByType<FoundationWorld>(); world.enabled = false;
            world.StartAuthority(42, SessionMode.Solo);
            world.Session.Join("", true, 0, out var member);
            player = world.AddPlayer(member.Actor);
            Apply(CommandType.Start); Apply(CommandType.Ready); Apply(CommandType.Start); Apply(CommandType.Start);
            Box(new Vector3(30, -.25f, 0), new Vector3(24, .5f, 24));
            player.Respawn(new Vector3(30, .02f, 0));
            cargo = world.Items[0];
            Put(cargo, Quaternion.identity);
        }

        [UnityTearDown] public IEnumerator Teardown()
        {
            world.StopSession();
            foreach (var go in fixtures) if (go) Object.Destroy(go);
            fixtures.Clear();
            yield return null;
        }

        Reason Apply(CommandType type, CarryableBody item = null) =>
            world.Apply(new CommandEnvelope { CommandType = type, TargetId = item ? item.NetworkId : 0 }, player.Actor);

        GameObject Box(Vector3 point, Vector3 size)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.layer = 8; go.transform.position = point; go.transform.localScale = size;
            fixtures.Add(go); Physics.SyncTransforms(); return go;
        }

        void Put(CarryableBody item, Quaternion rotation)
        {
            item.ResetState();
            item.Body.position = player.Eye + Vector3.forward * 1.7f;
            item.Body.rotation = rotation;
            item.SuppressDamageUntil = 1000;
            Physics.SyncTransforms();
        }

        void Grab()
        {
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Grab, cargo));
        }

        void Tick()
        {
            player.Input(new CommandEnvelope(), world.Now);
            world.Step(.02f);
        }

        [Test] public void S020_P01_NormalCarryAndRelease()
        {
            Grab();
            for (int i = 0; i < 20; i++) Tick();
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            Assert.AreEqual(0, cargo.HandleCount);
            Assert.AreEqual(Handling.Free, cargo.State.Handling);
            Assert.IsFalse(cargo.Body.isKinematic);
            cargo.Drive(); Assert.AreEqual(Vector3.zero, cargo.LastForce);
        }

        [Test] public void S020_P02_ManipulateBeforeRelease()
        {
            Grab();
            for (int i = 0; i < 20; i++)
            {
                cargo.UpdateManipulation(player.Actor, true, new Vector2(1, .5f), Quaternion.identity, .02f);
                Tick();
            }
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            Assert.AreEqual(0, cargo.HandleCount);
            // Manipulation torque must stop.
            cargo.Drive(); Assert.AreEqual(Vector3.zero, cargo.LastTorque);
        }

        [Test] public void S020_P03_FloorSupportedRelease()
        {
            Grab();
            // Authored long cargo is .6 m tall; support it at the floor, away from the player capsule.
            cargo.Body.position = new Vector3(30, cargo.Definition.BoundsSize.y * .5f, 1.7f);
            cargo.Body.rotation = Quaternion.identity;
            Physics.SyncTransforms();
            var before = cargo.Body.position;
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            Assert.AreEqual(before, cargo.Body.position, "Ordinary support needs no correction");
        }

        [Test] public void S020_P04_WallContactRelease()
        {
            Grab();
            Box(cargo.Body.position + Vector3.right * (cargo.Definition.BoundsSize.x * .5f + .2f - .01f), new Vector3(.4f, 4, 8));
            var before = cargo.Body.position;
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            Assert.AreEqual(before, cargo.Body.position, "Normal wall contact needs no correction");
        }

        [Test] public void S020_P05_DeepObstructedReleaseBlocked()
        {
            // Encase cargo in thick walls.
            Box(new Vector3(30, 1.5f, 1.7f), new Vector3(4, 4, 4));
            cargo.Body.position = new Vector3(30, 1.5f, 1.7f); // inside the wall
            Physics.SyncTransforms();
            Grab();
            var result = Apply(CommandType.Release);
            // Should be blocked — cargo inside thick geometry.
            Assert.AreEqual(Reason.Blocked, result);
            Assert.AreEqual(1, cargo.HandleCount, "Grip must remain on blocked release");
            Assert.AreEqual(Handling.Held, cargo.State.Handling);
        }

        [Test] public void S020_P06_NearbySafeResolution()
        {
            Grab();
            var before = cargo.Body.position;
            Box(before - Vector3.up * (cargo.Definition.BoundsSize.y * .5f + .05f - .08f), new Vector3(8, .1f, 8));
            Assert.IsFalse(ReleaseSafety.IsPoseSafe(cargo.Body, cargo.Definition.BoundsCenter, cargo.Definition.BoundsSize, before, cargo.Body.rotation, world.Config.ReleasePenetrationTolerance));
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            float displacement = Vector3.Distance(before, cargo.Body.position);
            Assert.AreEqual(.15f, displacement, .00001f);
            Assert.LessOrEqual(displacement, world.Config.SafeReleaseSearchDistance);
            Assert.IsTrue(ReleaseSafety.IsPoseSafe(cargo.Body, cargo.Definition.BoundsCenter, cargo.Definition.BoundsSize, cargo.Body.position, cargo.Body.rotation, world.Config.ReleasePenetrationTolerance));
            TestContext.WriteLine($"Resolved release displacement: {displacement} m");
        }

        [Test] public void S020_P07_NoSafePoseRejection()
        {
            // Fully encase cargo.
            Box(new Vector3(30, 1.5f, 1.7f), new Vector3(6, 6, 6));
            cargo.Body.position = new Vector3(30, 1.5f, 1.7f);
            Physics.SyncTransforms();
            Grab();
            Assert.AreEqual(Reason.Blocked, Apply(CommandType.Release));
            Assert.AreEqual(1, cargo.HandleCount);
        }

        [Test] public void S020_P08_RetryAfterBlockedRelease()
        {
            var enclosure = Box(new Vector3(30, 1.5f, 1.7f), new Vector3(6, 6, 6));
            cargo.Body.position = new Vector3(30, 1.5f, 1.7f);
            Physics.SyncTransforms();
            Grab();
            Assert.AreEqual(Reason.Blocked, Apply(CommandType.Release));
            Assert.AreEqual(1, cargo.HandleCount);
            // Move cargo out.
            enclosure.SetActive(false); // Destroy is deferred until the next frame.
            cargo.Body.position = player.Eye + Vector3.forward * 1.7f;
            Physics.SyncTransforms();
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            Assert.AreEqual(0, cargo.HandleCount);
        }

        GameObject ObstructionWorkload(int steps)
        {
            Grab();
            var barrier = Box(new Vector3(30, 1.5f, .8f), new Vector3(8, 4, .4f));
            float linear = 0, angular = 0, force = 0, torque = 0, depth = 0;
            var collider = cargo.GetComponentInChildren<Collider>();
            var obstacle = barrier.GetComponent<Collider>();
            for (int i = 0; i < steps; i++)
            {
                Tick();
                linear = Mathf.Max(linear, cargo.Body.linearVelocity.magnitude);
                angular = Mathf.Max(angular, cargo.Body.angularVelocity.magnitude);
                force = Mathf.Max(force, cargo.LastForce.magnitude); torque = Mathf.Max(torque, cargo.LastTorque.magnitude);
                if (Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation,
                    obstacle, obstacle.transform.position, obstacle.transform.rotation, out _, out var penetration)) depth = Mathf.Max(depth, penetration);
                Assert.AreEqual(1, cargo.HandleCount, "Workload must stay held");
                Assert.IsTrue(SafeMath.Finite(cargo.Body.position) && SafeMath.Finite(cargo.Body.rotation));
                Assert.IsTrue(SafeMath.Finite(cargo.Body.linearVelocity) && SafeMath.Finite(cargo.Body.angularVelocity));
                Assert.LessOrEqual(linear, world.Config.BodySpeedMax + .001f);
                Assert.LessOrEqual(angular, world.Config.AngularSpeedMax + .001f);
                Assert.LessOrEqual(force, world.Config.TotalForceMax + .001f);
                Assert.LessOrEqual(torque, world.Config.TotalTorqueMax + .001f);
            }
            Assert.Greater(depth, 0, "Workload must exercise real contact");
            Assert.Less(depth, .08f, "Retain the established solver penetration ceiling");
            TestContext.WriteLine($"Obstruction duration={steps * .02f}s linear={linear} angular={angular} force={force} torque={torque} penetration={depth}");
            return barrier;
        }

        [Test] public void S020_P09_ProlongedObstruction() => ObstructionWorkload(500);

        [Test] public void S020_P10_ReleaseAfterProlongedObstruction()
        {
            var barrier = ObstructionWorkload(500);
            barrier.SetActive(false); Physics.SyncTransforms();
            float recoveryPeak = 0;
            for (int i = 0; i < 150; i++) { Tick(); recoveryPeak = Mathf.Max(recoveryPeak, cargo.Body.linearVelocity.magnitude); Assert.AreEqual(1, cargo.HandleCount); }
            Assert.Less(recoveryPeak, world.Config.MaxReleaseLinearSpeed, "Recovery must not launch at the hard velocity ceiling");
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            TestContext.WriteLine($"Recovery peak={recoveryPeak}; release linear={cargo.Body.linearVelocity.magnitude} angular={cargo.Body.angularVelocity.magnitude}");
            Assert.LessOrEqual(cargo.Body.linearVelocity.magnitude, world.Config.MaxReleaseLinearSpeed + .001f);
            Assert.LessOrEqual(cargo.Body.angularVelocity.magnitude, world.Config.MaxReleaseAngularSpeed + .001f);
        }

        [Test] public void S020_P11_RapidManipulationBeforeRelease()
        {
            Grab();
            for (int i = 0; i < 100; i++)
            {
                cargo.UpdateManipulation(player.Actor, true, new Vector2(i % 2 == 0 ? 100 : -100, 50), Quaternion.identity, .02f);
                Tick();
            }
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            Assert.LessOrEqual(cargo.Body.angularVelocity.magnitude, world.Config.MaxReleaseAngularSpeed + .01f);
        }

        [Test] public void S020_P12_ReGrab()
        {
            Grab();
            for (int i = 0; i < 10; i++) Tick();
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            for (int i = 0; i < 10; i++) Tick();
            Grab();
            Assert.IsTrue(cargo.TryHeldOrientation(player.Actor, out var state));
            Assert.Less(Quaternion.Angle(cargo.Body.rotation, state.Target), .1f);
        }

        [UnityTest] public IEnumerator S020_P13_DestroyedHeldCargo()
        {
            Grab();
            var state = cargo.State;
            Object.Destroy(cargo.gameObject); yield return null;
            Assert.AreEqual(Lifecycle.Lost, state.Lifecycle);
            Assert.AreEqual(0, state.Holders.Count);
            Assert.DoesNotThrow(() => world.Step(.02f));
            var frame = world.Capture();
            Assert.AreEqual(world.Items.Length, frame.Items.Length);
        }

        [Test] public void S020_P14_RepeatedBlockedReleaseRequests()
        {
            Box(new Vector3(30, 1.5f, 1.7f), new Vector3(6, 6, 6));
            cargo.Body.position = new Vector3(30, 1.5f, 1.7f);
            Physics.SyncTransforms();
            Grab();
            cargo.UpdateManipulation(player.Actor, true, Vector2.one, Quaternion.identity, .02f);
            cargo.TryHeldOrientation(player.Actor, out var orientation);
            int grip = cargo.HeldGripIndex(player.Actor); uint revision = cargo.State.Revision;
            for (int i = 0; i < 50; i++) Apply(CommandType.Release);
            long before = GC.GetAllocatedBytesForCurrentThread();
            bool allBlocked = true;
            for (int i = 0; i < 1000; i++) allBlocked &= Apply(CommandType.Release) == Reason.Blocked;
            long bytes = GC.GetAllocatedBytesForCurrentThread() - before;
            Assert.IsTrue(allBlocked); Assert.AreEqual(0, bytes);
            Assert.AreEqual(grip, cargo.HeldGripIndex(player.Actor)); Assert.AreEqual(revision, cargo.State.Revision);
            Assert.IsTrue(cargo.TryHeldOrientation(player.Actor, out var after));
            Assert.AreEqual(orientation.Target, after.Target); Assert.IsTrue(after.Active);
            Assert.IsTrue(cargo.UpdateManipulation(player.Actor, true, Vector2.one, Quaternion.identity, .02f));
            TestContext.WriteLine("1000 warmed blocked production release requests: " + bytes + " bytes");
        }

        [Test] public void S020_P15_PlayerControlsRegression()
        {
            Grab();
            for (int i = 0; i < 10; i++) Tick();
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            // Verify player still works.
            player.Input(new CommandEnvelope { X = 1, Z = 0, Yaw = 90, Pitch = 0 }, world.Now);
            player.Drive(world, .02f);
            Assert.IsTrue(SafeMath.Finite(player.transform.position));
            Assert.IsTrue(player.RequestJump());
        }

        [Test] public void S020_InvalidReleaseStatePreservesCommittedGrip()
        {
            Grab(); cargo.Body.isKinematic = true;
            Assert.AreEqual(Reason.InvalidState, Apply(CommandType.Release));
            Assert.AreEqual(1, cargo.HandleCount); Assert.IsTrue(cargo.State.IsHeldBy(player.Actor));
        }

        [TestCase(1f, 1f)] [TestCase(8f, 6f)]
        public void S020_ProductionReleaseVelocityPolicy(float linear, float angular)
        {
            Grab(); cargo.Body.linearVelocity = Vector3.right * linear; cargo.Body.angularVelocity = Vector3.up * angular;
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            Assert.AreEqual(Vector3.right * Mathf.Min(linear, world.Config.MaxReleaseLinearSpeed), cargo.Body.linearVelocity);
            Assert.AreEqual(Vector3.up * Mathf.Min(angular, world.Config.MaxReleaseAngularSpeed), cargo.Body.angularVelocity);
            TestContext.WriteLine($"Velocity input={linear}/{angular}, released={cargo.Body.linearVelocity.magnitude}/{cargo.Body.angularVelocity.magnitude}");
        }

        [Test] public void S020_ThousandWarmedCarryReleaseSafetyOperationsAllocateZero()
        {
            Grab();
            // Warmup.
            for (int i = 0; i < 50; i++)
            {
                ReleaseSafety.Evaluate(cargo, world.Config, out _);
                var l = cargo.Body.linearVelocity; var a = cargo.Body.angularVelocity;
                ReleaseSafety.SanitizeVelocity(ref l, ref a, world.Config);
            }
            long start = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < 1000; i++)
            {
                ReleaseSafety.Evaluate(cargo, world.Config, out _);
                var l = cargo.Body.linearVelocity; var a = cargo.Body.angularVelocity;
                ReleaseSafety.SanitizeVelocity(ref l, ref a, world.Config);
            }
            long bytes = GC.GetAllocatedBytesForCurrentThread() - start;
            TestContext.WriteLine("1000 warmed carry/release safety operations: " + bytes + " bytes");
            Assert.AreEqual(0, bytes);
        }
    }
}
