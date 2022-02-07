using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Effects;

using UnityEngine;
using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Scripting.Nodes
{
    [UnitCategory("pdxpartyparrot/Core")]
    [UnitTitle("Play Stinger")]
    public class PlayStingerScriptNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        private ControlInput _inputTrigger;

        [DoNotSerialize]
        [PortLabelHidden]
        private ControlOutput _outputTrigger;

        [DoNotSerialize]
        private ValueInput _stinger;

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
            _stinger = ValueInput<AudioClip>("Stinger", null);

            Requirement(_stinger, _inputTrigger);
        }

        private void Invoke(Flow flow)
        {
            if(!EffectsManager.Instance.EnableAudio) {
                return;
            }

            AudioClip stinger = flow.GetValue<AudioClip>(_stinger);

            AudioManager.Instance.PlayStinger(stinger);
        }
    }
}
