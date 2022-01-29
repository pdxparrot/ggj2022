using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Effects
{
    [Inspectable]
    [Serializable]
    public class ViewerShakeConfig
    {
        [SerializeField]
        private FloatRangeConfig _durationRange = new FloatRangeConfig(0.5f, 0.5f);

        [Inspectable]
        public FloatRangeConfig DurationRange
        {
            get => _durationRange;
            set => _durationRange = value;
        }

        [SerializeField]
        private FloatRangeConfig _forceRange = new FloatRangeConfig(1.0f, 1.0f);

        [Inspectable]
        public FloatRangeConfig ForceRange => _forceRange;

        [SerializeField]
        private FloatRangeConfig _xVelocityRange = new FloatRangeConfig(-1.0f, 1.0f);

        [Inspectable]
        public FloatRangeConfig XVelocityRange
        {
            get => _xVelocityRange;
            set => _xVelocityRange = value;
        }

        [SerializeField]
        private FloatRangeConfig _yVelocityRange = new FloatRangeConfig(-1.0f, 1.0f);

        [Inspectable]
        public FloatRangeConfig YVelocityRange
        {
            get => _yVelocityRange;
            set => _yVelocityRange = value;
        }
    }
}
