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

        #region Unity Lifecycle

        private void Awake()
        {
            _seedContainer.Clear();
        }

        #endregion

        public void Reset()
        {
            _seedContainer.Clear();
        }

        public void SeedCollected()
        {
            Instantiate(_seedPrefab, _seedContainer);
        }
    }
}
