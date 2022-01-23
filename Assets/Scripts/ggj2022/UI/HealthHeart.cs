using UnityEngine;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.ggj2022.UI
{
    public sealed class HealthHeart : MonoBehaviour
    {
        public enum HealthValue
        {
            Empty,
            Half,
            Full,
        }

        [SerializeField]
        private GameObject _leftHalf;

        [SerializeField]
        private GameObject _rightHalf;

        [SerializeField]
        [ReadOnly]
        private HealthValue _health;

        public HealthValue Health => _health;

        #region Unity Lifecycle

        private void Awake()
        {
            UpdateHealth(HealthValue.Empty);
        }

        #endregion

        public void UpdateHealth(HealthValue value)
        {
            _health = value;

            _leftHalf.SetActive(HealthValue.Half == _health || HealthValue.Full == _health);
            _rightHalf.SetActive(HealthValue.Full == _health);
        }
    }
}
