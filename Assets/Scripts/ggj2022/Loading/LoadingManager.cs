using UnityEngine;

using pdxpartyparrot.Game.Loading;
using pdxpartyparrot.ggj2022.NPCs;
using pdxpartyparrot.ggj2022.Players;
using pdxpartyparrot.ggj2022.UI;

namespace pdxpartyparrot.ggj2022.Loading
{
    public sealed class LoadingManager : LoadingManager<LoadingManager>
    {
        [Space(10)]

        #region Manager Prefabs

        [Header("Project Manager Prefabs")]

        [SerializeField]
        private GameManager _gameManagerPrefab;

        [SerializeField]
        private GameUIManager _gameUiManagerPrefab;

        [SerializeField]
        private PlayerManager _playerManager;

        [SerializeField]
        private NPCManager _npcManager;

        #endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            GameManager.CreateFromPrefab(_gameManagerPrefab, ManagersContainer);
            GameUIManager.CreateFromPrefab(_gameUiManagerPrefab, ManagersContainer);
            PlayerManager.CreateFromPrefab(_playerManager, ManagersContainer);
            NPCManager.CreateFromPrefab(_npcManager, ManagersContainer);
        }
    }
}
