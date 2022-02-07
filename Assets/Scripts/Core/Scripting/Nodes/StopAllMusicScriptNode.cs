using pdxpartyparrot.Core.Audio;

using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Scripting.Nodes
{
    [UnitCategory("pdxpartyparrot/Core")]
    [UnitTitle("Stop All Music")]
    public class StopAllMusicScriptNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        private ControlInput _inputTrigger;

        [DoNotSerialize]
        [PortLabelHidden]
        private ControlOutput _outputTrigger;

        protected override void Definition()
        {
            // flow control
            _inputTrigger = ControlInput("Invoke", (flow) => {
                Invoke(flow);
                return _outputTrigger;
            });
            _outputTrigger = ControlOutput("Exit");

            Succession(_inputTrigger, _outputTrigger);
        }

        private void Invoke(Flow flow)
        {
            AudioManager.Instance.StopAllMusic();
        }
    }
}
