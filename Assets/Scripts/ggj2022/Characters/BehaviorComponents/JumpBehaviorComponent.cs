using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.ggj2022.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Characters.BehaviorComponents
{
    public sealed class JumpBehaviorComponent : Game.Characters.BehaviorComponents.JumpBehaviorComponent
    {
        private PlayerBehavior GamePlayerBehavior => (PlayerBehavior)Behavior;

        public override float JumpHeight => GamePlayerBehavior.JumpHeightModifier * base.JumpHeight;

        [SerializeField]
        private EffectTrigger _smallJumpEffect;

        [SerializeField]
        private EffectTrigger _bigJumpEffect;

        protected override EffectTrigger JumpEffect => GamePlayerBehavior.ForestSpiritBehavior.IsLarge ? _bigJumpEffect : _smallJumpEffect;
    }
}
