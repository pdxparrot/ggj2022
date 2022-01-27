using UnityEngine;

using pdxpartyparrot.Core.Animation;

namespace pdxpartyparrot.ggj2022.Players
{
    [RequireComponent(typeof(SpriteAnimationHelper))]
    public sealed class ForestSpiritModel : MonoBehaviour
    {
        [SerializeField]
        private PlayerBehavior _behavior;

        [SerializeField]
        private GameObject _smallModel;

        private Animator _smallAnimator;

        [SerializeField]
        private GameObject _largeModel;

        private Animator _largeAnimator;

        #region Unity Lifecycle

        private void Awake()
        {
            _smallAnimator = _smallModel.GetComponent<Animator>();
            _largeAnimator = _largeModel.GetComponent<Animator>();

            SetForm(ForestSpiritBehavior.SpiritForm.Small);
        }

        #endregion

        public void SetForm(ForestSpiritBehavior.SpiritForm form)
        {
            _smallModel.SetActive(ForestSpiritBehavior.SpiritForm.Small == form);
            _largeModel.SetActive(ForestSpiritBehavior.SpiritForm.Large == form);

            _behavior.Animator = ForestSpiritBehavior.SpiritForm.Small == form ? _smallAnimator : _largeAnimator;
        }
    }
}
