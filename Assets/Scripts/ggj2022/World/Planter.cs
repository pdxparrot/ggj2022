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

            GameManager.Instance.RestartLevelEvent += RestartLevelEventHandler;
            GameManager.Instance.PlantersAvailableChangedEvent += PlantersAvailableChangedEventHandler;
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.PlantersAvailableChangedEvent -= PlantersAvailableChangedEventHandler;
                GameManager.Instance.RestartLevelEvent -= RestartLevelEventHandler;

                GameManager.Instance.UnRegisterPlanter(_areaId);
            }
        }

        #endregion

        private void Enable(bool enabled)
        {
            _model.SetActive(enabled);
            _vfx.SetActive(enabled);
        }

        private void TriggerScriptEvent(string name, params object[] args)
        {
            CustomEvent.Trigger(gameObject, name, args);
        }


        public bool PlantSeed()
        {
            if(!GameManager.Instance.PlantingAllowed) {
                TriggerScriptEvent("EnemiesRemain");
                return false;
            }

            if(IsPlanted) {
                TriggerScriptEvent("PlanterFull");
                return false;
            }

            _planted = true;

            TriggerScriptEvent("SeedPlanted");

            GameManager.Instance.SeedPlanted(_areaId);

            // TODO: this may change to swapping VFX or something instead of disabling
            Enable(false);

            return true;
        }

        #region Event Handlers

        private void RestartLevelEventHandler(object sender, EventArgs args)
        {
            _planted = false;
        }

        private void PlantersAvailableChangedEventHandler(object sender, EventArgs args)
        {
            if(!IsPlanted) {
                Enable(GameManager.Instance.PlantingAllowed);
            }
        }

        #endregion
    }
}
