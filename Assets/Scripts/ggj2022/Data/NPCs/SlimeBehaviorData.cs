using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Data.NPCs
{
    [CreateAssetMenu(fileName = "SlimeBehaviorData", menuName = "pdxpartyparrot/ggj2022/Data/NPCs/SlimeBehavior Data")]
    [Serializable]
    public sealed class SlimeBehaviorData : Game.Data.Characters.NPCBehaviorData
    {
        [SerializeField]
        private int _damageAmount = 1;

        public int DamageAmount => _damageAmount;
    }
}
