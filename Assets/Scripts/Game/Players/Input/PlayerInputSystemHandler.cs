using System;

using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.InputSystem;

namespace pdxpartyparrot.Game.Players.Input
{
    // TODO: add handlers for device lost / device regained / controls changed

    public abstract class PlayerInputSystemHandler : PlayerInputHandler
    {
        [SerializeField]
        [ReadOnly]
        private bool _pollMove;

        protected bool PollMove
        {
            get => _pollMove;
            set => _pollMove = value;
        }

        [SerializeField]
        [ReadOnly]
        private bool _pollLook;

        protected bool PollLook
        {
            get => _pollLook;
            set => _pollLook = value;
        }

        private InputAction _moveAction;

        private InputAction _lookAction;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _moveAction = InputHelper.PlayerInput.actions.FindAction(InputManager.Instance.InputData.MoveActionName);
            if(null == _moveAction) {
                Debug.LogWarning("Missing move action");
            }

            _lookAction = InputHelper.PlayerInput.actions.FindAction(InputManager.Instance.InputData.LookActionName);
            if(null == _lookAction) {
                Debug.LogWarning("Missing look action");
            }

            // TODO: get a hook to the look InvertVector2 processor so we can modify it?
            GameStateManager.Instance.GameManager.Settings.SettingsUpdatedEvent += SettingsUpdatedEventHandler;
        }

        protected virtual void OnDestroy()
        {
            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.GameManager.Settings.SettingsUpdatedEvent -= SettingsUpdatedEventHandler;
            }
        }

        protected override void Update()
        {
            base.Update();

            if(PollMove) {
                DoPollMove();
            }

            if(PollLook) {
                DoPollLook();
            }
        }

        #endregion

        public override void Initialize(short playerControllerId)
        {
            base.Initialize(playerControllerId);

            InvertLookVertical = GameStateManager.Instance.GameManager.Settings.InvertLookVertical;
        }

        protected virtual void DoPollMove()
        {
            if(!InputAllowed || null == _moveAction) {
                return;
            }

            DoMove(_moveAction);
        }

        protected virtual void DoPollLook()
        {
            if(!InputAllowed || null == _lookAction) {
                return;
            }

            DoLook(_lookAction);
        }

        #region Common Actions

        public void OnPauseAction(InputAction.CallbackContext context)
        {
            if(!IsInputAllowed(context)) {
                return;
            }

            if(InputManager.Instance.EnableDebug) {
                Debug.Log($"Pause: {context.action.phase}");
            }

            if(context.performed) {
                OnPause();
            }
        }

        protected virtual void DoMove(InputAction action)
        {
            Vector2 axes = action.ReadValue<Vector2>();
            OnMove(new Vector3(axes.x, axes.y, 0.0f));
        }

        public void OnMoveAction(InputAction.CallbackContext context)
        {
            if(!IsInputAllowed(context)) {
                return;
            }

            /*if(Core.Input.InputManager.Instance.EnableDebug) {
                Debug.Log($"Move: {context.action.phase}");
            }*/

            if(context.performed) {
                PollMove = true;
                DoPollMove();
            } else if(context.canceled) {
                PollMove = false;
                OnMove(Vector3.zero);
            }
        }

        protected virtual void DoLook(InputAction action)
        {
            Vector2 axes = action.ReadValue<Vector2>();
            OnLook(new Vector3(axes.x, axes.y, 0.0f));
        }

        public void OnLookAction(InputAction.CallbackContext context)
        {
            if(!IsInputAllowed(context)) {
                return;
            }

            /*if(Core.Input.InputManager.Instance.EnableDebug) {
                Debug.Log($"Look: {context.action.phase}");
            }*/

            if(context.performed) {
                PollLook = true;
                DoPollLook();
            } else if(context.canceled) {
                PollLook = false;
                OnLook(Vector3.zero);
            }
        }

        #endregion

        #region Event Handlers

        private void SettingsUpdatedEventHandler(object sender, EventArgs args)
        {
            InvertLookVertical = GameStateManager.Instance.GameManager.Settings.InvertLookVertical;
        }

        #endregion
    }
}
