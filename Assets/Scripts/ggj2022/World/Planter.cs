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

        public bool CanPlantSeed => !_planted;


        #region Effects

        [SerializeField]
        private EffectTrigger _plantEffect;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        #endregion

        public void PlantSeed()
        {
            if(!CanPlantSeed) {
                return;
            }

            _planted = true;

            _plantEffect.Trigger();
        }
    }
}
