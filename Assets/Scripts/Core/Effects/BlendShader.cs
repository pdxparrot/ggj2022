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
        private string _parameter = "BlendPct";

        public string Parameter => _parameter;

        [SerializeField]
        private List<Renderer> _renderers = new List<Renderer>();

        protected IReadOnlyCollection<Renderer> Renderers => _renderers;

        [SerializeField]
        [ReadOnly]
        private float _lastPercent;

        public float LastPercent => _lastPercent;

        [SerializeField]
        [ReadOnly]
        private bool _dirty;

        public bool IsDirty
        {
            get => _dirty;
            protected set => _dirty = value;
        }

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
            if(IsDirty) {
                foreach(Renderer renderer in _renderers) {
                    foreach(Material material in renderer.materials) {
                        material.SetFloat(_parameter, _lastPercent);
                    }
                }
                IsDirty = false;
            }
        }

        #endregion

        public void SetPercent(float percent)
        {
            _lastPercent = percent;
            IsDirty = true;
        }
    }
}
