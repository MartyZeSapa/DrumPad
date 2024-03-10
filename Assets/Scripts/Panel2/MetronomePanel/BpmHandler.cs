using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BpmHandler : MonoBehaviour
{
    [SerializeField]
    private Button plus;
    [SerializeField]
    private Button minus;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMP_InputField bpmValue;
    [SerializeField]

    void Start()
    {
        plus.onClick.AddListener(IncrementBpm);
        minus.onClick.AddListener(DecrementBpm);
        slider.onValueChanged.AddListener(SliderValueChanged);
        bpmValue.onEndEdit.AddListener(InputFieldValueChanged);

        bpmValue.text = slider.value.ToString();
    }

    public void IncrementBpm()
    {
        slider.value += 1;
    }

    public void DecrementBpm()
    {
        slider.value -= 1;
    }

    private void SliderValueChanged(float value)
    {
        bpmValue.text = value.ToString();
    }

    private void InputFieldValueChanged(string text)
    {
        if (float.TryParse(text, out float value))
        {
            slider.value = value;
        }
    }
}
