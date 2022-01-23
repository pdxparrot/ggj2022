using System.Collections.Generic;

using UnityEngine;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.ggj2022.UI
{
    public sealed class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private HealthHeart _heartPrefab;

        private readonly List<HealthHeart> _hearts = new List<HealthHeart>();

        #region Unity Lifecycle

        private void Awake()
        {
            transform.Clear();
        }

        #endregion

        public void ResetHealth(int health)
        {
            _hearts.Clear();
            transform.Clear();

            HealthHeart heart = null;
            for(int i = 0; i < health; ++i) {
                if(i % 2 == 0) {
                    heart = Instantiate(_heartPrefab, transform);
                    _hearts.Add(heart);

                    heart.UpdateHealth(HealthHeart.HealthValue.Half);
                } else {
                    heart.UpdateHealth(HealthHeart.HealthValue.Full);
                }
            }
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
