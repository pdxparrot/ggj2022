using System.Collections.Generic;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Cinematics
{
    public sealed class CinematicsManager : SingletonBehavior<CinematicsManager>
    {
        [SerializeField]
        private CinematicsData _data;

        private readonly Dictionary<string, Cinematic> _cinematicsPrefabs = new Dictionary<string, Cinematic>();

        public bool IsRunningCinematic => false;

        #region Unity Lifecycle

        private void Awake()
        {
            InitDebugMenu();

            foreach(Cinematic cinematicPrefab in _data.CinematicsPrefabs) {
                if(_cinematicsPrefabs.ContainsKey(cinematicPrefab.Id)) {
                    Debug.LogWarning($"Overwriting cinematic {cinematicPrefab.Id}");
                }
                _cinematicsPrefabs[cinematicPrefab.Id] = cinematicPrefab;
            }
        }

        #endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.CinematicsManager");
            debugMenuNode.RenderContentsAction = () => {
                // TODO:
            };
        }
    }
}
