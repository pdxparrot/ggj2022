using System.Collections.Generic;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.NPCs;
using pdxpartyparrot.ggj2022.Players;

using UnityEngine;
using UnityEngine.InputSystem;

namespace pdxpartyparrot.ggj2022.NPCs
{
    public sealed class NPCManager : NPCManager<NPCManager>
    {
        [SerializeField]
        private Key _stompEnemiesKey = Key.K;

        #region Unity Lifecycle

#if UNITY_EDITOR
        private void FixedUpdate()
        {
            if(Keyboard.current[_stompEnemiesKey].wasPressedThisFrame) {
                StompEnemies();
            }
        }
#endif

        #endregion

        public void SpawnEnemies(Transform container)
        {
            IReadOnlyCollection<SpawnPoint> spawnPoints = SpawnManager.Instance.GetSpawnPoints(GameManager.Instance.GameGameData.SlimeSpawnTag);

            Debug.Log($"Spawning slimes from {spawnPoints.Count} spawners");

            foreach(SpawnPoint spawnPoint in spawnPoints) {
                spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.SlimePrefab, GameManager.Instance.GameGameData.SlimeBehaviorData, container);
            }
        }

#if UNITY_EDITOR
        public void StompEnemies()
        {
            Player player = PlayerManager.Instance.GetPlayer();

            List<INPC> enemies = new List<INPC>(NPCs);
            foreach(INPC npc in enemies) {
                Slime slime = npc as Slime;
                if(null != slime) {
                    slime.SlimeBehavior.Stomp(player);
                }
            }
        }
#endif
    }
}
