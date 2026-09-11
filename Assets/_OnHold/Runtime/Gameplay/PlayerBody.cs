using System.Collections.Generic;
using UnityEngine;
using OnHold.Core;
using OnHold.Domain;

namespace OnHold.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerBody : MonoBehaviour
    {
        public Actor Actor { get; private set; }
        public Vector3 Velocity { get; private set; }
        public Vector3 Eye => transform.position + Vector3.up * config.EyeHeight;
        public Vector3 Forward => Quaternion.Euler(pitch, yaw, 0) * Vector3.forward;
        public float Yaw => yaw;
        public float Pitch => pitch;
        public bool Grounded { get; private set; }
        public float VerticalSpeed => vertical;
        public float LastPushForce { get; private set; }
        CharacterController controller;
        FoundationConfig config;
        float yaw, pitch, vertical, simulationDeltaTime;
        Vector2 movement;
        Vector3 horizontal;
        double lastInput;
        bool jumpPending;
        readonly HashSet<Rigidbody> pushed = new HashSet<Rigidbody>();

        public void Initialize(Actor actor, bool authority, FoundationConfig tuning)
        {
            Actor = actor; config = tuning; controller = GetComponent<CharacterController>();
            controller.height = config.PlayerHeight; controller.radius = config.PlayerRadius;
            controller.center = Vector3.up * (config.PlayerHeight / 2);
            controller.skinWidth = config.SkinWidth; controller.stepOffset = config.StepHeight;
            controller.slopeLimit = config.SlopeLimit + .1f; // Contact-normal floating point tolerance. controller.minMoveDistance = 0;
            controller.enabled = authority;
        }
        public void Input(CommandEnvelope e, double now)
        {
            if (!controller.enabled || !Numbers.Finite(e.X) || !Numbers.Finite(e.Z) || !Numbers.Finite(e.Yaw) || !Numbers.Finite(e.Pitch)) return;
            movement = Vector2.ClampMagnitude(new Vector2(e.X, e.Z), 1);
            yaw = Mathf.Repeat(e.Yaw, 360); pitch = Mathf.Clamp(e.Pitch, config.PitchMin, config.PitchMax); lastInput = now;
        }
        public bool RequestJump()
        {
            if (!controller.enabled || !Grounded || vertical > 0 || jumpPending) return false;
            jumpPending = true; return true;
        }
        public void ClearInput() { movement = Vector2.zero; horizontal = Vector3.zero; jumpPending = false; }
        bool ProbeGround(out RaycastHit hit)
        {
            float radius = config.PlayerRadius - config.SkinWidth;
            return Physics.SphereCast(transform.position + Vector3.up * (config.PlayerRadius + config.SkinWidth), radius,
                Vector3.down, out hit, config.GroundProbe + config.SkinWidth * 2,
                (1 << 8) | (1 << 10) | (1 << 11), QueryTriggerInteraction.Ignore) &&
                Vector3.Angle(hit.normal, Vector3.up) <= config.SlopeLimit + .1f;
        }
        public void Drive(FoundationWorld world, float dt)
        {
            if (!controller.enabled || !Numbers.Finite(dt) || dt <= 0) return;
            if (world.Now - lastInput > config.LeaseTimeout) ClearInput();
            if (!SafeMath.Finite(transform.position) || !Numbers.Finite(vertical)) { world.CleanupActor(Actor); Respawn(world.SafePlayerPoint(Actor.Id)); return; }
            simulationDeltaTime = dt;
            var before = transform.position;
            bool supportFound = ProbeGround(out var support);
            Grounded = vertical <= 0 && supportFound;
            var desired = Quaternion.Euler(0, yaw, 0) * new Vector3(movement.x, 0, movement.y) * config.PlayerSpeed;
            horizontal = Grounded ? desired : Vector3.MoveTowards(horizontal, desired, config.PlayerSpeed * config.AirControl * dt);
            if (Grounded) vertical = -2f;
            if (jumpPending && Grounded) { vertical = Mathf.Sqrt(2 * config.Gravity * config.JumpHeight); Grounded = false; }
            jumpPending = false;
            // Analytic per-tick displacement keeps the configured apex independent of physics frequency.
            float dy = vertical * dt - .5f * config.Gravity * dt * dt;
            vertical = Mathf.Max(vertical - config.Gravity * dt, -config.FallSpeedMax);
            var move = horizontal * dt + Vector3.up * dy;
            if (Grounded)
            {
                // Tangential travel and vertical adhesion are resolved separately by this
                // one motor. Combining them cancels uphill motion or adds idle slope drift.
                var tangent = Vector3.ProjectOnPlane(horizontal, support.normal);
                move = tangent.normalized * horizontal.magnitude * dt;
            }
            if (Grounded && Physics.Raycast(transform.position + Vector3.up * .12f, Vector3.down, out _, .35f, 1 << 11, QueryTriggerInteraction.Ignore))
                move += world.Platform.PointVelocity(transform.position) * dt;
            // Keep stepping available while descending across a step edge; a sphere normal at
            // that edge can temporarily be steep. Upward jump travel never steps.
            controller.stepOffset = vertical > 0 ? 0 : config.StepHeight;
            pushed.Clear(); LastPushForce = 0;
            var flags = controller.Move(move);
            if (Grounded) flags |= controller.Move(Vector3.down * (2 * dt));
            if ((flags & CollisionFlags.Above) != 0 && vertical > 0) vertical = 0;
            Grounded = vertical <= 0 && ProbeGround(out _);
            if (Grounded) vertical = -2;
            transform.rotation = Quaternion.Euler(0, yaw, 0);
            Velocity = (transform.position - before) / dt;
            if (transform.position.y < world.KillY) { world.CleanupActor(Actor); Respawn(world.SafePlayerPoint(Actor.Id)); }
        }
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var body = hit.rigidbody;
            if (!body || body.isKinematic || hit.normal.y > .5f || hit.moveDirection.y < -.5f || pushed.Count >= 16 || !pushed.Add(body)) return;
            var cargo = body.GetComponent<CarryableBody>();
            if (cargo && (cargo.State.Handling != Handling.Free || cargo.State.Lifecycle != Lifecycle.Available)) return;
            var direction = Vector3.ProjectOnPlane(horizontal, Vector3.up).normalized;
            float speed = Vector3.Dot(body.linearVelocity, direction);
            if (direction == Vector3.zero || speed >= config.PushSpeedMax) return;
            LastPushForce = Mathf.Min(config.PushForceMax, (config.PushSpeedMax - speed) * body.mass / simulationDeltaTime);
            body.AddForce(direction * LastPushForce, ForceMode.Force);
        }
        public void Respawn(Vector3 position)
        {
            if (!SafeMath.Finite(position)) return;
            bool enabled = controller.enabled; controller.enabled = false; transform.position = position; controller.enabled = enabled;
            vertical = 0; ClearInput(); Grounded = false; Velocity = Vector3.zero;
        }
    }
}
