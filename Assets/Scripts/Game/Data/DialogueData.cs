using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [Serializable]
    public abstract class DialogueData : ScriptableObject
    {
        [SerializeField]
        [Tooltip("How long should new dialogues be open before listening for input")]
        private float _inputDelay = 0.5f;

        public float InputDelay => _inputDelay;
    }
}
