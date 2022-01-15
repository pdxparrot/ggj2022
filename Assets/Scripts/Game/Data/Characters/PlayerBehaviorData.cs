using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data.Characters
{
    [Serializable]
    public abstract class PlayerBehaviorData : CharacterBehaviorData
    {
        [Space(10)]

        #region Look

        [Header("Player Look")]

        [SerializeField]
        private bool _allowLookHorizontal;

        public bool AllowLookHorizontal => _allowLookHorizontal;

        [SerializeField]
        private float _horizontalLookSpeed = 75.0f;

        public float HorizontalLookSpeed => _horizontalLookSpeed;

        [Space(10)]

        [SerializeField]
        private bool _allowLookVertical;

        public bool AllowLookVertical => _allowLookVertical;

        [SerializeField]
        private float _verticalLookSpeed = 75.0f;

        public float VerticalLookSpeed => _verticalLookSpeed;

        [SerializeField]
        private float _maxVerticalRotation = 45.0f;

        public float MaxVerticalRotation => _maxVerticalRotation;

        #endregion

        [Space(10)]

        #region Movement

        [Header("Player Movement")]

        [SerializeField]
        private bool _alignMovementWithViewer;

        public bool AlignMovementWithViewer => _alignMovementWithViewer;

        #endregion
    }
}
