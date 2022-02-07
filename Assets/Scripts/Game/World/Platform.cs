using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.Game.World
{
    // TODO: use a WaypointFollower instead of having this handle that
    // NOTE: platforms need a separate physics collider on a sub-object
    [RequireComponent(typeof(Collider))]
    public abstract class Platform : MonoBehaviour
    {
        [SerializeField]
        private Waypoint _initialWaypoint;

        [SerializeField]
        private float _speed = 5.0f;

        [SerializeField]
        [ReadOnly]
        private Waypoint _nextWaypoint;

        [SerializeReference]
        [ReadOnly]
        private ITimer _cooldown;

        [SerializeField]
        private Transform _actorContainer;

        protected Transform ActorContainer => _actorContainer;

        private Transform _transform;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            GetComponent<Collider>().isTrigger = true;

            _transform = GetComponent<Transform>();

            _cooldown = TimeManager.Instance.AddTimer();

            SetWaypoint(_initialWaypoint);
        }

        protected virtual void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_cooldown);
                _cooldown = null;
            }
        }

        protected virtual void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            if(null == _nextWaypoint || _cooldown.IsRunning) {
                return;
            }

            if(PartyParrotManager.Instance.IsPaused || null == GameStateManager.Instance.GameManager || !GameStateManager.Instance.GameManager.IsGameReady || GameStateManager.Instance.GameManager.IsGameOver) {
                return;
            }

            _transform.MoveTowards(_nextWaypoint.Transform.position, _speed * dt);

            if(Vector3.Distance(_transform.position, _nextWaypoint.Transform.position) < float.Epsilon) {
                _cooldown.Start(_nextWaypoint.Cooldown);

                _transform.position = _nextWaypoint.Transform.position;
                SetWaypoint(_nextWaypoint.NextWaypoint);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            OnEnterPlatform(other.gameObject);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            OnExitPlatform(other.gameObject);
        }

        #endregion

        private void SetWaypoint(Waypoint waypoint)
        {
            _nextWaypoint = waypoint;

            if(null == _nextWaypoint) {
                Debug.Log("No next waypoint, stopping");
                return;
            }
        }

        protected virtual void OnEnterPlatform(GameObject gameObject)
        {
            Debug.Log($"{gameObject.name} entered platform {name}");
        }

        protected virtual void OnExitPlatform(GameObject gameObject)
        {
            Debug.Log($"{gameObject.name} exited platform {name}");

        }
    }
}
