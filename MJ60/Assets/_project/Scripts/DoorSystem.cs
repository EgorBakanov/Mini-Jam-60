public class DoorSystem : PausableSystem
{
    public bool CanOpen => IsPaused;
}