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





    public void OnDrop(PointerEventData eventData)  // Zm�n� barvu a text Sample Panelu
                                                    // nahrad� SampleData v Beatech na indexes activatedButton a zm�n� stisknut�m tla��tk�m barvu,
                                                    // zm�n� SampleData v�ech m2Buttons


    {
        SoundData soundData = eventData.pointerDrag.GetComponent<SoundData>();
        if (soundData == null) return; // Proper error handling


        AudioClip droppedClip = soundData.soundClip;                        // Odkaz na clip dropnut�ho objektu
        int sampleIndex = soundData.sampleIndex;                            //        sampleIndex
        Color panelColor = soundData.GetComponent<Image>().color;       //          barvu


        #region safety check    // Jestli existuje samplePanel s t�mto Samplem

        if (!gameManager.IsUniqueSamplePanel(droppedClip))
        {
            notificationController.ShowNotification("A row with this sample already exists.");
            return;
        }

        #endregion





            // Zm�n� barvu Sample Panelu
        UpdateSamplePanelUI(droppedClip.name, panelColor);         //       text


        sampleData = new SampleData(droppedClip, panelColor, sampleIndex);   // Zalo�� glob�ln� SampleData


        foreach (var m2ButtonScript in activatedButtons)
        {
            m2ButtonScript.GetComponent<Image>().color = sampleData.color;  // Zm�n� barvu v�ech stisknut�ch tla��tek

            if (m2ButtonScript.buttonClicked) // Odstran� star� a p�id� nov� SampleData do Sublist� Beat�
            {
                gameManager.ReplaceSampleDataInBeat(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData, sampleData);
            }
        }

        gameManager.ReplaceM2ButtonSampleData(rowIndex, sampleData);  // Updatne SampleData v�ech tla��tek   





    }

    

    public void UpdateSamplePanelUI(string newButtonName, Color newColor)
    {
        GetComponent<Image>().color = newColor;

        SamplePanelText.text = newButtonName;

    }

    #endregion



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region Metody volan� z GameManager - UpdateMode2UI()





    public void UpdateSamplePanel(SampleData newSampleData) // Nastav� nov� SampleData
    {
        sampleData = newSampleData;

        UpdateSamplePanelUI(sampleData.audioClip.name, sampleData.color);   // Zm�n� barvu a text samplePanelu





        ReplaceActivatedButtons();
    }


    private void ReplaceActivatedButtons()    // Resetuje v�echna tla��tka, zm�n� v�em SampleData
                                            // Nastav� tla��tka co maj� b�t clicked
    {
        activatedButtons.Clear();

        GameManager.Instance.ReplaceM2ButtonSampleData(rowIndex, sampleData);   // Zm�n� SampleData v�ech tla��tek a resetuje je


        var buttonIndexesForSample = GameManager.Instance.GetAllButtonIndexesForSample(sampleData);     // V�em aktivn�m tla��tk�m 
        foreach (int index in buttonIndexesForSample)
        {
            var m2buttonScript = GameManager.Instance.GetM2ButtonScriptByIndex(rowIndex, index);    // Vyt�hne script tla��tka
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