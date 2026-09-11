using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using OnHold.Core;
using OnHold.Domain;
using OnHold.Gameplay;

namespace OnHold.Networking
{
    [Serializable] sealed class Hello { public int protocol; public string build, content, ticket; }
    [Serializable] sealed class Welcome { public int actor; public uint generation; public ulong epoch; public string ticket; }
    public sealed class NetworkSession : MonoBehaviour
    {
        public FoundationWorld World;
        public NetworkManager Manager;
        public UnityTransport Transport;
        public const string BuildVersion = "foundation.1";
        public Actor LocalActor { get; private set; }
        public ulong Epoch { get; private set; }
        public string Status { get; private set; } = "Choose Relay or local connection.";
        public string JoinCode { get; private set; } = "";
        public string Profile = "N0";
        public bool Busy { get; private set; }
        public bool Connected => Manager != null && Manager.IsConnectedClient;
        public bool IsHost => Manager != null && Manager.IsHost;
        public long SentBytes { get; private set; }
        public long ReceivedBytes { get; private set; }
        public ulong Rtt => Connected ? Transport.GetCurrentRtt(NetworkManager.ServerClientId) : 0;
        public int Rejects => gates.Values.Sum(g => g.Rejected);
        public int GateCount => gates.Count;
        public Reason LastReason { get; private set; } = Reason.Accepted;
        public event Action<Reason> Rejected;
        readonly Dictionary<ulong, SessionMember> clients = new Dictionary<ulong, SessionMember>();
        readonly Dictionary<ulong, CommandGate> gates = new Dictionary<ulong, CommandGate>();
        string ticket = "";
        ulong sequence;
        int operation;
        bool registered, stopping;
        double connectionStarted;
        string lastSemantic = "";
        void Awake()
        {
            World.Initialize(); Manager.NetworkConfig.NetworkTransport = Transport;
            Manager.NetworkConfig.EnableSceneManagement = false; Manager.NetworkConfig.ConnectionApproval = true;
            Manager.NetworkConfig.TickRate = (uint)World.Config.SnapshotHz;
            Manager.NetworkConfig.ClientConnectionBufferTimeout = 10;
            Manager.ConnectionApprovalCallback = Approve;
            Manager.OnClientConnectedCallback += OnConnected;
            Manager.OnClientDisconnectCallback += OnDisconnected;
            World.SnapshotReady += Broadcast;
            Transport.DisconnectTimeoutMS = 3000;
            Transport.MaxConnectAttempts = 5; Transport.ConnectTimeoutMS = 1000;
        }
        public async Task StartConnection(bool host, bool relay, OnHold.Domain.SessionMode mode, string addressOrCode, ushort port = 7777, string authProfile = "default")
        {
            if (Busy || Manager.IsListening) return;
            Busy = true; int current = ++operation; connectionStarted = Time.realtimeSinceStartupAsDouble;
            try
            {
                Status = relay ? "Connecting to Unity Relay…" : "Connecting locally…";
                if (relay)
                {
                    if (UnityServices.State != ServicesInitializationState.Initialized)
                        await UnityServices.InitializeAsync(new InitializationOptions().SetProfile(authProfile));
                    if (current != operation) return;
                    if (!AuthenticationService.Instance.IsSignedIn) await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    if (current != operation) return;
                    if (host)
                    {
                        var allocation = await RelayService.Instance.CreateAllocationAsync(3);
                        if (current != operation) return;
                        JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                        if (current != operation) return;
                        Transport.SetRelayServerData(allocation.ToRelayServerData("dtls"));
                    }
                    else
                    {
                        var allocation = await RelayService.Instance.JoinAllocationAsync(addressOrCode.Trim().ToUpperInvariant());
                        if (current != operation) return;
                        Transport.SetRelayServerData(allocation.ToRelayServerData("dtls")); JoinCode = addressOrCode.Trim().ToUpperInvariant();
                    }
                }
                else Transport.SetConnectionData(addressOrCode, port, "127.0.0.1");
                if (current != operation) return;
                UnityTransport.s_DriverConstructor = Profile == "N0" ? null : new EmulatedDriver(Profile);
                if (host)
                {
                    ticket = ""; Epoch = BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0) | 1;
                    World.StartAuthority(Epoch, mode);
                }
                else World.StartGuest();
                Manager.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(new Hello { protocol = CommandGate.Protocol, build = BuildVersion, content = World.ContentHash, ticket = ticket }));
                bool started = host ? Manager.StartHost() : Manager.StartClient();
                if (!started) throw new InvalidOperationException("Transport did not start");
                RegisterMessages();
                Status = host ? "Lobby ready. Share the Relay code with your test partner." : "Waiting for host approval…";
            }
            catch (Exception ex)
            {
                // Service exception messages can include URLs/tokens; only the type is logged.
                if (current == operation) { Status = "Connection failed (" + ex.GetType().Name + "). Check services and connection details, then retry."; Stop(false); }
            }
            finally { if (current == operation) Busy = false; }
        }
        void RegisterMessages()
        {
            if (registered) return; registered = true;
            Manager.CustomMessagingManager.RegisterNamedMessageHandler("oh.command", CommandReceived);
            Manager.CustomMessagingManager.RegisterNamedMessageHandler("oh.frame", FrameReceived);
            Manager.CustomMessagingManager.RegisterNamedMessageHandler("oh.welcome", WelcomeReceived);
            Manager.CustomMessagingManager.RegisterNamedMessageHandler("oh.reason", ReasonReceived);
        }
        void Approve(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = false; response.CreatePlayerObject = false; response.Pending = false;
            Reason reason = Reason.InvalidPayload;
            if (request.Payload != null && request.Payload.Length <= 512)
            {
                try
                {
                    var hello = JsonUtility.FromJson<Hello>(Encoding.UTF8.GetString(request.Payload));
                    if (hello == null || hello.ticket == null || hello.ticket.Length > 64) reason = Reason.InvalidPayload;
                    else if (hello.protocol != CommandGate.Protocol || hello.build != BuildVersion) reason = Reason.BuildMismatch;
                    else if (hello.content != World.ContentHash) reason = Reason.ContentMismatch;
                    else reason = World.Session.Join(hello.ticket, request.ClientNetworkId == NetworkManager.ServerClientId, World.Now, out var member) == Reason.Accepted
                        ? RegisterClient(request.ClientNetworkId, member) : JoinReason(hello.ticket);
                }
                catch (ArgumentException) { reason = Reason.InvalidPayload; }
            }
            response.Approved = reason == Reason.Accepted; response.Reason = reason.ToString();
        }
        Reason JoinReason(string value)
        {
            // Only used after a failed Join (which leaves state unchanged).
            return World.Session.Join(value, false, World.Now, out _);
        }
        Reason RegisterClient(ulong id, SessionMember member) { clients[id] = member; gates[id] = new CommandGate(); return Reason.Accepted; }
        void OnConnected(ulong id)
        {
            if (!Manager.IsServer) return;
            if (!clients.TryGetValue(id, out var member)) return;
            World.AddPlayer(member.Actor);
            var welcome = new Welcome { actor = member.Actor.Id, generation = member.Actor.Generation, epoch = World.Session.Epoch, ticket = member.Ticket };
            if (id == NetworkManager.ServerClientId) ApplyWelcome(welcome);
            else
            {
                SendJson("oh.welcome", id, JsonUtility.ToJson(welcome), NetworkDelivery.ReliableSequenced);
                SendJson("oh.frame", id, JsonUtility.ToJson(World.Capture()), NetworkDelivery.ReliableFragmentedSequenced);
            }
            UnityEngine.Debug.Log("ONHOLD connected actors=" + World.Players.Count + " tick=" + World.Tick);
        }
        void ApplyWelcome(Welcome w) { LocalActor = new Actor(w.actor, w.generation); Epoch = w.epoch; ticket = w.ticket; sequence = 0; Status = "Connected. Ready for briefing."; }
        void OnDisconnected(ulong id)
        {
            if (stopping) return;
            if (World.IsAuthority && clients.TryGetValue(id, out var member))
            {
                ulong detected = World.Tick;
                World.Session.Disconnect(member.Actor, World.Now, World.Config.ReconnectSeconds, World.RemovePlayer);
                clients.Remove(id); gates.Remove(id);
                UnityEngine.Debug.Log("ONHOLD disconnect detectedTick=" + detected + " cleanupTick=" + World.Tick + " holders=" + World.ActiveHandles);
            }
            else if (!World.IsAuthority)
            {
                string reason = Manager.DisconnectReason;
                Status = string.IsNullOrEmpty(reason) ? "Host connection lost. Unsaved run progress is unavailable. Last committed campaign is preserved." : "Connection rejected: " + reason;
                Stop(false);
            }
        }
        public void Send(CommandType type, int target = 0, uint revision = 0, Vector3 input = default, float yaw = 0, float pitch = 0)
        {
            if (!Connected || LocalActor.Id == 0) return;
            var command = new CommandEnvelope { ProtocolVersion = CommandGate.Protocol, SessionEpoch = Epoch, ConnectionGeneration = LocalActor.Generation,
                Sequence = ++sequence, RequestId = sequence, TargetId = target, ExpectedRevision = revision, CommandType = type,
                X = input.x, Y = input.y, Z = input.z, Yaw = yaw, Pitch = pitch };
            if (IsHost) QueueCommand(NetworkManager.ServerClientId, command, 88);
            else
            {
                int size = System.Runtime.InteropServices.Marshal.SizeOf<CommandEnvelope>();
                using var writer = new FastBufferWriter(size, Allocator.Temp);
                unsafe { byte* ptr = (byte*)Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf(ref command); writer.WriteBytesSafe(ptr, size); }
                SentBytes += writer.Length;
                var delivery = type == CommandType.Move || type == CommandType.Winch ? NetworkDelivery.UnreliableSequenced : NetworkDelivery.ReliableSequenced;
                Manager.CustomMessagingManager.SendNamedMessage("oh.command", NetworkManager.ServerClientId, writer, delivery);
            }
        }
        void CommandReceived(ulong sender, FastBufferReader reader)
        {
            if (!Manager.IsServer || !gates.ContainsKey(sender)) return;
            ReceivedBytes += reader.Length;
            int envSize = System.Runtime.InteropServices.Marshal.SizeOf<CommandEnvelope>();
            if (reader.Length > CommandGate.MaxPayload || !reader.TryBeginRead(envSize)) { Flag(sender, Reason.InvalidPayload); return; }
            CommandEnvelope command = default;
            unsafe { byte* ptr = (byte*)Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf(ref command); reader.ReadBytesSafe(ptr, envSize); }
            QueueCommand(sender, command, reader.Length);
        }
        void QueueCommand(ulong sender, CommandEnvelope command, int size)
        {
            if (!World.Enqueue(() =>
            {
                if (!clients.TryGetValue(sender, out var member) || !gates.TryGetValue(sender, out var gate)) return;
                var result = gate.Execute(command, size, member.Actor, World.Session.Epoch, World.Now, World.Apply);
                if (result != Reason.Accepted && result != Reason.AlreadyApplied) Flag(sender, result);
                if (gate.ShouldDisconnect && sender != NetworkManager.ServerClientId) Manager.DisconnectClient(sender, "Sustained invalid or excessive input");
            })) Flag(sender, Reason.RateLimited);
        }
        readonly Dictionary<ulong, double> lastWarning = new Dictionary<ulong, double>();
        void Flag(ulong sender, Reason reason)
        {
            if (sender == NetworkManager.ServerClientId) { LastReason = reason; Rejected?.Invoke(reason); return; }
            if (lastWarning.TryGetValue(sender, out var at) && World.Now - at < .2) return;
            lastWarning[sender] = World.Now;
            using var writer = new FastBufferWriter(4, Allocator.Temp); writer.WriteValueSafe((int)reason);
            Manager.CustomMessagingManager.SendNamedMessage("oh.reason", sender, writer, NetworkDelivery.ReliableSequenced);
        }
        void ReasonReceived(ulong sender, FastBufferReader reader)
        {
            if (sender != NetworkManager.ServerClientId || reader.Length != 4) return;
            reader.ReadValueSafe(out int code); if (!Enum.IsDefined(typeof(Reason), code)) return;
            LastReason = (Reason)code; Rejected?.Invoke(LastReason);
        }
        void FrameReceived(ulong sender, FastBufferReader reader)
        {
            if (Manager.IsServer || sender != NetworkManager.ServerClientId) return;
            string json = ReadJson(reader, 16384); if (json == null) return;
            try { var frame = JsonUtility.FromJson<WorldFrame>(json); if (frame.Epoch == Epoch) World.Receive(frame); } catch (ArgumentException) { Status = "Invalid host snapshot"; }
        }
        void WelcomeReceived(ulong sender, FastBufferReader reader)
        {
            if (Manager.IsServer || sender != NetworkManager.ServerClientId) return;
            string json = ReadJson(reader, 512); if (json == null) return;
            try { var w = JsonUtility.FromJson<Welcome>(json); if (w.actor > 0 && w.generation > 0 && w.epoch != 0 && w.ticket?.Length == 64) ApplyWelcome(w); } catch (ArgumentException) { Status = "Invalid handshake"; }
        }
        string ReadJson(FastBufferReader reader, int bound)
        {
            if (reader.Length < 4 || reader.Length > bound) return null;
            reader.ReadValueSafe(out int count); if (count < 0 || count != reader.Length - 4) return null;
            var bytes = new byte[count]; reader.ReadBytesSafe(ref bytes, count); ReceivedBytes += reader.Length; return Encoding.UTF8.GetString(bytes);
        }
        void SendJson(string message, ulong client, string json, NetworkDelivery delivery)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            using var writer = new FastBufferWriter(bytes.Length + 4, Allocator.Temp);
            writer.WriteValueSafe(bytes.Length); writer.WriteBytesSafe(bytes); SentBytes += writer.Length;
            Manager.CustomMessagingManager.SendNamedMessage(message, client, writer, delivery);
        }
        void Broadcast(WorldFrame frame)
        {
            if (!Manager.IsServer || !Manager.IsListening) return;
            var signature = frame.Phase + ":" + frame.Connections + ":" + frame.Driver + ":" + frame.CampaignRevision + ":" + string.Join(",", frame.Items.Select(i => i.Revision));
            var mode = signature != lastSemantic ? NetworkDelivery.ReliableFragmentedSequenced : NetworkDelivery.Unreliable;
            lastSemantic = signature; string json = JsonUtility.ToJson(frame);
            foreach (var client in Manager.ConnectedClientsIds) if (client != NetworkManager.ServerClientId) SendJson("oh.frame", client, json, mode == NetworkDelivery.Unreliable ? NetworkDelivery.Unreliable : mode);
        }
        void Update()
        {
            if ((Busy || Manager.IsListening && !Connected) && Time.realtimeSinceStartupAsDouble - connectionStarted > 20)
            { Status = "Connection timed out. Check the host and Relay code, then retry."; Stop(false); }
        }
        public void Stop(bool clearTicket = true)
        {
            if (stopping) return; stopping = true; operation++; Busy = false;
            if (registered && Manager.CustomMessagingManager != null)
                foreach (var key in new[] { "oh.command", "oh.frame", "oh.welcome", "oh.reason" }) Manager.CustomMessagingManager.UnregisterNamedMessageHandler(key);
            registered = false; if (Manager.IsListening) Manager.Shutdown();
            World.StopSession(); clients.Clear(); gates.Clear(); lastWarning.Clear(); UnityTransport.s_DriverConstructor = null;
            LocalActor = default; if (clearTicket) ticket = ""; stopping = false;
        }
        void OnDestroy()
        {
            Stop(); Manager.ConnectionApprovalCallback = null; Manager.OnClientConnectedCallback -= OnConnected;
            Manager.OnClientDisconnectCallback -= OnDisconnected; World.SnapshotReady -= Broadcast;
        }
    }
}
