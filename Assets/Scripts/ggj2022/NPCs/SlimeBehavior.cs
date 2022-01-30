using System.Linq;

using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.ggj2022.Data.NPCs;
using pdxpartyparrot.ggj2022.Players;
using pdxpartyparrot.ggj2022.World;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2022.NPCs
{
    public sealed class SlimeBehavior : NPCBehavior
    {
        private enum State
        {
            Dead,
            Idle,
            Patrol,
            //Chase,
            //Leash,
        }

        private Slime Slime => (Slime)Owner;

        private SlimeBehaviorData SlimeBehaviorData => (SlimeBehaviorData)BehaviorData;

        [SerializeField]
        private GameObject _seedModel;

        public override Vector3 MoveDirection => Slime.MoveDirection;

        public override float MoveSpeed => MoveSpeedModifier * base.MoveSpeed;

        private float MoveSpeedModifier => _state switch {
            State.Dead => 0.0f,
            //State.Chase => SlimeBehaviorData.ChaseSpeedModifier,
            //State.Leash => SlimeBehaviorData.LeashSpeedModifier,
            _ => 1.0f,
        };

        [SerializeField]
        [ReadOnly]
        private string _areaId;

        [SerializeField]
        [ReadOnly]
        private bool _hasSeed;

        public bool HasSeed => _hasSeed;

        [SerializeField]
        [ReadOnly]
        private State _state;

        public bool IsDead => State.Dead == _state;

        [SerializeField]
        [ReadOnly]
        private float _leashTarget;

        [SerializeField]
        [ReadOnly]
        private float _patrolTarget;

        [SerializeField]
        [ReadOnly]
        private float? _target;

        private ITimer _idleTimer;

        private bool IsIdling => _idleTimer.IsRunning;

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        protected override void OnEnable()
        {
            base.OnEnable();

            _idleTimer = TimeManager.Instance.AddTimer();

            InitDebugMenu();
        }

        protected override void OnDisable()
        {
            DestroyDebugMenu();

            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_idleTimer);
            }

            base.OnDisable();
        }

        #endregion

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is Slime);
            Assert.IsTrue(behaviorData is SlimeBehaviorData);

            base.Initialize(behaviorData);

            _seedModel.SetActive(false);
        }

        public void GiveSeed()
        {
            _hasSeed = true;
            _seedModel.SetActive(true);

            GameManager.Instance.SeedSpawned();
        }

        public override bool OnThink(float dt)
        {
            switch(_state) {
            case State.Idle:
                HandleIdle();
                break;
            case State.Patrol:
                HandlePatrol();
                break;
                /*case State.Chase:
                    HandleChase();
                    break;*/
                /*case State.Leash:
                    HandleLeash();
                    break;*/
            }

            return true;
        }

        #region NPC State

        private void SetState(State state)
        {
            if(NPCManager.Instance.DebugBehavior) {
                Debug.Log($"Slime {Owner.Id} set state {state}");
            }

            _state = state;
            switch(_state) {
            case State.Dead:
                NPCOwner.Stop(true, true);
                Slime.SetPassive();
                break;
            case State.Idle:
                NPCOwner.Stop(true, true);
                Slime.SetPassive();
                break;
            case State.Patrol:
                Slime.SetAgent();
                break;
                /*case State.Chase:
                    Slime.SetAgent();
                    break;*/
                /*case State.Leash:
                    Slime.SetAgent();
                    break;*/
            }
        }

        private void HandleIdle()
        {
            // check for a player nearby to chase
            /*if(ChasePlayer()) {
                return;
            }*/

            // if we're not resting, start patrolling
            if(!IsIdling) {
                SetState(State.Patrol);
                return;
            }
        }

        private void HandlePatrol()
        {
            // check for a player nearby to chase
            /*if(ChasePlayer()) {
                return;
            }*/

            // if we don't have a target, pick a new one
            if(null == _target) {
                float direction = PartyParrotManager.Instance.Random.NextSign();
                float distance = SlimeBehaviorData.PatrolRange.GetRandomValue() * direction;
                float target = Owner.Movement.Position.x + distance;

                // is the distance too far to patrol?
                if(Mathf.Abs(target - _leashTarget) > SlimeBehaviorData.PatrolRange.Max) {
                    // try the other direction
                    target = Owner.Movement.Position.x - distance;

                    // if we're still too far, cap it
                    if(Mathf.Abs(target - _leashTarget) > SlimeBehaviorData.PatrolRange.Max) {
                        if(direction < 0.0f) {
                            target = Mathf.Max(Owner.Movement.Position.x + distance, _leashTarget - SlimeBehaviorData.PatrolRange.Max);
                        } else {
                            target = Mathf.Min(Owner.Movement.Position.x + distance, _leashTarget + SlimeBehaviorData.PatrolRange.Max);
                        }
                    }
                }

                _patrolTarget = target;
                _target = _patrolTarget;
            }

            // have we reached our target?
            if(Mathf.Abs(_target.Value - Owner.Movement.Position.x) <= SlimeBehaviorData.StoppingDistance) {
                Idle();
                return;
            }

            // update our path
            if(!Slime.UpdatePath(new Vector3(_target.Value, 0.0f, 0.0f))) {
                Idle();
                return;
            }
        }

        /*private void HandleChase()
        {
            // if we lost our target, idle
            if(null == _target) {
                SetState(State.Idle);
                return;
            }
        }*/

        /*private void HandleLeash()
        {
            // if we lost our target, idle
            if(null == _target) {
                SetState(State.Idle);
                return;
            }
        }*/

        #endregion

        private void Idle()
        {
            _target = null;
            SetState(State.Idle);

            _idleTimer.Start(SlimeBehaviorData.IdleTimeRange.GetRandomValue());
        }

        /*private bool ChasePlayer()
        {
            // TODO: don't chase dead players

            // TODO: check for a nearby player and chase it
            // target = player
            // SetState(State.Chase)

            return false;
        }*/

        #region Events

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            SlimeSpawnPoint slimeSpawnPoint = (SlimeSpawnPoint)spawnpoint;
            _areaId = slimeSpawnPoint.AreaId;

            GameManager.Instance.EnemySpawned(_areaId);

            _leashTarget = transform.position.x;

            SetState(State.Idle);

            return false;
        }

        public override bool TriggerEnter(GameObject triggerObject)
        {
            if(IsDead) {
                return false;
            }

            Player player = triggerObject.GetComponent<Player>();
            if(null == player || player.GamePlayerBehavior.ForestSpiritBehavior.IsDead) {
                return false;
            }

            if(player.GamePlayerBehavior.ForestSpiritBehavior.IsStomp) {
                Stomp(player);
            } else {
                player.GamePlayerBehavior.ForestSpiritBehavior.Damage(SlimeBehaviorData.DamageAmount);
            }

            return true;
        }

        #endregion

        public void OnStuck()
        {
            Idle();
        }

        public void Stomp(Player player)
        {
            SetState(State.Dead);

            Slime.TriggerScriptEvent("Stomped");

            Owner.DeSpawn(false);

            player.GamePlayerBehavior.ForestSpiritBehavior.Stomped(this);

            GameManager.Instance.EnemyStomped(_areaId);
        }

        #region Debug Menu

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"ggj2022.SlimeBehavior {Owner.Id}");
            _debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label($"State: {_state}");
                GUILayout.Label($"Has Seed: {HasSeed}");
                if(GUILayout.Button("Stomp")) {
                    Stomp(PlayerManager.Instance.Players.ElementAt(0) as Player);
                }
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }

        #endregion
    }
}
