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
        #region Duration

        [SerializeField]
        private float _durationMin = 0.5f;

        [Inspectable]
        public float DurationMin
        {
            get => _durationMin;
            set => _durationMin = value;
        }

        [SerializeField]
        private float _durationMax = 0.5f;

        [Inspectable]
        public float DurationMax
        {
            get => _durationMax;
            set => _durationMax = value;
        }

        public FloatRangeConfig DurationRange => new FloatRangeConfig(DurationMin, DurationMax);

        #endregion

        #region Force

        [SerializeField]
        private float _forceMin = 0.1f;

        [Inspectable]
        public float ForceMin
        {
            get => _forceMin;
            set => _forceMin = value;
        }

        [SerializeField]
        private float _forceMax = 1.0f;

        [Inspectable]
        public float ForceMax
        {
            get => _forceMax;
            set => _forceMax = value;
        }

        public FloatRangeConfig ForceRange => new FloatRangeConfig(ForceMin, ForceMax);

        #endregion

        #region Velocity (X)

        [SerializeField]
        private float _velocityXMin = 0.1f;

        [Inspectable]
        public float VelocityXMin
        {
            get => _velocityXMin;
            set => _velocityXMin = value;
        }

        [SerializeField]
        private float _velocityXMax = 1.0f;

        [Inspectable]
        public float VelocityXMax
        {
            get => _velocityXMax;
            set => _velocityXMax = value;
        }

        public FloatRangeConfig VelocityXRange => new FloatRangeConfig(VelocityXMin, VelocityXMax);

        #endregion

        #region Velocity (Y)

        [SerializeField]
        private float _velocityYMin = 0.1f;

        [Inspectable]
        public float VelocityYMin
        {
            get => _velocityYMin;
            set => _velocityYMin = value;
        }

        [SerializeField]
        private float _velocityYMax = 1.0f;

        [Inspectable]
        public float VelocityYMax
        {
            get => _velocityYMax;
            set => _velocityYMax = value;
        }

        public FloatRangeConfig VelocityYRange => new FloatRangeConfig(VelocityYMin, VelocityYMax);

        #endregion
    }
}
