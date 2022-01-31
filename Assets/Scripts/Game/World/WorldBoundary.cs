using UnityEngine;
using Unity.AI.Navigation;

namespace pdxpartyparrot.Game.World
{
    [RequireComponent(typeof(NavMeshModifier))]
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
