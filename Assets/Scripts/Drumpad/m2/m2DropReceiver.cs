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

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image volumeSliderFill;
    [SerializeField] private Button autoFillButton;
    [SerializeField] private Button removeButton;


    public List<M2Button> activatedButtons = new();

    private GameManager gameManager;
    private NotificationController notificationController;

    void Start()
    {

        gameManager = GameManager.Instance;
        notificationController = NotificationController.Instance;

        removeButton.onClick.AddListener(RemoveThisRow);
        autoFillButton.onClick.AddListener(AutoFillThisRow);

        if (sampleData != null)
        {
            volumeSlider.value = gameManager.GetSampleVolume(sampleData.audioClip.name);
        }
        volumeSlider.onValueChanged.AddListener(VolumeSliderChange); // Listen to volume changes
    }

    private void VolumeSliderChange(float volume)
    {
        if (sampleData != null)
        {
            gameManager.SetSampleVolume(sampleData.audioClip.name, volume);
        }
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


        foreach (var button in activatedButtons)
        {
            button.GetComponent<Image>().color = sampleData.color;  // Zmìní barvu všech stisknutých tlaèítek

            if (button.buttonClicked) // Odstraní stará a pøidá nová SampleData do Sublistù Beatù
            {
                gameManager.ReplaceSampleDataInBeat(button.buttonIndex, button.m2ButtonSampleData, sampleData);
            }
        }

        gameManager.ReplaceM2ButtonSampleData(rowIndex, sampleData);  // Updatne SampleData všech tlaèítek   
        gameManager.SetSampleVolume(sampleData.audioClip.name, volumeSlider.value);
    }



    public void UpdateSamplePanelUI(string newButtonName, Color newColor)
    {
        GetComponent<Image>().color = newColor;

        SamplePanelText.text = newButtonName;


        volumeSliderFill.color = newColor;

    }

    #endregion



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region UpdateSamplePanel()





    public void UpdateSamplePanel(SampleData newSampleData) // Nastaví nová SampleData
    {
        sampleData = newSampleData;

        volumeSlider.value = GameManager.Instance.GetSampleVolume(newSampleData.audioClip.name);
        UpdateSamplePanelUI(sampleData.audioClip.name, sampleData.color);   // Zmìní barvu a text samplePanelu





        ReplaceActivatedButtons();
    }


    private void ReplaceActivatedButtons()    // Resetuje všechna tlaèítka, zmìní všem SampleData
                                              // Nastaví tlaèítka co mají být clicked
    {
        activatedButtons.Clear();

        GameManager.Instance.ResetRowsButtonState(rowIndex);
        GameManager.Instance.ReplaceM2ButtonSampleData(rowIndex, sampleData);   // Zmìní SampleData všech tlaèítek a resetuje je


        var buttonIndexesForSample = GameManager.Instance.GetAllButtonIndexesForSample(sampleData);     // Všem aktivním tlaèítkùm 
        foreach (int index in buttonIndexesForSample)
        {
            var m2buttonScript = GameManager.Instance.GetM2ButtonScriptByIndex(rowIndex, index);    // Vytáhne script tlaèítka
            if (m2buttonScript != null)
            {
                activatedButtons.Add(m2buttonScript);
                m2buttonScript.SetActiveButton(true, sampleData.color); // Clicked = true, color, 
            }
        }


    }
    #endregion









    ////////////////////////////////////////////////////////////////
    public void RemoveThisRow()
    {
        gameManager.RemoveRow(rowIndex);
    }



    private void AutoFillThisRow()
    {


        List<Button> firstSectionButtons = gameManager.mode2Rows[rowIndex].GetRange(0, 16);
        List<List<Button>> allSections = new List<List<Button>>();

        // Divide all buttons into sections
        for (int i = 0; i < 4; i++)
        {
            allSections.Add(gameManager.mode2Rows[rowIndex].GetRange(i * 16, 16));
        }

        // Iterate through each button in the first section
        for (int i = 0; i < firstSectionButtons.Count; i++)
        {
            M2Button firstSectionButtonScript = firstSectionButtons[i].GetComponent<M2Button>();
            bool isActive = firstSectionButtonScript.buttonClicked;

            Color activeColor = firstSectionButtonScript.GetComponent<Image>().color;
            SampleData activeSample = firstSectionButtonScript.m2ButtonSampleData;

            // Apply this state to the corresponding button in other sections
            for (int sec = 1; sec < 4; sec++)
            {
                Button buttonToModify = allSections[sec][i];
                M2Button buttonScript = buttonToModify.GetComponent<M2Button>();
                buttonScript.SetActiveButton(isActive, activeColor);

                // Check if the sample already exists in the beat before adding or removing
                if (isActive && !gameManager.BeatContainsSample(buttonScript.buttonIndex, activeSample.sampleIndex))
                {
                    gameManager.AddSampleDataToBeat(buttonScript.buttonIndex, activeSample);
                    activatedButtons.Add(buttonScript); // Ensure all autofilled buttons are added to the activated buttons list
                }
                else if (!isActive)
                {
                    gameManager.RemoveSampleDataFromBeat(buttonScript.buttonIndex, activeSample);
                    activatedButtons.Remove(buttonScript);
                }
            }
        }
    }

}