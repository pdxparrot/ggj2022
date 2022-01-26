using JetBrains.Annotations;

using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Cinematics;

using UnityEngine;

namespace pdxpartyparrot.Game.Effects.EffectTriggerComponents
{
    public class DialogueEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        [CanBeNull]
        private Dialogue _dialoguePrefab;

        [SerializeField]
        private string _dialogueId;

        public override bool WaitForComplete => true;

        [SerializeField]
        [ReadOnly]
        private bool _isShowing;

        public override bool IsDone => !_isShowing;

        public override void OnStart()
        {
            if(null == _dialoguePrefab) {
                DialogueManager.Instance.ShowDialogue(_dialogueId, OnComplete, OnCancel);
            } else {
                DialogueManager.Instance.ShowDialogue(_dialoguePrefab, OnComplete, OnCancel);
            }

            _isShowing = true;
        }

        public override void OnStop()
        {
            DialogueManager.Instance.CancelDialogue();
        }

        private void OnComplete()
        {
            _isShowing = false;
        }

        private void OnCancel()
        {
            _isShowing = false;
        }
    }
}
