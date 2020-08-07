using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SystemPauserUi : MonoBehaviour, IPointerClickHandler
{
    public Slider slider;
    public PausableSystem pausableSystem;
    public TMP_Text text;

    private void OnEnable()
    {
        text.text = pausableSystem.name;
        pausableSystem.OnValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        pausableSystem.OnValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(float value)
    {
        slider.value = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pausableSystem.Pause();
    }
}
