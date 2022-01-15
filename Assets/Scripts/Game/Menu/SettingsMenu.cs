using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Game.Menu
{
    public abstract class SettingsMenu : MenuPanel
    {
        [Space(10)]

        [SerializeField]
        private Toggle _invertYAxisToggle;

        #region Unity Lifecycle

        protected override void OnEnable()
        {
            base.OnEnable();

            _invertYAxisToggle.isOn = GameStateManager.Instance.GameManager.Settings.InvertLookVertical;
        }

        #endregion

        #region Events

        public void OnInvertYAxisToggle(bool state)
        {
            GameStateManager.Instance.GameManager.Settings.InvertLookVertical = state;
        }

        #endregion
    }
}
