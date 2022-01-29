using System;

namespace pdxpartyparrot.ggj2022
{
    public sealed class TransitionUpdateEventArgs : EventArgs
    {
        private string _areaId;

        public string AreaId => _areaId;

        private float _enemiesStompedTransitionPercent;

        public float EnemiesStompedTransitionPercent => _enemiesStompedTransitionPercent;

        private float _seedsPlantedTransitionPercent;

        public float SeedsPlantedTransitionPercent => _seedsPlantedTransitionPercent;

        public TransitionUpdateEventArgs(string areaId, int enemiesStomped, int totalEnemies, int seedsPlanted, int totalPlanters)
        {
            _areaId = areaId;

            _enemiesStompedTransitionPercent = totalEnemies == 0 ? 0 : enemiesStomped / (float)totalEnemies;
            _seedsPlantedTransitionPercent = totalPlanters == 0 ? 0 : seedsPlanted / (float)totalPlanters;
        }
    }
}
