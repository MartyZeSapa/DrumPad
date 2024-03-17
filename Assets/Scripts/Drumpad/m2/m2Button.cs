using UnityEngine;
using UnityEngine.UI;

public class m2Button : MonoBehaviour
{
    
    public int buttonIndex;
    public Color defaultColor;

    public SampleData m2ButtonSampleData;

    public bool buttonClicked = false;

    void Awake()
    {
        defaultColor = GetComponent<Image>().color;
    }
}


    