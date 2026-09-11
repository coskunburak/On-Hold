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
    public sealed class InteractionFeedbackIntegrationTests
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
            player.Respawn(new Vector3(30, .02f, 0)); cargo = world.Items[0]; Put(cargo, player.Eye + Vector3.forward * 1.7f);
        }
        [UnityTearDown] public IEnumerator Teardown()
        { world.StopSession(); foreach (var go in fixtures) if (go) Object.Destroy(go); fixtures.Clear(); yield return null; }
        Reason Apply(CommandType type, CarryableBody item = null) => world.Apply(new CommandEnvelope { CommandType = type, TargetId = item ? item.NetworkId : 0 }, player.Actor);
        void Put(CarryableBody item, Vector3 point)
        { item.ResetState(); item.Body.position = point; item.Body.rotation = Quaternion.identity; Physics.SyncTransforms(); }
        InteractionFeedback Query(Vector3 direction = default, bool owns = true) => InteractionRay.QueryFeedback(world, player.Actor, player.Eye, direction == default ? Vector3.forward : direction, owns);
        [Test] public void S017_TargetTransitionsClearStableIdentityAndAction()
        {
            var a = Query(); Assert.AreEqual(cargo.NetworkId, a.TargetId); Assert.AreEqual(CommandType.Grab, a.Action); Assert.IsTrue(a.Actionable);
            var none = Query(Vector3.up); Assert.AreEqual(0, none.TargetId); Assert.IsNull(none.Action); Assert.AreEqual(InteractionNotice.None, none.Notice);
            Put(cargo, player.Eye + Vector3.right * 8); Put(world.Items[1], player.Eye + Vector3.forward * 1.7f);
            var b = Query(); Assert.AreEqual(world.Items[1].NetworkId, b.TargetId); Assert.IsTrue(b.Actionable);
        }
        [Test] public void S017_WallAndRangeNeverShowFalseGrab()
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube); fixtures.Add(wall); wall.layer = 8;
            wall.transform.position = player.Eye + Vector3.forward * .8f; wall.transform.localScale = Vector3.one * .3f; Physics.SyncTransforms();
            Assert.IsFalse(Query().Actionable); wall.SetActive(false); Assert.IsTrue(Query().Actionable);
            Put(cargo, player.Eye + Vector3.forward * 5); Assert.IsFalse(Query().Actionable);
            Put(cargo, player.Eye + Vector3.forward * (world.Config.Reach + .1f)); Assert.IsFalse(Query().Actionable, "Visible front face does not imply center is reachable");
        }
        [Test] public void S017_ChildColliderSeamsNormalizeToSameOwner()
        {
            foreach (var c in cargo.GetComponentsInChildren<Collider>()) c.enabled = false;
            for (int i = 0; i < 2; i++)
            {
                var child = new GameObject("child collider"); fixtures.Add(child); child.layer = 10; child.transform.SetParent(cargo.transform, false);
                child.transform.localPosition = new Vector3(i == 0 ? -.25f : .25f, 0, 0); child.AddComponent<BoxCollider>().size = new Vector3(.5f, 1, .5f);
            }
            Physics.SyncTransforms();
            var a = Query(new Vector3(-.05f, 0, 1)); var b = Query(new Vector3(.05f, 0, 1));
            Assert.AreNotSame(a.Target.Hit.collider, b.Target.Hit.collider); Assert.AreEqual(cargo.NetworkId, a.TargetId); Assert.AreEqual(a.TargetId, b.TargetId); Assert.AreEqual(a.Action, b.Action);
        }
        [Test] public void S017_SuppressedAndDestroyedTargetsClearImmediately()
        {
            Assert.IsTrue(Query().Actionable); var hidden = Query(owns: false); Assert.IsFalse(hidden.Visible); Assert.IsNull(hidden.Action);
            cargo.gameObject.SetActive(false); Assert.IsFalse(Query().Actionable); cargo.gameObject.SetActive(true);
        }
        [Test] public void S017_ThousandWarmedQueriesDoNotMutateOrCreateObjects()
        {
            for (int i = 0; i < 50; i++) Query();
            int count = Object.FindObjectsByType<Transform>(FindObjectsInactive.Include).Length; long start = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < 1000; i++) Query();
            long bytes = GC.GetAllocatedBytesForCurrentThread() - start;
            TestContext.WriteLine("1000 warmed feedback queries allocated " + bytes + " bytes");
            Assert.LessOrEqual(bytes, 128); Assert.AreEqual(count, Object.FindObjectsByType<Transform>(FindObjectsInactive.Include).Length);
            Assert.AreEqual(0, cargo.HandleCount); Assert.AreEqual(0, cargo.State.Revision); Assert.AreEqual(Handling.Free, cargo.State.Handling);
        }

        sealed class Attachment : IAttachment { public void Dispose() { } }
        [Test] public void S018_PreviewIsReadOnlyAndCommitUsesThePreviewedAuthoredGrip()
        {
            player.Respawn(new Vector3(29.4f, .02f, 0));
            var candidate = world.QueryGrabCandidate(player.Actor, cargo);
            Assert.IsTrue(candidate.Success); Assert.AreEqual(0, candidate.GripIndex);
            Assert.AreEqual(cargo.NetworkId, candidate.ItemId);
            Assert.Less(Vector3.Distance(cargo.GripWorldPose(0).position, candidate.WorldPose.position), .0001f);
            uint revision = cargo.State.Revision;
            for (int j = 0; j < 50; j++) world.QueryGrabCandidate(player.Actor, cargo);
            long start = GC.GetAllocatedBytesForCurrentThread();
            for (int j = 0; j < 1000; j++) world.QueryGrabCandidate(player.Actor, cargo);
            long bytes = GC.GetAllocatedBytesForCurrentThread() - start;
            Assert.LessOrEqual(bytes, 128); TestContext.WriteLine("1000 warmed grip previews allocated " + bytes + " bytes");
            Assert.AreEqual(revision, cargo.State.Revision); Assert.AreEqual(0, cargo.State.Holders.Count); Assert.AreEqual(Handling.Free, cargo.State.Handling);
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Grab, cargo)); Assert.AreEqual(candidate.GripIndex, cargo.HeldGripIndex(player.Actor));
            var held = Query(); Assert.AreEqual(CommandType.Release, held.Action); Assert.IsFalse(held.HasGrip);
            Assert.AreEqual(Reason.Accepted, Apply(CommandType.Release)); var free = Query();
            Assert.AreEqual(CommandType.Grab, free.Action); Assert.IsTrue(free.HasGrip); Assert.AreEqual(-1, cargo.HeldGripIndex(player.Actor));
        }
        [Test] public void S018_RangeOcclusionRecoveryAndInvalidActorRejectPreview()
        {
            Assert.IsTrue(Query().HasGrip);
            Put(cargo, player.Eye + Vector3.forward * 4); Assert.IsFalse(Query().HasGrip); Assert.IsFalse(world.QueryGrabCandidate(player.Actor, cargo).Success);
            Put(cargo, player.Eye + Vector3.forward * 1.7f);
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube); fixtures.Add(wall); wall.layer = 8;
            wall.transform.position = player.Eye + Vector3.forward * .8f; wall.transform.localScale = Vector3.one * .3f; Physics.SyncTransforms();
            Assert.IsFalse(Query().HasGrip); Assert.IsFalse(world.QueryGrabCandidate(player.Actor, cargo).Success);
            wall.SetActive(false); Assert.IsTrue(Query().HasGrip);
            cargo.RecoveryPending = true; Assert.IsFalse(Query().HasGrip); cargo.RecoveryPending = false;
            Assert.IsFalse(world.QueryGrabCandidate(new Actor(player.Actor.Id, 999), cargo).Success);
        }
        [Test] public void S018_TerminalLostAndDeliveredNeverOfferGrip()
        {
            cargo.State.MarkLost(); Assert.IsFalse(Query().HasGrip); Assert.IsFalse(world.QueryGrabCandidate(player.Actor, cargo).Success);
            cargo.ResetState(); Put(cargo, player.Eye + Vector3.forward * 1.7f);
            var run = new ContractRun("preview.delivery", new[] { cargo.State }, new[] { cargo.InstanceId }, 1, 0, 0, 600, world.Config);
            run.EvaluateDelivery(cargo.State, true, 0, 0, 0); Assert.IsTrue(run.EvaluateDelivery(cargo.State, true, 0, 0, world.Config.DeliveryDwell));
            Assert.AreEqual(Lifecycle.Delivered, cargo.State.Lifecycle); Assert.IsFalse(Query().HasGrip); Assert.IsFalse(world.QueryGrabCandidate(player.Actor, cargo).Success);
        }
        [Test] public void S018_SecuredAndCapacityAreUnavailableWithoutReservation()
        {
            var slot = new AnchorSlot("test.anchor", "test.platform", 1000);
            Assert.AreEqual(Reason.Accepted, cargo.State.Secure(slot, cargo.State.Revision, true, true, 0, world.Config, () => new Attachment()));
            var unavailable = Query(); Assert.IsFalse(unavailable.HasGrip); Assert.IsFalse(unavailable.Actionable); Assert.AreEqual(InteractionNotice.Secured, unavailable.Notice);
            cargo.State.Unsecure(cargo.State.Revision, true, true, 0, world.Config); Assert.IsTrue(Query().HasGrip);
            // Real physical grips held by two fixture actors, without a network/co-op client.
            var second = world.AddPlayer(new Actor(2, 1)); second.Respawn(new Vector3(29.4f, .02f, 0));
            Assert.AreEqual(Reason.Accepted, world.Apply(new CommandEnvelope { CommandType = CommandType.Grab, TargetId = cargo.NetworkId }, second.Actor));
            var remaining = world.QueryGrabCandidate(player.Actor, cargo); Assert.IsTrue(remaining.Success); Assert.AreEqual(1, remaining.GripIndex);
            var third = world.AddPlayer(new Actor(3, 1)); third.Respawn(new Vector3(30.6f, .02f, 0));
            Assert.AreEqual(Reason.Accepted, world.Apply(new CommandEnvelope { CommandType = CommandType.Grab, TargetId = cargo.NetworkId }, third.Actor));
            Assert.IsFalse(world.QueryGrabCandidate(player.Actor, cargo).Success); Assert.AreEqual(InteractionNotice.NoAvailableGrip, Query().Notice);
            Assert.AreEqual(2, cargo.HandleCount);
        }
        [Test] public void S018_MultipleGripsCameraJitterTieBreakAndWorldPoseAreStable()
        {
            player.Respawn(new Vector3(29.6f, .02f, 0));
            for (int j = 0; j < 200; j++)
            {
                var ray = new Vector3(.23f + Mathf.Sin(j) * .002f, 0, 1);
                var state = Query(ray); Assert.IsTrue(state.HasGrip); Assert.AreEqual(0, state.Grip.GripIndex);
            }
            player.Respawn(new Vector3(30.4f, .02f, 0)); Assert.AreEqual(1, world.QueryGrabCandidate(player.Actor, cargo).GripIndex);
            player.Respawn(new Vector3(30, .02f, 0)); Assert.AreEqual(0, world.QueryGrabCandidate(player.Actor, cargo).GripIndex);
            cargo.Body.rotation = Quaternion.Euler(0, 30, 0); Physics.SyncTransforms();
            var result = world.QueryGrabCandidate(player.Actor, cargo);
            Assert.IsTrue(result.Success); Assert.AreEqual(cargo.Body.rotation, result.WorldPose.rotation);
            Assert.Less(Vector3.Distance(cargo.Body.position + cargo.Body.rotation * cargo.Definition.Grips[result.GripIndex], result.WorldPose.position), .0001f);
        }
        [Test] public void S018_AuthorityRevalidatesAfterPreviewBecomesInvalid()
        {
            Assert.IsTrue(Query().HasGrip); cargo.State.MarkLost();
            Assert.AreEqual(Reason.InvalidState, Apply(CommandType.Grab, cargo)); Assert.AreEqual(0, cargo.HandleCount); Assert.IsFalse(Query().HasGrip);
        }
        [UnityTest] public IEnumerator S018_DestroyedSelectedOwnerClearsWithoutPresentationException()
        {
            // Despawn from the authoritative registry, then destroy before the next presentation refresh.
            var previous = Query(); Assert.IsTrue(previous.HasGrip);
            var original = world.Items; world.Items = new[] { original[1] };
            Object.Destroy(cargo.gameObject); yield return null;
            Assert.IsFalse(previous.Target.Valid); Assert.IsFalse(previous.HasGrip); Assert.IsFalse(Query().HasGrip);
        }

        [Test] public void S018_CenterInReachDoesNotMakeAnOutOfReachGripActionable()
        {
            Put(cargo, player.Eye + Vector3.forward * (world.Config.Reach - .05f));
            Assert.IsTrue(InteractionRay.Detect(player.Eye, Vector3.forward, world.Config).Valid);
            Assert.IsTrue(world.Reach(player.Actor, cargo.Body.worldCenterOfMass, cargo));
            Assert.IsFalse(Query().HasGrip); Assert.IsFalse(Query().Actionable);
            Assert.IsFalse(world.QueryGrabCandidate(player.Actor, cargo).Success);
        }
    }
}
