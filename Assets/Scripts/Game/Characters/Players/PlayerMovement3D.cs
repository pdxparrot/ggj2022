using pdxpartyparrot.Core.Data.Actors.Components;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters
{
    /*
    NOTE: we do not use Unity's CharacterController for all of these reasons:

    https://forum.unity.com/threads/proper-collision-detection-with-charactercontroller.292598/
    */

    public class PlayerMovement3D : CharacterMovement3D
    {
        protected override void InitRigidbody(ActorBehaviorComponentData behaviorData)
        {
            base.InitRigidbody(behaviorData);

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            RigidBody.interpolation = RigidbodyInterpolation.None;
        }
    }
}
