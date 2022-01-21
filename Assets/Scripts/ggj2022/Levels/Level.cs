using System;

using pdxpartyparrot.Game.Level;

namespace pdxpartyparrot.ggj2022.Level
{
    public sealed class Level : LevelHelper
    {
        protected override void Reset()
        {
            base.Reset();

            UnityEngine.Debug.LogWarning("TODO: reset the level!");
        }
    }
}
