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

    private Timer _timer;
    protected SystemState State { get; private set; }

    public event Action<float> OnValueChanged;
    public event Action OnPause;
    public event Action OnUnpause;

    public void Pause()
    {
        if (State == SystemState.ReadyToPause)
        {
            State = SystemState.Pause;
            _timer = pauseTime;
            OnPause?.Invoke();
        }
    }

    protected virtual void Awake()
    {
        _timer = 0;
        State = SystemState.ReadyToPause;
    }

    protected virtual void Update()
    {
        _timer.Tick(Time.deltaTime);
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
        OnValueChanged?.Invoke(Mathf.Clamp01(1 - _timer / cooldownTime));

        if (_timer.IsDone)
            State = SystemState.ReadyToPause;
    }

    private void UpdateOnPause()
    {
        OnValueChanged?.Invoke(Mathf.Clamp01(_timer / pauseTime));

        if (_timer.IsDone)
        {
            State = SystemState.Cooldown;
            _timer = cooldownTime;
            OnUnpause?.Invoke();
        }
    }
}