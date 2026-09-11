using System;
using Unity.Collections;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Utilities;

namespace OnHold.Networking
{
    // ASK-035 v0.1 family, send-side per endpoint. Both peers use the same profile.
    public sealed class EmulatedDriver : INetworkStreamDriverConstructor
    {
        readonly int delay, jitter, loss;
        public EmulatedDriver(string profile)
        {
            switch (profile)
            {
                case "N1": delay = 25; jitter = 15; loss = 0; break;
                case "N2": delay = 50; jitter = 15; loss = 1; break;
                case "N3": delay = 75; jitter = 40; loss = 1; break;
                case "N4": delay = 125; jitter = 40; loss = 5; break;
                default: throw new ArgumentException("Unknown v0.1 profile");
            }
        }
        public void CreateDriver(UnityTransport transport, out NetworkDriver driver, out NetworkPipeline unreliable, out NetworkPipeline sequenced, out NetworkPipeline reliable)
        {
            var settings = transport.GetDefaultNetworkSettings();
            settings.WithSimulatorStageParameters(400, mode: ApplyMode.SentPacketsOnly, packetDelayMs: delay,
                packetJitterMs: jitter, packetDropPercentage: loss, randomSeed: 9042026);
            driver = NetworkDriver.Create(new UDPNetworkInterface(), settings);
            transport.GetDefaultPipelineConfigurations(out var a, out var b, out var c);
            var simId = NetworkPipelineStageId.Get<SimulatorPipelineStage>();
            unreliable = driver.CreatePipeline(Append(a, simId));
            sequenced = driver.CreatePipeline(Append(b, simId));
            reliable = driver.CreatePipeline(Append(c, simId));
            a.Dispose(); b.Dispose(); c.Dispose();
        }
        static NativeArray<NetworkPipelineStageId> Append(NativeArray<NetworkPipelineStageId> source, NetworkPipelineStageId extra)
        {
            var result = new NativeArray<NetworkPipelineStageId>(source.Length + 1, Allocator.Temp);
            NativeArray<NetworkPipelineStageId>.Copy(source, result, source.Length);
            result[source.Length] = extra;
            return result;
        }
    }
}
