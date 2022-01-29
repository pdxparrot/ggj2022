using System;

using pdxpartyparrot.Game.Data.Characters.BehaviorComponents;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Data.Players
{
    [CreateAssetMenu(fileName = "ForestSpiritBehaviorData", menuName = "pdxpartyparrot/ggj2022/Data/Players/ForestSpiritBehavior Data")]
    [Serializable]
    public sealed class ForestSpiritBehaviorData : CharacterBehaviorComponentData
    {
        [SerializeField]
        private float _largeSpiritMoveSpeedModifier = 0.5f;

        public float LargeSpiritMoveSpeedModifier => _largeSpiritMoveSpeedModifier;

        [SerializeField]
        private float _largeSpiritJumpHeightModifier = 0.5f;

        public float LargeSpiritJumpHeightModifier => _largeSpiritJumpHeightModifier;

        [SerializeField]
        private int _startingHealth = 10;

        public int StartingHealth => _startingHealth;

        [SerializeField]
        private int _maxHealth = 10;

        public int MaxHealth => _maxHealth;

        [Space(10)]

        #region Animation Parameters

        [SerializeField]
        private string _damagedParam = "OnHurt";

        public string DamagedParam => _damagedParam;

        [SerializeField]
        private string _deathParam = "OnDeath";

        public string DeathParam => _deathParam;

        #endregion
    }
}
