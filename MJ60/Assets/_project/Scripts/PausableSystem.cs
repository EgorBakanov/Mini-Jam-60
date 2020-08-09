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

    private Timer _pauseTimer;
    protected SystemState State { get; private set; }
    public bool IsPaused => State == SystemState.Pause;

    public event Action<float> OnValueChanged;
    public event Action OnPause;
    public event Action OnUnpause;

    public void Pause()
    {
        if (State == SystemState.ReadyToPause)
        {
            State = SystemState.Pause;
            _pauseTimer = pauseTime;
            OnPause?.Invoke();
        }
    }

    protected virtual void Awake()
    {
        _pauseTimer = 0;
        State = SystemState.ReadyToPause;
    }

    protected virtual void Update()
    {
        _pauseTimer.Tick(Time.deltaTime);
        switch (State)
        {
            case SystemState.Pause:
                UpdateOnPause();
                break;
            case SystemState.Cooldown:
                UpdateOnCooldown();
                break;
        }
    }

    private void UpdateOnCooldown()
    {
        OnValueChanged?.Invoke(Mathf.Clamp01(1 - _pauseTimer / cooldownTime));

        if (_pauseTimer.IsDone)
            State = SystemState.ReadyToPause;
    }

    private void UpdateOnPause()
    {
        OnValueChanged?.Invoke(Mathf.Clamp01(_pauseTimer / pauseTime));

        if (_pauseTimer.IsDone)
        {
            State = SystemState.Cooldown;
            _pauseTimer = cooldownTime;
            OnUnpause?.Invoke();
        }
    }
}