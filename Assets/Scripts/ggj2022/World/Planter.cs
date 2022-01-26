using System;

using UnityEngine;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Interactables;

namespace pdxpartyparrot.ggj2022.World
{
    [RequireComponent(typeof(Collider))]
    public sealed class Planter : MonoBehaviour, IInteractable
    {
        public bool CanInteract => true;

        public Type InteractableType => typeof(Planter);

        [SerializeField]
        [ReadOnly]
        private bool _planted;

        public bool IsPlanted => _planted;

        #region Effects

        [SerializeField]
        private EffectTrigger _plantEffect;

        [SerializeField]
        private EffectTrigger _enemiesRemainLevelEffect;

        [SerializeField]
        private EffectTrigger _planterFullLevelEffect;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        #endregion

        public void PlantSeed()
        {
            if(!GameManager.Instance.PlantingAllowed) {
                _enemiesRemainLevelEffect.Trigger();
                return;
            }

            if(IsPlanted) {
                _planterFullLevelEffect.Trigger();
                return;
            }

            _planted = true;

            _plantEffect.Trigger();
        }
    }
}
