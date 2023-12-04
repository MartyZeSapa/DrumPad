using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorChanger : MonoBehaviour
{
    public Slider slider;

    public TMP_InputField hexInputField;


    public Image Background;
    public Image backgroundSliderBackground;



    private bool updatingHexField = false;

    private float backgroundSliderValue;
    private float headAndDrumpadSliderValue;


    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderChanged);

        hexInputField.onValueChanged.AddListener(OnHexInputField1EndEdit);
    }

    public void OnSliderChanged(float value)
    {
        backgroundSliderValue = value;
        Color newColor = Color.HSVToRGB(value, 1f, 1f);   //(hue, saturation, brightness) 0-1

        Background.color = newColor;
        backgroundSliderBackground.color = newColor;

        if (!updatingHexField)  // updatingHexField je true pouze pokud zrovna upravuju hexField
        {
            hexInputField.text = "#" + ColorUtility.ToHtmlStringRGB(newColor);  // P�evede newColor z RGB na String
        }
    }


    public void OnHexInputField1EndEdit(string hexValue)
    {
        
        if (!hexValue.StartsWith("#")) // Zajisti �e text v input fieldu za��n� "#"
        {
            hexValue = "#" + hexValue;
            hexInputField.text = hexValue;
        }

        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexValue, out newColor))   // P�evede z textu input fieldu na RGB
        {
            updatingHexField = true;

            float hue = 0f;
            Color.RGBToHSV(newColor, out hue, out _, out _);  // P�evede z RGB na HSV (hue (0-1), saturation, value) a vyt�hne hue komponent a ulo�� ji do hue prom�nn�

            backgroundSliderValue = hue; // Nastav� hue jako value slideru
            slider.value = hue; // Updatne UI slider aby ukazoval aktualn� value

            Background.color = newColor;
            backgroundSliderBackground.color = newColor;

            updatingHexField = false;
        }
    }

}
