#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Core.Actors;

#if USE_NETWORKING
using UnityEngine;
using Unity.Netcode;
#endif

namespace pdxpartyparrot.Core.Network
{
    //[RequireComponent(typeof(Actor))]
#if USE_NETWORKING
    [RequireComponent(typeof(NetworkObject))]
    [RequireComponent(typeof(NetworkTransform))]
#endif
    public abstract class NetworkActor : NetworkBehaviour
    {
#if USE_NETWORKING
        public NetworkObject NetworkObject { get; private set; }

        public NetworkTransform NetworkTransform { get; private set; }
#endif

        protected Actor Actor { get; private set; }

        #region Unity Lifecycle

        protected virtual void Awake()
        {
#if USE_NETWORKING
            NetworkObject = GetComponent<NetworkObject>();
            NetworkTransform = GetComponent<NetworkTransform>();
#endif

            Actor = GetComponent<Actor>();
        }

        #endregion
    }
}
