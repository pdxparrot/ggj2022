using pdxpartyparrot.Core.Effects.EffectTriggerComponents;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Players
{
    public sealed class ForestSpiritBehavior : MonoBehaviour
    {
        [SerializeField]
        private Player _owner;

        public Player Owner => _owner;

        [SerializeField]
        private RumbleEffectTriggerComponent[] _rumbleEffects;

        public void Initialize()
        {
            foreach(RumbleEffectTriggerComponent rumble in _rumbleEffects) {
                rumble.PlayerInput = Owner.PlayerInputHandler.InputHelper;
            }
        }

        #region Actions

        public void SwapForms()
        {
            Debug.Log("swap forms!");
        }

        #endregion
    }
}
