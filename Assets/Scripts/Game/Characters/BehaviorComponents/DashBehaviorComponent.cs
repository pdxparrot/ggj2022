using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data.Characters.BehaviorComponents;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.BehaviorComponents
{
    // TODO: make air dashing configurable
    // TODO: make it possible to perform a dash action
    // without invoking the cooldown (action parameter)
    public class DashBehaviorComponent : CharacterBehaviorComponent
    {
        #region Actions

        public class DashAction : CharacterBehaviorAction
        {
            public static DashAction Default = new DashAction();
        }

        #endregion

        [SerializeField]
        private DashBehaviorComponentData _data;

        #region Effects

        [Header("Effects")]

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _dashStartEffect;

        [CanBeNull]
        protected virtual EffectTrigger DashStartEffect => _dashStartEffect;

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _dashStopEffect;

        [CanBeNull]
        protected virtual EffectTrigger DashStopEffect => _dashStopEffect;

        #endregion

        [SerializeReference]
        [ReadOnly]
        private ITimer _dashTimer;

        public bool IsDashing => _dashTimer.IsRunning;

        [SerializeReference]
        [ReadOnly]
        private ITimer _cooldownTimer;

        public bool IsDashCooldown => _cooldownTimer.IsRunning;

        public bool CanDash => !IsDashing && !IsDashCooldown;

        [SerializeField]
        [ReadOnly]
        private bool _wasUseGravity;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _dashTimer = TimeManager.Instance.AddTimer();
            _dashTimer.StopEvent += DashStopEventHandler;
            _dashTimer.TimesUpEvent += DashTimesUpEventHandler;

            _cooldownTimer = TimeManager.Instance.AddTimer();
        }

        protected override void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_dashTimer);
                TimeManager.Instance.RemoveTimer(_cooldownTimer);
            }

            _dashTimer = null;
            _cooldownTimer = null;

            base.OnDestroy();
        }

        #endregion

        public override bool OnPhysicsUpdate(float dt)
        {
            if(!IsDashing) {
                return false;
            }

            Vector3 moveDirection = Behavior.Owner.FacingDirection;
            Vector3 velocity = moveDirection * _data.DashSpeed;

            Behavior.Owner.Movement.Move(velocity * dt);

            return true;
        }

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is DashAction)) {
                return false;
            }

            if(Core.Input.InputManager.Instance.EnableDebug) {
                Debug.Log($"Dash!");
            }

            StartDashing();

            return true;
        }

        private void StartDashing()
        {
            Behavior.CharacterMovement.IsComponentControlling = true;

            if(_data.DisableGravity) {
                _wasUseGravity = Behavior.Owner.Movement.UseGravity;
                Behavior.Owner.Movement.UseGravity = false;
            }
            Behavior.CharacterMovement.EnableDynamicCollisionDetection(true);

            _dashTimer.Start(_data.DashTimeSeconds);

            if(null != DashStartEffect) {
                DashStartEffect.Trigger();
            }

            if(null != Behavior.Animator) {
                Behavior.Animator.SetTrigger(_data.DashStartParam);
            }
        }

        private void StopDashing()
        {
            if(null != Behavior.Animator) {
                Behavior.Animator.SetTrigger(_data.DashStopParam);
            }

            if(null != DashStopEffect) {
                DashStopEffect.Trigger();
            }

            _cooldownTimer.Start(_data.DashCooldownSeconds);

            Behavior.CharacterMovement.EnableDynamicCollisionDetection(false);

            if(_data.DisableGravity) {
                Behavior.Owner.Movement.UseGravity = _wasUseGravity;
            }

            Behavior.CharacterMovement.IsComponentControlling = false;
        }

        #region Event Handlers

        protected virtual void DashStopEventHandler(object sender, EventArgs args)
        {
            StopDashing();
        }

        protected virtual void DashTimesUpEventHandler(object sender, EventArgs args)
        {
            StopDashing();
        }

        #endregion
    }
}
