using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    public Slider backgroundSlider;
    public Slider headAndDrumpadSlider;

    public Image Background;

    public Image headPanel;
    public Image m1DrumpadPanel;
    public Image m2DrumpadPanel;
    public Image m3DrumpadPanel;

    void Start()
    {
        backgroundSlider.onValueChanged.AddListener(OnBackgroundSliderChanged);
        headAndDrumpadSlider.onValueChanged.AddListener(OnHeadAndDrumpadSliderChanged);
    }



    public void OnBackgroundSliderChanged(float value)
    {
        Color newColor;
        if (value < 0.1f) // Black to grey zone.
        {
            float brightness = value / 0.1f * 0.5f;
            newColor = Color.HSVToRGB(0f, 0f, brightness);
        }
        else if (value >= 0.1f && value < 0.9f) // Full color zone.
        {
            float hue = (value - 0.1f) / 0.8f;
            newColor = Color.HSVToRGB(hue, 1f, 1f);
        }
        else // Grey to white zone.
        {
            float brightness = 0.5f + (value - 0.9f) / 0.1f * 0.5f;
            newColor = Color.HSVToRGB(0f, 0f, brightness);
        }
        Background.color = newColor;
    }

    public void OnHeadAndDrumpadSliderChanged(float value)
    {
        Color newColor;
        if (value < 0.1f) // Black to grey zone.
        {
            float brightness = value / 0.1f * 0.5f;
            newColor = Color.HSVToRGB(0f, 0f, brightness);
        }
        else if (value >= 0.1f && value < 0.9f) // Full color zone.
        {
            float hue = (value - 0.1f) / 0.8f;
            newColor = Color.HSVToRGB(hue, 1f, 1f);
        }
        else // Grey to white zone.
        {
            float brightness = 0.5f + (value - 0.9f) / 0.1f * 0.5f;
            newColor = Color.HSVToRGB(0f, 0f, brightness);
        }
        headPanel.color = newColor;
        m1DrumpadPanel.color = newColor;
        m2DrumpadPanel.color = newColor;
        m3DrumpadPanel.color = newColor;

    }




}
