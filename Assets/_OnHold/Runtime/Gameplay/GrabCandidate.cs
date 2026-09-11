using UnityEngine;
using OnHold.Core;
namespace OnHold.Gameplay
{
    // An observation, never a reservation or authority token. Default is an invalid candidate.
    public readonly struct GrabCandidate
    {
        public int ItemId { get; }
        public int GripIndex { get; }
        public Pose WorldPose { get; }
        public float Distance { get; }
        public Reason Reason { get; }
        public bool Success => ItemId > 0 && GripIndex >= 0 && Reason == Reason.Accepted;
        public GrabCandidate(Reason reason)
        { Reason = reason; ItemId = 0; GripIndex = -1; WorldPose = default; Distance = 0; }
        public GrabCandidate(int itemId, int gripIndex, Pose worldPose, float distance)
        { ItemId = itemId; GripIndex = gripIndex; WorldPose = worldPose; Distance = distance; Reason = Reason.Accepted; }
    }
}
