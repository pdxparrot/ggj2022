using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Effects
{
    public sealed class AreaBlendShader : BlendShader
    {
        [SerializeField]
        private string _areaId;

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

        #region Unity Lifecycle

        protected override void Awake()
        {
            GameManager.Instance.TransitionUpdateEvent += TransitionUpdateEventHandler;
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.TransitionUpdateEvent -= TransitionUpdateEventHandler;
            }
        }

        protected override void Update()
        {
            if(_currentSeedsPlantedPercent != _targetSeedsPlantedPercent) {
                //Debug.Log($"Updating area {_areaId} blend ('{Parameter}' => {LastPercent}, '{_seedsPlantedParameter}' => {_lastSeedsPlantedPercent})");

                float dt = Time.deltaTime;
                _currentSeedsPlantedPercent = LerpParameter(_seedsPlantedParameter, _currentSeedsPlantedPercent, _targetSeedsPlantedPercent, LerpSpeed * dt);
            }

            base.Update();
        }

        #endregion

        #region Event Handlers

        private void TransitionUpdateEventHandler(object sender, TransitionUpdateEventArgs args)
        {
            if(args.AreaId != string.Empty && args.AreaId != _areaId) {
                return;
            }

            _targetSeedsPlantedPercent = args.SeedsPlantedTransitionPercent;
            SetTargetPercent(args.EnemiesStompedTransitionPercent);
        }

        #endregion
    }
}
