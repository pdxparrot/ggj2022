using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using pdxpartyparrot.Core.UI;

namespace pdxpartyparrot.Game.UI
{
    [RequireComponent(typeof(UIObject))]
    public class LevelSelectUI : MonoBehaviour
    {
        [Serializable]
        protected class Level
        {
            [SerializeField]
            public string name;

            [SerializeField]
            public bool enabled = true;

            [SerializeField]
            public Image image;

            [SerializeField]
            public Button button;

            [SerializeField]
            public Sprite incomplete;

            [SerializeField]
            public Sprite complete;

            public void Initialize()
            {
                button.interactable = enabled;
                image.overrideSprite = incomplete;
            }

            public void Complete()
            {
                image.overrideSprite = complete;
                button.interactable = false;
            }
        }

        [SerializeField]
        private Level[] _levels;

        protected IReadOnlyCollection<Level> Levels => _levels;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            foreach(Level level in _levels) {
                level.Initialize();
            }
        }

        #endregion

        protected void EnableButtonInteract(Button button, bool interactable)
        {
            button.interactable = interactable;
            if(button.interactable) {
                button.Select();
                button.Highlight();
            }
        }
    }
}
