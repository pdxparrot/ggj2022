using JetBrains.Annotations;

using pdxpartyparrot.Game.UI;

namespace pdxpartyparrot.ggj2022.UI
{
    public sealed class GameUIManager : GameUIManager<GameUIManager>
    {
        [CanBeNull]
        public GameUI GameGameUI => (GameUI)GameUI;
    }
}
