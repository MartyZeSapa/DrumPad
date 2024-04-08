using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorChanger : MonoBehaviour
{   
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TMP_InputField hexInputField;

    [SerializeField]
    private Image Background;
    [SerializeField]
    private Image backgroundSliderBackground;

    private NotificationController notificationController;



    private bool updatingHexField = false;

    private float backgroundSliderValue;


    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderChanged);

        hexInputField.onEndEdit.AddListener(OnHexInputFieldEndEdit);

        notificationController = NotificationController.Instance;
    }

    private void OnSliderChanged(float value)
    {
        backgroundSliderValue = value;
        Color newColor = Color.HSVToRGB(value, 1f, 1f);   //(hue, saturation, brightness) 0-1

        Background.color = newColor;
        backgroundSliderBackground.color = newColor;

        notificationController.backgroundColor = newColor;

        if (!updatingHexField)  // updatingHexField je true pouze pokud zrovna upravuju hexField
        {
            hexInputField.text = "#" + ColorUtility.ToHtmlStringRGB(newColor);  // Pøevede newColor z RGB na String
        }
    }


    private void OnHexInputFieldEndEdit(string hexValue)
    {
        
        if (!hexValue.StartsWith("#")) // Zajisti že text v input fieldu zaèíná "#"
        {
            hexValue = "#" + hexValue;
            hexInputField.text = hexValue;
        }

        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexValue, out newColor))   // Pøevede z textu input fieldu na RGB
        {
            updatingHexField = true;

            float hue = 0f;
            Color.RGBToHSV(newColor, out hue, out _, out _);  // Pøevede z RGB na HSV (hue (0-1), saturation, value) a vytáhne hue komponent a uloží ji do hue promìnné

            backgroundSliderValue = hue; // Nastaví hue jako value slideru
            slider.value = hue; // Updatne UI slider aby ukazoval aktualní value

            Background.color = newColor;
            backgroundSliderBackground.color = newColor;

            notificationController.backgroundColor = newColor;

            updatingHexField = false;
        }
        else // If the hex string is not a valid color.
        {
            Debug.LogWarning("Incorrect hexadecimal code"); // Log an error message.
                                                     //  reset the hexInputField to the last valid color
            hexInputField.text = "#" + ColorUtility.ToHtmlStringRGB(Background.color);
        }


    }

}
