using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI dustText;

    public void SetPowerSliderValue(float _value, float _max)
    {
        powerSlider.value = _value;
        powerSlider.maxValue = _max;
        powerText.text = $"Power ({Mathf.Round(_value)}/{_max})";
    }
    public void SetDustValue(float _value, float _max)
    {
        dustText.text = $"Dust ({Mathf.Round(_value)}/{_max})";
    }
}