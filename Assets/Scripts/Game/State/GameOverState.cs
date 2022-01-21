using JetBrains.Annotations;

using System;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    // TODO: we probably need a TransitionToInitialState effect trigger component
    // now that this can no longer automatically transition on its own

    public abstract class GameOverState : SubGameState
    {
        [SerializeField]
        [CanBeNull]
        private Menu.Menu _menuPrefab;

        private Menu.Menu _menu;

        [SerializeField]
        [CanBeNull]
        private GameOverUI _overlayPrefab;

        private GameOverUI _overlay;

        [SerializeField]
        private bool _automaticOverlayTransition = true;

        [SerializeField]
        [Tooltip("If a menu prefab is not set, and an overlay prefab is not set or automatic overlay transition is not true, the game over state will transition automatically after this duration")]
        private float _completeWaitTimeSeconds = 5.0f;

        [SerializeReference]
        [ReadOnly]
        private ITimer _completeTimer;

        public override void OnEnter()
        {
            base.OnEnter();

            // TODO: show game over UI
        }

        protected override void DoEnter()
        {
            base.DoEnter();

            if(null != _menuPrefab && null != GameStateManager.Instance.GameUIManager) {
                _menu = GameStateManager.Instance.GameUIManager.InstantiateUIPrefab(_menuPrefab);
                _menu.Initialize();
            } else if(null != _overlayPrefab) {
                _overlay = GameStateManager.Instance.GameUIManager.InstantiateUIPrefab(_overlayPrefab);
                if(_automaticOverlayTransition) {
                    StartCompleteTimer();
                }
            } else {
                StartCompleteTimer();
            }
        }

        protected override void DoExit()
        {
            if(null != _menu) {
                Destroy(_menu.gameObject);
                _menu = null;
            }

            if(null != _overlay) {
                Destroy(_overlay.gameObject);
                _overlay = null;
            }

            if(null != _completeTimer) {
                // NOTE: this relies on the state unloading process
                // yielding for a frame so we're out of the TimeManager loop
                TimeManager.Instance.RemoveTimer(_completeTimer);
                _completeTimer = null;
            }

            AudioManager.Instance.StopAllMusic();

            base.DoExit();
        }

        public virtual void Initialize()
        {
        }

        private void StartCompleteTimer()
        {
            _completeTimer = TimeManager.Instance.AddTimer();
            _completeTimer.TimesUpEvent += CompleteTimerTimesUpEventHandler;
            _completeTimer.Start(_completeWaitTimeSeconds);
        }

        #region Event Handlers

        private void CompleteTimerTimesUpEventHandler(object sender, EventArgs args)
        {
            GameStateManager.Instance.TransitionToInitialStateAsync();
        }

        #endregion
    }
}
