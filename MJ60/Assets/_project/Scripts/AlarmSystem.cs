using System;
using UnityEngine;

public class AlarmSystem : PausableSystem
{
    private enum AlarmState
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
        if(State == SystemState.Pause)
            return;

        if (_state == AlarmState.Alarm)
        {
            _alarmTimer = alarmTime;
            return;
        }
        
        _state = AlarmState.Raise;
    }

    public void ForceAlarm()
    {
        if(State == SystemState.Pause)
            return;
        
        _state = AlarmState.Alarm;
        _alarmTimer = alarmTime;
        _value = 1f;
        OnAlarmValueChanged?.Invoke(_value);
    }

    public event Action<float> OnAlarmValueChanged;

    private AlarmState _state;
    private float _value;
    private Timer _alarmTimer;
    private float _dt;

    protected override void Awake()
    {
        base.Awake();
        _state = AlarmState.None;
        _value = 0;
        _alarmTimer = 0;
    }

    protected override void Update()
    {
        base.Update();
        _dt = Time.deltaTime;
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
        if (!_alarmTimer.Tick(_dt))
            return;
        
        _state = AlarmState.Restore;
    }

    private void UpdateOnRestore()
    {
        _value -= _dt / timeToFullRestore;
        if (_value <= 0f)
        {
            _state = AlarmState.None;
            _value = 0f;
        }

        OnAlarmValueChanged?.Invoke(_value);
    }

    private void UpdateOnRaise()
    {
        _value += _dt / timeToAlarm;
        if (_value >= 1f)
        {
            _state = AlarmState.Alarm;
            _alarmTimer = alarmTime;
            _value = 1f;
        }
        else _state = AlarmState.Restore;

        OnAlarmValueChanged?.Invoke(_value);
    }
}