using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Game.UI
{
    public abstract class GameUI : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        [CanBeNull]
        private Image _fadeOverlay;

        [CanBeNull]
        public Image FadeOverlay => _fadeOverlay;

        #region Unity Lifecycle

        private void Awake()
        {
            _fadeOverlay.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        #endregion

        public virtual void Initialize(UnityEngine.Camera uiCamera)
        {
            _canvas.worldCamera = uiCamera;
        }
    }
}
