using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.Game.Cinematics
{
    [RequireComponent(typeof(UIObject))]
    [RequireComponent(typeof(ScriptMachine))]
    public class Dialogue : MonoBehaviour
    {
        [SerializeField]
        private Dialogue _nextDialogue;

        public Dialogue NextDialogue => _nextDialogue;

        [SerializeField]
        private bool _allowCancel;

        #region Effects

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _enableEffect;

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _disableEffect;

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _continueEffect;

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _cancelEffect;

        [SerializeField]
        [ReadOnly]
        private ITimer _showTimer;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _showTimer = TimeManager.Instance.AddTimer();
        }

        private void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_showTimer);
                _showTimer = null;
            }
        }

        private void OnEnable()
        {
            InputManager.Instance.EventSystem.UIModule.submit.action.performed += OnSubmit;
            InputManager.Instance.EventSystem.UIModule.cancel.action.performed += OnCancel;

            if(null != _enableEffect) {
                _enableEffect.Trigger();
            }

            _showTimer.Start(DialogueManager.Instance.DialogueData.InputDelay);
        }

        private void OnDisable()
        {
            if(null != _disableEffect) {
                _disableEffect.Trigger();
            }

            if(InputManager.HasInstance) {
                InputManager.Instance.EventSystem.UIModule.submit.action.performed -= OnSubmit;
                InputManager.Instance.EventSystem.UIModule.cancel.action.performed -= OnCancel;
            }
        }

        #endregion

        // this is called on the prefab so it must use GetComponent()
        // rather than relying on a cached UIObject
        public string GetId()
        {
            return GetComponent<UIObject>().Id;
        }

        #region Event Handlers

        private void OnSubmit(InputAction.CallbackContext context)
        {
            if(_showTimer.IsRunning) {
                return;
            }

            if(null != _continueEffect) {
                _continueEffect.Trigger(() => {
                    DialogueManager.Instance.AdvanceDialogue();
                });
            } else {
                DialogueManager.Instance.AdvanceDialogue();
            }
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            if(!_allowCancel) {
                return;
            }

            if(_showTimer.IsRunning) {
                return;
            }

            if(null != _cancelEffect) {
                _cancelEffect.Trigger(() => {
                    DialogueManager.Instance.CancelDialogue();
                });
            } else {
                DialogueManager.Instance.CancelDialogue();
            }
        }

        #endregion
    }
}
