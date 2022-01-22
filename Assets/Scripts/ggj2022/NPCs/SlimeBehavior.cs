using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.ggj2022.Data.NPCs;
using pdxpartyparrot.ggj2022.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2022.NPCs
{
    public sealed class SlimeBehavior : NPCBehavior
    {
        private Slime Slime => (Slime)Owner;

        private SlimeBehaviorData SlimeBehaviorData => (SlimeBehaviorData)BehaviorData;

        [SerializeField]
        private GameObject _seedModel;

        public override Vector3 MoveDirection
        {
            get
            {
                return Vector3.zero;
            }
        }

        [SerializeField]
        [ReadOnly]
        private bool _hasSeed;

        public bool HasSeed => _hasSeed;

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        protected override void Awake()
        {
            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            DestroyDebugMenu();
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
        }

        #region Events

        public override bool TriggerEnter(GameObject triggerObject)
        {
            Player player = triggerObject.GetComponent<Player>();
            if(null == player) {
                return false;
            }

            if(player.Movement.Velocity.y > 0.0f) {
                Stomp(player);
            } else {
                player.GamePlayerBehavior.ForestSpiritBehavior.Damage(SlimeBehaviorData.DamageAmount);
            }

            return true;
        }

        #endregion

        private void Stomp(Player player)
        {
            Debug.Log("Stomp!");
            Owner.DeSpawn(false);

            if(HasSeed) {
                player.GamePlayerBehavior.ForestSpiritBehavior.CollectSeed();
            }
        }

        #region Debug Menu

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"ggj2022.SlimeBehavior {Owner.Id}");
            _debugMenuNode.RenderContentsAction = () => {
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
