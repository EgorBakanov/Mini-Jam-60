using System;
using UnityEngine;

public abstract class PausableSystem : MonoBehaviour
{
    protected enum SystemState
    {
        ReadyToPause,
        Pause,
        Cooldown,
    }

    public new string name;
    public float pauseTime;
    public float cooldownTime;

    private float _timerValue;
    protected SystemState State { get; private set; }

    public event Action<float> OnValueChanged;
    public event Action OnPause;
    public event Action OnUnpause;

    public void Pause()
    {
        if (State == SystemState.ReadyToPause)
        {
            State = SystemState.Pause;
            _timerValue = pauseTime;
            OnPause?.Invoke();
        }
    }

    private void Awake()
    {
        _timerValue = 1f;
        State = SystemState.ReadyToPause;
    }

    private void Update()
    {
        switch (State)
        {
            case SystemState.Pause:
                UpdateOnPause(Time.deltaTime);
                break;
            case SystemState.Cooldown:
                UpdateOnCooldown(Time.deltaTime);
                break;
        }
    }

    private void UpdateOnCooldown(float deltaTime)
    {
        _timerValue -= deltaTime;
        OnValueChanged?.Invoke(Mathf.Clamp01(1 - _timerValue / cooldownTime));

        if (_timerValue <= 0)
            State = SystemState.ReadyToPause;
    }

    private void UpdateOnPause(float deltaTime)
    {
        _timerValue -= deltaTime;
        OnValueChanged?.Invoke(Mathf.Clamp01(_timerValue / pauseTime));

        if (_timerValue <= 0)
        {
            State = SystemState.Cooldown;
            _timerValue = cooldownTime;
            OnUnpause?.Invoke();
        }
    }
}