using System;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Cinematics
{
    // TODO: if this could pre-allocate all of the cinematics we need
    // rather than instantiating them, that would save a lot of thrash
    public sealed class CinematicsManager : SingletonBehavior<CinematicsManager>
    {
        [SerializeField]
        [ReadOnly]
        private Cinematic _currentCinematic;

        private Action _onComplete;

        private Action _onSkip;

        public bool IsRunningCinematic => null != _currentCinematic;

        #region Unity Lifecycle

        private void Awake()
        {
            InitDebugMenu();
        }

        #endregion

        public void StartCinematic(Cinematic cinematicPrefab, Action onComplete = null, Action onSkip = null)
        {
            if(null == cinematicPrefab) {
                onComplete?.Invoke();

                _onComplete = null;
                _onSkip = null;
                return;
            }

            // TODO: instantiate and start the cinematic

            _onComplete = onComplete;
            _onSkip = onSkip;

            Debug.Log($"Running cinematic {_currentCinematic.name}");
        }

        public void SkipCinematic()
        {
            if(!IsRunningCinematic) {
                return;
            }

            Destroy(_currentCinematic.gameObject);
            _currentCinematic = null;

            _onSkip?.Invoke();

            _onComplete = null;
            _onSkip = null;
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.CinematicsManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label(IsRunningCinematic ? $"Running cinematic ${_currentCinematic.name}" : "Not running cinematic");

                // TODO: add buttons for starting and skipping cinematics
            };
        }
    }
}
