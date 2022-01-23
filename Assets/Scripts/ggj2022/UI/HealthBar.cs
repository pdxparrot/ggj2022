using System.Collections.Generic;

using UnityEngine;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.ggj2022.UI
{
    public sealed class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private HealthHeart _heartPrefab;

        [SerializeField]
        private Transform _heartContainer;

        private readonly List<HealthHeart> _hearts = new List<HealthHeart>();

        #region Unity Lifecycle

        private void Awake()
        {
            _heartContainer.Clear();
        }

        #endregion

        public void Reset(int maxHealth, int startingHealth)
        {
            _hearts.Clear();
            _heartContainer.Clear();

            int heartCount = (int)Mathf.Ceil(maxHealth / 2.0f);
            for(int i = 0; i < heartCount; ++i) {
                HealthHeart heart = Instantiate(_heartPrefab, _heartContainer);
                _hearts.Add(heart);
            }

            UpdateHealth(startingHealth);
        }

        public void UpdateHealth(int health)
        {
            foreach(HealthHeart heart in _hearts) {
                if(health == 0) {
                    heart.UpdateHealth(HealthHeart.HealthValue.Empty);
                    continue;
                }

                health--;
                if(health == 0) {
                    heart.UpdateHealth(HealthHeart.HealthValue.Half);
                } else {
                    health--;
                    heart.UpdateHealth(HealthHeart.HealthValue.Full);
                }
            }
        }
    }
}
