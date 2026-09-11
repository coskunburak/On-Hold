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
    public sealed class PlayablePhysicsTests
    {
        FoundationWorld world;
        PlayerBody player;
        CarryableBody cargo;
        readonly List<GameObject> fixtures = new List<GameObject>();
        static readonly Vector3 Origin = new Vector3(30, .02f, 0);
        [UnitySetUp] public IEnumerator Setup()
        {
            yield return SceneManager.LoadSceneAsync("Assets/_OnHold/Scenes/Bootstrap.unity");
            Object.FindAnyObjectByType<FoundationFrontend>().enabled = false;
            world = Object.FindAnyObjectByType<FoundationWorld>(); world.enabled = false;
            world.StartAuthority(42, SessionMode.Solo); world.Session.Join("", true, 0, out var member); player = world.AddPlayer(member.Actor);
            Apply(CommandType.Start); Apply(CommandType.Ready); Apply(CommandType.Start); Apply(CommandType.Start);
            Box("Physics test floor", new Vector3(30, -.25f, 0), new Vector3(24, .5f, 24));
            player.Respawn(Origin); cargo = world.Items[0]; Tick(10);
        }
        [UnityTearDown] public IEnumerator Teardown()
        {
            world.StopSession(); foreach (var go in fixtures) if (go) Object.Destroy(go); fixtures.Clear(); yield return null;
        }
        Reason Apply(CommandType type, CarryableBody target = null) => world.Apply(new CommandEnvelope { CommandType = type, TargetId = target ? target.NetworkId : 0 }, player.Actor);
        GameObject Box(string name, Vector3 position, Vector3 size, int layer = 8)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube); go.name = name; go.layer = layer;
            go.transform.position = position; go.transform.localScale = size; fixtures.Add(go); Physics.SyncTransforms(); return go;
        }
        void Put(CarryableBody item, Vector3 position)
        {
            item.ResetState(); item.Body.position = position; item.Body.rotation = Quaternion.identity; Physics.SyncTransforms();
        }
        void Tick(int count, Vector2 movement = default, float yaw = 0, float pitch = 0)
        {
            for (int i = 0; i < count; i++)
            {
                player.Input(new CommandEnvelope { X = movement.x, Z = movement.y, Yaw = yaw, Pitch = pitch }, world.Now); world.Step(.02f);
                Assert.IsTrue(SafeMath.Finite(player.transform.position));
                foreach (var item in world.Items)
                {
                    Assert.IsTrue(SafeMath.Finite(item.Body.position)); Assert.IsTrue(SafeMath.Finite(item.Body.rotation));
                    Assert.IsTrue(SafeMath.Finite(item.Body.linearVelocity)); Assert.IsTrue(SafeMath.Finite(item.Body.angularVelocity));
                    Assert.LessOrEqual(item.LastForce.magnitude, world.Config.TotalForceMax + .1f);
                    Assert.LessOrEqual(item.LastTorque.magnitude, world.Config.TotalTorqueMax + .1f);
                    Assert.LessOrEqual(item.Body.linearVelocity.magnitude, world.Config.BodySpeedMax + .1f);
                    Assert.LessOrEqual(item.Body.angularVelocity.magnitude, world.Config.AngularSpeedMax + .1f);
                }
            }
        }
        [Test] public void S012_RayNearestRangeWallLayersTriggersAndNoHotPathAllocation()
        {
            Put(cargo, new Vector3(30, player.Eye.y, 2));
            var result = InteractionRay.Detect(player.Eye, Vector3.forward, world.Config); Assert.AreSame(cargo, result.Item); Assert.Greater(result.Hit.distance, 0);
            Put(world.Items[1], new Vector3(30, player.Eye.y, 1)); Assert.AreSame(world.Items[1], InteractionRay.Detect(player.Eye, Vector3.forward, world.Config).Item);
            Put(world.Items[1], new Vector3(36, 1, 0));
            var block = Box("Occluder", new Vector3(30, player.Eye.y, 1), Vector3.one * .3f);
            Assert.IsFalse(InteractionRay.Detect(player.Eye, Vector3.forward, world.Config).Valid);
            Assert.AreEqual(Reason.TooFar, Apply(CommandType.Grab, cargo), "Authority must also reject occluded item");
            block.layer = 13; Assert.IsTrue(InteractionRay.Detect(player.Eye, Vector3.forward, world.Config).Valid);
            block.layer = 8; block.GetComponent<Collider>().isTrigger = true; Physics.SyncTransforms();
            Assert.IsTrue(InteractionRay.Detect(player.Eye, Vector3.forward, world.Config).Valid);
            world.Config.InteractionTriggers = 2; Assert.IsFalse(InteractionRay.Detect(player.Eye, Vector3.forward, world.Config).Valid); world.Config.InteractionTriggers = 1;
            for (int i = 0; i < 50; i++) InteractionRay.Detect(player.Eye, Vector3.forward, world.Config);
            long before = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < 1000; i++) InteractionRay.Detect(player.Eye, Vector3.forward, world.Config);
            Assert.LessOrEqual(GC.GetAllocatedBytesForCurrentThread() - before, 128, "1000 warmed ray queries");
            Put(cargo, new Vector3(30, player.Eye.y, 5)); Assert.IsFalse(InteractionRay.Detect(player.Eye, Vector3.forward, world.Config).Valid);
            Assert.IsFalse(InteractionRay.Detect(new Vector3(float.NaN, 0, 0), Vector3.forward, world.Config).Valid);
        }
        [Test] public void S013_PushIsMassDependentBoundedAndIgnoresKinematicBodies()
        {
            var lightGo = Box("Light body", new Vector3(30, .35f, 1.3f), Vector3.one * .7f, 10);
            var light = lightGo.AddComponent<Rigidbody>(); light.mass = 5; light.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            Tick(35, Vector2.up); float lightTravel = light.position.z - 1.3f; Assert.Greater(lightTravel, .2f); Assert.Less(light.linearVelocity.magnitude, 4);
            lightGo.SetActive(false); player.Respawn(Origin); Tick(10); Put(cargo, new Vector3(30, .35f, 1.3f));
            for (int j = 0; j < 35; j++) { Tick(1, Vector2.up); if (j % 5 == 0) TestContext.WriteLine($"heavy tick={j}, p={player.transform.position}, c={cargo.Body.position}, speed={cargo.Body.linearVelocity}, push={player.LastPushForce}"); }
            float heavyTravel = cargo.Body.position.z - 1.3f;
            Assert.Greater(heavyTravel, .01f); Assert.Less(heavyTravel, lightTravel, $"heavy={heavyTravel}, light={lightTravel}");
            Assert.Less(player.Velocity.magnitude, 4);
            cargo.Body.isKinematic = true; var pos = cargo.Body.position; Tick(30, Vector2.up); Assert.Less(Vector3.Distance(pos, cargo.Body.position), .001f);
        }
        [Test] public void S014_EligibleGrabDuplicateWrongActorDistanceAndTerminalRules()
        {
            Put(cargo, new Vector3(30, .4f, 1.6f));
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Grab, cargo)); Assert.AreEqual(1, cargo.HandleCount); Assert.AreEqual(Handling.Held, cargo.State.Handling);
            Assert.AreEqual(Reason.AlreadyApplied, Apply(CommandType.Grab, cargo)); Assert.AreEqual(1, cargo.HandleCount);
            Assert.AreEqual(Reason.AuthFailed, world.Apply(new CommandEnvelope { CommandType = CommandType.Grab, TargetId = cargo.NetworkId }, new Actor(player.Actor.Id, 99)));
            Put(world.Items[1], new Vector3(31, .4f, 1)); Assert.AreEqual(Reason.Busy, Apply(CommandType.Grab, world.Items[1]));
            Apply(CommandType.Release); Put(cargo, new Vector3(30, .4f, 8)); Assert.AreEqual(Reason.TooFar, Apply(CommandType.Grab, cargo));
            Put(cargo, new Vector3(30, .4f, 1.6f)); cargo.State.MarkLost(); Assert.AreEqual(Reason.InvalidState, Apply(CommandType.Grab, cargo)); Assert.AreEqual(0, cargo.HandleCount);
        }
        [Test] public void S015_TwentyPhysicalGrabReleaseCyclesLeaveNoAttachments()
        {
            for (int i = 0; i < 20; i++)
            {
                Put(cargo, new Vector3(30, .4f, 1.5f)); Assert.AreEqual(Reason.Accepted, Apply(CommandType.Grab, cargo));
                Tick(12); Assert.AreEqual(1, cargo.HandleCount); var releasePosition = cargo.Body.position; var releaseVelocity = cargo.Body.linearVelocity;
                Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
                Assert.AreEqual(releasePosition, cargo.Body.position); Assert.AreEqual(releaseVelocity, cargo.Body.linearVelocity); Tick(5);
                Assert.AreEqual(Handling.Free, cargo.State.Handling); Assert.AreEqual(0, cargo.State.Holders.Count); Assert.AreEqual(0, cargo.HandleCount);
                Assert.IsNull(cargo.GetComponent<Joint>()); Assert.IsFalse(cargo.Body.isKinematic); Assert.AreEqual(Lifecycle.Available, cargo.State.Lifecycle);
                Assert.AreEqual(Vector3.zero, cargo.LastForce);
            }
        }
        [Test] public void S016_DistanceBreakAndInvalidForceInputCleanlyRelease()
        {
            Put(cargo, new Vector3(30, .4f, 1.5f)); Assert.AreEqual(Reason.Accepted, Apply(CommandType.Grab, cargo));
            player.Respawn(Origin + Vector3.left * 6); Tick(1); Assert.AreEqual(0, cargo.HandleCount); Assert.AreEqual(Handling.Free, cargo.State.Handling);
            Assert.IsFalse(CarryableBody.TryGripForce(world.Config, new Vector3(float.NaN, 0, 0), Vector3.zero, Vector3.zero, 100, out var f)); Assert.AreEqual(Vector3.zero, f);
            Assert.IsFalse(CarryableBody.TryGripForce(world.Config, Vector3.one, Vector3.positiveInfinity, Vector3.zero, 100, out f));
            Assert.IsTrue(CarryableBody.TryGripForce(world.Config, Vector3.one * 1e6f, Vector3.zero, Vector3.zero, 100, out f)); Assert.LessOrEqual(f.magnitude, world.Config.HandForceMax + .01f);
        }
        [Test] public void S016_TwelveLongCargoWalkStrafeTurnDoorwayJumpReverseReleaseCycles()
        {
            Box("Carry door left", new Vector3(28.8f, 1.5f, 4), new Vector3(.4f, 3, .4f));
            Box("Carry door right", new Vector3(31.2f, 1.5f, 4), new Vector3(.4f, 3, .4f));
            Box("Carry wall", new Vector3(34, 1.5f, 2), new Vector3(.3f, 3, 8));
            for (int cycle = 0; cycle < 12; cycle++)
            {
                Apply(CommandType.Release); player.Respawn(Origin); Tick(10); Put(cargo, new Vector3(30, .4f, 1.5f)); Put(world.Items[1], new Vector3(32, .4f, 3));
                Assert.AreEqual(Reason.Accepted, Apply(CommandType.Grab, cargo)); var start = cargo.Body.position;
                Tick(35); Assert.Greater(cargo.Body.position.y, .5f, "Carry must lift the long cargo");
                Tick(35, Vector2.up); Tick(15, Vector2.right);
                for (int i = 0; i < 20; i++) Tick(1, Vector2.up, i * 4.5f);
                Tick(20, Vector2.up, 90); Tick(20, Vector2.down, 90);
                if (player.Grounded) { Assert.IsTrue(player.RequestJump()); Tick(35, yaw: 90); }
                Tick(25, Vector2.left, 0); Tick(30, Vector2.up); Tick(25, Vector2.down);
                Assert.Greater(Vector3.Distance(start, cargo.Body.position), .5f); Assert.AreEqual(Lifecycle.Available, cargo.State.Lifecycle, "Ordinary carry must not destroy cargo");
                Assert.AreEqual(1, cargo.HandleCount, "Ordinary movement must retain grip");
                Apply(CommandType.Release); Tick(40); Assert.AreEqual(0, cargo.HandleCount); Assert.AreEqual(0, cargo.State.Holders.Count); Assert.IsFalse(cargo.Body.isKinematic);
                TestContext.WriteLine($"cycle={cycle + 1}, cargo={cargo.Body.position}, integrity={cargo.State.Integrity:F1}, speed={cargo.Body.linearVelocity.magnitude:F3}");
            }
        }
    }
}
