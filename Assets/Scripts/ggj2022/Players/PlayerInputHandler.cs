using pdxpartyparrot.Game.Characters.BehaviorComponents;
using pdxpartyparrot.Game.Players.Input;
using pdxpartyparrot.ggj2022.Data.Players;

using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace pdxpartyparrot.ggj2022.Players
{
    public sealed class PlayerInputHandler : SideScollerPlayerInputHandler
    {
        private Player GamePlayer => (Player)Player;

        protected override bool InputEnabled => base.InputEnabled && !GamePlayer.GamePlayerBehavior.ForestSpiritBehavior.IsDead;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerInputData is PlayerInputData);
            Assert.IsTrue(Player is Player);
        }

        #endregion

        #region Actions

        public void OnJumpAction(InputAction.CallbackContext context)
        {
            if(!IsInputAllowed(context)) {
                return;
            }

            if(context.performed) {
                GamePlayer.PlayerBehavior.ActionPerformed(JumpBehaviorComponent.JumpAction.Default);
            }
        }

        public void OnInteractAction(InputAction.CallbackContext context)
        {
            if(!IsInputAllowed(context)) {
                return;
            }

            if(context.performed) {
                GamePlayer.PlayerBehavior.ActionPerformed(ForestSpiritBehavior.InteractAction.Default);
            }
        }

        public void OnFormSwapAction(InputAction.CallbackContext context)
        {
            if(!IsInputAllowed(context)) {
                return;
            }

            if(context.performed) {
                GamePlayer.PlayerBehavior.ActionPerformed(ForestSpiritBehavior.FormSwapAction.Default);
            }
        }

        #endregion
    }
}
