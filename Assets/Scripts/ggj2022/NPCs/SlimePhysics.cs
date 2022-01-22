using pdxpartyparrot.Core;
using pdxpartyparrot.Game.NPCs;
using pdxpartyparrot.Game.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.NPCs
{
    public sealed class SlimePhysics : NPCPhysics3D
    {
        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Collider.sharedMaterial = PartyParrotManager.Instance.FrictionlessMaterial;
        }

        #endregion
    }
}
