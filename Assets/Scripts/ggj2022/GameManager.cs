using pdxpartyparrot.Game;
using pdxpartyparrot.ggj2022.Data;

namespace pdxpartyparrot.ggj2022
{
    public sealed class GameManager : GameManager<GameManager>
    {
        public GameData GameGameData => (GameData)GameData;
    }
}
