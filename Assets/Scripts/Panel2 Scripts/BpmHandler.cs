using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BpmHandler : MonoBehaviour
{
    public Button plus;
    public Button minus;
    public Slider slider;
    public TMP_InputField bpmValue;

    void Start()
    {
        plus.onClick.AddListener(PlusClick);
        minus.onClick.AddListener(MinusClick);
        slider.onValueChanged.AddListener(SliderValueChanged);
        bpmValue.onEndEdit.AddListener(InputFieldValueChanged);

        bpmValue.text = slider.value.ToString();   //kdyz se apka zapne vypise se value slideru do text input fieldu
    }

    private void PlusClick()
    {
        slider.value += 1;
    }

    private void MinusClick()
    {
        slider.value -= 1;
    }

    private void SliderValueChanged(float value)
    {
        bpmValue.text = value.ToString();
    }

    private void InputFieldValueChanged(string text)
    {
        if (float.TryParse(text, out float value)) //prevede zadane cislo(string) na int, a zmeni hodnotu slideru
        {
            slider.value = value;
        }
    }
}
