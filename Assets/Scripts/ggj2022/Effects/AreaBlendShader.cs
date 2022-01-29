using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Effects
{
    public sealed class AreaBlendShader : BlendShader
    {
        [SerializeField]
        private string _areaId;

        [SerializeField]
        private string _seedsPlantedParameter = "Growth Override";

        [SerializeField]
        [ReadOnly]
        private float _lastSeedsPlantedPercent;

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
            if(IsDirty) {
                //Debug.Log($"Updating area {_areaId} blend ('{Parameter}' => {LastPercent}, '{_seedsPlantedParameter}' => {_lastSeedsPlantedPercent})");

                foreach(Renderer renderer in Renderers) {
                    foreach(Material material in renderer.materials) {
                        material.SetFloat(_seedsPlantedParameter, _lastSeedsPlantedPercent);
                    }
                }
            }

            base.Update();
        }

        #endregion

        #region Event Handlers

        private void TransitionUpdateEventHandler(object sender, TransitionUpdateEventArgs args)
        {
            if(args.AreaId != _areaId) {
                return;
            }

            _lastSeedsPlantedPercent = args.SeedsPlantedTransitionPercent;
            SetPercent(args.EnemiesStompedTransitionPercent);
        }

        #endregion
    }
}
