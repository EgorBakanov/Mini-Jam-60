using System.Collections.Generic;
using UnityEngine;

public class PausableSystemManager : MonoBehaviour
{
    public TestSystem testSystem;
    public DoorSystem doorSystem;
    public AlarmSystem alarmSystem;
    public NavigationSystem navigationSystem;
    public AlarmCameraSystem alarmCameraSystem;

    public HashSet<PausableSystem> availableSystems = new HashSet<PausableSystem>();
}