using Cinemachine;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Scripting.Nodes
{
    [UnitCategory("pdxpartyparrot/Core")]
    [UnitTitle("Cinemachine Viewer Shake")]
    public class CinemachineViewerShakeScriptNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        private ControlInput _inputTrigger;

        [DoNotSerialize]
        [PortLabelHidden]
        private ControlOutput _outputTrigger;

        [DoNotSerialize]
        private ValueInput _viewerShakeConfig;

        [DoNotSerialize]
        private ValueInput _impulseSource;

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
            _viewerShakeConfig = ValueInput<ViewerShakeConfig>("Shake Config");
            _impulseSource = ValueInput<CinemachineImpulseSource>("Impulse Source", null);

            Requirement(_viewerShakeConfig, _inputTrigger);
            Requirement(_impulseSource, _inputTrigger);
        }

        private void Invoke(Flow flow)
        {
            if(!EffectsManager.Instance.EnableViewerShake) {
                return;
            }

            ViewerShakeConfig viewerShakeConfig = flow.GetValue<ViewerShakeConfig>(_viewerShakeConfig);
            CinemachineImpulseSource impulseSource = flow.GetValue<CinemachineImpulseSource>(_impulseSource);

            impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Rumble;

            Vector3 velocity = new Vector3(viewerShakeConfig.VelocityXRange.GetRandomValue() * PartyParrotManager.Instance.Random.NextSign(),
                                           viewerShakeConfig.VelocityYRange.GetRandomValue() * PartyParrotManager.Instance.Random.NextSign(),
                                           0.0f);
            impulseSource.m_DefaultVelocity = velocity;

            float duration = viewerShakeConfig.DurationRange.GetRandomValue();
            impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;

            float force = viewerShakeConfig.ForceRange.GetRandomValue();
            impulseSource.GenerateImpulseWithForce(force);
        }
    }
}
