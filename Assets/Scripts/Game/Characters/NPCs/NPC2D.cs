using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.NPCs
{
    public abstract class NPC2D : Actor2D, INPC
    {
        public GameObject GameObject => gameObject;

        #region Network

        public override bool IsLocalActor => false;

        #endregion

        #region Behavior

        public NPCBehavior NPCBehavior => (NPCBehavior)Behavior;

        #endregion

        #region Pathing

        public bool HasPath => false;

        public Vector3 NextPosition => Vector3.zero;

        public Vector3 MoveDirection
        {
            get
            {
                if(!HasPath) {
                    return Vector3.zero;
                }

                Vector3 nextPosition = NextPosition;
                return (nextPosition - Movement.Position).normalized;
            }
        }

        #endregion

        [CanBeNull]
        private PooledObject _pooledObject;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Collider.isTrigger = true;

            _pooledObject = GetComponent<PooledObject>();
            if(null != _pooledObject) {
                _pooledObject.RecycleEvent += RecycleEventHandler;
            }
        }

        #endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            Assert.IsTrue(Behavior is NPCBehavior);
        }

        public virtual void OnBehaviorInitialized()
        {
        }

        public override void SetFacing(Vector3 direction)
        {
            direction = new Vector3(direction.x, 0.0f, 0.0f);

            if(Mathf.Approximately(direction.sqrMagnitude, 0.0f)) {
                return;
            }

            base.SetFacing(direction);
        }

        #region Pathing

        public bool UpdatePath(Vector3 target, float range = 10.0f)
        {
            Debug.LogWarning("TODO: NPC2D.UpdatePath()");

            return false;
        }

        public void ResetPath(bool idle)
        {
            Debug.LogWarning("TODO: NPC2D.ResetPath()");

            if(idle) {
                NPCBehavior.OnIdle();
            }
        }

        #endregion

        public void Stop(bool resetPath, bool idle)
        {
            Debug.LogWarning("TODO: NPC2D.Stop()");

            if(resetPath) {
                ResetPath(idle);
            } else if(idle) {
                NPCBehavior.OnIdle();
            }
        }

        public void Recycle()
        {
            NPCBehavior.OnRecycle();
            if(null != _pooledObject) {
                _pooledObject.Recycle();
            }
        }

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            GameStateManager.Instance.NPCManager.RegisterNPC(this);

            return true;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnReSpawn(spawnpoint)) {
                return false;
            }

            GameStateManager.Instance.NPCManager.RegisterNPC(this);

            return true;
        }

        public override void OnDeSpawn()
        {
            GameStateManager.Instance.NPCManager.UnregisterNPC(this);

            base.OnDeSpawn();
        }

        #endregion

        #region Event Handlers

        private void RecycleEventHandler(object sender, EventArgs args)
        {
            OnDeSpawn();
        }

        #endregion
    }
}
