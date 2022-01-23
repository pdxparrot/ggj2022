using JetBrains.Annotations;

using Cinemachine;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Camera
{
    // this has to be set as the Body in order to work correctly
    //[RequireComponent(typeof(CinemachineFramingTransposer))]
    public class SideScrollerViewer25D : CinemachineViewer, IPlayerViewer
    {
        [Space(10)]

        [Header("Target")]

        // NOTE: this should be added to a container GameObject
        [SerializeField]
        [CanBeNull]
        private CinemachineTargetGroup _targetGroup;

        public Viewer Viewer => this;

        private CinemachineFramingTransposer _transposer;

        [CanBeNull]
        private CinemachineConfiner _confiner;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _transposer = GetCinemachineComponent<CinemachineFramingTransposer>();
            Assert.IsNotNull(_transposer);

            _confiner = GetComponent<CinemachineConfiner>();

            if(_targetGroup != null) {
                LookAt(_targetGroup.transform);
                Follow(_targetGroup.transform);
            }
        }

        #endregion

        public virtual void Initialize(GameData gameData)
        {
            Viewer.Set3D(gameData.FoV);

            _transposer.m_GroupFramingMode = CinemachineFramingTransposer.FramingMode.HorizontalAndVertical;
            _transposer.m_CameraDistance = gameData.Distance;
        }

        public void SetBounds(Collider2D bounds)
        {
            Debug.Log("Setting viewer bounds");

            _confiner.m_ConfineScreenEdges = true;
            _confiner.m_BoundingShape2D = bounds;
        }

        public void AddTarget(Actor actor, float weight = 1.0f)
        {
            Debug.Log($"Adding viewer target {actor.Id}");

            _targetGroup.AddMember(actor.transform, weight, actor.Radius);
        }

        public void RemoveTarget(Actor actor)
        {
            Debug.Log($"Removing viewer target {actor.Id}");

            _targetGroup.RemoveMember(actor.transform);
        }
    }
}
