using pdxpartyparrot.Game.Players.Input;
using pdxpartyparrot.ggj2022.Data.Players;

using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2022.Players
{
    public sealed class PlayerInputHandler : SideScollerPlayerInputHandler
    {
        private Player GamePlayer => (Player)Player;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerInputData is PlayerInputData);
            Assert.IsTrue(Player is Player);
        }

        #endregion
    }
}
