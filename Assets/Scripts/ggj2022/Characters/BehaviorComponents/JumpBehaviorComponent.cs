using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.ggj2022.Players;

namespace pdxpartyparrot.ggj2022.Characters.BehaviorComponents
{
    public sealed class JumpBehaviorComponent : Game.Characters.BehaviorComponents.JumpBehaviorComponent
    {
        private PlayerBehavior GamePlayerBehavior => (PlayerBehavior)Behavior;

        public override float JumpHeight => GamePlayerBehavior.JumpHeightModifier * base.JumpHeight;

        protected override EffectTrigger JumpEffect => GamePlayerBehavior.ForestSpiritBehavior.IsLarge ? base.JumpEffect : base.JumpEffect;
    }
}
