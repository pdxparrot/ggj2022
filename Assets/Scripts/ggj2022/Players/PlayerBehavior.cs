using UnityEngine;
using UnityEngine.Assertions;

using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.ggj2022.Data.Players;

namespace pdxpartyparrot.ggj2022.Players
{
    [RequireComponent(typeof(ForestSpiritBehavior))]
    public sealed class PlayerBehavior : Game.Characters.Players.PlayerBehavior
    {
        public PlayerBehaviorData GamePlayerBehaviorData => (PlayerBehaviorData)PlayerBehaviorData;

        public Player GamePlayer => (Player)Owner;

        private ForestSpiritBehavior _forestSpiritBehavior;

        public ForestSpiritBehavior ForestSpiritBehavior => _forestSpiritBehavior;

        public override bool CanMove => base.CanMove && !ForestSpiritBehavior.IsDead;

        public override float MoveSpeed => ForestSpiritBehavior.IsDead ? 0 : _forestSpiritBehavior.MoveSpeedModifier * base.MoveSpeed;

        public float JumpHeightModifier => _forestSpiritBehavior.JumpHeightModifier;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _forestSpiritBehavior = GetComponent<ForestSpiritBehavior>();
        }

        #endregion

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is Player);
            Assert.IsTrue(behaviorData is PlayerBehaviorData);

            base.Initialize(behaviorData);

            Assert.IsTrue(HasBehaviorComponent<ForestSpiritBehavior>(), "ForestSpiritBehavior must be added to the behavior components!");
        }
    }
}
