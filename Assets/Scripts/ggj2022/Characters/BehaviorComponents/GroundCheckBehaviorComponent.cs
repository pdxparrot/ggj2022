using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.ggj2022.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Characters.BehaviorComponents
{
    public sealed class GroundCheckBehaviorComponent : Game.Characters.BehaviorComponents.GroundCheckBehaviorComponent
    {
        private PlayerBehavior GamePlayerBehavior => (PlayerBehavior)Behavior;

        [SerializeField]
        private EffectTrigger _smallGroundedEffect;

        [SerializeField]
        private EffectTrigger _bigGroundedEffect;


        protected override EffectTrigger GroundedEffect => GamePlayerBehavior.ForestSpiritBehavior.IsLarge ? _bigGroundedEffect : _smallGroundedEffect;
    }
}
