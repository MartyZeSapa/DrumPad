using UnityEngine;
using UnityEngine.UI;

public class m2Button : MonoBehaviour
{


    public int buttonIndex;
    public Color defaultColor = new Color(230, 230, 230);

    public SampleData m2ButtonSampleData;

    public bool buttonClicked = false;



    public void ResetButtonState(SampleData newSampleData)
    {
        m2ButtonSampleData = newSampleData;
        buttonClicked = false;
        GetComponent<Image>().color = defaultColor;
    }

    public void SetButtonState(bool clicked, Color newColor)
    {
        buttonClicked = clicked;
        GetComponent<Image>().color = clicked ? newColor : defaultColor;
    }
}


    