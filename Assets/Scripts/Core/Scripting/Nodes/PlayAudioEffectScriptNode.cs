using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Effects;

using UnityEngine;
using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Scripting.Nodes
{
    // TODO: rename this Play SFX or something so it's clear it's for the SFX channel
    [UnitCategory("pdxpartyparrot/Core")]
    [UnitTitle("Play Audio Effect")]
    public class PlayAudioEffectScriptNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        private ControlInput _inputTrigger;

        [DoNotSerialize]
        [PortLabelHidden]
        private ControlOutput _outputTrigger;

        [DoNotSerialize]
        private ValueInput _audioClip;

        [DoNotSerialize]
        private ValueInput _audioSource;

        [SerializeField]
        private ValueInput _loop;

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
            _audioSource = ValueInput<AudioSource>("Audio Source", null);
            _loop = ValueInput<bool>("Loop", false);

            Requirement(_audioClip, _inputTrigger);
            // audio source not required
            Requirement(_loop, _inputTrigger);
        }

        private void Invoke(Flow flow)
        {
            if(!EffectsManager.Instance.EnableAudio) {
                return;
            }

            AudioClip audioClip = flow.GetValue<AudioClip>(_audioClip);
            AudioSource audioSource = flow.GetValue<AudioSource>(_audioSource);
            bool loop = flow.GetValue<bool>(_loop);

            if(null != audioSource) {
                AudioManager.Instance.InitSFXAudioMixerGroup(audioSource);

                audioSource.clip = audioClip;
                audioSource.loop = loop;
                audioSource.Play();
            } else {
                AudioManager.Instance.PlayOneShot(audioClip);
            }
        }
    }
}
