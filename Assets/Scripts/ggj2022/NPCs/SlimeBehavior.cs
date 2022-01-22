using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.ggj2022.Data.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2022.NPCs
{
    public sealed class SlimeBehavior : NPCBehavior
    {
        private Slime Slime => (Slime)Owner;

        private SlimeBehaviorData SlimeBehaviorData => (SlimeBehaviorData)BehaviorData;

        public override Vector3 MoveDirection
        {
            get
            {
                return Vector3.zero;
            }
        }

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
        }

        #region Debug Menu

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"ggj2022.SlimeBehavior {Owner.Id}");
            _debugMenuNode.RenderContentsAction = () => {
                if(GUILayout.Button("Kill")) {
                    Debug.LogWarning("TODO: Kill slime!");
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
