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

        #region Unity Lifecycle

        private void Awake()
        {
            if(null == _renderer) {
                _renderer = GetComponent<Renderer>();
            }
        }

        private void Update()
        {
            foreach(Material material in _renderer.materials) {
                material.SetFloat(_parameter, _lastPercent);
            }
        }

        #endregion

        public void SetPercent(float percent)
        {
            _lastPercent = percent;
        }
    }
}
