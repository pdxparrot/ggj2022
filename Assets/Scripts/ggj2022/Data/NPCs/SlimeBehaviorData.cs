using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Data.NPCs
{
    [CreateAssetMenu(fileName = "SlimeBehaviorData", menuName = "pdxpartyparrot/ggj2022/Data/NPCs/SlimeBehavior Data")]
    [Serializable]
    public sealed class SlimeBehaviorData : Game.Data.Characters.NPCBehaviorData
    {
        [Space(10)]

        [SerializeField]
        private int _damageAmount = 1;

        public int DamageAmount => _damageAmount;

        [SerializeField]
        private FloatRangeConfig _idleTimeRange = new FloatRangeConfig(1.0f, 3.0f);

        public FloatRangeConfig IdleTimeRange => _idleTimeRange;

        [SerializeField]
        private FloatRangeConfig _patrolRange = new FloatRangeConfig(5.0f, 10.0f);

        public FloatRangeConfig PatrolRange => _patrolRange;

        [SerializeField]
        private float _chaseSpeedModifier = 2.0f;

        public float ChaseSpeedModifier => _chaseSpeedModifier;

        [SerializeField]
        private float _leashSpeedModifier = 2.0f;

        public float LeashSpeedModifier => _leashSpeedModifier;
    }
}
