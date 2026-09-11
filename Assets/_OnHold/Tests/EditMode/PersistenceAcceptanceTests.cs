using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OnHold.Core;
using OnHold.Domain;
using OnHold.Persistence;

namespace OnHold.Tests
{
    public sealed class PersistenceAcceptanceTests
    {
        string directory, path;
        [SetUp] public void SetUp() { directory = Path.Combine(Path.GetTempPath(), "onhold-test-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory); path = Path.Combine(directory, "campaign.json"); }
        [TearDown] public void TearDown() { if (Directory.Exists(directory)) Directory.Delete(directory, true); }
        RunResult Result(string id = "run.a")
        {
            var c = new FoundationConfig(); var item = new ItemState("cargo.a", "item.a", 10, 100);
            var r = new ContractRun(id, new[] { item }, new[] { item.Id }, 1, 100, 0, 600, c);
            r.EvaluateDelivery(item, true, 0, 0, 0); r.EvaluateDelivery(item, true, 0, 0, 1); return r.Finish(FinishReason.Timeout);
        }
        [Test] public void TC018_ResultRepeatedOneHundredTimesAcrossReloadsPaysOnce()
        {
            var result = Result();
            for (int i = 0; i < 100; i++) Assert.AreEqual(200, new CampaignStore(path).Settle(result).Balance);
            var loaded = new CampaignStore(path).Load(); Assert.AreEqual(1, loaded.Revision); Assert.AreEqual(1, loaded.CompletedRuns.Count);
        }
        [Test] public void CurrentCheckpointReplacesInitializedCollectionDefaults()
        {
            var original = new Campaign();
            var loaded = CampaignStore.Decode(CampaignStore.Encode(original));
            Assert.AreEqual(original.CampaignId, loaded.CampaignId);
            CollectionAssert.AreEqual(original.UnlockedContracts, loaded.UnlockedContracts);
            Assert.AreEqual(1, loaded.UnlockedContracts.Count);
        }
        [TestCase(CommitBoundary.AfterTempWrite)] [TestCase(CommitBoundary.AfterFlush)] [TestCase(CommitBoundary.BeforeReplace)] [TestCase(CommitBoundary.AfterReplace)]
        public void TC018_InterruptionBoundaryPreservesOldOrNewAndRetrySameId(CommitBoundary boundary)
        {
            var store = new CampaignStore(path); store.Settle(Result("run.first"));
            store.Fault = b => { if (b == boundary) throw new IOException("Injected " + b); };
            Assert.Throws<IOException>(() => store.Settle(Result("run.second")));
            var restart = new CampaignStore(path); Assert.AreEqual(boundary == CommitBoundary.AfterReplace ? 400 : 200, restart.Load().Balance);
            Assert.AreEqual(400, restart.Settle(Result("run.second")).Balance); Assert.AreEqual(400, restart.Settle(Result("run.second")).Balance);
        }
        [Test] public void TC018_CorruptPrimaryDoesNotWipeAndBackupRequiresExplicitRestore()
        {
            var store = new CampaignStore(path); store.Settle(Result("run.first")); store.Settle(Result("run.second")); File.WriteAllText(path, "corrupt");
            Assert.Throws<CheckpointException>(() => store.Load()); Assert.AreEqual("corrupt", File.ReadAllText(path));
            Assert.AreEqual(200, store.RestoreBackup().Balance); Assert.AreEqual(1, Directory.GetFiles(directory, "*.preserved.*").Length);
        }
        [Test] public void TC018_BothCorruptFilesRemainUntouched()
        {
            File.WriteAllText(path, "bad-primary"); File.WriteAllText(path + ".bak", "bad-backup");
            var store = new CampaignStore(path); Assert.Throws<CheckpointException>(() => store.Load()); Assert.Catch(() => store.RestoreBackup());
            Assert.AreEqual("bad-primary", File.ReadAllText(path)); Assert.AreEqual("bad-backup", File.ReadAllText(path + ".bak"));
        }
        [Test] public void TC018_FutureSchemaCannotBeOverwrittenOrDowngradedFromBackup()
        {
            var store = new CampaignStore(path); store.Settle(Result("run.first")); store.Settle(Result("run.second"));
            File.WriteAllText(path, "{\"schemaVersion\":999}");
            Assert.Throws<FutureSchemaException>(() => store.Load()); Assert.Throws<FutureSchemaException>(() => store.Settle(Result())); Assert.Throws<FutureSchemaException>(() => store.RestoreBackup());
            Assert.AreEqual("{\"schemaVersion\":999}", File.ReadAllText(path));
        }
        [Test] public void TC018_V1MigrationIsIdempotentAndPreservesOriginalUntilCommit()
        {
            var source = JObject.FromObject(new Campaign()); source.Remove("SchemaVersion"); source.Remove("Purchases"); source["schemaVersion"] = 1;
            string original = source.ToString(); File.WriteAllText(path, original);
            var store = new CampaignStore(path); Assert.AreEqual(2, store.Load().SchemaVersion); Assert.AreEqual(2, store.Load().SchemaVersion); Assert.AreEqual(original, File.ReadAllText(path));
            store.Settle(Result()); Assert.AreEqual(original, File.ReadAllText(path + ".bak")); Assert.AreEqual(200, store.Load().Balance);
        }
        [Test] public void TC018_ChecksumAndContentReferencesValidated()
        {
            string valid = CampaignStore.Encode(new Campaign()); var envelope = JObject.Parse(valid); envelope["sha256"] = "wrong";
            Assert.Throws<CheckpointException>(() => CampaignStore.Decode(envelope.ToString()));
            var campaign = new Campaign(); campaign.UnlockedContracts[0] = "arbitrary.file"; Assert.Throws<ArgumentException>(campaign.Validate);
            campaign = new Campaign { Balance = -1 }; Assert.Throws<ArgumentException>(campaign.Validate);
            campaign = new Campaign { Revision = 4 }; Assert.Throws<ArgumentException>(campaign.Validate);
        }
        [Test] public void TC019_PurchaseUsesHostCatalogHubAndDurableIdentity()
        {
            var store = new CampaignStore(path); store.Settle(Result()); var upgrade = new UpgradeDefinition { Id = "upgrade.winch.1", Price = 100 };
            var value = store.Buy("purchase.a", upgrade, 1, false, SessionPhase.Hub, out var reason); Assert.AreEqual(Reason.AuthFailed, reason); Assert.AreEqual(200, value.Balance);
            store.Buy("purchase.a", upgrade, 1, true, SessionPhase.Active, out reason); Assert.AreEqual(Reason.InvalidState, reason);
            value = store.Buy("purchase.a", upgrade, 1, true, SessionPhase.Hub, out reason); Assert.AreEqual(Reason.Accepted, reason); Assert.AreEqual(100, value.Balance);
            value = new CampaignStore(path).Buy("purchase.a", upgrade, 1, true, SessionPhase.Hub, out reason); Assert.AreEqual(Reason.AlreadyApplied, reason); Assert.AreEqual(100, value.Balance);
            Assert.Contains("contract.foundation.01", value.UnlockedContracts);
        }
        [Test] public void TC019_NegativePriceInsufficientFundsAndOverflowRejected()
        {
            var c = new Campaign(); var upgrade = new UpgradeDefinition { Id = "upgrade.winch.1", Price = -1 };
            Assert.AreEqual(Reason.InvalidContent, c.Buy("purchase.a", upgrade, 0, true, SessionPhase.Hub, out _));
            upgrade.Price = 100; Assert.AreEqual(Reason.InsufficientFunds, c.Buy("purchase.a", upgrade, 0, true, SessionPhase.Hub, out _));
            c.Balance = 1000000000000; Assert.Throws<ArgumentException>(() => c.Settle(Result()));
        }
        [Test] public void TC019_WriteFailureDoesNotSplitMoneyAndEquipment()
        {
            var store = new CampaignStore(path); store.Settle(Result()); store.Fault = b => { if (b == CommitBoundary.BeforeReplace) throw new IOException("Injected"); };
            Assert.Throws<IOException>(() => store.Buy("purchase.a", new UpgradeDefinition { Id = "upgrade.winch.1", Price = 100 }, 1, true, SessionPhase.Hub, out _));
            var loaded = new CampaignStore(path).Load(); Assert.AreEqual(200, loaded.Balance); Assert.AreEqual(0, loaded.Upgrades.Count);
        }
    }
}
