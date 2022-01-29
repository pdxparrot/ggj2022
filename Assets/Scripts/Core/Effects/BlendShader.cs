using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects
{
    public class BlendShader : MonoBehaviour
    {
        [SerializeField]
        private string _parameter = "BlendPct";

        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        [ReadOnly]
        private float _lastPercent;

        [SerializeField]
        [ReadOnly]
        private bool _dirty;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            if(null == _renderer) {
                _renderer = GetComponent<Renderer>();
            }
        }

        protected virtual void Update()
        {
            if(_dirty) {
                foreach(Material material in _renderer.materials) {
                    material.SetFloat(_parameter, _lastPercent);
                }
                _dirty = false;
            }
        }

        #endregion

        public void SetPercent(float percent)
        {
            _lastPercent = percent;
            _dirty = true;
        }
    }
}
