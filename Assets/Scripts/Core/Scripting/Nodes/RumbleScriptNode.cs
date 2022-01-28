using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Input;

using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Scripting.Nodes
{
    [UnitCategory("pdxpartyparrot/Core")]
    [UnitTitle("Rumble")]
    public class RumbleScriptNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        private ControlInput _inputTrigger;

        [DoNotSerialize]
        [PortLabelHidden]
        private ControlOutput _outputTrigger;

        [DoNotSerialize]
        private ValueInput _rumbleConfig;

        [DoNotSerialize]
        private ValueInput _playerInput;

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
            _rumbleConfig = ValueInput<RumbleConfig>("Rumble Config");
            _playerInput = ValueInput<PlayerInputHelper>("Player Input", null);

            Requirement(_rumbleConfig, _inputTrigger);
            Requirement(_playerInput, _inputTrigger);
        }

        private void Invoke(Flow flow)
        {
            if(!EffectsManager.Instance.EnableRumble) {
                return;
            }

            RumbleConfig rumbleConfig = flow.GetValue<RumbleConfig>(_rumbleConfig);
            PlayerInputHelper playerInput = flow.GetValue<PlayerInputHelper>(_playerInput);

            playerInput.Rumble(rumbleConfig);
        }
    }
}
