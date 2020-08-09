using System;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public enum SystemType
    {
        Alarm,
        AlarmCamera,
        Door,
        Navigation,
    }
    
    public SystemType system;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform != GameManager.Instance.player)
            return;

        var pausableSystem = PickSystem();

        if (!GameManager.Instance.pausableSystemManager.availableSystems.Add(pausableSystem))
            return;
        
        GameManager.Instance.uiManager.Add(pausableSystem);
    }

    private PausableSystem PickSystem()
    {
        switch (system)
        {
            case SystemType.Alarm: return GameManager.Instance.pausableSystemManager.alarmSystem;
            case SystemType.AlarmCamera: return GameManager.Instance.pausableSystemManager.alarmCameraSystem;
            case SystemType.Door: return GameManager.Instance.pausableSystemManager.doorSystem;
            case SystemType.Navigation: return GameManager.Instance.pausableSystemManager.navigationSystem;
            default:
                return null;
        }
    }
}
