using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Slider))]
public class Scale : MonoBehaviour
{
    private const float MinValue = 0;
    private const float MaxValue = 1;

    [SerializeField] private AudioSource _fillingSound;

    private Slider _slider;
    
    public bool IsEmpty { get; private set; } = true;
    public float CurrentValue => _slider.value;

    public Action OnSliderFilled;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _fillingSound.Play();
        _slider.value = MinValue;
        IsEmpty = true;
        StartCoroutine(FillSlider());
    }

    private void OnDisable()
    {
        _slider.value = MinValue;
        IsEmpty = true;
        StopFill();
    }

    private void StopFill()
    {
        StopCoroutine(FillSlider());
    }

    private void OnValueChanged()
    {
        _slider.value += Time.deltaTime;
    }

    private IEnumerator FillSlider()
    {
        while (IsEmpty == true)
        {
            OnValueChanged();

            if (_slider.value >= MaxValue)
                IsEmpty = false;

            yield return new WaitForSeconds(0);
        }

        OnSliderFilled?.Invoke();
    }
}
