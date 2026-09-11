using UnityEngine;
using OnHold.Core;
using OnHold.Domain;

namespace OnHold.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ControlledPlatform : MonoBehaviour
    {
        public Transform Panel;
        public Transform[] Anchors;
        public float MinHeight = .25f, MaxHeight = 4;
        public AnchorSlot[] Slots { get; private set; }
        public DriverLease Lease { get; } = new DriverLease();
        public SupportedSet Support { get; private set; } = new SupportedSet();
        public float Speed { get; private set; }
        public Vector3 LinearVelocity { get; private set; }
        public Vector3 AngularVelocity { get; private set; }
        public Rigidbody Body { get; private set; }
        public Reason LockReason { get; private set; } = Reason.Accepted;
        FoundationWorld world;
        float pitch, roll;
        Vector3 origin;
        public void Initialize(FoundationWorld world)
        {
            this.world = world; Body = GetComponent<Rigidbody>(); Body.isKinematic = true; origin = transform.position;
            Slots = new AnchorSlot[Anchors.Length]; for (int i = 0; i < Slots.Length; i++) Slots[i] = new AnchorSlot("anchor." + i, "platform.foundation.01", 250);
        }
        public void ResetState() { Lease.Stop(); Support = new SupportedSet(); Speed = 0; pitch = roll = 0; Body.position = origin; Body.rotation = Quaternion.identity; LinearVelocity = AngularVelocity = Vector3.zero; }
        public bool DeckContact(CarryableBody item)
        {
            // Authored bounds bottom corners must touch the deck plane in platform local space.
            int near = 0;
            for (int i = 0; i < 8; i++)
            {
                var p = transform.InverseTransformPoint(item.transform.TransformPoint(item.Definition.Corner(i)));
                if (Mathf.Abs(p.x) <= world.Config.DeckWidth / 2 + .02f && Mathf.Abs(p.z) <= world.Config.DeckDepth / 2 + .02f && p.y >= .08f && p.y <= .24f) near++;
            }
            return near >= 2;
        }
        public bool ValidPlacement(CarryableBody item)
        {
            if (!DeckContact(item)) return false;
            for (int i = 0; i < 8; i++) { var p = transform.InverseTransformPoint(item.transform.TransformPoint(item.Definition.Corner(i))); if (Mathf.Abs(p.x) > world.Config.DeckWidth / 2 || Mathf.Abs(p.z) > world.Config.DeckDepth / 2) return false; }
            return true;
        }
        public void Drive(float dt)
        {
            var c = world.Config;
            foreach (var item in world.Items)
            {
                if (!item) continue;
                if (!item.Body || !item.isActiveAndEnabled || item.State.Lifecycle != Lifecycle.Available || item.RecoveryPending) { Support.Remove(item.InstanceId); continue; }
                var p = transform.InverseTransformPoint(item.Body.worldCenterOfMass);
                Support.Observe(item.InstanceId, item.Definition.MassKg, p.x, p.z, DeckContact(item), item.State.Handling == Handling.Secured, world.Now, c);
            }
            foreach (var player in world.Players.Values)
            {
                if (!player || !player.isActiveAndEnabled) continue;
                var p = transform.InverseTransformPoint(player.transform.position);
                bool supported = Mathf.Abs(p.x) <= c.DeckWidth / 2 && Mathf.Abs(p.z) <= c.DeckDepth / 2 && p.y > .08f && p.y < .4f;
                Support.Observe("player." + player.Actor.Id, c.PlayerMass, p.x, p.z, supported, false, world.Now, c);
            }
            Support.Calculate(c.BaseMass); Lease.Watchdog(world.Now, c.LeaseTimeout);
            float targetPitch = Mathf.Clamp((float)Support.CentreZ / (c.DeckDepth / 2), -1, 1) * c.TiltMax;
            float targetRoll = -Mathf.Clamp((float)Support.CentreX / (c.DeckWidth / 2), -1, 1) * c.TiltMax;
            pitch = Mathf.MoveTowards(pitch, Mathf.Lerp(pitch, targetPitch, 1 - Mathf.Exp(-c.TiltResponse * dt)), c.TiltSpeed * dt);
            roll = Mathf.MoveTowards(roll, Mathf.Lerp(roll, targetRoll, 1 - Mathf.Exp(-c.TiltResponse * dt)), c.TiltSpeed * dt);
            float intent = Lease.Intent;
            LockReason = Reason.Accepted;
            if (Mathf.Max(Mathf.Abs(pitch), Mathf.Abs(roll)) > c.TiltMax * .95f) { Lease.Stop(); intent = 0; LockReason = Reason.Blocked; }
            float nextSpeed = Mathf.MoveTowards(Speed, intent * c.WinchSpeed, c.WinchAcceleration * dt);
            var next = Body.position; next.y = Mathf.Clamp(next.y + nextSpeed * dt, MinHeight, MaxHeight);
            if (nextSpeed != 0 && Physics.BoxCast(Body.position, new Vector3(c.DeckWidth / 2, .12f, c.DeckDepth / 2), Vector3.up * Mathf.Sign(nextSpeed), out _, Body.rotation, Mathf.Abs(next.y - Body.position.y), 1 << 8, QueryTriggerInteraction.Ignore))
            { next = Body.position; Lease.Stop(); LockReason = Reason.Blocked; }
            var rotation = Quaternion.Euler(pitch, 0, roll);
            LinearVelocity = (next - Body.position) / dt; Speed = LinearVelocity.y;
            var delta = rotation * Quaternion.Inverse(Body.rotation); delta.ToAngleAxis(out var angle, out var axis); if (angle > 180) angle -= 360;
            AngularVelocity = SafeMath.Finite(axis) ? axis * (angle * Mathf.Deg2Rad / dt) : Vector3.zero;
            Body.MovePosition(next); Body.MoveRotation(rotation);
        }
        public Vector3 PointVelocity(Vector3 point) => LinearVelocity + Vector3.Cross(AngularVelocity, point - Body.position);
        public IAttachment Attach(CarryableBody item)
        {
            if (item.Body.isKinematic) return null;
            return new SecuredHandle(item, this);
        }
        sealed class SecuredHandle : IAttachment
        {
            readonly CarryableBody item;
            readonly ControlledPlatform platform;
            readonly Vector3 localPosition;
            readonly Quaternion localRotation;
            bool disposed;
            public SecuredHandle(CarryableBody item, ControlledPlatform platform)
            {
                this.item = item; this.platform = platform; localPosition = platform.transform.InverseTransformPoint(item.Body.position); localRotation = Quaternion.Inverse(platform.Body.rotation) * item.Body.rotation;
                item.Body.isKinematic = true; platform.attachments.Add(this);
            }
            public void Drive() { item.Body.MovePosition(platform.Body.position + platform.Body.rotation * localPosition); item.Body.MoveRotation(platform.Body.rotation * localRotation); }
            public void Dispose()
            {
                if (disposed) return; disposed = true; platform.attachments.Remove(this);
                if (!item || !item.Body) return;
                item.Body.isKinematic = false; item.Body.linearVelocity = Vector3.ClampMagnitude(platform.PointVelocity(item.Body.position), item.World.Config.BodySpeedMax); item.Body.angularVelocity = platform.AngularVelocity;
            }
        }
        readonly System.Collections.Generic.List<SecuredHandle> attachments = new System.Collections.Generic.List<SecuredHandle>();
        public int AttachmentCount => attachments.Count;
        public void DriveAttachments() { foreach (var attachment in attachments) attachment.Drive(); }
    }
}
