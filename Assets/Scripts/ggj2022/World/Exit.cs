using System;

using pdxpartyparrot.Game.Interactables;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.World
{
    [RequireComponent(typeof(Collider))]
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
    }
}
