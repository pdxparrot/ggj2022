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

        #region Unity Lifecycle

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        #endregion

        public void ExitLevel()
        {
            if(GameManager.Instance.ExitAvailable) {
                CustomEvent.Trigger(gameObject, "ExitLevel");
                return;
            }

            CustomEvent.Trigger(gameObject, "SeedsRemain");
        }
    }
}
