using System;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters;
using pdxpartyparrot.Game.Characters.Players.BehaviorComponents;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.ggj2022.Data.Players;
using pdxpartyparrot.ggj2022.NPCs;
using pdxpartyparrot.ggj2022.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Players
{
    [RequireComponent(typeof(Interactables3D))]
    public sealed class ForestSpiritBehavior : PlayerBehaviorComponent
    {
        #region Actions

        public class FormSwapAction : CharacterBehaviorAction
        {
            public static FormSwapAction Default = new FormSwapAction();
        }

        public class InteractAction : CharacterBehaviorAction
        {
            public static InteractAction Default = new InteractAction();
        }

        #endregion

        public enum SpiritForm
        {
            Small,
            Large,
        }

        private PlayerBehavior GamePlayerBehavior => (PlayerBehavior)PlayerBehavior;

        [SerializeField]
        private ForestSpiritBehaviorData _data;

        [SerializeField]
        [ReadOnly]
        private SpiritForm _currentForm;

        public bool IsLarge => SpiritForm.Large == _currentForm;

        public float MoveSpeedModifier => IsLarge ? _data.LargeSpiritMoveSpeedModifier : 1.0f;

        public float JumpHeightModifier => IsLarge ? _data.LargeSpiritJumpHeightModifier : 1.0f;

        [SerializeField]
        ForestSpiritModel _forestSpiritModel;

        [SerializeField]
        [ReadOnly]
        private int _health;

        public int Health => _health;

        public bool IsDead => _health <= 0;

        public bool IsStomp => !IsDead && IsLarge && PlayerBehavior.Owner.Movement.Velocity.y < 0;

        [SerializeField]
        [ReadOnly]
        private int _seedCount;

        public int SeedCount => _seedCount;

        public bool HasSeeds => SeedCount > 0;

        public bool CanExit => GameManager.Instance.ExitAvailable;

        #region Effects

        [SerializeField]
        private EffectTrigger _formSwapLargeEffect;

        [SerializeField]
        private EffectTrigger _formSwapSmallEffect;

        [SerializeField]
        private EffectTrigger _stompedEffect;

        [SerializeField]
        private EffectTrigger _seedCollectedEffect;

        [SerializeField]
        private EffectTrigger _damagedEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        [SerializeField]
        private EffectTrigger _plantEffect;

        #endregion

        [SerializeField]
        private RumbleEffectTriggerComponent[] _rumbleEffects;

        private Interactables _interactables;

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _interactables = GetComponent<Interactables>();

            GameManager.Instance.GameReadyEvent += GameReadyEventHandler;
        }

        private void OnEnable()
        {
            InitDebugMenu();
        }

        private void OnDisable()
        {
            DestroyDebugMenu();
        }

        #endregion

        public override void Initialize(CharacterBehavior behavior)
        {
            base.Initialize(behavior);

            foreach(RumbleEffectTriggerComponent rumble in _rumbleEffects) {
                rumble.PlayerInput = GamePlayerBehavior.Player.PlayerInputHandler.InputHelper;
            }
        }

        void SetForm(SpiritForm form)
        {
            _currentForm = form;
            _forestSpiritModel.SetForm(_currentForm);
            GamePlayerBehavior.GamePlayer.SetForm(_currentForm);
        }

        public void Damage(int amount)
        {
            _health -= amount;

            if(_health <= 0) {
                _health = 0;

                _deathEffect.Trigger(() => {
                    GameManager.Instance.PlayerDied();
                });
            } else {
                GameManager.Instance.PlayerDamaged(_health);

                _damagedEffect.Trigger();
            }
        }

        public void Stomped(SlimeBehavior enemy)
        {
            _stompedEffect.Trigger();

            if(enemy.HasSeed) {
                _seedCollectedEffect.Trigger(() => {
                    _seedCount++;

                    GameManager.Instance.SeedCollected();
                });
            }
        }

        #region Actions

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(action is FormSwapAction) {
                switch(_currentForm) {
                case SpiritForm.Small:
                    SetForm(SpiritForm.Large);
                    _formSwapLargeEffect.Trigger();
                    break;
                case SpiritForm.Large:
                    SetForm(SpiritForm.Small);
                    _formSwapSmallEffect.Trigger();
                    break;
                }

                return true;
            } else if(action is InteractAction) {
                // first try and exit
                if(_interactables.HasInteractables<Exit>()) {
                    if(CanExit) {
                        GameManager.Instance.Exit();
                        return true;
                    } else {
                        Debug.Log("Exit unavailable!");
                        return true;
                    }
                }

                // next try to plant a seed
                Planter planter = _interactables.GetFirstInteractable<Planter>();
                if(null != planter && planter.CanPlantSeed) {
                    if(HasSeeds) {
                        planter.PlantSeed();

                        _interactables.RemoveInteractable(planter);

                        _plantEffect.Trigger();
                        return true;
                    } else {
                        Debug.Log("No seeds available to plant!");
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            _health = _data.StartingHealth;
            _seedCount = 0;

            SetForm(SpiritForm.Small);

            return base.OnSpawn(spawnpoint);
        }

        #endregion

        #region Event Handlers

        private void GameReadyEventHandler(object sender, EventArgs args)
        {
            GameManager.Instance.Reset(_data.MaxHealth, Health);
        }

        #endregion

        #region Debug Menu

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"ggj2022.ForestSpiritBehavior {Behavior.Owner.Id}");
            _debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label($"Health: {Health}");
                if(GUILayout.Button("Damage")) {
                    Damage(1);
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
