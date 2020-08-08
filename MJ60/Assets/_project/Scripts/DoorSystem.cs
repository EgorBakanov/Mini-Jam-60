public class DoorSystem : PausableSystem
{
    public bool CanOpen => State == SystemState.Pause;
}