using pdxpartyparrot.Core.Effects.EffectTriggerComponents;

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
