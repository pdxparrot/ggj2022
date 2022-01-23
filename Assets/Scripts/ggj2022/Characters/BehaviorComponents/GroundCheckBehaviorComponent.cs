using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.ggj2022.Players;

namespace pdxpartyparrot.ggj2022.Characters.BehaviorComponents
{
    public sealed class GroundCheckBehaviorComponent : Game.Characters.BehaviorComponents.GroundCheckBehaviorComponent
    {
        private PlayerBehavior GamePlayerBehavior => (PlayerBehavior)Behavior;

        protected override EffectTrigger GroundedEffect => GamePlayerBehavior.ForestSpiritBehavior.IsLarge ? base.GroundedEffect : base.GroundedEffect;
    }
}
