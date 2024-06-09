using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M1Popup : MonoBehaviour
{
    #region Inicializace

    public static M1Popup Instance;
    [SerializeField] private GameManager gameManager;

    public M1Button m1ButtonScript;
    private List<SampleData> currentBeat;
    private int currentIndex;

    [SerializeField] private List<M1PopupSample> samplePanelScripts = new();

    [SerializeField] private GameObject samplePopup;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private GameObject overlayPanel;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        HidePopup();
    }

    #endregion

    ////////////////////////////////////////////////
    

    public void HidePopup()
    {
        samplePopup.SetActive(false);
        overlayPanel.SetActive(false);
    }


    public void ShowPopup(int buttonIndex, M1Button m1Button)  
    {
        currentBeat = gameManager.Beats[buttonIndex];
        currentIndex = buttonIndex;
        m1ButtonScript = m1Button;

        UpdatePopupPanel();
    }



    public void UpdatePopupPanel()
    {
        ClearSamplePanels();
        for (int i = 0; i < 4; i++)
        {
            var samplePanelScript = samplePanelScripts[i];
            samplePanelScript.m1Button = m1ButtonScript;

            if (i < currentBeat.Count)
            {

                samplePanelScript.UpdateSamplePanel(currentBeat[i], currentIndex);
            }
            else
            {
                samplePanelScript.beatIndex = currentIndex;
            }
        }


        popupText.text = $"Beat {currentIndex + 1}";
        samplePopup.SetActive(true);
        overlayPanel.SetActive(true);
    }

    private void ClearSamplePanels()
    {
        foreach (var samplePanelScript in samplePanelScripts)
        {
            samplePanelScript.ClearSamplePanel();
        }
    }



}
