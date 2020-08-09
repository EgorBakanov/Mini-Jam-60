using System.Collections;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class Timer
{
    public float Value
    {
        get => _value;
        set
        {
            IsDone = value <= 0f;
            _value = Mathf.Max(0f, value);
        }
    }

    public bool IsDone { get; private set; }

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
    public static implicit operator CustomYieldInstruction(Timer timer) => new YieldTimer(timer);
    public override string ToString()
    {
        return _value.ToString(CultureInfo.InvariantCulture);
    }
    
    private class YieldTimer : CustomYieldInstruction
    {
        private readonly Timer _timer;

        public override bool keepWaiting => !_timer.Tick(Time.deltaTime);

        public YieldTimer(Timer timer)
        {
            _timer = timer;
        }
    }
}