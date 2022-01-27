using System;

using UnityEngine;
using Unity.VisualScripting;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Interactables;

namespace pdxpartyparrot.ggj2022.World
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(ScriptMachine))]
    public sealed class Planter : MonoBehaviour, IInteractable
    {
        public bool CanInteract => true;

        public Type InteractableType => typeof(Planter);

        [SerializeField]
        [ReadOnly]
        private bool _planted;

        public bool IsPlanted => _planted;

        #region Unity Lifecycle

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        #endregion

        public void PlantSeed()
        {
            if(!GameManager.Instance.PlantingAllowed) {
                CustomEvent.Trigger(gameObject, "EnemiesRemain");
                return;
            }

            if(IsPlanted) {
                CustomEvent.Trigger(gameObject, "PlanterFull");
                return;
            }

            _planted = true;

            CustomEvent.Trigger(gameObject, "SeedPlanted");
        }
    }
}
