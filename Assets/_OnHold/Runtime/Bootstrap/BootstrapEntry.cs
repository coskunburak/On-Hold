using UnityEngine;
using OnHold.Gameplay;
using OnHold.Networking;

namespace OnHold.Bootstrap
{
    [DefaultExecutionOrder(-100)]
    public sealed class BootstrapEntry : MonoBehaviour
    {
        public FoundationWorld World;
        public NetworkSession Network;
        void Awake() { World.Initialize(); }
        void OnDestroy() { if (Network && Network.Manager) Destroy(Network.Manager.gameObject); }
    }
}
