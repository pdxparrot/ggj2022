using System.Collections.Generic;
using System.Linq;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace pdxpartyparrot.ggj2022.Effects
{
    public sealed class AreaBlendShader : BlendShader
    {
        [SerializeField]
        private string _areaId;

        [SerializeField]
        private bool _global;

        // NOTE: this is the shader property Reference value, not the Name value
        [SerializeField]
        [Tooltip("The shader property Reference value")]
        private string _seedsPlantedParameter = "_Growth_Override";

        [SerializeField]
        [ReadOnly]
        private float _currentSeedsPlantedPercent;

        [SerializeField]
        [ReadOnly]
        private float _targetSeedsPlantedPercent;

        [Space(10)]

        [SerializeField]
        [FormerlySerializedAs("_vfxMultiplier")]
        private string _enemiesStompedVfxMultiplier = "spawnRateMultiplier";

        [SerializeField]
        [FormerlySerializedAs("_vfx")]
        private List<VisualEffect> _enemiesStompedVfx = new List<VisualEffect>();

        [SerializeField]
        private string _seedsPlantedVfxMultiplier = "spawnRateMultiplier_Seeds";

        [SerializeField]
        private List<VisualEffect> _seedsPlantedVfx = new List<VisualEffect>();

        #region Unity Lifecycle

        protected override void Awake()
        {
            GameManager.Instance.TransitionUpdateEvent += TransitionUpdateEventHandler;

            if(_global) {
                Assert.IsFalse(_areaId.Any());
            }
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.TransitionUpdateEvent -= TransitionUpdateEventHandler;
            }
        }

        protected override void Update()
        {
            float dt = Time.deltaTime;

            float enemiesBefore = CurrentPercent;
            float seedsBefore = _currentSeedsPlantedPercent;

            if(_currentSeedsPlantedPercent != _targetSeedsPlantedPercent) {
                _currentSeedsPlantedPercent = LerpParameter(_seedsPlantedParameter, _currentSeedsPlantedPercent, _targetSeedsPlantedPercent, LerpSpeed * dt);
            }

            base.Update();

            if(enemiesBefore != TargetPercent) {
                foreach(VisualEffect vfx in _enemiesStompedVfx) {
                    vfx.SetFloat(_enemiesStompedVfxMultiplier, 1.0f - CurrentPercent);
                }
            }

            if(seedsBefore != _targetSeedsPlantedPercent) {
                foreach(VisualEffect vfx in _seedsPlantedVfx) {
                    vfx.SetFloat(_seedsPlantedVfxMultiplier, 1.0f - _currentSeedsPlantedPercent);
                }
            }
        }

        #endregion

        #region Event Handlers

        private void TransitionUpdateEventHandler(object sender, TransitionUpdateEventArgs args)
        {
            // global areas only listen to global updates
            // areas only listen to their own area
            if(_global && args.AreaId.Any() || args.AreaId != _areaId) {
                return;
            }

            _targetSeedsPlantedPercent = args.SeedsPlantedTransitionPercent;
            SetTargetPercent(args.EnemiesStompedTransitionPercent);
        }

        #endregion
    }
}
