using UnityEngine;
using UnityEngine.InputSystem;

using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.Game.UI
{
    [RequireComponent(typeof(UIObject))]
    public abstract class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private float _delay = 0.5f;

        [SerializeField]
        [ReadOnly]
        private ITimer _delayTimer;

        #region Unity Lifecycle

        private void Awake()
        {
            _delayTimer = TimeManager.Instance.AddTimer();
        }

        private void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_delayTimer);
                _delayTimer = null;
            }
        }

        private void OnEnable()
        {
            _delayTimer.Start(_delay);

            InputManager.Instance.EventSystem.UIModule.submit.action.performed += OnSubmit;
            InputManager.Instance.EventSystem.UIModule.cancel.action.performed += OnCancel;
        }

        private void OnDisable()
        {
            _delayTimer.Stop();

            if(InputManager.HasInstance) {
                InputManager.Instance.EventSystem.UIModule.submit.action.performed -= OnSubmit;
                InputManager.Instance.EventSystem.UIModule.cancel.action.performed -= OnCancel;
            }
        }

        #endregion

        protected virtual void OnSubmit()
        {
            GameStateManager.Instance.TransitionToInitialStateAsync();
        }

        protected virtual void OnCancel()
        {
            GameStateManager.Instance.TransitionToInitialStateAsync();
        }

        #region Event Handlers

        private void OnSubmit(InputAction.CallbackContext context)
        {
            if(_delayTimer.IsRunning) {
                return;
            }

            OnSubmit();
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            if(_delayTimer.IsRunning) {
                return;
            }

            OnCancel();
        }

        #endregion
    }
}
