using pdxpartyparrot.Game.Cinematics;

using Unity.VisualScripting;

namespace pdxpartyparrot.Game.Scripting.Nodes
{
    [UnitCategory("pdxpartyparrot/Game")]
    [UnitTitle("Show Dialogue")]
    public class ShowDialogueScriptNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        private ControlInput _inputTrigger;

        [DoNotSerialize]
        [PortLabelHidden]
        private ControlOutput _outputTrigger;

        [DoNotSerialize]
        private ValueInput _dialogueId;

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
            _dialogueId = ValueInput<string>("Dialogue ID", string.Empty);

            Requirement(_dialogueId, _inputTrigger);
        }

        private void Invoke(Flow flow)
        {
            DialogueManager.Instance.ShowDialogue(flow.GetValue<string>(_dialogueId));
        }
    }
}
