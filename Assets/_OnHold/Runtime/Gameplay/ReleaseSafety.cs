using UnityEngine;
using OnHold.Core;

namespace OnHold.Gameplay
{
    public enum ReleaseResult { Released, NoSafePose, InvalidState }

    /// <summary>
    /// S020 — stateless release-safety logic. All methods are deterministic and allocation-free after warmup.
    /// </summary>
    public static class ReleaseSafety
    {
        static readonly Collider[] overlapBuffer = new Collider[32];

        // Fixed deterministic candidate offsets, ordered: current → upward → nearby.
        // Scaled by SafeReleaseSearchDistance / 0.5 at query time.
        static readonly Vector3[] offsets =
        {
            Vector3.zero,
            new Vector3(0, .15f, 0),
            new Vector3(0, .30f, 0),
            new Vector3(0, .15f, -.15f),
            new Vector3(.15f, .15f, 0),
            new Vector3(-.15f, .15f, 0),
            new Vector3(0, .15f, .15f),
            new Vector3(0, .30f, -.15f),
            new Vector3(.15f, .30f, 0),
            new Vector3(-.15f, .30f, 0),
        };

        /// <summary>
        /// Returns true when the cargo at the given pose does not meaningfully overlap blocking geometry.
        /// Uses NonAlloc overlap. Buffer overflow is treated as unsafe.
        /// </summary>
        public static bool IsPoseSafe(Rigidbody body, Vector3 boundsCenter, Vector3 boundsSize,
            Vector3 position, Quaternion rotation, float tolerance)
        {
            if (!body || !SafeMath.Finite(position) || !SafeMath.Finite(rotation) ||
                !SafeMath.Finite(boundsCenter) || !SafeMath.Finite(boundsSize) ||
                !Numbers.Finite(tolerance) || tolerance < 0 ||
                boundsSize.x <= 0 || boundsSize.y <= 0 || boundsSize.z <= 0 ||
                Mathf.Abs(Quaternion.Dot(rotation, rotation) - 1f) > .001f) return false;
            var center = position + rotation * boundsCenter;
            // Tolerance is a distance on each face, independent of the cargo aspect ratio.
            var halfExtents = boundsSize * .5f - Vector3.one * tolerance;
            if (halfExtents.x <= 0 || halfExtents.y <= 0 || halfExtents.z <= 0) return false;
            int mask = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 11); // world, player, cargo, platform
            int count = Physics.OverlapBoxNonAlloc(center, halfExtents, overlapBuffer, rotation, mask, QueryTriggerInteraction.Ignore);
            if (count >= overlapBuffer.Length) return false; // overflow → conservative unsafe
            for (int i = 0; i < count; i++)
            {
                var c = overlapBuffer[i];
                if (!c) continue;
                if (c.attachedRigidbody == body) continue; // own colliders
                return false;
            }
            return true;
        }

        /// <summary>
        /// Evaluate release pose and attempt bounded safe resolution.
        /// Returns Released with safePosition set, or NoSafePose / InvalidState.
        /// </summary>
        public static ReleaseResult Evaluate(CarryableBody item, FoundationConfig config, out Vector3 safePosition)
        {
            safePosition = Vector3.zero;
            if (!item || !item.Body || !item.Definition) return ReleaseResult.InvalidState;
            return Evaluate(item.Body, item.Definition.BoundsCenter, item.Definition.BoundsSize, config, out safePosition);
        }

        public static ReleaseResult Evaluate(Rigidbody body, Vector3 bc, Vector3 bs,
            FoundationConfig config, out Vector3 safePosition)
        {
            safePosition = Vector3.zero;
            if (!body || body.isKinematic || config == null || !SafeMath.Finite(body.position) ||
                !SafeMath.Finite(body.rotation) || Mathf.Abs(Quaternion.Dot(body.rotation, body.rotation) - 1f) > .001f ||
                !SafeMath.Finite(bc) || !SafeMath.Finite(bs) || bs.x <= 0 || bs.y <= 0 || bs.z <= 0)
                return ReleaseResult.InvalidState;
            var origin = body.position;
            var rotation = body.rotation;
            safePosition = origin;
            float tol = config.ReleasePenetrationTolerance;

            if (IsPoseSafe(body, bc, bs, origin, rotation, tol))
                return ReleaseResult.Released;

            float scale = config.SafeReleaseSearchDistance / .5f;
            for (int i = 1; i < offsets.Length; i++)
            {
                var candidate = origin + offsets[i] * scale;
                if (Vector3.Distance(origin, candidate) > config.SafeReleaseSearchDistance) continue;
                if (IsPoseSafe(body, bc, bs, candidate, rotation, tol))
                {
                    safePosition = candidate;
                    return ReleaseResult.Released;
                }
            }
            return ReleaseResult.NoSafePose;
        }

        /// <summary>
        /// Clamp release velocities. Returns false if NaN/Infinity detected (and zeros the offending vector).
        /// </summary>
        public static bool SanitizeVelocity(ref Vector3 linear, ref Vector3 angular, FoundationConfig config)
        {
            bool ok = true;
            if (!SafeMath.Finite(linear)) { linear = Vector3.zero; ok = false; }
            if (!SafeMath.Finite(angular)) { angular = Vector3.zero; ok = false; }
            linear = Vector3.ClampMagnitude(linear, config.MaxReleaseLinearSpeed);
            angular = Vector3.ClampMagnitude(angular, config.MaxReleaseAngularSpeed);
            return ok;
        }
    }
}
