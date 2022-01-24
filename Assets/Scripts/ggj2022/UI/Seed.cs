using UnityEngine;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.ggj2022.UI
{
    public sealed class Seed : MonoBehaviour
    {
        [SerializeField]
        private GameObject _seed;

        [SerializeField]
        private GameObject _plant;

        [SerializeField]
        [ReadOnly]
        private bool _planted;

        public bool IsPlanted => _planted;

        #region Unity Lifecycle

        private void Awake()
        {
            _planted = false;

            _seed.SetActive(true);
            _plant.SetActive(false);
        }

        #endregion

        public void SetPlanted()
        {
            _planted = true;

            _seed.SetActive(false);
            _plant.SetActive(true);
        }
    }
}
