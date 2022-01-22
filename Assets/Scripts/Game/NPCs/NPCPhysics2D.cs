using pdxpartyparrot.Core;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.World;

using UnityEngine;

namespace pdxpartyparrot.Game.NPCs
{
    [RequireComponent(typeof(Collider))]
    public class NPCPhysics2D : MonoBehaviour
    {
        [SerializeField]
        private NPC2D _owner;

        protected NPC2D Owner => _owner;

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
