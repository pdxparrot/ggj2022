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

        public int SeedCount => _seedCount;

        #region Unity Lifecycle

        private void Awake()
        {
            Assert.IsTrue(SpawnAmount.Min >= SeedCount);
        }

        #endregion

        protected override void InitActors(ICollection<Actor> actors)
        {
            base.InitActors(actors);

            int seedCount = 0;
            foreach(Actor actor in actors) {
                if(seedCount >= SeedCount) {
                    break;
                }

                Slime slime = actor as Slime;
                if(null != slime) {
                    slime.SlimeBehavior.GiveSeed();
                    seedCount++;
                }
            }
        }
    }
}
