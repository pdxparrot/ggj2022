using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Camera
{
    // for a 2.5D camera, use a CinemachineFramingTransposer body
    // for a true 3D camera, use Cinemachine3rdPersonFollow body

    public class FollowCamera3D : CinemachineViewer, IPlayerViewer
    {
        public Viewer Viewer => this;

        public virtual void Initialize(GameData gameData)
        {
            Viewer.Set3D(gameData.FoV);
        }

        public void FollowTarget(Transform target)
        {
            LookAt(target);
            Follow(target);
        }
    }
}
