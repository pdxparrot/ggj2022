using System;

using UnityEngine;

using pdxpartyparrot.ggj2022.Camera;

namespace pdxpartyparrot.ggj2022.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "pdxpartyparrot/ggj2022/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        public GameViewer GameViewerPrefab => (GameViewer)ViewerPrefab;

        #region Project Game States

        //[Header("Project Game States")]

        #endregion
    }
}
