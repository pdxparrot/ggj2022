using JetBrains.Annotations;

using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data.Characters;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.Players
{
    public abstract class PlayerBehavior : CharacterBehavior
    {
        [CanBeNull]
        public PlayerBehaviorData PlayerBehaviorData => (PlayerBehaviorData)CharacterBehaviorData;

        public IPlayer Player => (IPlayer)Owner;

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private Vector3 _moveDirection;

        public Vector3 MoveDirection => _moveDirection;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lookDirection;

        public Vector3 LookDirection => _lookDirection;

        [SerializeField]
        [ReadOnly]
        private Quaternion _lookRotation;

        public Quaternion LookRotation
        {
            get => _lookRotation;
            private set => _lookRotation = value;
        }

        public virtual Transform LookTarget { get; } = null;

        public float HorizontalLookSpeed => PlayerBehaviorData == null ? 0.0f : PlayerBehaviorData.HorizontalLookSpeed;

        public float VerticalLookSpeed => PlayerBehaviorData == null ? 0.0f : PlayerBehaviorData.VerticalLookSpeed;

        #region Unity Lifecycle

        protected override void Update()
        {
            base.Update();

            float dt = Time.deltaTime;

            // set move direction from input
            // smooth acceleration, but not deceleration
            Vector3 moveDirection = Player.PlayerInputHandler.LastMove.sqrMagnitude < MoveDirection.sqrMagnitude
                        ? Player.PlayerInputHandler.LastMove
                        : Vector3.MoveTowards(MoveDirection, Player.PlayerInputHandler.LastMove, dt * Player.PlayerInputHandler.PlayerInputData.MovementLerpSpeed);
            SetMoveDirection(moveDirection);

            // set look direction from input
            Vector3 lookDirection = Vector3.MoveTowards(LookDirection, Player.PlayerInputHandler.LastLook, dt * Player.PlayerInputHandler.PlayerInputData.LookLerpSpeed);
            SetLookDirection(lookDirection);
        }

        protected virtual void LateUpdate()
        {
            float dt = Time.deltaTime;

            if(PlayerBehaviorData.AllowLookHorizontal) {
                float velocity = LookDirection.x * HorizontalLookSpeed;
                LookRotation *= Quaternion.AngleAxis(velocity * dt, Vector3.up);
            }

            if(PlayerBehaviorData.AllowLookVertical) {
                float velocity = LookDirection.y * VerticalLookSpeed;

                // TODO: if we can ever control this from the InputAction processors
                // we can get rid of this math here
                velocity *= Player.PlayerInputHandler.InvertLookVertical ? 1.0f : -1.0f;

                LookRotation *= Quaternion.AngleAxis(velocity * dt, Vector3.right);
            }

            // clamp the look vertical rotation
            Vector3 rotation = LookRotation.eulerAngles;
            if(rotation.x > 180.0f) {
                rotation.x = Mathf.Clamp(rotation.x, 360.0f - PlayerBehaviorData.MaxVerticalRotation, 360.0f);
            } else {
                rotation.x = Mathf.Clamp(rotation.x, 0.0f, PlayerBehaviorData.MaxVerticalRotation);
            }
            rotation.z = 0.0f;

            LookRotation = Quaternion.Euler(rotation);

            if(null != LookTarget) {
                LookTarget.rotation = LookRotation;
            }

            Owner.IsMoving = !Mathf.Approximately(MoveDirection.sqrMagnitude, 0.0f);
        }

        #endregion

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is IPlayer);
            Assert.IsTrue(behaviorData is PlayerBehaviorData);

            base.Initialize(behaviorData);

            _moveDirection = Vector3.zero;

            _lookDirection = Vector3.zero;
            _lookRotation = Quaternion.identity;
        }

        public virtual void InitializeLocalPlayerBehavior()
        {
        }

        public void SetMoveDirection(Vector3 moveDirection)
        {
            _moveDirection = CanMove ? Vector3.ClampMagnitude(moveDirection, 1.0f) : Vector3.zero;
        }

        public void SetLookDirection(Vector3 lookDirection)
        {
            _lookDirection = lookDirection;
        }

        // TODO: not sure if the alignmovement with viewer code here works anymore

        protected override void AnimationUpdate(float dt)
        {
            if(!CanMove) {
                base.AnimationUpdate(dt);
                return;
            }

            Vector3 forward = MoveDirection;
            if(PlayerBehaviorData.AlignMovementWithViewer && null != Player.Viewer) {
                // align with the camera instead of the movement
                forward = (Quaternion.AngleAxis(LookRotation.eulerAngles.y, Vector3.up) * MoveDirection).normalized;
            }

            AlignToMovement(forward);

            if(null != Animator) {
                Animator.SetFloat(CharacterBehaviorData.MoveXAxisParam, CanMove ? Mathf.Abs(MoveDirection.x) : 0.0f);
                Animator.SetFloat(CharacterBehaviorData.MoveZAxisParam, CanMove ? Mathf.Abs(MoveDirection.z) : 0.0f);
            }

            base.AnimationUpdate(dt);
        }

        protected override void PhysicsUpdate(float dt)
        {
            if(!CanMove) {
                base.PhysicsUpdate(dt);
                return;
            }

            if(!CharacterBehaviorData.AllowAirControl && IsFalling) {
                return;
            }

            // TODO: this interferes with forces :(

            Vector3 velocity = MoveDirection * MoveSpeed;
            Quaternion rotation = Owner.Movement.Rotation;
            if(PlayerBehaviorData.AlignMovementWithViewer && null != Player.Viewer) {
                // rotate with the camera instead of the movement
                rotation = Quaternion.AngleAxis(LookRotation.eulerAngles.y, Vector3.up);
            }
            velocity = rotation * velocity;

            if(Owner.Movement.IsKinematic) {
                Owner.Movement.Move(velocity * dt);
            } else {
                velocity.y = Owner.Movement.Velocity.y;
                Owner.Movement.Velocity = velocity;
            }

            base.PhysicsUpdate(dt);
        }

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            LookRotation = spawnpoint.transform.rotation;

            return base.OnSpawn(spawnpoint);
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            LookRotation = spawnpoint.transform.rotation;

            return base.OnReSpawn(spawnpoint);
        }
    }
}
