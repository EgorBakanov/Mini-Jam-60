using UnityEngine;

public class AlarmCamera : MonoBehaviour
{
    public Light viewLight;
    public Color normalState;
    public Color triggeredState;
    public Color alarmState;
    
    private bool _isTriggered;

    private void Update()
    {
        if (GameManager.Instance.pausableSystemManager.alarmCameraSystem.IsPaused)
        {
            viewLight.enabled = false;
            return;
        }

        viewLight.enabled = true;
        if (_isTriggered)
        {
            GameManager.Instance.pausableSystemManager.alarmSystem.ForceAlarm();
            viewLight.color = triggeredState;
        }
        else if (GameManager.Instance.pausableSystemManager.alarmSystem.IsAlarmed)
            viewLight.color = alarmState;
        else viewLight.color = normalState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.player)
        {
            _isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.player)
        {
            _isTriggered = false;
        }
    }
}
