using pdxpartyparrot.Game.Cinematics;

using Unity.VisualScripting;

namespace pdxpartyparrot.Game.Scripting.Nodes
{
    public class ShowDialogueScriptNode : Unit
    {
        [DoNotSerialize]
        private ControlInput _inputTrigger;

        [DoNotSerialize]
        private ControlOutput _outputTrigger;

        [DoNotSerialize]
        private ValueInput _dialogueId;

        protected override void Definition()
        {
            // flow control
            _inputTrigger = ControlInput(string.Empty, (flow) => {
                DialogueManager.Instance.ShowDialogue(flow.GetValue<string>(_dialogueId));
                return _outputTrigger;
            });
            _outputTrigger = ControlOutput(string.Empty);

            Succession(_inputTrigger, _outputTrigger);

            // inputs
            _dialogueId = ValueInput<string>("Dialogue ID", string.Empty);

            Requirement(_dialogueId, _inputTrigger);
        }
    }
}
