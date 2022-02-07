using System;
using System.Collections.Generic;

using UnityEngine;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;
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
        private DialogueData _dialogueData;

        public DialogueData DialogueData => _dialogueData;

        public T GetDialogueData<T>() where T : DialogueData
        {
            return (T)DialogueData;
        }

        [SerializeField]
        [ReadOnly]
        private Dialogue _currentDialogue;

        private Action _onComplete;

        private Action _onCancel;

        public bool IsShowingDialogue => null != _currentDialogue;

        private readonly Dictionary<string, Dialogue> _dialoguePrefabs = new Dictionary<string, Dialogue>();

        #region Unity Lifecycle

        private void Awake()
        {
            InitDebugMenu();

            foreach(Dialogue dialoguePrefab in _dialogueData.DialoguePrefabs) {
                _dialoguePrefabs.Add(dialoguePrefab.GetId(), dialoguePrefab);
            }
        }

        #endregion

        public void ShowDialogue(string id, Action onComplete = null, Action onCancel = null)
        {
            Dialogue dialoguePrefab = _dialoguePrefabs.GetValueOrDefault(id);
            if(null == dialoguePrefab) {
                Debug.LogWarning($"Missing dialogue prefab {id}!");
                return;
            }

            ShowDialogue(dialoguePrefab, onComplete, onCancel);
        }

        private void ShowDialogue(Dialogue dialoguePrefab, Action onComplete = null, Action onCancel = null)
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

                GUILayout.BeginVertical("Registered dialogues:", GUI.skin.box);
                foreach(string dialogueId in _dialoguePrefabs.Keys) {
                    GUILayout.Label(dialogueId);
                }
                GUILayout.EndVertical();

                // TODO: add buttons for showing, advancing, and canceling dialogues
            };
        }
    }
}
