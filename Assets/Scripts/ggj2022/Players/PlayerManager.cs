using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Game.Players;
using pdxpartyparrot.ggj2022.Data.Players;

namespace pdxpartyparrot.ggj2022.Players
{
    public sealed class PlayerManager : PlayerManager<PlayerManager>
    {
        public PlayerData GamePlayerData => (PlayerData)PlayerData;

        [CanBeNull]
        public Player GetPlayer()
        {
            return Players.First() as Player;
        }
    }
}
