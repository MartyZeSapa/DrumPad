using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BpmHandler : MonoBehaviour
{
    [SerializeField] private Button plus;
    [SerializeField] private Button minus;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField bpmValue;

    private int bpm = 200;

    public static AudioPlaybackManager audioPlaybackManager;
    void Start()
    {


        plus.onClick.AddListener(IncrementBpm);
        minus.onClick.AddListener(DecrementBpm);

        slider.onValueChanged.AddListener(SliderValueChanged);
        bpmValue.onEndEdit.AddListener(InputFieldValueChanged);

        bpmValue.text = slider.value.ToString();

        audioPlaybackManager = AudioPlaybackManager.Instance;

        slider.value = bpm;
    }

    public void IncrementBpm()
    {

        UpdateBpm((int)bpm + 1);

    }

    public void DecrementBpm()
    {

        UpdateBpm((int)bpm - 1);
    }

    private void SliderValueChanged(float value)
    {
        UpdateBpm((int)value);
    }

    private void InputFieldValueChanged(string text)
    {
        if (int.TryParse(text, out int value))
        {
            UpdateBpm(value);
        }
    }



    private void UpdateBpm(int newBpm)
    {
        if (newBpm < 40)
        {
            bpm = 40;
        }
        else if(newBpm > 600)
        {
            bpm = 600;
        }    


        bpm = newBpm;

        slider.value = bpm;
        bpmValue.text = bpm.ToString();

        audioPlaybackManager.SetBPM(bpm);
    }
}