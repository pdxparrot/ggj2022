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

        public TransitionUpdateEventArgs(int enemiesStomped, int totalEnemies, int seedsPlanted, int totalPlanters)
            : this(string.Empty, enemiesStomped, totalEnemies, seedsPlanted, totalPlanters)
        {
        }

        public TransitionUpdateEventArgs(string areaId, int enemiesStomped, int totalEnemies, int seedsPlanted, int totalPlanters)
        {
            _areaId = areaId;

            _enemiesStompedTransitionPercent = totalEnemies == 0 ? 1.0f : enemiesStomped / (float)totalEnemies;
            _seedsPlantedTransitionPercent = totalPlanters == 0 ? 1.0f : seedsPlanted / (float)totalPlanters;
        }
    }
}
