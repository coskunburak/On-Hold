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
    public sealed class HeldManipulationTests
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
            world.StartAuthority(42, SessionMode.Solo); world.Session.Join("", true, 0, out var member); player = world.AddPlayer(member.Actor);
            Apply(CommandType.Start); Apply(CommandType.Ready); Apply(CommandType.Start); Apply(CommandType.Start);
            Box(new Vector3(30, -.25f, 0), new Vector3(24, .5f, 24));
            player.Respawn(new Vector3(30, .02f, 0)); cargo = world.Items[0];
            Put(cargo, Quaternion.identity);
        }
        [UnityTearDown] public IEnumerator Teardown()
        { world.StopSession(); foreach (var go in fixtures) if (go) Object.Destroy(go); fixtures.Clear(); yield return null; }
        Reason Apply(CommandType type, CarryableBody item = null) => world.Apply(new CommandEnvelope { CommandType = type, TargetId = item ? item.NetworkId : 0 }, player.Actor);
        GameObject Box(Vector3 point, Vector3 size)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube); go.layer = 8; go.transform.position = point; go.transform.localScale = size;
            fixtures.Add(go); Physics.SyncTransforms(); return go;
        }
        void Put(CarryableBody item, Quaternion rotation)
        {
            item.ResetState(); item.Body.position = player.Eye + Vector3.forward * 1.7f; item.Body.rotation = rotation;
            item.SuppressDamageUntil = 1000; Physics.SyncTransforms();
        }
        void Grab()
        {
            var preview = world.QueryGrabCandidate(player.Actor, cargo); Assert.IsTrue(preview.Success);
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Grab, cargo)); Assert.AreEqual(preview.GripIndex, cargo.HeldGripIndex(player.Actor));
            Assert.IsTrue(cargo.TryHeldOrientation(player.Actor, out var state)); Assert.Less(Quaternion.Angle(cargo.Body.rotation, state.Target), .01f);
        }
        void Input(Vector2 degrees)
        { Assert.IsTrue(cargo.UpdateManipulation(player.Actor, true, degrees, Quaternion.identity, .02f)); }
        void Tick()
        {
            player.Input(new CommandEnvelope(), world.Now); world.Step(.02f);
            Assert.IsTrue(SafeMath.Finite(cargo.Body.position)); Assert.IsTrue(SafeMath.Finite(cargo.Body.rotation));
            Assert.IsTrue(SafeMath.Finite(cargo.Body.angularVelocity));
            Assert.LessOrEqual(cargo.LastTorque.magnitude, world.Config.TotalTorqueMax + .01f);
            Assert.LessOrEqual(cargo.LastManipulationTorque.magnitude, world.Config.ManipulationTorqueMax + .01f);
            Assert.LessOrEqual(cargo.Body.angularVelocity.magnitude, world.Config.AngularSpeedMax + .01f);
            Assert.Less(Vector3.Distance(cargo.Body.position, player.Eye), 6);
        }
        [Test] public void S019_GrabYawPitchPreservesCommittedGripAndDynamicBody()
        {
            Grab(); int index = cargo.HeldGripIndex(player.Actor); var rotation = cargo.Body.rotation; var parent = cargo.transform.parent;
            for (int i = 0; i < 50; i++) { Input(new Vector2(1.5f, 0)); Tick(); Assert.AreEqual(index, cargo.HeldGripIndex(player.Actor)); }
            Assert.Greater(Quaternion.Angle(rotation, cargo.Body.rotation), 15); rotation = cargo.Body.rotation;
            for (int i = 0; i < 50; i++) { Input(new Vector2(0, 1.5f)); Tick(); }
            Assert.Greater(Quaternion.Angle(rotation, cargo.Body.rotation), 15);
            Assert.AreEqual(index, cargo.HeldGripIndex(player.Actor)); Assert.AreSame(parent, cargo.transform.parent); Assert.IsFalse(cargo.Body.isKinematic);
            foreach (var collider in cargo.GetComponentsInChildren<Collider>()) Assert.IsTrue(collider.enabled);
        }
        [Test] public void S019_ReleaseStopsTorqueAndRegrabInitializesFromOtherObject()
        {
            Grab(); for (int i = 0; i < 30; i++) { Input(Vector2.one); Tick(); }
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
            Assert.IsFalse(cargo.TryHeldOrientation(player.Actor, out _)); Assert.AreEqual(0, cargo.HandleCount);
            var velocity = cargo.Body.angularVelocity; cargo.Drive(); Assert.AreEqual(Vector3.zero, cargo.LastTorque);
            Assert.AreEqual(velocity, cargo.Body.angularVelocity);
            Assert.IsFalse(cargo.UpdateManipulation(player.Actor, true, Vector2.one, Quaternion.identity, .02f));
            cargo.Body.position += Vector3.right * 8; cargo = world.Items[1]; var own = Quaternion.Euler(15, -33, 12); Put(cargo, own); Grab();
            Assert.IsTrue(cargo.TryHeldOrientation(player.Actor, out var state)); Assert.IsFalse(state.Engaged); Assert.Less(Quaternion.Angle(own, state.Target), .01f);
        }
        [Test] public void S019_ModifierExitRetainsTargetAndResetClearsAllState()
        {
            Grab(); Input(Vector2.one); cargo.TryHeldOrientation(player.Actor, out var before);
            cargo.EndManipulation(player.Actor); cargo.TryHeldOrientation(player.Actor, out var after);
            Assert.IsFalse(after.Active); Assert.IsTrue(after.Engaged); Assert.AreEqual(before.Target, after.Target);
            cargo.ResetState(); Assert.AreEqual(0, cargo.HandleCount); Assert.IsFalse(cargo.TryHeldOrientation(player.Actor, out _));
        }
        [Test] public void S019_DisableCargoAndInvalidGripReleaseWithoutStaleControl()
        {
            Grab(); Input(Vector2.one); cargo.enabled = false; Assert.AreEqual(0, cargo.HandleCount);
            Assert.IsFalse(cargo.CanManipulate(player.Actor)); cargo.Drive(); Assert.AreEqual(Vector3.zero, cargo.LastTorque);
            cargo.enabled = true; Grab();
            var original = cargo.Definition; var clone = Object.Instantiate(original); cargo.Definition = clone;
            try { clone.Grips = Array.Empty<Vector3>(); cargo.Drive(); Assert.AreEqual(0, cargo.HandleCount); Assert.IsFalse(cargo.CanManipulate(player.Actor)); }
            finally { cargo.Definition = original; Object.Destroy(clone); }
        }
        [Test] public void S019_DisabledPlayerCannotRetainManipulation()
        {
            Grab(); Input(Vector2.one); player.enabled = false; Assert.IsFalse(cargo.CanManipulate(player.Actor));
            cargo.Drive(); Assert.AreEqual(0, cargo.HandleCount); Assert.AreEqual(Vector3.zero, cargo.LastTorque);
        }
        [Test] public void S019_KinematicInvalidationAndWrongActorCannotControlCargo()
        {
            Grab(); Input(Vector2.one);
            Assert.IsFalse(cargo.UpdateManipulation(new Actor(player.Actor.Id, 999), true, Vector2.one, Quaternion.identity, .02f));
            cargo.Body.isKinematic = true; Assert.IsFalse(cargo.CanManipulate(player.Actor));
            cargo.Drive(); Assert.AreEqual(0, cargo.HandleCount); Assert.AreEqual(Vector3.zero, cargo.LastTorque);
        }
        [UnityTest] public IEnumerator S019_DestroyHeldCargoWhileWorldContinues()
        {
            Grab(); Input(Vector2.one); var state = cargo.State; Object.Destroy(cargo.gameObject); yield return null;
            Assert.AreEqual(0, state.Holders.Count); Assert.AreEqual(Lifecycle.Lost, state.Lifecycle);
            Assert.DoesNotThrow(() => world.Step(.02f));
            var frame = world.Capture(); Assert.AreEqual(world.Items.Length, frame.Items.Length);
            Assert.AreEqual(cargo.NetworkId, frame.Items[0].Id); Assert.AreEqual((int)Lifecycle.Lost, frame.Items[0].Lifecycle);
            Assert.AreEqual(0, world.ActiveHandles); Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release));
        }
        [TestCase(0, 100f)] [TestCase(1, 35f)] [TestCase(1, 5f)] [TestCase(0, 250f)]
        public void S019_ProlongedAlternatingInputRemainsBoundedAcrossMassAndSize(int index, float mass)
        {
            cargo.Body.position += Vector3.right * 8; cargo = world.Items[index]; Put(cargo, Quaternion.identity); cargo.Body.mass = mass; Grab();
            float peak = 0, torque = 0;
            for (int i = 0; i < 1000; i++)
            {
                Input(new Vector2(i % 40 < 20 ? float.MaxValue : -float.MaxValue, i % 60 < 30 ? 5000 : -5000)); Tick();
                peak = Mathf.Max(peak, cargo.Body.angularVelocity.magnitude); torque = Mathf.Max(torque, cargo.LastTorque.magnitude);
                Assert.AreEqual(1, cargo.HandleCount, "Workload must remain held throughout");
            }
            TestContext.WriteLine($"mass={mass} size={cargo.Definition.BoundsSize} 1000 steps peak angular={peak} rad/s torque={torque} Nm");
        }
        [TestCase(false)] [TestCase(true)]
        public void S019_WallAndFloorConstrainRotationThroughSolver(bool floor)
        {
            Grab();
            // A nearby surface intersects the desired swept volume, but not the starting pose.
            var obstacle = floor ? Box(new Vector3(30, .92f, 1.7f), new Vector3(8, .5f, 8)) :
                Box(new Vector3(30, 1.6f, 2.45f), new Vector3(8, 5, .4f));
            var colliders = cargo.GetComponentsInChildren<Collider>(); var barrier = obstacle.GetComponent<Collider>();
            float maxDepth = 0, maxError = 0; bool contact = false;
            for (int i = 0; i < 250; i++)
            {
                Input(floor ? new Vector2(0, 2) : new Vector2(2, 0));
                var previous = cargo.Body.rotation; Tick();
                Assert.Less(Quaternion.Angle(previous, cargo.Body.rotation), world.Config.AngularSpeedMax * Mathf.Rad2Deg * .02f + 3);
                foreach (var collider in colliders)
                    if (Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation, barrier, obstacle.transform.position,
                        obstacle.transform.rotation, out _, out float depth)) { maxDepth = Mathf.Max(maxDepth, depth); contact = true; }
                cargo.TryHeldOrientation(player.Actor, out var state); maxError = Mathf.Max(maxError, Quaternion.Angle(state.Target, cargo.Body.rotation));
                Assert.AreEqual(1, cargo.HandleCount);
            }
            TestContext.WriteLine($"floor={floor} contact={contact} max penetration={maxDepth} max orientation error={maxError}");
            Assert.IsTrue(contact, "Exercise actual contact"); Assert.Less(maxDepth, .08f, "Normal solver tolerance, never force target penetration");
            Assert.Greater(maxError, 10, "Requested and achieved orientation may differ");
        }
        [Test] public void S019_ThousandWarmedProductionInputQueryDriveIterationsAllocateZero()
        {
            Grab();
            for (int i = 0; i < 50; i++) Iterate();
            long start = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < 1000; i++) Iterate();
            long bytes = GC.GetAllocatedBytesForCurrentThread() - start;
            TestContext.WriteLine("1000 warmed production manipulation input + query + Drive iterations: " + bytes + " bytes"); Assert.AreEqual(0, bytes);
        }
        void Iterate()
        {
            cargo.UpdateManipulation(player.Actor, true, Vector2.one, Quaternion.identity, .02f);
            cargo.TryHeldOrientation(player.Actor, out _); cargo.Drive(.02f);
        }
    }
}
