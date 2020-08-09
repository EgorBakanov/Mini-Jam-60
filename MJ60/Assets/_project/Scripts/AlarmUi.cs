using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AlarmUi : MonoBehaviour
{
    public Slider slider;
    public CanvasGroup wrapper;
    public float noneScale = 1;
    public float alarmScale = 1;
    public float rescaleTime;
    public Ease rescaleEase;
    public float pauseAlpha;
    
    private AlarmSystem _alarmSystem;
    
    private void Start()
    {
        _alarmSystem = GameManager.Instance.pausableSystemManager.alarmSystem;
        wrapper.alpha = pauseAlpha;
        _alarmSystem.OnAlarmValueChanged += OnAlarmValueChanged;
        _alarmSystem.OnPause += OnPause;
        _alarmSystem.OnUnpause += OnUnpause;
    }

    private void OnUnpause()
    {
        wrapper.alpha = 1;
    }

    private void OnPause()
    {
        wrapper.alpha = pauseAlpha;
    }

    private void OnAlarmValueChanged(float value)
    {
        var old = slider.value;
        slider.value = value;
        wrapper.alpha = 1f;
        if (value == 0f)
        {
            wrapper.transform.DOScale(noneScale, rescaleTime).SetEase(rescaleEase);
            wrapper.alpha = pauseAlpha;
        }
        else if (value == 1f)
            wrapper.transform.DOScale(alarmScale, rescaleTime).SetEase(rescaleEase);
        else if(old == 0f || old == 1f)
            wrapper.transform.DOScale(1f, rescaleTime).SetEase(rescaleEase);
    }
}
