using System;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Game.Interactables;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.World
{
    [RequireComponent(typeof(Collider))]
    public sealed class Exit : MonoBehaviour, IInteractable
    {
        public bool CanInteract => true;

        public Type InteractableType => typeof(Exit);

        #region Effects

        [SerializeField]
        private EffectTrigger _seedsRemainEffect;

        [SerializeField]
        private EffectTrigger _exitLevelEffect;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        #endregion

        public void ExitLevel()
        {
            if(GameManager.Instance.ExitAvailable) {
                _exitLevelEffect.Trigger();
                return;
            }

            _seedsRemainEffect.Trigger();
        }
    }
}
