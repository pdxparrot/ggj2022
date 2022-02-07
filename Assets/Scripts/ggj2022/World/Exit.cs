using System;

using pdxpartyparrot.Game.Interactables;

using UnityEngine;
using Unity.VisualScripting;

namespace pdxpartyparrot.ggj2022.World
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(ScriptMachine))]
    public sealed class Exit : MonoBehaviour, IInteractable
    {
        public bool CanInteract => true;

        public Type InteractableType => typeof(Exit);

        [SerializeField]
        private GameObject _model;

        #region Unity Lifecycle

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;

            Enable(true);

            GameManager.Instance.ExitAvailableChangedEvent += ExitAvailableChangedEventHandler;
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.ExitAvailableChangedEvent -= ExitAvailableChangedEventHandler;
            }
        }

        #endregion

        private void Enable(bool enabled)
        {
            _model.SetActive(enabled);
        }

        public void ExitLevel()
        {
            if(GameManager.Instance.ExitAvailable) {
                CustomEvent.Trigger(gameObject, "ExitLevel");
                return;
            }

            CustomEvent.Trigger(gameObject, "SeedsRemain");
        }

        #region Event Handlers

        private void ExitAvailableChangedEventHandler(object sender, EventArgs args)
        {
            //Enable(GameManager.Instance.ExitAvailable);
        }

        #endregion
    }
}
