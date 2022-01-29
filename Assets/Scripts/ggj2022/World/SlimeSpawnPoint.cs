using System.Collections.Generic;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2022.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2022.World
{
    public sealed class SlimeSpawnPoint : SpawnPoint
    {
        [SerializeField]
        private int _seedCount = 1;

        [SerializeField]
        private string _areaId;

        #region Unity Lifecycle

        private void Awake()
        {
            Assert.IsTrue(SpawnAmount.Min >= _seedCount);
        }

        #endregion

        protected override void InitActors(ICollection<Actor> actors)
        {
            base.InitActors(actors);

            int seedCount = 0;
            foreach(Actor actor in actors) {
                Slime slime = actor as Slime;
                if(null == slime) {
                    continue;
                }

                slime.SlimeBehavior.SetAreaId(_areaId);

                if(seedCount < _seedCount) {
                    slime.SlimeBehavior.GiveSeed();
                    seedCount++;
                }
            }
        }
    }
}
