using UnityEngine;

namespace pdxpartyparrot.Game.World
{
    public abstract class WorldBoundary : MonoBehaviour
    {
        protected void HandleCollisionEnter(GameObject go)
        {
            IWorldBoundaryCollisionListener listener = go.GetComponent<IWorldBoundaryCollisionListener>();
            if(null == listener) {
                return;
            }

            listener.OnWorldBoundaryCollisionEnter(this);
        }

        protected void HandleCollisionExit(GameObject go)
        {
            IWorldBoundaryCollisionListener listener = go.GetComponent<IWorldBoundaryCollisionListener>();
            if(null == listener) {
                return;
            }

            listener.OnWorldBoundaryCollisionExit(this);
        }
    }
}
