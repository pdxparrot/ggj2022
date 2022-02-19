#if USE_NETWORKING
#pragma warning disable 0618    // disable obsolete warning for now

using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Network
{
    // TOOD: https://github.com/Unity-Technologies/multiplayer-community-contributions/tree/main/com.community.netcode.extensions/Runtime/NetworkDiscovery can replace this
    public sealed class NetworkDiscovery : MonoBehaviour//Unity.Netcode.NetworkDiscovery
    {
        #region Events

        public event EventHandler<ReceivedBroadcastEventArgs> ReceivedBroadcastEvent;

        #endregion

        /*public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            Debug.Log($"[NetworkDiscovery]: Broadcast from {fromAddress}: {data}");

            int idx = data.IndexOf(":", StringComparison.Ordinal);
            if(idx < 0 || idx >= data.Length - 1) {
                return;
            }

            ReceivedBroadcastEvent?.Invoke(this, new ReceivedBroadcastEventArgs(fromAddress, data, data.Substring(idx + 1)));
        }*/
    }
}
#endif
