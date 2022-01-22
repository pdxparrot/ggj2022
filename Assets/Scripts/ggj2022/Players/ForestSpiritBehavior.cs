using System;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters;
using pdxpartyparrot.Game.Characters.Players.BehaviorComponents;
using pdxpartyparrot.ggj2022.Data.Players;

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

        private enum SpiritForm
        {
            Small,
            Large,
        }

        private PlayerBehavior GamePlayerBehavior => (PlayerBehavior)PlayerBehavior;

        [SerializeField]
        private ForestSpiritBehaviorData _data;

        [SerializeField]
        [ReadOnly]
        private SpiritForm _currentForm = SpiritForm.Small;

        public float MoveSpeedModifier => _currentForm == SpiritForm.Small ? 1.0f : _data.LargeSpiritMoveSpeedModifier;

        public float JumpHeightModifier => _currentForm == SpiritForm.Small ? 1.0f : _data.LargeSpiritJumpHeightModifier;

        [SerializeField]
        [ReadOnly]
        private int _health;

        public int Health => _health;

        [SerializeField]
        [ReadOnly]
        private int _seedCount;

        public int SeedCount => _seedCount;

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

        public void Damage(int amount)
        {
            _health -= amount;

            if(_health <= 0) {
                _health = 0;
                GameManager.Instance.PlayerDied();
            } else {
                GameManager.Instance.PlayerDamaged(_health);
            }
        }

        public void CollectSeed()
        {
            _seedCount++;

            GameManager.Instance.SeedCollected(_seedCount);
        }

        #region Actions

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is FormSwapAction)) {
                return false;
            }

            switch(_currentForm) {
            case SpiritForm.Small:
                _currentForm = SpiritForm.Large;
                break;
            case SpiritForm.Large:
                _currentForm = SpiritForm.Small;
                break;
            }

            Debug.Log($"Form swap: {_currentForm}");

            return true;
        }

        #endregion

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            _health = _data.StartingHealth;
            _seedCount = 0;

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
