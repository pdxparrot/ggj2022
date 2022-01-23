using UnityEngine;

using pdxpartyparrot.Core.Animation;

namespace pdxpartyparrot.ggj2022.Players
{
    [RequireComponent(typeof(SpriteAnimationHelper))]
    public sealed class ForestSpiritModel : MonoBehaviour
    {
        [SerializeField]
        private GameObject _smallModel;

        [SerializeField]
        private GameObject _largeModel;

        #region Unity Lifecycle

        private void Awake()
        {
            _smallModel.SetActive(true);
            _largeModel.SetActive(false);
        }

        #endregion

        public void SetForm(ForestSpiritBehavior.SpiritForm form)
        {
            _smallModel.SetActive(ForestSpiritBehavior.SpiritForm.Small == form);
            _largeModel.SetActive(ForestSpiritBehavior.SpiritForm.Large == form);
        }
    }
}
