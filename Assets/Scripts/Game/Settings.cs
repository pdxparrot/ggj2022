using System;

using UnityEngine;

namespace pdxpartyparrot.Game
{
    // non-permanent settings
    // useful in game jams for saved settings
    // that should be reset between sessions
    // (where it's expected to be a different person playing)
    [Serializable]
    public class Settings
    {
        #region Events

        public event EventHandler<EventArgs> SettingsUpdatedEvent;

        #endregion

        [SerializeField]
        private bool _invertLookVertical;

        public bool InvertLookVertical
        {
            get => _invertLookVertical;

            set
            {
                _invertLookVertical = value;
                SettingsUpdatedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual void Reset()
        {
            _invertLookVertical = false;
        }
    }
}
