using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SystemPauserUi : MonoBehaviour, IPointerClickHandler
{
    public Slider slider;
    public PausableSystem pausableSystem;
    public TMP_Text text;
    public Image fill;
    public Gradient fillGradient;
    public RectTransform buttonWrapper;
    public float openTime;
    public Ease openEase;

    public void Init(PausableSystem system)
    {
        pausableSystem = system;
        text.text = pausableSystem.name;
        pausableSystem.OnValueChanged += OnValueChanged;
        buttonWrapper.localScale = Vector3.zero;
        buttonWrapper.DOScale(Vector3.one, openTime).SetEase(openEase);
    }

    private void OnDestroy()
    {
        pausableSystem.OnValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(float value)
    {
        slider.value = value;
        fill.color = fillGradient.Evaluate(value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pausableSystem.Pause();
    }
}
