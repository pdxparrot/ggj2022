using pdxpartyparrot.Core.Effects;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Effects
{
    public class AreaBlendShader : BlendShader
    {
        [SerializeField]
        private string _areaId;

        #region Unity Lifecycle

        protected override void Awake()
        {
            // TODO: hook up game manager event listener
        }

        private void OnDestroy()
        {
            // TODO: free game manager event listener
        }

        #endregion
    }
}
