using Unity.VisualScripting;

namespace pdxpartyparrot.ggj2022.Scripting.Nodes
{
    [UnitCategory("pdxpartyparrot/ggj2022")]
    [UnitTitle("Exit Game")]
    public class ExitGameScriptNode : Unit
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
            GameManager.Instance.Exit();
        }
    }
}
