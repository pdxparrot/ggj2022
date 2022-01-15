using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data.Players
{
    [Serializable]
    public abstract class PlayerData : ScriptableObject
    {
        [SerializeField]
        private string _respawnTag = "PlayerRespawn";

        public string RespawnTag => _respawnTag;
    }
}
