using System;
using System.Collections.Generic;
using System.Linq;
using OnHold.Core;

namespace OnHold.Domain
{
    public enum FinishReason { Timeout, Vote, Abort }
    public sealed class RunResult
    {
        public string RunId { get; }
        public long Payout { get; }
        public bool Success { get; }
        public FinishReason Reason { get; }
        internal RunResult(string id, long payout, bool success, FinishReason reason) { RunId = id; Payout = payout; Success = success; Reason = reason; }
    }
    public sealed class ContractRun
    {
        readonly Dictionary<string, ItemState> items;
        readonly Dictionary<string, double> dwellStart = new Dictionary<string, double>();
        readonly Dictionary<string, long> ledger = new Dictionary<string, long>();
        readonly HashSet<string> mandatory;
        readonly int quota;
        readonly long bonus;
        readonly FoundationConfig config;
        HashSet<int> voters, votes;
        double voteEnd, nextVote, tickStart;
        public string Id { get; }
        public double Deadline { get; private set; }
        public long ProvisionalValue { get; private set; }
        public int DeliveredCount => ledger.Count;
        public IReadOnlyDictionary<string, long> Ledger => ledger;
        public RunResult Result { get; private set; }
        public bool Success => ledger.Count >= quota && mandatory.All(ledger.ContainsKey);
        public bool MandatoryLost => mandatory.Any(id => items[id].Lifecycle == Lifecycle.Lost);
        public ContractRun(string id, IEnumerable<ItemState> source, IEnumerable<string> mandatoryIds, int quota, long bonus, double start, double duration, FoundationConfig config)
        {
            if (!Numbers.Id(id) || !Numbers.Finite(start) || !Numbers.Finite(duration) || duration <= 0 || bonus < 0 || bonus > 1000000000) throw new ArgumentException("contract");
            items = source.ToDictionary(i => i.Id); mandatory = new HashSet<string>(mandatoryIds);
            if (quota < mandatory.Count || quota > items.Count || quota <= 0 || mandatory.Any(i => !items.ContainsKey(i))) throw new ArgumentException("quota/mandatory");
            Id = id; Deadline = start + duration; this.quota = quota; this.bonus = bonus; this.config = config; tickStart = start;
        }
        public bool BeginTick(double now)
        {
            if (!Numbers.Finite(now) || now < tickStart) throw new ArgumentException("host time");
            tickStart = now;
            if (Result != null) return false;
            if (now >= Deadline) { Finish(FinishReason.Timeout); return false; }
            if (voters != null && now >= voteEnd) { voters = null; votes = null; nextVote = now + config.VoteCooldown; }
            return true;
        }
        // Invoke after physics/damage/terminal cleanup, once per item using OR across valid zones.
        public bool EvaluateDelivery(ItemState item, bool allCornersInside, double linearSpeed, double angularDegrees, double now)
        {
            if (Result != null || now >= Deadline || !items.TryGetValue(item.Id, out var registered) || !ReferenceEquals(registered, item)) return false;
            bool eligible = item.Lifecycle == Lifecycle.Available && item.Handling == Handling.Free && item.Integrity >= config.IntegrityMin &&
                allCornersInside && Numbers.Finite(linearSpeed) && linearSpeed >= 0 && linearSpeed < config.DeliverySpeed &&
                Numbers.Finite(angularDegrees) && angularDegrees >= 0 && angularDegrees < config.DeliveryAngularDegrees;
            if (!eligible) { dwellStart.Remove(item.Id); return false; }
            if (!dwellStart.TryGetValue(item.Id, out var since)) { dwellStart[item.Id] = now; return false; }
            if (now - since + 1e-9 < config.DeliveryDwell || ledger.ContainsKey(item.Id)) return false;
            long value = (long)Math.Floor(item.BaseValue * item.Integrity / 100.0);
            long next = checked(ProvisionalValue + value);
            item.Deliver(); ledger.Add(item.Id, value); ProvisionalValue = next; dwellStart.Remove(item.Id); return true;
        }
        public void RecoveryPenalty(double penalty) { if (!Numbers.Finite(penalty) || penalty < 0) throw new ArgumentException("penalty"); if (Result == null) Deadline -= penalty; }
        public Reason Vote(int actor, int host, IEnumerable<int> connected, double now)
        {
            if (Result != null || !Success) return Reason.InvalidState;
            if (voters == null)
            {
                if (now < nextVote) return Reason.Busy;
                voters = new HashSet<int>(connected); votes = new HashSet<int>(); voteEnd = now + config.VoteSeconds;
            }
            if (now >= voteEnd) { voters = null; votes = null; nextVote = now + config.VoteCooldown; return Reason.InvalidState; }
            if (!voters.Contains(actor)) return Reason.AuthFailed;
            votes.Add(actor);
            if (votes.Contains(host) && votes.Count > voters.Count / 2) { Finish(FinishReason.Vote); return Reason.Accepted; }
            return Reason.VoteRequired;
        }
        public RunResult Finish(FinishReason reason)
        {
            if (Result != null) return Result;
            if (reason == FinishReason.Vote && !Success) throw new InvalidOperationException("Invalid finish vote");
            bool success = reason != FinishReason.Abort && Success;
            Result = new RunResult(Id, reason == FinishReason.Abort ? 0 : checked(ProvisionalValue + (success ? bonus : 0)), success, reason);
            foreach (var item in items.Values) item.ClearAttachments();
            return Result;
        }
    }
}
