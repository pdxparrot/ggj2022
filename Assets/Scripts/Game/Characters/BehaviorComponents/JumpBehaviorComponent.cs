using JetBrains.Annotations;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Game.Data.Characters.BehaviorComponents;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.BehaviorComponents
{
    public class JumpBehaviorComponent : CharacterBehaviorComponent
    {
        #region Actions

        public class JumpAction : CharacterBehaviorAction
        {
            public static JumpAction Default = new JumpAction();
        }

        #endregion

        [SerializeField]
        private JumpBehaviorComponentData _data;

        public virtual float JumpHeight => _data.JumpHeight;

        [Space(10)]

        #region Effects

        [Header("Effects")]

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _jumpEffect;

        [CanBeNull]
        protected virtual EffectTrigger JumpEffect => _jumpEffect;

        #endregion

        #region Actions

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is JumpAction)) {
                return false;
            }

            if(!Behavior.IsGrounded || Behavior.IsSliding) {
                if(Core.Input.InputManager.Instance.EnableDebug) {
                    Debug.LogWarning($"Jump failed, grounded: {Behavior.IsGrounded}, sliding: {Behavior.IsSliding}");
                }
                return false;
            }

            if(Core.Input.InputManager.Instance.EnableDebug) {
                Debug.Log($"Jump!");
            }

            Behavior.CharacterMovement.Jump(JumpHeight);

            if(null != JumpEffect) {
                JumpEffect.Trigger();
            }

            if(null != Behavior.Animator) {
                Behavior.Animator.SetTrigger(_data.JumpParam);
            }

            return true;
        }

        #endregion
    }
}
