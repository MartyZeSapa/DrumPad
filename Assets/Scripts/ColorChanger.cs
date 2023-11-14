using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorChanger : MonoBehaviour
{
    public Slider backgroundSlider;
    public Slider headAndDrumpadSlider;

    public Image backgroundSliderBackground;
    public Image headAndDrumpadSliderBackground;



    public TMP_InputField hexInputField1;
    public TMP_InputField hexInputField2;

    public Image Background;
    public Image headPanel;
    public Image m1DrumpadPanel;
    public Image m2DrumpadPanel;
    public Image m3DrumpadPanel;

    private bool updatingHexField = false;

    private float backgroundSliderValue = 0f;
    private float headAndDrumpadSliderValue = 0f;

    void Start()
    {
        backgroundSlider.onValueChanged.AddListener(OnBackgroundSliderChanged);
        headAndDrumpadSlider.onValueChanged.AddListener(OnHeadAndDrumpadSliderChanged);

        hexInputField1.onValueChanged.AddListener(OnHexInputFieldChanged);
        hexInputField2.onValueChanged.AddListener(OnHexInputField2Changed);
    }

    public void OnBackgroundSliderChanged(float value)
    {
        backgroundSliderValue = value;

        Color newColor = Color.HSVToRGB(value, 1f, 1f);

        Background.color = newColor;
        backgroundSliderBackground.color = newColor;


        if (!updatingHexField)
        {
            hexInputField1.text = "#" + ColorUtility.ToHtmlStringRGB(newColor);
        }
    }

    public void OnHeadAndDrumpadSliderChanged(float value)
    {
        headAndDrumpadSliderValue = value;

        Color newColor = Color.HSVToRGB(value, 1f, 1f);

        headPanel.color = newColor;
        m1DrumpadPanel.color = newColor;
        m2DrumpadPanel.color = newColor;
        m3DrumpadPanel.color = newColor;

        headAndDrumpadSliderBackground.color = newColor;


        if (!updatingHexField)
        {
            hexInputField2.text = "#" + ColorUtility.ToHtmlStringRGB(newColor);
        }
    }

    public void OnHexInputFieldChanged(string hexValue)
    {
        // Pøidáme "#" pøed hodnotu v textovém poli, pokud již není pøítomná.
        if (!hexValue.StartsWith("#"))
        {
            hexValue = "#" + hexValue;
            hexInputField1.text = hexValue;
        }

        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexValue, out newColor))
        {
            updatingHexField = true;

            float hue = 0f;
            Color.RGBToHSV(newColor, out hue, out _, out _);

            backgroundSliderValue = hue;
            backgroundSlider.value = hue;

            Background.color = newColor;
            backgroundSliderBackground.color = newColor;

            updatingHexField = false;
        }
    }


    public void OnHexInputField2Changed(string hexValue)
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString("#" + hexValue, out newColor))
        {
            updatingHexField = true;

            float hue = 0f;
            Color.RGBToHSV(newColor, out hue, out _, out _);

            headAndDrumpadSliderValue = hue;
            headAndDrumpadSlider.value = hue;

            headPanel.color = newColor;
            m1DrumpadPanel.color = newColor;
            m2DrumpadPanel.color = newColor;
            m3DrumpadPanel.color = newColor;

            headAndDrumpadSliderBackground.color = newColor;

            updatingHexField = false;
        }
    }
}
