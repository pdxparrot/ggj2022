using System;

using pdxpartyparrot.Game.Cinematics;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Data
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "pdxpartyparrot/ggj2022/Data/Dialogue Data")]
    [Serializable]
    public sealed class DialogueData : Game.Data.DialogueData
    {
        [Space(10)]

        #region Dialogues

        [SerializeField]
        private Dialogue _seedsRemainDialoguePrefab;

        public Dialogue SeedsRemainDialoguePrefab => _seedsRemainDialoguePrefab;

        [SerializeField]
        private Dialogue _enemiesRemainDialoguePrefab;

        public Dialogue EnemiesRemainDialoguePrefab => _enemiesRemainDialoguePrefab;

        [SerializeField]
        private Dialogue _planterFullDialoguePrefab;

        public Dialogue PlanterFullDialoguePrefab => _planterFullDialoguePrefab;

        #endregion
    }
}
