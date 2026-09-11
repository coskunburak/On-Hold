using UnityEngine;
using OnHold.Core;
using OnHold.Domain;

namespace OnHold.Gameplay
{
    public readonly struct InteractionTarget
    {
        public CarryableBody Item { get; }
        public RaycastHit Hit { get; }
        public bool Valid => Item != null;
        public InteractionTarget(CarryableBody item, RaycastHit hit) { Item = item; Hit = hit; }
    }
    public static class InteractionRay
    {
        public static InteractionFeedback QueryFeedback(FoundationWorld world, Actor actor, Vector3 origin, Vector3 forward, bool ownsInput)
        {
            if (!ownsInput) return default;
            foreach (var held in world.Items)
                if (held && held.State != null && held.State.IsHeldBy(actor))
                    return new InteractionFeedback(true, action: CommandType.Release, availability: InteractionAvailability.Available);
            var target = Detect(origin, forward, world.Config);
            if (!target.Valid) return new InteractionFeedback(true);
            var candidate = world.QueryGrabCandidate(actor, target.Item);
            return candidate.Success
                ? new InteractionFeedback(true, target, CommandType.Grab, InteractionAvailability.Available, grip: candidate)
                : new InteractionFeedback(true, target, availability: InteractionAvailability.Unavailable,
                    notice: InteractionFeedback.Explain(candidate.Reason, target.Item.State.Handling));
        }
        // A single closest-hit query preserves occlusion. Filtering never skips a blocking wall.
        public static InteractionTarget Detect(Vector3 origin, Vector3 forward, FoundationConfig config)
        {
            if (!SafeMath.Finite(origin) || !SafeMath.Finite(forward) || forward.sqrMagnitude < .0001f) return default;
            if (!Physics.Raycast(origin, forward.normalized, out var hit, config.Reach, config.InteractionMask,
                (QueryTriggerInteraction)config.InteractionTriggers)) return default;
            var item = hit.collider.GetComponentInParent<CarryableBody>();
            return item && item.State != null && item.State.Lifecycle == Lifecycle.Available && !item.RecoveryPending
                ? new InteractionTarget(item, hit) : default;
        }
    }
}
