using System;

using UnityEngine;

using pdxpartyparrot.ggj2022.Camera;
using pdxpartyparrot.ggj2022.Data.NPCs;
using pdxpartyparrot.ggj2022.NPCs;

namespace pdxpartyparrot.ggj2022.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "pdxpartyparrot/ggj2022/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        public GameViewer GameViewerPrefab => (GameViewer)ViewerPrefab;

        [Space(10)]

        [Header("NPCs")]

        [SerializeField]
        private string _slimeSpawnTag = "Slime";

        public string SlimeSpawnTag => _slimeSpawnTag;

        [SerializeField]
        private Slime _slimePrefab;

        public Slime SlimePrefab => _slimePrefab;

        [SerializeField]
        private SlimeData _slimeData;

        public SlimeData SlimeData => _slimeData;

        [SerializeField]
        private SlimeBehaviorData _slimeBehaviorData;

        public SlimeBehaviorData SlimeBehaviorData => _slimeBehaviorData;
    }
}
