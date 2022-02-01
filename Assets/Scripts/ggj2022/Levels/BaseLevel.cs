namespace pdxpartyparrot.ggj2022.Level
{
    public interface ILevel
    {
        enum State
        {
            OnFire,
            Ash,
            Alive,
        }

        State WorldState { get; }
    }
}
