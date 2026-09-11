using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnHold.Domain;

namespace OnHold.Persistence
{
    public enum CommitBoundary { AfterTempWrite, AfterFlush, BeforeReplace, AfterReplace }
    public sealed class FutureSchemaException : IOException { public FutureSchemaException() : base("This checkpoint requires a newer build. It has not been changed.") { } }
    public sealed class CheckpointException : IOException { public CheckpointException(string message) : base(message) { } }
    public sealed class CampaignStore
    {
        // All store instances serialize writes, including multiple UI retry paths.
        static readonly object WriteLock = new object();
        readonly string path;
        public Action<CommitBoundary> Fault;
        const int MaxBytes = 16 * 1024 * 1024;
        // A checkpoint replaces collection defaults; populating them would duplicate the
        // always-unlocked foundation contract on every round trip and invalidate the save.
        static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error, TypeNameHandling = TypeNameHandling.None, ObjectCreationHandling = ObjectCreationHandling.Replace, MaxDepth = 16 };
        public CampaignStore(string path) { this.path = Path.GetFullPath(path); }
        public Campaign Load()
        {
            lock (WriteLock)
            {
                if (!File.Exists(path))
                {
                    if (File.Exists(path + ".bak")) throw new CheckpointException("Primary checkpoint missing. A backup exists; use Restore backup.");
                    return new Campaign();
                }
                try { return Decode(ReadBounded(path)); }
                catch (FutureSchemaException) { throw; }
                catch (Exception ex) when (ex is JsonException || ex is ArgumentException || ex is IOException || ex is FormatException || ex is OverflowException)
                { throw new CheckpointException("Checkpoint is unreadable. Original preserved. Restore a validated backup or export files for recovery."); }
            }
        }
        public Campaign Settle(RunResult result)
        {
            lock (WriteLock)
            {
                var current = Load(); var next = current.Settle(result);
                if (!ReferenceEquals(current, next)) Commit(next);
                return next;
            }
        }
        public Campaign Buy(string transaction, UpgradeDefinition definition, long expectedRevision, bool isHost, SessionPhase phase, out OnHold.Core.Reason reason)
        {
            lock (WriteLock)
            {
                var current = Load(); reason = current.Buy(transaction, definition, expectedRevision, isHost, phase, out var next);
                if (reason == OnHold.Core.Reason.Accepted) Commit(next);
                return next;
            }
        }
        public void Commit(Campaign next)
        {
            lock (WriteLock)
            {
                next.Validate();
                if (File.Exists(path))
                {
                    var previous = Load();
                    if (previous.CampaignId != next.CampaignId || next.Revision != previous.Revision + 1) throw new CheckpointException("Stale checkpoint write");
                }
                AtomicWrite(Encode(next));
            }
        }
        void AtomicWrite(string text)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string temporary = path + ".pending";
            var bytes = Encoding.UTF8.GetBytes(text);
            using (var stream = new FileStream(temporary, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                stream.Write(bytes, 0, bytes.Length); Fault?.Invoke(CommitBoundary.AfterTempWrite);
                stream.Flush(true); Fault?.Invoke(CommitBoundary.AfterFlush);
            }
            Decode(ReadBounded(temporary)); Fault?.Invoke(CommitBoundary.BeforeReplace);
            if (File.Exists(path)) File.Replace(temporary, path, path + ".bak");
            else File.Move(temporary, path);
            Fault?.Invoke(CommitBoundary.AfterReplace);
        }
        public Campaign RestoreBackup()
        {
            lock (WriteLock)
            {
                // Never use backup restoration to downgrade a future primary.
                if (File.Exists(path))
                {
                    try { Decode(ReadBounded(path)); }
                    catch (FutureSchemaException) { throw; }
                    catch (Exception ex) when (ex is JsonException || ex is IOException || ex is ArgumentException || ex is FormatException || ex is OverflowException) { }
                }
                var backup = Decode(ReadBounded(path + ".bak")); backup.Validate();
                if (File.Exists(path)) File.Copy(path, path + ".preserved." + Guid.NewGuid().ToString("N"), false);
                string recovery = path + ".restore";
                using (var s = new FileStream(recovery, FileMode.Create, FileAccess.Write, FileShare.None)) { var bytes = Encoding.UTF8.GetBytes(Encode(backup)); s.Write(bytes, 0, bytes.Length); s.Flush(true); }
                if (File.Exists(path)) File.Replace(recovery, path, null); else File.Move(recovery, path);
                return Load();
            }
        }
        static string ReadBounded(string file)
        {
            if (new FileInfo(file).Length > MaxBytes) throw new CheckpointException("Checkpoint exceeds supported size");
            return File.ReadAllText(file, Encoding.UTF8);
        }
        public static string Encode(Campaign value)
        {
            value.Validate(); string payload = JsonConvert.SerializeObject(value, Formatting.None, JsonSettings);
            return new JObject { ["schemaVersion"] = Campaign.CurrentSchema, ["sha256"] = Hash(payload), ["payload"] = payload }.ToString(Formatting.Indented);
        }
        public static Campaign Decode(string text)
        {
            if (text == null || text.Length > MaxBytes) throw new CheckpointException("Invalid checkpoint size");
            JObject obj;
            using (var reader = new JsonTextReader(new StringReader(text)) { MaxDepth = 16, DateParseHandling = DateParseHandling.None })
                obj = JObject.Load(reader, new JsonLoadSettings { DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error });
            int schema = obj.Value<int?>("schemaVersion") ?? throw new CheckpointException("Missing schema");
            if (schema > Campaign.CurrentSchema) throw new FutureSchemaException();
            Campaign value;
            if (schema == 1)
            {
                // Explicit v1 fixture: same campaign fields; no durable Hub purchases existed.
                obj.Remove("schemaVersion"); obj["SchemaVersion"] = Campaign.CurrentSchema;
                obj["Purchases"] = new JArray();
                value = JsonConvert.DeserializeObject<Campaign>(obj.ToString(), JsonSettings);
            }
            else if (schema == 2)
            {
                if (obj.Count != 3) throw new CheckpointException("Unexpected envelope fields");
                string payload = obj.Value<string>("payload");
                if (payload == null || Hash(payload) != obj.Value<string>("sha256")) throw new CheckpointException("Checksum mismatch");
                value = JsonConvert.DeserializeObject<Campaign>(payload, JsonSettings);
            }
            else throw new CheckpointException("Unsupported schema");
            if (value == null) throw new CheckpointException("Empty checkpoint"); value.Validate(); return value;
        }
        public static string Hash(string text) { using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "").ToLowerInvariant(); }
    }
    [Serializable]
    public sealed class LocalSettings
    {
        public float Sensitivity = .12f, FieldOfView = 80, UiScale = 1, MasterVolume = .5f;
        public bool ToggleHold, InvertY;
        public string Language = "en", InputOverrides = "";
        public void Validate()
        {
            if (!OnHold.Core.Numbers.Finite(Sensitivity) || Sensitivity < .01 || Sensitivity > 1 || !OnHold.Core.Numbers.Finite(FieldOfView) || FieldOfView < 70 || FieldOfView > 100 ||
                !OnHold.Core.Numbers.Finite(UiScale) || UiScale < .8 || UiScale > 1.5 || !OnHold.Core.Numbers.Finite(MasterVolume) || MasterVolume < 0 || MasterVolume > 1 ||
                (Language != "en" && Language != "tr") || InputOverrides == null || InputOverrides.Length > 20000) throw new ArgumentException("settings");
        }
    }
}
