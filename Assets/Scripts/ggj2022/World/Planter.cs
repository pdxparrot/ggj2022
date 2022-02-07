using System;

using UnityEngine;
using UnityEngine.VFX;
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
        private GameObject _model;

        [SerializeField]
        private GameObject _vfx;

        [SerializeField]
        private string _areaId;

        [SerializeField]
        [ReadOnly]
        private bool _planted;

        public bool IsPlanted => _planted;

        #region Unity Lifecycle

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;

            Enable(false);

            GameManager.Instance.RegisterPlanter(_areaId);

            GameManager.Instance.PlantersAvailableChangedEvent += PlantersAvailableChangedEventHandler;
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.PlantersAvailableChangedEvent -= PlantersAvailableChangedEventHandler;

                GameManager.Instance.UnRegisterPlanter(_areaId);
            }
        }

        #endregion

        private void Enable(bool enabled)
        {
            _model.SetActive(enabled);
            _vfx.SetActive(enabled);
        }

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

            GameManager.Instance.SeedPlanted(_areaId);
        }

        #region Event Handlers

        private void PlantersAvailableChangedEventHandler(object sender, EventArgs args)
        {
            Enable(GameManager.Instance.PlantingAllowed);
        }

        #endregion
    }
}
