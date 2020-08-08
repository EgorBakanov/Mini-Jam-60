using System.Collections.Generic;
using UnityEngine;

public class PausableSystemManager : MonoBehaviour
{
    public TestSystem testSystem;

    public HashSet<PausableSystem> availableSystems = new HashSet<PausableSystem>();
}