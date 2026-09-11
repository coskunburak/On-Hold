using System;
using System.Collections.Generic;
using OnHold.Core;

namespace OnHold.Domain
{
    public enum CommandType : byte { Move, Grab, Release, Secure, Unsecure, Lease, Winch, ReleaseLease, Ready, Start, VoteFinish, Abort, Hub, NewRun, Ping, Jump, StopInput }
    [Serializable]
    public struct CommandEnvelope
    {
        public int ProtocolVersion;
        public ulong SessionEpoch;
        public uint ConnectionGeneration;
        public ulong RequestId, Sequence;
        public int TargetId;
        public uint ExpectedRevision;
        public CommandType CommandType;
        public float X, Y, Z, Yaw, Pitch;
    }
    public sealed class CommandGate
    {
        readonly Dictionary<ulong, Reason> recent = new Dictionary<ulong, Reason>();
        readonly Queue<ulong> order = new Queue<ulong>();
        double inputTokens = 60, actionTokens = 16, lastTime;
        ulong lastSequence;
        public int CacheCount => recent.Count;
        public int AbuseScore { get; private set; }
        public bool ShouldDisconnect => AbuseScore >= 100;
        public int Rejected { get; private set; }
        public const int Protocol = 1, MaxPayload = 128, CacheCapacity = 256;
        public Reason Execute(CommandEnvelope e, int payloadBytes, Actor actor, ulong epoch, double now, Func<CommandEnvelope, Actor, Reason> apply)
        {
            Reason Reject(Reason r, bool abuse = false) { Rejected++; if (abuse) AbuseScore = Math.Min(100, AbuseScore + 1); return r; }
            if (payloadBytes <= 0 || payloadBytes > MaxPayload || !Numbers.Finite(now) || now < lastTime || !ValidPayload(e)) return Reject(Reason.InvalidPayload, true);
            if (e.ProtocolVersion != Protocol || e.SessionEpoch != epoch) return Reject(Reason.WrongSession);
            if (actor.Id <= 0 || actor.Generation != e.ConnectionGeneration) return Reject(Reason.OldConnection);
            double elapsed = now - lastTime; lastTime = now;
            inputTokens = Math.Min(60, inputTokens + elapsed * 30);
            actionTokens = Math.Min(16, actionTokens + elapsed * 16);
            bool input = e.CommandType == CommandType.Move || e.CommandType == CommandType.Winch;
            if (inputTokens < 1 || (!input && actionTokens < 1)) return Reject(Reason.RateLimited, true);
            inputTokens--; if (!input) actionTokens--;
            // Only interactions enter the 256 cache. At 16/s + 16 burst it covers >10 seconds.
            // Movement uses the monotonic sequence high-water mark, so eviction cannot replay it.
            if (!input && recent.TryGetValue(e.RequestId, out var cached)) return cached;
            if (e.Sequence <= lastSequence || e.RequestId == 0) return Reject(Reason.OldSequence);
            lastSequence = e.Sequence;
            var result = apply(e, actor);
            if (!input)
            {
                recent[e.RequestId] = result; order.Enqueue(e.RequestId);
                while (order.Count > CacheCapacity) recent.Remove(order.Dequeue());
            }
            if (result != Reason.Accepted && result != Reason.AlreadyApplied) Rejected++;
            return result;
        }
        public static bool ValidPayload(CommandEnvelope e) => Enum.IsDefined(typeof(CommandType), e.CommandType) &&
            e.TargetId >= 0 && e.TargetId <= 1024 && e.RequestId > 0 && e.Sequence > 0 &&
            Numbers.Finite(e.X) && Numbers.Finite(e.Y) && Numbers.Finite(e.Z) && Numbers.Finite(e.Yaw) && Numbers.Finite(e.Pitch) &&
            Math.Abs(e.X) <= 1 && Math.Abs(e.Y) <= 1 && Math.Abs(e.Z) <= 1 && Math.Abs(e.Yaw) <= 360 && Math.Abs(e.Pitch) <= 85;
    }
}
