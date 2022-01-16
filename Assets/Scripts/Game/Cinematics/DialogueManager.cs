using System;

using UnityEngine;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.Game.Cinematics
{
    // TODO: if this could pre-allocate all of the dialogues we need
    // rather than instantiating them, that would save a lot of thrash
    // OR another option is a small set of dialogue prefabs
    // and more data that can be swapped in and out as needed
    // (ideally allowing for images and rich text)
    public sealed class DialogueManager : SingletonBehavior<DialogueManager>
    {
        [SerializeField]
        [Tooltip("How long should new dialogues be open before listening for input")]
        private float _inputDelay = 0.5f;

        public float InputDelay => _inputDelay;

        [SerializeField]
        [ReadOnly]
        private Dialogue _currentDialogue;

        private Action _onComplete;

        private Action _onCancel;

        public bool IsShowingDialogue => null != _currentDialogue;

        #region Unity Lifecycle

        private void Awake()
        {
            InitDebugMenu();
        }

        #endregion

        public void ShowDialogue(Dialogue dialoguePrefab, Action onComplete = null, Action onCancel = null)
        {
            if(null == dialoguePrefab) {
                onComplete?.Invoke();

                _onComplete = null;
                _onCancel = null;
                return;
            }

            _currentDialogue = GameStateManager.Instance.GameUIManager.InstantiateUIPrefab(dialoguePrefab);
            _onComplete = onComplete;
            _onCancel = onCancel;

            Debug.Log($"Showing dialogue {_currentDialogue.name}");
        }

        public void AdvanceDialogue()
        {
            if(!IsShowingDialogue) {
                return;
            }

            Dialogue previousDialogue = _currentDialogue;
            _currentDialogue = null;

            ShowDialogue(previousDialogue.NextDialogue, _onComplete, _onCancel);

            Destroy(previousDialogue.gameObject);
        }

        public void CancelDialogue()
        {
            if(!IsShowingDialogue) {
                return;
            }

            Destroy(_currentDialogue.gameObject);
            _currentDialogue = null;

            _onCancel?.Invoke();

            _onComplete = null;
            _onCancel = null;
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.DialogueManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label(IsShowingDialogue ? $"Showing dialogue ${_currentDialogue.name}" : "Not showing dialogue");

                // TODO: add buttons for showing, advancing, and canceling dialogues
            };
        }
    }
}
