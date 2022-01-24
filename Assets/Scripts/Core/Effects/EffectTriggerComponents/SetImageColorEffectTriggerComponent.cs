using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class SetImageColorEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private Image _image;

        public Image Image
        {
            get => _image;
            set => _image = value;
        }

        [SerializeField]
        private Color _color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        public override bool WaitForComplete => false;

        public override void OnStart()
        {
            Image.color = _color;
        }
    }
}
