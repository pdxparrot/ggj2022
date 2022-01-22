using pdxpartyparrot.Core;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.World;

using UnityEngine;

namespace pdxpartyparrot.Game.NPCs
{
    [RequireComponent(typeof(Collider))]
    public class NPCPhysics : MonoBehaviour
    {
        [SerializeField]
        private INPC _owner;

        protected INPC Owner => _owner;

        private Collider _collider;

        protected Collider Collider => _collider;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        #endregion
    }
}
