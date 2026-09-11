using UnityEngine;
using OnHold.Domain;
using OnHold.Core;

namespace OnHold.Gameplay
{
    // Presentation mapping boundary; actions reuse the existing command contract.
    public enum InteractionAvailability { None, Available, Unavailable }
    public enum InteractionNotice { None, MoveCloser, Secured, NoAvailableGrip, Unavailable }
    public readonly struct InteractionFeedback
    {
        public GrabCandidate Grip { get; }
        public bool HasGrip => Action == CommandType.Grab && Actionable && Target.Valid && Grip.Success;
        public bool Visible { get; }
        public InteractionTarget Target { get; }
        public int TargetId => Target.Valid ? Target.Item.NetworkId : 0;
        public CommandType? Action { get; }
        public InteractionAvailability Availability { get; }
        public InteractionNotice Notice { get; }
        public bool Actionable => Availability == InteractionAvailability.Available && Action.HasValue && (Action != CommandType.Grab || Target.Valid);
        public InteractionFeedback(bool visible, InteractionTarget target = default, CommandType? action = null,
            InteractionAvailability availability = InteractionAvailability.None, InteractionNotice notice = InteractionNotice.None, GrabCandidate grip = default)
        { Visible = visible; Target = target; Action = action; Availability = availability; Notice = notice; Grip = grip; }
        public static InteractionNotice Explain(Reason reason, Handling handling)
        {
            if (handling == Handling.Secured) return InteractionNotice.Secured;
            switch (reason)
            {
                case Reason.TooFar: return InteractionNotice.MoveCloser;
                case Reason.Busy: case Reason.AttachmentFailed: return InteractionNotice.NoAvailableGrip;
                default: return InteractionNotice.Unavailable;
            }
        }
    }
}
