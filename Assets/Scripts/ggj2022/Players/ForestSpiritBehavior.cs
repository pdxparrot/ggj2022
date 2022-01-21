using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2022.Data.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Players
{
    public sealed class ForestSpiritBehavior : MonoBehaviour
    {
        private enum SpiritForm
        {
            Small,
            Large,
        }

        [SerializeField]
        protected ForestSpiritBehaviorData _forestSpiritBehaviorData;

        [SerializeField]
        [ReadOnly]
        private SpiritForm _currentForm = SpiritForm.Small;

        [SerializeField]
        private Player _owner;

        public Player Owner => _owner;

        public float MoveSpeedModifier => _currentForm == SpiritForm.Small ? 1.0f : _forestSpiritBehaviorData.LargeSpiritMoveSpeedModifier;

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
            switch(_currentForm) {
            case SpiritForm.Small:
                _currentForm = SpiritForm.Large;
                break;
            case SpiritForm.Large:
                _currentForm = SpiritForm.Small;
                break;
            }

            Debug.Log($"Form swap: {_currentForm}");
        }

        #endregion
    }
}
