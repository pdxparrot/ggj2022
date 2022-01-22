using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.World
{
    // TODO: this should be split into SpawnPoint for spawning a single thing at a time
    // and MultiSpawnPoint for spawning multiple things at a time
    // (and probably a BaseSpawnPoint for shared functionality)
    // right now this is overcomplicated and confusing trying to handle both at once
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private string[] _tags;

        public string[] Tags => _tags;

        [Space(10)]

        #region Range

        [Header("Range")]

        [SerializeField]
        private FloatRangeConfig _xSpawnRange;

        [SerializeField]
        private FloatRangeConfig _ySpawnRange;

        [SerializeField]
        private FloatRangeConfig _zSpawnRange;

        #endregion

        [Space(10)]

        [SerializeField]
        private IntRangeConfig _spawnAmount = new IntRangeConfig(1, 1);

        protected IntRangeConfig SpawnAmount => _spawnAmount;

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private Actor _owner;

        private Action _onRelease;

        #region Unity Lifecycle

        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            Release();
            Unregister();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(_xSpawnRange.Max == 0.0f ? 1.0f : Mathf.Abs(_xSpawnRange.Max) * 2.0f,
                                                                _ySpawnRange.Max == 0.0f ? 1.0f : Mathf.Abs(_ySpawnRange.Max) * 2.0f,
                                                                _zSpawnRange.Max == 0.0f ? 1.0f : Mathf.Abs(_zSpawnRange.Max) * 2.0f));
        }

        #endregion

        private void Register()
        {
            if(enabled && SpawnManager.HasInstance) {
                SpawnManager.Instance.RegisterSpawnPoint(this);
            }
        }

        private void Unregister()
        {
            if(enabled && SpawnManager.HasInstance) {
                SpawnManager.Instance.UnregisterSpawnPoint(this);
            }
        }

        protected virtual void InitActor(Actor actor)
        {
            Transform actorTransform = actor.transform;
            Transform thisTransform = transform;

            Vector3 offset = new Vector3(_xSpawnRange.GetRandomValue() * PartyParrotManager.Instance.Random.NextSign(),
                                         _ySpawnRange.GetRandomValue() * PartyParrotManager.Instance.Random.NextSign(),
                                         _zSpawnRange.GetRandomValue() * PartyParrotManager.Instance.Random.NextSign());

            actorTransform.position = thisTransform.position + offset;
            actorTransform.rotation = thisTransform.rotation;

            actor.gameObject.SetActive(true);
        }

        private void InitActor(Actor actor, Guid id, ActorBehaviorComponentData behaviorData)
        {
            actor.Initialize(id);

            InitActor(actor);

            if(null != actor.Behavior && null != behaviorData) {
                actor.Behavior.Initialize(behaviorData);
            }
        }

        protected virtual void InitActors(ICollection<Actor> actors)
        {
        }

        [CanBeNull]
        private Actor DoSpawnFromPrefab(Actor prefab, Guid id, ActorBehaviorComponentData behaviorData, Transform parent, bool activate)
        {
            Actor actor = Instantiate(prefab, parent);
            actor.gameObject.SetActive(activate);

            if(!Spawn(actor, id, behaviorData)) {
                Destroy(actor);
                return null;
            }

            return actor;
        }

        public List<Actor> SpawnFromPrefab(Actor prefab, ActorBehaviorComponentData behaviorData, Transform parent = null, bool activate = true)
        {
            return SpawnFromPrefab(prefab, () => Guid.NewGuid(), behaviorData, parent, activate);
        }

        public List<Actor> SpawnFromPrefab(Actor prefab, Func<Guid> idGenerator, ActorBehaviorComponentData behaviorData, Transform parent = null, bool activate = true)
        {
#if USE_NETWORKING
            Debug.LogWarning("You probably meant to use NetworkManager.SpawnNetworkPrefab");
#endif

            int amount = _spawnAmount.GetRandomValue(1);

            List<Actor> actors = new List<Actor>();
            for(int i = 0; i < amount; ++i) {
                Guid id = idGenerator();

                Actor actor = DoSpawnFromPrefab(prefab, id, behaviorData, parent, activate);
                if(null == actor) {
                    Debug.LogError("Failed to spawn from prefab!");
                    return actors;
                }

                actors.Add(actor);
            }

            InitActors(actors);

            return actors;
        }

        [CanBeNull]
        public Actor SpawnSingleFromPrefab(Actor prefab, ActorBehaviorComponentData behaviorData, Transform parent = null, bool activate = true)
        {
            return SpawnSingleFromPrefab(prefab, Guid.NewGuid(), behaviorData, parent, activate);
        }

        [CanBeNull]
        public Actor SpawnSingleFromPrefab(Actor prefab, Guid id, ActorBehaviorComponentData behaviorData, Transform parent = null, bool activate = true)
        {
#if USE_NETWORKING
            Debug.LogWarning("You probably meant to use NetworkManager.SpawnNetworkPrefab");
#endif

            return DoSpawnFromPrefab(prefab, id, behaviorData, parent, activate);
        }

        public List<Actor> SpawnNPCPrefab(Actor prefab, ActorBehaviorComponentData behaviorData, Transform parent = null, bool active = true)
        {
            return SpawnFromPrefab(prefab, behaviorData, parent, active);
        }

        [CanBeNull]
        public Actor SpawnSingleNPCPrefab(Actor prefab, ActorBehaviorComponentData behaviorData, Transform parent = null, bool active = true)
        {
            return SpawnSingleFromPrefab(prefab, behaviorData, parent, active);
        }

        public bool Spawn(Actor actor, Guid id, ActorBehaviorComponentData behaviorData)
        {
            InitActor(actor, id, behaviorData);

            return actor.OnSpawn(this);
        }

        public bool SpawnPlayer(Actor actor)
        {
            InitActor(actor);

            // NOTE: players spawn and then deactivate
            // so that the level can respawn them as it needs to

            bool ret = actor.OnSpawn(this);
            actor.gameObject.SetActive(false);
            return ret;
        }

        public bool ReSpawn(Actor actor)
        {
            InitActor(actor);

            return actor.OnReSpawn(this);
        }

        public bool Acquire(Actor owner, Action onRelease = null, bool force = false)
        {
            if(!force && null != _owner) {
                return false;
            }

            Release();

            _owner = owner;
            _onRelease = onRelease;

            Unregister();

            return true;
        }

        public void Release()
        {
            if(null == _owner) {
                return;
            }

            _onRelease?.Invoke();
            _owner = null;

            Register();
        }
    }
}
