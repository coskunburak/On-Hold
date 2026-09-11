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

namespace OnHold.Tests
{
    public sealed class PlayableMotorTests
    {
        FoundationWorld world;
        PlayerBody player;
        readonly List<GameObject> fixtures = new List<GameObject>();
        static readonly Vector3 Origin = new Vector3(30, .02f, 0);
        [UnitySetUp] public IEnumerator Setup()
        {
            yield return SceneManager.LoadSceneAsync("Assets/_OnHold/Scenes/Bootstrap.unity");
            Object.FindAnyObjectByType<FoundationFrontend>().enabled = false;
            world = Object.FindAnyObjectByType<FoundationWorld>(); world.enabled = false;
            world.StartAuthority(42, SessionMode.Solo);
            world.Session.Join("", true, 0, out var member); player = world.AddPlayer(member.Actor);
            world.Apply(new CommandEnvelope { CommandType = CommandType.Start }, player.Actor);
            world.Apply(new CommandEnvelope { CommandType = CommandType.Ready }, player.Actor);
            world.Apply(new CommandEnvelope { CommandType = CommandType.Start }, player.Actor);
            world.Apply(new CommandEnvelope { CommandType = CommandType.Start }, player.Actor);
            Box("Test floor", new Vector3(30, -.25f, 0), new Vector3(24, .5f, 24));
            player.Respawn(Origin); Tick(10);
        }
        [UnityTearDown] public IEnumerator Teardown()
        {
            world.StopSession(); foreach (var go in fixtures) if (go) Object.Destroy(go); fixtures.Clear(); yield return null;
        }
        GameObject Box(string name, Vector3 position, Vector3 size)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube); go.name = name; go.layer = 8;
            go.transform.position = position; go.transform.localScale = size; fixtures.Add(go); Physics.SyncTransforms(); return go;
        }
        void Tick(int count, Vector2 input = default, float yaw = 0, float dt = .02f)
        {
            for (int i = 0; i < count; i++)
            {
                player.Input(new CommandEnvelope { X = input.x, Z = input.y, Yaw = yaw }, world.Now);
                world.Step(dt);
                Assert.IsTrue(SafeMath.Finite(player.transform.position)); Assert.IsTrue(SafeMath.Finite(player.Velocity));
            }
        }
        [Test] public void S005_DuplicateSpawnCallbackIsIdempotentAndAuthoredSpawnIsClear()
        {
            Assert.AreSame(player, world.AddPlayer(player.Actor)); Assert.AreEqual(1, world.Players.Count);
            var pos = world.SafePlayerPoint(1); Assert.Greater(pos.y, 0);
            Box("Blocked spawn", pos + Vector3.up, Vector3.one * 2);
            Assert.AreNotEqual(pos, world.SafePlayerPoint(1));
            Assert.AreEqual(world.Config.EyeHeight, player.Eye.y - player.transform.position.y, .001);
        }
        [Test] public void S006_NonAuthorityIgnoresLookAndMoveAndPitchIsClamped()
        {
            player.Input(new CommandEnvelope { Yaw = 720, Pitch = 100 }, world.Now); Tick(1, yaw: 0);
            player.Input(new CommandEnvelope { Pitch = 100 }, world.Now); Assert.AreEqual(85, player.Pitch);
            player.Initialize(player.Actor, false, world.Config);
            var pos = player.transform.position;
            player.Input(new CommandEnvelope { Yaw = 123, Pitch = -30, Z = 1 }, world.Now); player.Drive(world, .02f);
            Assert.AreEqual(0, player.Yaw); Assert.AreEqual(85, player.Pitch); Assert.AreEqual(pos, player.transform.position);
        }
        [Test] public void S007_ForwardDiagonalYawAndFixedFrequencyHaveEqualSpeed()
        {
            foreach (float dt in new[] { 1f / 30, 1f / 50, 1f / 100 })
            foreach (Vector2 direction in new[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.one })
            {
                player.Respawn(Origin); Tick(10); var start = player.transform.position;
                Tick(Mathf.RoundToInt(1 / dt), direction, 90, dt);
                var displacement = Vector3.ProjectOnPlane(player.transform.position - start, Vector3.up);
                Assert.AreEqual(world.Config.PlayerSpeed, displacement.magnitude, .08f, $"dt={dt} input={direction}");
                if (direction == Vector2.up) Assert.Greater(displacement.x, 2.9f);
            }
        }
        [Test] public void S007_StopInputAndAirControlAreBounded()
        {
            Tick(10, Vector2.up); player.ClearInput(); var start = player.transform.position;
            world.Step(.02f); Assert.Less(Vector3.Distance(start, player.transform.position), .05f);
            Assert.IsTrue(player.RequestJump()); Tick(1); Tick(5, Vector2.right);
            Assert.Less(Mathf.Abs(player.Velocity.x), world.Config.PlayerSpeed * .5f);
        }
        [Test] public void S008_RepeatedJumpApexNoAirJumpAndLanding()
        {
            for (int cycle = 0; cycle < 8; cycle++)
            {
                Assert.IsTrue(player.Grounded); float floor = player.transform.position.y;
                Assert.IsTrue(player.RequestJump()); Tick(1); Assert.IsFalse(player.Grounded);
                Assert.IsFalse(player.RequestJump()); float apex = player.transform.position.y;
                for (int i = 0; i < 50; i++) { Tick(1); apex = Mathf.Max(apex, player.transform.position.y); if (!player.Grounded) Assert.IsFalse(player.RequestJump()); }
                Assert.AreEqual(world.Config.JumpHeight, apex - floor, .08f);
                Assert.IsTrue(player.Grounded); Assert.AreEqual(floor, player.transform.position.y, .05);
            }
        }
        [Test] public void S008_EdgeFallsWallIsNotGroundAndKillPlaneRecovers()
        {
            Box("Ledge", Origin + new Vector3(0, 1, 0), new Vector3(2, 2, 2));
            player.Respawn(Origin + Vector3.up * 2.1f); Tick(15); Assert.IsTrue(player.Grounded);
            Tick(30, Vector2.right); Assert.IsFalse(player.Grounded); Tick(70); Assert.IsTrue(player.Grounded);
            Assert.Less(player.transform.position.y, .1f);
            player.Respawn(new Vector3(30, -4.9f, 20)); Tick(10); Assert.Less(player.transform.position.x, 10); Assert.IsTrue(SafeMath.Finite(player.Velocity));
        }
        [Test] public void S009_WallCornerSeamAndCeilingStayBounded()
        {
            Box("Wall", Origin + new Vector3(0, 1.5f, 2), new Vector3(8, 3, .2f));
            Box("Corner", Origin + new Vector3(2, 1.5f, 0), new Vector3(.2f, 3, 8));
            Tick(150, Vector2.one); Assert.Less(player.transform.position.z, 1.8f); Assert.Less(player.transform.position.x, 31.8f);
            player.Respawn(Origin + new Vector3(0, 1, 1.5f)); Tick(1, Vector2.up);
            Assert.IsFalse(player.Grounded, "Wall contact must not ground an airborne capsule"); Assert.IsFalse(player.RequestJump());
            Tick(100, Vector2.one);
            var settled = player.transform.position; Tick(50, Vector2.one); Assert.Less(Vector3.Distance(settled, player.transform.position), .02f);
            player.Respawn(Origin); Tick(10);
            Box("Ceiling", Origin + new Vector3(0, 2.1f, 0), new Vector3(2, .2f, 2));
            Assert.IsTrue(player.RequestJump()); float max = 0;
            for (int i = 0; i < 50; i++) { Tick(1); max = Mathf.Max(max, player.transform.position.y); }
            Assert.Less(max, .3f); Assert.IsTrue(player.Grounded);
        }
        [Test] public void S009_NarrowDoorwayAndFloorSeamAreTraversable()
        {
            Box("Door left", Origin + new Vector3(-.9f, 1.3f, 2), new Vector3(1, 2.6f, .4f));
            Box("Door right", Origin + new Vector3(.9f, 1.3f, 2), new Vector3(1, 2.6f, .4f));
            Box("Seam", Origin + new Vector3(0, -.01f, 3), new Vector3(.8f, .02f, 2));
            Tick(80, Vector2.up); Assert.Greater(player.transform.position.z, 4); Assert.Less(player.transform.position.y, .1f);
        }
        [TestCase(.12f, true)] [TestCase(.28f, true)] [TestCase(.8f, false)]
        public void S010_SmallAndMediumStepsPassTallStepBlocks(float height, bool pass)
        {
            Box("Step", new Vector3(30, height / 2, 2), new Vector3(4, height, 2));
            Tick(65, Vector2.up);
            Assert.AreEqual(pass, player.transform.position.z > 2.5f, $"height={height}, pos={player.transform.position}");
            Assert.Less(player.transform.position.y, height + .1f);
        }
        [TestCase(20f, true)] [TestCase(45f, true)] [TestCase(60f, false)]
        public void S010_WalkableSlopeUphillDownhillAndSteepRejection(float angle, bool pass)
        {
            // Top face starts at z=1, y=0, continuous with the floor.
            var ramp = Box("Ramp", Vector3.zero, new Vector3(4, .2f, 5));
            var rotation = Quaternion.Euler(-angle, 0, 0); ramp.transform.rotation = rotation;
            ramp.transform.position = new Vector3(30, 0, 1) + rotation * new Vector3(0, -.1f, 2.5f); Physics.SyncTransforms();
            Tick(65, Vector2.up);
            Assert.AreEqual(pass, player.transform.position.y > .5f, $"angle={angle}, pos={player.transform.position}");
            if (pass)
            {
                var pos = player.transform.position; Tick(30); Assert.Less(Vector3.Distance(pos, player.transform.position), .06f);
                int air = 0;
                for (int i = 0; i < 65; i++) { Tick(1, Vector2.down); if (!player.Grounded) air++; }
                Assert.Less(air, 4, "Continuous downhill must retain ground"); Assert.Less(player.transform.position.y, .15f);
            }
            else { Assert.Less(player.transform.position.z, 1.5f); Assert.IsFalse(player.RequestJump() && !player.Grounded); }
        }
    }
}
