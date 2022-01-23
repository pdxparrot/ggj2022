using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [Serializable]
    public abstract class GameData : ScriptableObject
    {
        [Header("World")]

        [SerializeField]
        private string _worldLayer = "World";

        public LayerMask WorldLayer => LayerMask.NameToLayer(_worldLayer);

        #region Viewer

        [Space(10)]

        [Header("Viewer")]

        [SerializeField]
        private Viewer _viewerPrefab;

        public Viewer ViewerPrefab => _viewerPrefab;

        // TODO: for this crap that can be set on the camera we should just set it there
        // and document in the README what to set for each thing

        // TODO: this probably isn't the best way to handle this or the best place to put it
        // TODO: also, this is the *2D* viewport size and entirely irrelevant to 3D games
        // and that should be made clearer in the data
        [SerializeField]
        [Tooltip("The orthographic size of the 2D camera, which is also the height of the viewport.")]
        private float _viewportSize = 10;

        public float ViewportSize => _viewportSize;

        // TODO: this probably isn't the best way to handle this or the best place to put it
        // TODO: also, this is the *3D* fov and entirely irrelevant to 2D games
        // and that should be made clearer in the data
        [SerializeField]
        [Tooltip("The Field of View of the 3D camera.")]
        private float _fov = 60;

        public float FoV => _fov;

        // TODO: this probably isn't the best way to handle this or the best place to put it
        // TODO: also, this is the *3D* distance and entirely irrelevant to 2D games
        // and that should be made clearer in the data
        [SerializeField]
        [Tooltip("The Distance of the 3D camera.")]
        private float _distance = 10;

        public float Distance => _distance;

        #endregion

        [Space(10)]

        [Header("Players")]

        [SerializeField]
        private int _maxLocalPlayers = 1;

        public int MaxLocalPlayers => _maxLocalPlayers;

        [SerializeField]
        [Tooltip("Spawn a player for each connected game pad, up to the max")]
        private bool _gamepadsArePlayers;

        public bool GamepadsArePlayers => _gamepadsArePlayers;

        [SerializeField]
        [Tooltip("Should the main game state attempt to spawn players as soon as the main scene loads?")]
        private bool _spawnPlayersOnLoad = true;

        public bool SpawnPlayersOnLoad => _spawnPlayersOnLoad;

        [Space(10)]

        #region Floating Text

        [Header("Floating text")]

        [SerializeField]
        [CanBeNull]
        private FloatingText _floatingTextPrefab;

        [CanBeNull]
        public FloatingText FloatingTextPrefab => _floatingTextPrefab;

        [SerializeField]
        private int _floatingTextPoolSize = 10;

        public int FloatingTextPoolSize => _floatingTextPoolSize;

        #endregion

        #region Game States

        [Header("Game States")]

        [SerializeField]
        private MainGameState _mainGameStatePrefab;

        public MainGameState MainGameStatePrefab => _mainGameStatePrefab;

        #endregion
    }
}
