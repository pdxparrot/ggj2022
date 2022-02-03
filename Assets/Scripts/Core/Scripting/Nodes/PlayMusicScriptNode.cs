using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Effects;

using UnityEngine;
using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Scripting.Nodes
{
    [UnitCategory("pdxpartyparrot/Core")]
    [UnitTitle("Play Music")]
    public class PlayMusicScriptNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        private ControlInput _inputTrigger;

        [DoNotSerialize]
        [PortLabelHidden]
        private ControlOutput _outputTrigger;

        [DoNotSerialize]
        private ValueInput _audioClip;

        //Immediately plays the music track if <= 0
        [DoNotSerialize]
        private ValueInput _transitionSeconds;

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
            _audioClip = ValueInput<AudioClip>("Audio Clip", null);
            _transitionSeconds = ValueInput<float>("Transition Seconds", 0.0f);

            Requirement(_audioClip, _inputTrigger);
            Requirement(_transitionSeconds, _inputTrigger);
        }

        private void Invoke(Flow flow)
        {
            if(!EffectsManager.Instance.EnableAudio) {
                return;
            }

            AudioClip audioClip = flow.GetValue<AudioClip>(_audioClip);
            float transitionSeconds = flow.GetValue<float>(_transitionSeconds);

            if(transitionSeconds > 0.0f) {
                AudioManager.Instance.TransitionMusicAsync(audioClip, transitionSeconds);
            } else {
                AudioManager.Instance.PlayMusic(audioClip);
            }
        }
    }
}
