using System;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters;
using pdxpartyparrot.Game.Characters.Players.BehaviorComponents;
using pdxpartyparrot.ggj2022.Data.Players;
using pdxpartyparrot.ggj2022.NPCs;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Players
{
    public sealed class ForestSpiritBehavior : PlayerBehaviorComponent
    {
        #region Actions

        public class FormSwapAction : CharacterBehaviorAction
        {
            public static FormSwapAction Default = new FormSwapAction();
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

        #endregion

        [SerializeField]
        private RumbleEffectTriggerComponent[] _rumbleEffects;

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

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

                    GameManager.Instance.SeedCollected(_seedCount);
                });
            }
        }

        #region Actions

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is FormSwapAction)) {
                return false;
            }

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
            GameManager.Instance.Reset(Health, SeedCount);
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
