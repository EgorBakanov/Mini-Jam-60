using System;
using UnityEngine;

public abstract class PausableSystem : MonoBehaviour
{
    private enum State
    {
        ReadyToPause,
        Pause,
        Cooldown,
    }

    public new string name;
    public float pauseTime;
    public float cooldownTime;

    private float _timerValue;
    private State _state;

    public event Action<float> OnValueChanged;
    public event Action OnPause;
    public event Action OnUnpause;

    public void Pause()
    {
        if (_state == State.ReadyToPause)
        {
            _state = State.Pause;
            _timerValue = pauseTime;
            OnPause?.Invoke();
        }
    }

    private void Awake()
    {
        _timerValue = 1f;
        _state = State.ReadyToPause;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Pause:
                UpdateOnPause(Time.deltaTime);
                break;
            case State.Cooldown:
                UpdateOnCooldown(Time.deltaTime);
                break;
        }
    }

    private void UpdateOnCooldown(float deltaTime)
    {
        _timerValue -= deltaTime;
        OnValueChanged?.Invoke(Mathf.Clamp01(1 - _timerValue / cooldownTime));

        if (_timerValue <= 0)
            _state = State.ReadyToPause;
    }

    private void UpdateOnPause(float deltaTime)
    {
        _timerValue -= deltaTime;
        OnValueChanged?.Invoke(Mathf.Clamp01(_timerValue / pauseTime));

        if (_timerValue <= 0)
        {
            _state = State.Cooldown;
            _timerValue = cooldownTime;
            OnUnpause?.Invoke();
        }
    }
}