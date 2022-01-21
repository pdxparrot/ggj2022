using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Data.Players
{
    [CreateAssetMenu(fileName = "ForestSpiritBehaviorData", menuName = "pdxpartyparrot/ggj2022/Data/Players/ForestSpiritBehavior Data")]
    [Serializable]
    public sealed class ForestSpiritBehaviorData : ScriptableObject
    {
        [SerializeField]
        private float _largeSpiritMoveSpeedModifier = 0.5f;

        public float LargeSpiritMoveSpeedModifier => _largeSpiritMoveSpeedModifier;
    }
}
