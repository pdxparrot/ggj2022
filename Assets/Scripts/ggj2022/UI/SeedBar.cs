using System.Collections.Generic;

using UnityEngine;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.ggj2022.UI
{
    public sealed class SeedBar : MonoBehaviour
    {
        [SerializeField]
        private Seed _seedPrefab;

        [SerializeField]
        private Transform _seedContainer;

        private readonly List<Seed> _seeds = new List<Seed>();

        #region Unity Lifecycle

        private void Awake()
        {
            _seedContainer.Clear();
        }

        #endregion

        public void Reset()
        {
            _seedContainer.Clear();
            _seeds.Clear();
        }

        public void SeedCollected()
        {
            Seed seed = Instantiate(_seedPrefab, _seedContainer);
            _seeds.Add(seed);
        }

        public void SeedPlanted()
        {
            foreach(Seed seed in _seeds) {
                if(!seed.IsPlanted) {
                    seed.SetPlanted();
                    return;
                }
            }

            Debug.LogWarning("No seed found to update as planted!");
        }
    }
}
