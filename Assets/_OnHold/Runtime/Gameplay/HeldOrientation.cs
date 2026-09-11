using UnityEngine;
using OnHold.Core;

namespace OnHold.Gameplay
{
    // Value state owned by the committed grip; never writes a body pose.
    public struct HeldOrientation
    {
        public Quaternion Target { get; private set; }
        public bool Initialized { get; private set; }
        public bool Engaged { get; private set; }
        public bool Active { get; private set; }

        public void Initialize(Quaternion actual)
        {
            this = default;
            if (!ValidRotation(actual)) return;
            Target = actual.normalized; Initialized = true;
        }
        public void EndInput() => Active = false;
        public void Reset() => this = default;
        public bool Update(bool active, Vector2 degrees, Quaternion view, Quaternion actual, float dt, FoundationConfig config)
        {
            Active = false;
            if (!Initialized || !active || !ValidRotation(actual) || !ValidRotation(view) ||
                !Numbers.Finite(degrees.x) || !Numbers.Finite(degrees.y) || !Numbers.Finite(dt) || dt <= 0) return false;
            // Ordinary carry remains unchanged until the player first asks to orient it.
            if (!Engaged) { Target = actual.normalized; Engaged = true; }
            Active = true;
            double length = System.Math.Sqrt((double)degrees.x * degrees.x + (double)degrees.y * degrees.y);
            float limit = config.ManipulationSpeed * Mathf.Min(dt, .1f);
            float scale = length > limit ? (float)(limit / length) : 1;
            var turn = degrees * scale;
            var axis = view.normalized * new Vector3(-turn.y, turn.x, 0);
            float angle = axis.magnitude;
            if (angle > .00001f) Target = (Quaternion.AngleAxis(angle, axis / angle) * Target).normalized;
            // Bound stored error under obstruction so repeated input cannot wind up multiple turns.
            Target = Quaternion.RotateTowards(actual, Target, config.ManipulationLeadMax).normalized;
            return true;
        }
        public static bool ValidRotation(Quaternion q)
        {
            float length = Quaternion.Dot(q, q);
            return SafeMath.Finite(q) && Numbers.Finite(length) && length > .000001f && length < 1000000;
        }
        public static Vector3 Correction(Quaternion actual, Quaternion target, Vector3 angularVelocity,
            Vector3 inertia, Quaternion inertiaRotation, float dt, FoundationConfig config)
        {
            if (!ValidRotation(actual) || !ValidRotation(target) || !ValidRotation(inertiaRotation) ||
                !SafeMath.Finite(angularVelocity) || !SafeMath.Finite(inertia) || inertia.x < 0 || inertia.y < 0 || inertia.z < 0 ||
                !Numbers.Finite(dt) || dt <= 0) return Vector3.zero;
            var error = (target.normalized * Quaternion.Inverse(actual.normalized)).normalized;
            if (error.w < 0) error = new Quaternion(-error.x, -error.y, -error.z, -error.w);
            error.ToAngleAxis(out float angle, out var axis);
            var displacement = angle > .001f ? axis * (Mathf.Min(angle, config.ManipulationLeadMax) * Mathf.Deg2Rad) : Vector3.zero;
            // Implicit critically damped PD response, converted through the body's world inertia.
            // The torque budget still limits heavy/large cargo; collisions remain solver-owned.
            float frequency = config.ManipulationResponse, damping = 2 * frequency, stiffness = frequency * frequency;
            var acceleration = (displacement * stiffness - angularVelocity * (damping + stiffness * dt)) /
                (1 + damping * dt + stiffness * dt * dt);
            var frame = actual.normalized * inertiaRotation.normalized;
            var torque = frame * Vector3.Scale(inertia, Quaternion.Inverse(frame) * acceleration);
            return SafeMath.Finite(torque) ? Vector3.ClampMagnitude(torque, config.ManipulationTorqueMax) : Vector3.zero;
        }
    }
}
