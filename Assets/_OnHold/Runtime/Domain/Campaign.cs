using System;
using System.Collections.Generic;
using System.Linq;
using OnHold.Core;

namespace OnHold.Domain
{
    public sealed class Campaign
    {
        public const int CurrentSchema = 2;
        public int SchemaVersion = CurrentSchema;
        public string CampaignId = Guid.NewGuid().ToString("N");
        public string ContentVersion = "foundation.1";
        public long Revision, Balance;
        public List<Settlement> CompletedRuns = new List<Settlement>();
        public List<Purchase> Purchases = new List<Purchase>();
        public List<string> UnlockedContracts = new List<string> { "contract.foundation.01" };
        public Dictionary<string, int> Upgrades = new Dictionary<string, int>();
        public Campaign Clone() => new Campaign { SchemaVersion = SchemaVersion, CampaignId = CampaignId, ContentVersion = ContentVersion,
            Revision = Revision, Balance = Balance, CompletedRuns = new List<Settlement>(CompletedRuns), Purchases = new List<Purchase>(Purchases),
            UnlockedContracts = new List<string>(UnlockedContracts), Upgrades = new Dictionary<string, int>(Upgrades) };
        public void Validate()
        {
            if (SchemaVersion != CurrentSchema || !Numbers.Id(CampaignId) || ContentVersion != "foundation.1" || Revision < 0 || Balance < 0 || Balance > 1000000000000L ||
                CompletedRuns == null || Purchases == null || UnlockedContracts == null || Upgrades == null || CompletedRuns.Count > 100000 || Purchases.Count > 10000 ||
                CompletedRuns.Any(r => r == null || !Numbers.Id(r.RunId) || r.TransactionId != "settle." + r.RunId || r.Payout < 0 || r.Payout > 1000000000000L) ||
                CompletedRuns.Select(r => r.RunId).Distinct().Count() != CompletedRuns.Count ||
                Purchases.Any(p => p == null || !Numbers.Id(p.TransactionId) || p.UpgradeId != "upgrade.winch.1" || p.Price <= 0 || p.Level != 1) ||
                Purchases.Select(p => p.TransactionId).Distinct().Count() != Purchases.Count ||
                Purchases.Select(p => p.UpgradeId).Distinct().Count() != Purchases.Count ||
                UnlockedContracts.Count != 1 || UnlockedContracts[0] != "contract.foundation.01" ||
                Upgrades.Any(p => p.Key != "upgrade.winch.1" || p.Value != 1) ||
                Upgrades.Count != Purchases.Count || Purchases.Any(p => !Upgrades.ContainsKey(p.UpgradeId)) ||
                Revision != CompletedRuns.Count + Purchases.Count)
                throw new ArgumentException("Invalid campaign schema, references, revision or ledger");
        }
        public Campaign Settle(RunResult result)
        {
            Validate();
            var old = CompletedRuns.FirstOrDefault(r => r.RunId == result.RunId);
            if (old != null)
            {
                if (old.Payout != result.Payout || old.Success != result.Success) throw new InvalidOperationException("Conflicting run result");
                return this;
            }
            var next = Clone(); next.Balance = checked(Balance + result.Payout); next.Revision++;
            next.CompletedRuns.Add(new Settlement { RunId = result.RunId, TransactionId = "settle." + result.RunId, Payout = result.Payout, Success = result.Success }); next.Validate(); return next;
        }
        // Prices come from adopted host content. No paid upgrade is exposed until a price is adopted.
        public Reason Buy(string transactionId, UpgradeDefinition definition, long expectedRevision, bool isHost, SessionPhase phase, out Campaign next)
        {
            next = this; Validate();
            if (!isHost) return Reason.AuthFailed;
            if (phase != SessionPhase.Hub) return Reason.InvalidState;
            if (!Numbers.Id(transactionId) || definition == null || definition.Id != "upgrade.winch.1" || definition.Price <= 0 || definition.Price > 1000000000) return Reason.InvalidContent;
            var old = Purchases.FirstOrDefault(p => p.TransactionId == transactionId);
            if (old != null) return old.UpgradeId == definition.Id && old.Price == definition.Price ? Reason.AlreadyApplied : Reason.InvalidPayload;
            if (expectedRevision != Revision) return Reason.StaleRevision;
            if (Upgrades.ContainsKey(definition.Id)) return Reason.AlreadyApplied;
            if (Balance < definition.Price) return Reason.InsufficientFunds;
            next = Clone(); next.Balance -= definition.Price; next.Upgrades.Add(definition.Id, 1); next.Revision++;
            next.Purchases.Add(new Purchase { TransactionId = transactionId, UpgradeId = definition.Id, Price = definition.Price, Level = 1 }); next.Validate(); return Reason.Accepted;
        }
    }
    public sealed class Settlement { public string RunId, TransactionId; public long Payout; public bool Success; }
    public sealed class Purchase { public string TransactionId, UpgradeId; public long Price; public int Level; }
    public sealed class UpgradeDefinition { public string Id; public long Price; }
}
