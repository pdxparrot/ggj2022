using System;
using System.Collections.Generic;

using pdxpartyparrot.Game.Cinematics;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "pdxpartyparrot/Game/Data/Dialogue Data")]
    [Serializable]
    public class DialogueData : ScriptableObject
    {
        [SerializeField]
        [Tooltip("How long should new dialogues be open before listening for input")]
        private float _inputDelay = 0.5f;

        public float InputDelay => _inputDelay;

        [SerializeField]
        private List<Dialogue> _dialoguePrefabs;

        public List<Dialogue> DialoguePrefabs => _dialoguePrefabs;
    }
}
