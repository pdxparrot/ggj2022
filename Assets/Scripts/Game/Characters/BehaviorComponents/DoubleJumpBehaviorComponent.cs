using JetBrains.Annotations;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data.Characters.BehaviorComponents;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.BehaviorComponents
{
    [RequireComponent(typeof(JumpBehaviorComponent))]
    public class DoubleJumpBehaviorComponent : CharacterBehaviorComponent
    {
        [SerializeField]
        private DoubleJumpBehaviorComponentData _data;

        [Space(10)]

        #region Effects

        [Header("Effects")]

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _doubleJumpEffect;

        [CanBeNull]
        protected virtual EffectTrigger DoubleJumpEffect => _doubleJumpEffect;

        #endregion

        [SerializeField]
        [ReadOnly]
        private int _doubleJumpCount;

        private bool CanDoubleJump => !Behavior.IsGrounded && (_data.DoubleJumpCount < 0 || _doubleJumpCount < _data.DoubleJumpCount);

        #region Unity Lifecycle

        private void LateUpdate()
        {
            if(Behavior.IsGrounded) {
                _doubleJumpCount = 0;
            }
        }

        #endregion

        // TODO: this is never called?
        public void ResetComponent()
        {
            _doubleJumpCount = 0;
        }

        #region Actions

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is JumpBehaviorComponent.JumpAction)) {
                return false;
            }

            if(!CanDoubleJump) {
                return false;
            }

            if(Core.Input.InputManager.Instance.EnableDebug) {
                Debug.Log($"Double jump!");
            }

            Behavior.CharacterMovement.Jump(_data.DoubleJumpHeight);

            if(null != DoubleJumpEffect) {
                DoubleJumpEffect.Trigger();
            }

            if(null != Behavior.Animator) {
                Behavior.Animator.SetTrigger(_data.DoubleJumpParam);
            }

            _doubleJumpCount++;
            return true;
        }

        #endregion
    }
}
