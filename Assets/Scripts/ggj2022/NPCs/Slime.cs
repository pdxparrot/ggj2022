using System;

using pdxpartyparrot.Game.Characters.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2022.NPCs
{
    public sealed class Slime : NPC25D
    {
        public SlimeBehavior SlimeBehavior => (SlimeBehavior)NPCBehavior;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

            SetPassive();
        }

        protected override void OnDestroy()
        {
            if(NPCManager.HasInstance) {
                NPCManager.Instance.UnregisterNPC(this);
            }

            base.OnDestroy();
        }

        #endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            Assert.IsTrue(Behavior is SlimeBehavior);
        }
    }
}
