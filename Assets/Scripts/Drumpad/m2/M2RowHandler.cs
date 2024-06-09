using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;

public class M2RowHandler : MonoBehaviour, IDropHandler
{


    #region Inicializace

    private GameManager gameManager;
    private NotificationController notificationController;

    public int rowIndex;
    public SampleData sampleData;
    [SerializeField] public Image[] m2SectionBorders = new Image[4];


    public Button[] Buttons = new Button[64];
    public M2Button[] ButtonScripts = new M2Button[64];
    public List<M2Button> activatedButtons = new();


    [SerializeField] private TextMeshProUGUI SamplePanelText;
    [SerializeField] private Image rowHandlerImage;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image volumeSliderFill;
    [SerializeField] private Button autoFillButton;
    [SerializeField] private Button removeButton;

    public List<M2Button[]> m2RowsButtonScripts;


    void Awake()
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


    #endregion


    #region Highlight
    public void HighlightSection(int sectionIndex)
    {

        UnhighlightAllSections();

        m2SectionBorders[sectionIndex].enabled = true;
    }


    public void UnhighlightAllSections()
    {
        foreach (var sectionBorder in m2SectionBorders)
        {
            sectionBorder.enabled = false;
        }
    }

    #endregion


    //////////////////////////////////////
    private void VolumeSliderChange(float volume)
    {
        if (sampleData != null)
        {
            gameManager.SetSampleVolume(sampleData.audioClip.name, volume);
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////


    public void OnDrop(PointerEventData eventData)
    {
        SoundData soundData = eventData.pointerDrag.GetComponent<SoundData>();
        if (soundData == null) return;


        #region safety check    // Jestli existuje samplePanel s tímto Samplem

        if (!gameManager.IsUniqueSamplePanel(soundData.soundClip))
        {
            notificationController.ShowNotification("A row with this sample already exists.");
            return;
        }

        #endregion


        sampleData = new SampleData(soundData.soundClip, soundData.sampleIndex, soundData.GetComponent<Image>().color);



        foreach (var button in activatedButtons)
        {
            if (button.buttonClicked)
            {
                gameManager.ReplaceSampleDataInBeat(button.buttonIndex, button.m2ButtonSampleData, sampleData);
            }
        }

        //gameManager.ResetM2ButtonsAndReplaceSampleData(rowIndex, sampleData);

        foreach (var button in ButtonScripts)
        {
            button.SetButtonsSampleData(sampleData);
        }


        foreach (var button in activatedButtons)
        {
            button.SetActiveButton(true);
        }

        gameManager.SetSampleVolume(sampleData.audioClip.name, volumeSlider.value);

        UpdateSamplePanelUI();
    }



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region UpdateSamplePanelUI(), UpdateSamplePanel(), ReplaceActivatedButtons()



    public void UpdateSamplePanelUI()
    {
        rowHandlerImage.color = sampleData.color;
        SamplePanelText.text = sampleData.audioClip.name;

        volumeSliderFill.color = sampleData.color;
    }
    public void UpdateSamplePanel(SampleData newSampleData)
    {

        gameManager = GameManager.Instance;


        sampleData = newSampleData;

        volumeSlider.value = gameManager.GetSampleVolume(newSampleData.audioClip.name);
        UpdateSamplePanelUI();


        ReplaceActivatedButtons();
    }


    private void ReplaceActivatedButtons()
    {
        activatedButtons.Clear();

        GameManager.Instance.ResetM2ButtonsAndReplaceSampleData(rowIndex, sampleData);


        var buttonIndexesForSample = GameManager.Instance.GetAllButtonIndexesForSample(sampleData);
        foreach (int index in buttonIndexesForSample)
        {
            var m2buttonScript = GameManager.Instance.m2RowsButtonScripts[rowIndex][index];
            if (m2buttonScript != null)
            {
                activatedButtons.Add(m2buttonScript);
                m2buttonScript.SetActiveButton(true);
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
        var firstSectionButtonScripts = ButtonScripts.Take(16).ToList();


        for (int i = 0; i < firstSectionButtonScripts.Count; i++)
        {
            M2Button firstSectionButtonScript = firstSectionButtonScripts[i];
            bool isClicked = firstSectionButtonScript.buttonClicked;


            for (int sec = 1; sec < 4; sec++)     // Apply this state to the corresponding button in other sections
            {
                M2Button buttonScript = ButtonScripts[sec * 16 + i];

                bool previousState = buttonScript.buttonClicked;
                        
                buttonScript.SetActiveButton(isClicked);


                if (isClicked && !gameManager.BeatContainsSample(buttonScript.buttonIndex, sampleData.sampleIndex))
                {
                    gameManager.AddSampleDataToBeat(buttonScript.buttonIndex, sampleData);
                    activatedButtons.Add(buttonScript);
                }
                else if (!isClicked)
                {
                    gameManager.RemoveSampleDataFromBeat(buttonScript.buttonIndex, sampleData);
                    activatedButtons.Remove(buttonScript);
                }
            }
        }
    }
}