using System;
using UnityEngine;

public class AlarmSystem : PausableSystem
{
    public enum AlarmState
    {
        None,
        Raise,
        Restore,
        Alarm
    }

    public float timeToAlarm;
    public float timeToFullRestore;
    public float alarmTime;

    public bool IsAlarmed => State != SystemState.Pause && _state == AlarmState.Alarm;
    
    public void Alarm()
    {
        if(State == SystemState.Pause || _state == AlarmState.Alarm)
            return;
        
        _state = AlarmState.Raise;
    }

    public void ForceAlarm()
    {
        if(State == SystemState.Pause)
            return;
        
        _state = AlarmState.Alarm;
    }

    public event Action<float> OnAlarmValueChanged;

    private AlarmState _state;
    private float _value;
    private Timer _timer;

    protected override void Awake()
    {
        base.Awake();
        _state = AlarmState.None;
        _value = 0;
        _timer = 0;
    }

    private void LateUpdate()
    {
        if(State == SystemState.Pause)
            return;
        
        switch (_state)
        {
            case AlarmState.Raise:
                UpdateOnRaise();
                break;
            case AlarmState.Restore:
                UpdateOnRestore();
                break;
            case AlarmState.Alarm:
                UpdateOnAlarm();
                break;
        }
    }

    private void UpdateOnAlarm()
    {
        if (!_timer.Tick(Time.deltaTime))
            return;

        _state = AlarmState.Restore;
    }

    private void UpdateOnRestore()
    {
        _value -= Time.deltaTime / timeToFullRestore;
        if (_value <= 0f)
        {
            _state = AlarmState.None;
            _value = 0f;
        }

        OnAlarmValueChanged?.Invoke(_value);
    }

    private void UpdateOnRaise()
    {
        _value += Time.deltaTime / timeToAlarm;
        if (_value >= 1f)
        {
            _state = AlarmState.Alarm;
            _timer = alarmTime;
            _value = 1f;
        }
        else _state = AlarmState.Restore;

        OnAlarmValueChanged?.Invoke(_value);
    }
}