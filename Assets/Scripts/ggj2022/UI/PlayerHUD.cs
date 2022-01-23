using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.UI
{
    public sealed class PlayerHUD : HUD
    {
        [SerializeField]
        private HealthBar _healthBar;

        [SerializeField]
        private SeedBar _seedBar;

        public void Reset(int maxHealth, int startingHealth)
        {
            _healthBar.Reset(maxHealth, startingHealth);
            _seedBar.Reset();
        }

        public void UpdateHealth(int health)
        {
            _healthBar.UpdateHealth(health);
        }

        public void SeedCollected()
        {
            _seedBar.SeedCollected();
        }
    }
}
