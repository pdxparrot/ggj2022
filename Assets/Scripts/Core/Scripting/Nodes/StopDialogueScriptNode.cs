using pdxpartyparrot.Core.Audio;

using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Scripting.Nodes
{
    [UnitCategory("pdxpartyparrot/Core")]
    [UnitTitle("Stop Dialogue")]
    public class StopDialogueScriptNode : Unit
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
            if(AudioManager.HasInstance) {
                AudioManager.Instance.StopDialogue();
            }
        }
    }
}
