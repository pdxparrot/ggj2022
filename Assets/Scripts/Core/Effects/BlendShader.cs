using System.Collections.Generic;
using System.Linq;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects
{
    public class BlendShader : MonoBehaviour
    {
        // NOTE: this is the shader property Reference value, not the Name value
        [SerializeField]
        [Tooltip("The shader property Reference value")]
        private string _parameter = "_BlendPct";

        public string Parameter => _parameter;

        [SerializeField]
        private List<Renderer> _renderers = new List<Renderer>();

        protected IReadOnlyCollection<Renderer> Renderers => _renderers;

        [SerializeField]
        private float _lerpSpeed;

        protected float LerpSpeed => _lerpSpeed;

        protected bool IsLerpParameter => _lerpSpeed > 0.0f;

        [SerializeField]
        [ReadOnly]
        private float _currentPercent;

        protected float CurrentPercent => _currentPercent;

        [SerializeField]
        [ReadOnly]
        private float _targetPercent;

        protected float TargetPercent => _targetPercent;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            if(!_renderers.Any()) {
                Renderer renderer = GetComponent<Renderer>();
                if(null != renderer) {
                    _renderers.Add(renderer);
                }
            }
        }

        protected virtual void Update()
        {
            float dt = UnityEngine.Time.deltaTime;

            if(_currentPercent != _targetPercent) {
                _currentPercent = LerpParameter(_parameter, _currentPercent, _targetPercent, LerpSpeed * dt);
            }
        }

        #endregion

        public void SetTargetPercent(float percent)
        {
            _targetPercent = percent;
        }

        protected float LerpParameter(string parameter, float current, float target, float distance)
        {
            float value = IsLerpParameter ? Mathf.MoveTowards(current, target, distance) : target;

            foreach(Renderer renderer in _renderers) {
                foreach(Material material in renderer.materials) {
                    material.SetFloat(parameter, value);
                }
            }

            return value;
        }
    }
}
