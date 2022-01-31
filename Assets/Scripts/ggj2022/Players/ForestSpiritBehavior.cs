using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
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

        public SpiritForm CurrentForm => _currentForm;

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

        public bool IsImmune => PlayerManager.Instance.PlayersImmune || false;

        public bool IsStomp => !IsDead && IsLarge && PlayerBehavior.Owner.Movement.Velocity.y < 0;

        [SerializeField]
        [ReadOnly]
        private int _seedCount;

        public int SeedCount => _seedCount;

        public bool HasSeeds => SeedCount > 0;

        private Interactables _interactables;

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _interactables = GetComponent<Interactables>();
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

        private void Reset()
        {
            _health = _data.StartingHealth;
            _seedCount = 0;

            SetForm(SpiritForm.Small);

            GameManager.Instance.Reset(_data.MaxHealth, Health);
        }

        void SetForm(SpiritForm form)
        {
            _currentForm = form;
            _forestSpiritModel.SetForm(_currentForm);
            GamePlayerBehavior.GamePlayer.SetForm(_currentForm);
        }

        public void Kill()
        {
            Damage(_health);
        }

        public void Damage(int amount)
        {
            if(IsDead || IsImmune) {
                return;
            }

            _health -= amount;

            if(IsDead) {
                _health = 0;

                GamePlayerBehavior.GamePlayer.Movement.Stop();

                GamePlayerBehavior.Animator.SetTrigger(_data.DeathParam);
                GamePlayerBehavior.GamePlayer.TriggerScriptEvent("Death");

                GameManager.Instance.PlayerDied();
            } else {
                GameManager.Instance.PlayerDamaged(_health);

                GamePlayerBehavior.Animator.SetTrigger(_data.DamagedParam);
                GamePlayerBehavior.GamePlayer.TriggerScriptEvent("Damaged");
            }
        }

        public void Stomped(SlimeBehavior enemy)
        {
            GamePlayerBehavior.GamePlayer.TriggerScriptEvent("Stomp");

            if(enemy.HasSeed) {
                _seedCount++;

                GameManager.Instance.SeedCollected();

                GamePlayerBehavior.GamePlayer.TriggerScriptEvent("SeedCollected");
            }
        }

        private void AttemptExit(Exit exit)
        {
            exit.ExitLevel();
        }

        private void AttemptPlantSeed(Planter planter)
        {
            if(!HasSeeds) {
                Debug.LogWarning("No seeds available to plant!");
                return;
            }

            planter.PlantSeed();
            _interactables.RemoveInteractable(planter);

            GamePlayerBehavior.GamePlayer.TriggerScriptEvent("Plant");
        }

        #region Actions

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(action is FormSwapAction) {
                switch(_currentForm) {
                case SpiritForm.Small:
                    SetForm(SpiritForm.Large);
                    break;
                case SpiritForm.Large:
                    SetForm(SpiritForm.Small);
                    break;
                }
                GamePlayerBehavior.GamePlayer.TriggerScriptEvent("FormSwap");

                return true;
            }

            if(action is InteractAction) {
                // first try and exit
                Exit exit = _interactables.GetFirstInteractable<Exit>();
                if(null != exit) {
                    AttemptExit(exit);
                    return true;
                }

                // next try to plant a seed
                Planter planter = _interactables.GetFirstInteractable<Planter>();
                if(null != planter) {
                    AttemptPlantSeed(planter);
                    return true;
                }

                return false;
            }

            return false;
        }

        #endregion

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            Reset();

            return base.OnSpawn(spawnpoint);
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            Reset();

            return base.OnSpawn(spawnpoint);
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
