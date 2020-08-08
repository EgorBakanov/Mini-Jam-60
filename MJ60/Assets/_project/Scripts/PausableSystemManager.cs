using System.Collections.Generic;
using UnityEngine;

public class PausableSystemManager : MonoBehaviour
{
    public TestSystem testSystem;
    public DoorSystem doorSystem;

    public HashSet<PausableSystem> availableSystems = new HashSet<PausableSystem>();
}