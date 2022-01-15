using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data.Characters.BehaviorComponents
{
    [CreateAssetMenu(fileName = "JumpBehaviorComponentData", menuName = "pdxpartyparrot/Game/Data/Behavior Components/JumpBehaviorComponent Data")]
    [Serializable]
    public class JumpBehaviorComponentData : CharacterBehaviorComponentData
    {
        [SerializeField]
        [Range(0, 100)]
        [Tooltip("How high does the character jump")]
        private float _jumpHeight = 10.0f;

        public float JumpHeight => _jumpHeight;

        [Space(10)]

        #region Animations

        [Header("Character Animations")]

        [SerializeField]
        private string _jumpParam = "OnJump";

        public string JumpParam => _jumpParam;

        #endregion
    }
}
