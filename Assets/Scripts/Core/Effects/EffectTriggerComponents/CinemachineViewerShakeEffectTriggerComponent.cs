using Cinemachine;

using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class CinemachineViewerShakeEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private CinemachineImpulseSource _impulseSource;

        [SerializeField]
        private FloatRangeConfig _durationRange = new FloatRangeConfig(0.5f, 0.5f);

        [SerializeField]
        private FloatRangeConfig _forceRange = new FloatRangeConfig(1.0f, 1.0f);

        [SerializeField]
        private FloatRangeConfig _xVelocityRange = new FloatRangeConfig(-1.0f, 1.0f);

        [SerializeField]
        private FloatRangeConfig _yVelocityRange = new FloatRangeConfig(-1.0f, 1.0f);

        [SerializeField]
        private bool _waitForComplete = true;

        public override bool WaitForComplete => _waitForComplete;

        [SerializeField]
        [ReadOnly]
        private bool _isPlaying;

        public override bool IsDone => !_isPlaying;

        public override void Initialize(EffectTrigger owner)
        {
            base.Initialize(owner);

            _impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Rumble;
        }

        public override void OnStart()
        {
            if(EffectsManager.Instance.EnableViewerShake) {
                Vector3 velocity = new Vector3(_xVelocityRange.GetRandomValue() * PartyParrotManager.Instance.Random.NextSign(),
                                               _yVelocityRange.GetRandomValue() * PartyParrotManager.Instance.Random.NextSign(),
                                               0.0f);
                _impulseSource.m_DefaultVelocity = velocity;

                float duration = _durationRange.GetRandomValue();
                _impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;

                float force = _forceRange.GetRandomValue();
                _impulseSource.GenerateImpulseWithForce(force);
            }

            Assert.IsTrue(_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.Duration >= 0);

            _isPlaying = true;

            // TODO: this won't respect pause in terms of duration and it really should
            // a coroutine that continually generates a short impulse over a large duration might make more sense
            TimeManager.Instance.RunAfterDelay(_impulseSource.m_ImpulseDefinition.m_TimeEnvelope.Duration, () => _isPlaying = false);
        }

        public override void OnStop()
        {
            // TODO: handle this
        }
    }
}
