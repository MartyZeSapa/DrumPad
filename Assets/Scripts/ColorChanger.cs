using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorChanger : MonoBehaviour
{
    public Slider backgroundSlider;
    public Slider headAndDrumpadSlider;

    public TMP_InputField hexInputField1;
    public TMP_InputField hexInputField2;


    public Image Background;
    public Image backgroundSliderBackground;

    public Image headPanel;
    public Image headAndDrumpadSliderBackground;
    public Image m1DrumpadPanel;
    public Image m2DrumpadPanel;
    public Image m3DrumpadPanel;



    private bool updatingHexField = false;

    private float backgroundSliderValue;
    private float headAndDrumpadSliderValue;


    void Start()
    {
        backgroundSlider.onValueChanged.AddListener(OnBackgroundSliderChanged);
        headAndDrumpadSlider.onValueChanged.AddListener(OnHeadAndDrumpadSliderChanged);

        hexInputField1.onValueChanged.AddListener(OnHexInputField1EndEdit);
        hexInputField2.onValueChanged.AddListener(OnHexInputField2EndEdit);
    }

    public void OnBackgroundSliderChanged(float value)
    {
        backgroundSliderValue = value;
        Color newColor = Color.HSVToRGB(value, 1f, 1f);   //(hue, saturation, brightness) 0-1

        Background.color = newColor;
        backgroundSliderBackground.color = newColor;

        if (!updatingHexField)  //updatingHexField je true pouze pokud zrovna upravuju hexField
        {
            hexInputField1.text = "#" + ColorUtility.ToHtmlStringRGB(newColor);  //prevede newColor z RGB na String
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

    public void OnHexInputField1EndEdit(string hexValue)
    {
        //zajisti ze text v input fieldu zacina "#"
        if (!hexValue.StartsWith("#"))
        {
            hexValue = "#" + hexValue;
            hexInputField1.text = hexValue;
        }

        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexValue, out newColor))   //prevede z textu input fieldu na RGB
        {
            updatingHexField = true;

            float hue = 0f;
            Color.RGBToHSV(newColor, out hue, out _, out _);  //prevede z RGB na HSV (hue (0-1), saturation, value) a vytahne hue komponent a ulozi ji do hue promenny

            backgroundSliderValue = hue; //nastavi hue jako value slideru
            backgroundSlider.value = hue; //updatne UI slider aby ukazoval aktualni value

            Background.color = newColor;
            backgroundSliderBackground.color = newColor;

            updatingHexField = false;
        }
    }

    public void OnHexInputField2EndEdit(string hexValue)
    {
        if (!hexValue.StartsWith("#"))
        {
            hexValue = "#" + hexValue;
            hexInputField2.text = hexValue;
        }

        // Process the color value here
        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexValue, out newColor))
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
