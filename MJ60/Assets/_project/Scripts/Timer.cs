﻿using UnityEngine;

public class Timer
{
    public float Value
    {
        get => _value;
        set => _value = Mathf.Max(0f, value);
    }

    public bool IsDone => Value <= 0f;

    private float _value;

    public Timer(float startTime) => Value = startTime;

    public Timer() : this(0f)
    {
    }

    public bool Tick(float deltaTime)
    {
        Value -= deltaTime;
        return IsDone;
    }

    public static implicit operator Timer(float time) => new Timer(time);
    public static implicit operator float(Timer timer) => timer._value;
}