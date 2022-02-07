using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Effects;

using UnityEngine;
using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Scripting.Nodes
{
    [UnitCategory("pdxpartyparrot/Core")]
    [UnitTitle("Play Dialogue")]
    public class PlayDialogueScriptNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        private ControlInput _inputTrigger;

        [DoNotSerialize]
        [PortLabelHidden]
        private ControlOutput _outputTrigger;

        [DoNotSerialize]
        private ValueInput _dialogue;

        protected override void Definition()
        {
            // flow control
            _inputTrigger = ControlInput("Invoke", (flow) => {
                Invoke(flow);
                return _outputTrigger;
            });
            _outputTrigger = ControlOutput("Exit");

            Succession(_inputTrigger, _outputTrigger);

            // inputs
            _dialogue = ValueInput<AudioClip>("Dialogue", null);

            Requirement(_dialogue, _inputTrigger);
        }

        private void Invoke(Flow flow)
        {
            if(!EffectsManager.Instance.EnableAudio) {
                return;
            }

            AudioClip dialogue = flow.GetValue<AudioClip>(_dialogue);

            AudioManager.Instance.PlayDialogue(dialogue);
        }
    }
}
