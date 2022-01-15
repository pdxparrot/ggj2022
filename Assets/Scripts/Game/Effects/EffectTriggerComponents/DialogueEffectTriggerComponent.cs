using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Cinematics;

using UnityEngine;

namespace pdxpartyparrot.Game.Effects.EffectTriggerComponents
{
    public class DialogueEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private Dialogue _dialoguePrefab;

        public override bool WaitForComplete => true;

        [SerializeField]
        [ReadOnly]
        private bool _isShowing;

        public override bool IsDone => !_isShowing;

        public override void OnStart()
        {
            DialogueManager.Instance.ShowDialogue(_dialoguePrefab,
            () => {
                _isShowing = false;
            },
            () => {
                _isShowing = false;
            });

            _isShowing = true;
        }

        public override void OnStop()
        {
            DialogueManager.Instance.CancelDialogue();
        }
    }
}
