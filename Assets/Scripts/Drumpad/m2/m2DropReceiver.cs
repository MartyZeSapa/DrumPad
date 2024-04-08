using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class M2DropReceiver : MonoBehaviour, IDropHandler
{


    #region Inicializace

    public int rowIndex;
    public SampleData sampleData;


    [SerializeField] private TextMeshProUGUI SamplePanelText;


    [SerializeField] private Button removeButton;


    public List<M2Button> activatedButtons = new();

    private GameManager gameManager;
    private NotificationController notificationController;

    void Start()
    {
       
        gameManager = GameManager.Instance;
        notificationController = NotificationController.Instance;
        removeButton.onClick.AddListener(RemoveThisRow);
    }



    #endregion



    ////////////////////////////////////////////////////////////////////////////////////


    #region OnDrop()





    public void OnDrop(PointerEventData eventData)  // Zmìní barvu a text Sample Panelu
                                                    // nahradí SampleData v Beatech na indexes activatedButton a zmìní stisknutým tlaèítkùm barvu,
                                                    // zmìní SampleData všech m2Buttons


    {
        SoundData soundData = eventData.pointerDrag.GetComponent<SoundData>();
        if (soundData == null) return; // Proper error handling


        AudioClip droppedClip = soundData.soundClip;                        // Odkaz na clip dropnutého objektu
        int sampleIndex = soundData.sampleIndex;                            //        sampleIndex
        Color panelColor = soundData.GetComponent<Image>().color;       //          barvu


        #region safety check    // Jestli existuje samplePanel s tímto Samplem

        if (!gameManager.IsUniqueSamplePanel(droppedClip))
        {
            notificationController.ShowNotification("A row with this sample already exists.");
            return;
        }

        #endregion





            // Zmìní barvu Sample Panelu
        UpdateSamplePanelUI(droppedClip.name, panelColor);         //       text


        sampleData = new SampleData(droppedClip, panelColor, sampleIndex);   // Založí globální SampleData


        foreach (var m2ButtonScript in activatedButtons)
        {
            m2ButtonScript.GetComponent<Image>().color = sampleData.color;  // Zmìní barvu všech stisknutých tlaèítek

            if (m2ButtonScript.buttonClicked) // Odstraní stará a pøidá nová SampleData do Sublistù Beatù
            {
                gameManager.ReplaceSampleDataInBeat(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData, sampleData);
            }
        }

        gameManager.ReplaceM2ButtonSampleData(rowIndex, sampleData);  // Updatne SampleData všech tlaèítek   





    }

    

    public void UpdateSamplePanelUI(string newButtonName, Color newColor)
    {
        GetComponent<Image>().color = newColor;

        SamplePanelText.text = newButtonName;

    }

    #endregion



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region Metody volané z GameManager - UpdateMode2UI()





    public void UpdateSamplePanel(SampleData newSampleData) // Nastaví nová SampleData
    {
        sampleData = newSampleData;

        UpdateSamplePanelUI(sampleData.audioClip.name, sampleData.color);   // Zmìní barvu a text samplePanelu





        ReplaceActivatedButtons();
    }


    private void ReplaceActivatedButtons()    // Resetuje všechna tlaèítka, zmìní všem SampleData
                                            // Nastaví tlaèítka co mají být clicked
    {
        activatedButtons.Clear();

        GameManager.Instance.ReplaceM2ButtonSampleData(rowIndex, sampleData);   // Zmìní SampleData všech tlaèítek a resetuje je


        var buttonIndexesForSample = GameManager.Instance.GetAllButtonIndexesForSample(sampleData);     // Všem aktivním tlaèítkùm 
        foreach (int index in buttonIndexesForSample)
        {
            var m2buttonScript = GameManager.Instance.GetM2ButtonScriptByIndex(rowIndex, index);    // Vytáhne script tlaèítka
            if (m2buttonScript != null)
            {
                activatedButtons.Add(m2buttonScript);
                m2buttonScript.SetActiveButtons(true, sampleData.color); // Clicked = true, color, 
            }
        }


    }
    #endregion









    ////////////////////////////////////////////////////////////////
    public void RemoveThisRow()
    {
        gameManager.RemoveRow(rowIndex);
    }


}