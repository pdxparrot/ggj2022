using System;

using UnityEngine;
using Unity.VisualScripting;

namespace pdxpartyparrot.Core.Input
{
    [Inspectable]
    [Serializable]
    public class RumbleConfig
    {
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float _seconds = 0.5f;

        [Inspectable]
        public float Seconds
        {
            get => _seconds;
            set => _seconds = value;
        }

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _lowFrequency = 0.5f;

        [Inspectable]
        public float LowFrequency
        {
            get => _lowFrequency;
            set => _lowFrequency = value;
        }

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _highFrequency = 0.5f;

        [Inspectable]
        public float HighFrequency
        {
            get => _highFrequency;
            set => _highFrequency = value;
        }
    }
}
