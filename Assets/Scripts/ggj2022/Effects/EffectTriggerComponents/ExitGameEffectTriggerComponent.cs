using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.ggj2022;

using UnityEngine;

namespace pdxpartyparrot.ggj2022.Effects.EffectTriggerComponents
{
    public class ExitGameEffectTriggerComponent : EffectTriggerComponent
    {
        public override bool WaitForComplete => true;

        public override void OnStart()
        {
            GameManager.Instance.Exit();
        }
    }
}
