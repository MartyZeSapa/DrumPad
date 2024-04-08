using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M2Button : MonoBehaviour
{


    public int buttonIndex;
    public bool buttonClicked = false;

    public Color defaultColor = new(230, 230, 230);

    public SampleData m2ButtonSampleData;


    private GameManager gameManager;
    private NotificationController notificationController;

    private List<SampleData> beat;


    void Start()
    {
        gameManager = GameManager.Instance;
        notificationController = NotificationController.Instance;
        beat = gameManager.Beats[buttonIndex];
    }










    public void OnClick(M2DropReceiver dropReceiver)
    {
        #region safety check
        if (m2ButtonSampleData == null)
        {
            return;
        }

        if (dropReceiver == null)
        {
            Debug.LogError("ButtonClicked: dropReceiver is null.");
            return;
        }
        #endregion

        buttonClicked = !buttonClicked;

        if (buttonClicked)
        {
            if (beat.Count < 4)
            {
                dropReceiver.activatedButtons.Add(this);    // Pøidá tlaèítko do activatedButtons

                GameManager.Instance.AddSampleDataToBeat(buttonIndex, m2ButtonSampleData);      // Pøidá tlaèítko do Beatu

                GetComponent<Image>().color = m2ButtonSampleData.color;
            }
            else
            {
                notificationController.ShowNotification($"Max samples reached on this beat.");

            }

        }
        else
        {
            dropReceiver.activatedButtons.Remove(this);   // Odebere m2ButtonScript z activatedButtons

            GameManager.Instance.RemoveSampleDataFromBeat(buttonIndex, m2ButtonSampleData);

            GetComponent<Image>().color = defaultColor;
        }


    }





    ////////////////////////////////////////////////////////////////////////////////////




    #region Metody volané z gameManager - UpdateMode2UI

    public void ReplaceButtonsSampleData(SampleData newSampleData)
    {
        m2ButtonSampleData = newSampleData;
        buttonClicked = false;
        GetComponent<Image>().color = defaultColor;
    }

    public void SetActiveButtons(bool clicked, Color newColor)
    {
        buttonClicked = clicked;
        GetComponent<Image>().color = clicked ? newColor : defaultColor;
    }

    #endregion - 
}


