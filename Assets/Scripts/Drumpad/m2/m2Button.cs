using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M2Button : MonoBehaviour
{
    #region Inicializace

    private GameManager gameManager;
    private NotificationController notificationController;

    public SampleData m2ButtonSampleData;
    private List<SampleData> beat;

    public int buttonIndex;
    public bool buttonClicked = false;

    [SerializeField] public Image buttonImage;
    [SerializeField]  private Color defaultColor;
    private Color activeHighlightColor;
    [SerializeField] private Color inactiveHighlightColor;

    [SerializeField] private Image borderImage;
    [SerializeField] private Color borderColor;
    [SerializeField] private Color borderHighlightColor;

    void Start()
    {
        gameManager = GameManager.Instance;
        notificationController = NotificationController.Instance;

        beat = gameManager.Beats[buttonIndex];
    }

    #endregion


    ////////////////////////////////////////////////

    #region Button Highlight

    public void Highlight()
    {
        if (isActiveAndEnabled == false)
        {
            return;
        }


        if (buttonClicked)
        {
            activeHighlightColor = CalculateHighlightColor();
            HighlightColor(activeHighlightColor);
        }
        else
        {
            HighlightColor(inactiveHighlightColor);
        }
    }



    
    private Color CalculateHighlightColor()
    {
        // Neon colors are fully saturated and very bright, so we max out these values.
        float h, s, v;
        Color.RGBToHSV(m2ButtonSampleData.color, out h, out s, out v); // Convert RGB to HSV
        s = 1.0f;
        v = 1.0f;
        return Color.HSVToRGB(h, s, v, false); // Convert back to RGB with no HDR
    }

    private void HighlightColor(Color highlightColor)
    {
        buttonImage.color = highlightColor;
        borderImage.color = borderHighlightColor;
    }

    public void Unhighlight()
    {
        buttonImage.color = buttonClicked ? m2ButtonSampleData.color : defaultColor;
        borderImage.color = borderColor;
    }


    #endregion

    //////////////////////////////////////////////////////////////////////////////////////////




    public void OnClick(M2RowHandler rowHandler)
    {
        if (m2ButtonSampleData == null )
        {
            Debug.LogWarning("SampleData null");
            return;
        }

        if (rowHandler == null)
        {
            Debug.LogError("rowHandler null");
            return;
        }
        






        buttonClicked = !buttonClicked;

        if (buttonClicked)
        {
            if (beat.Count < 4)
            {
                rowHandler.activatedButtons.Add(this);
                gameManager.AddSampleDataToBeat(buttonIndex, m2ButtonSampleData);
                buttonImage.color = m2ButtonSampleData.color;
            }
            else
            {
                notificationController.ShowNotification("Max samples reached on this beat.");
                buttonClicked = false;
            }
        }
        else
        {
            rowHandler.activatedButtons.Remove(this);
            gameManager.RemoveSampleDataFromBeat(buttonIndex, m2ButtonSampleData);
            buttonImage.color = defaultColor;
        }
    }




    ////////////////////////////////////////////////


    public void ResetButtonState()
    {
        buttonClicked = false;
        buttonImage.color = defaultColor;
    }
    public void SetButtonsSampleData(SampleData newSampleData)
    {
        m2ButtonSampleData = newSampleData;
    }


    public void SetActiveButton(bool clicked)
    {
        buttonClicked = clicked;
        if(clicked)
        {
            buttonImage.color = m2ButtonSampleData.color;
        }
        else
        {
            buttonImage.color = defaultColor;
        }
        
    }
    

}
