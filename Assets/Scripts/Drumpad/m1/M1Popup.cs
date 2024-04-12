using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M1Popup : MonoBehaviour
{
    public static M1Popup Instance { get; private set; }
    public M1Button m1ButtonScript;


    [SerializeField] private GameObject samplePopup;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private List<GameObject> samplePanels = new();
    [SerializeField] private GameObject overlayPanel;

    private List<SampleData> currentBeat;
    private int currentIndex;

    void Awake()    // Singleton, HidePopup()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        HidePopup();
    }


    public void HidePopup()
    {
        samplePopup.SetActive(false);
        overlayPanel.SetActive(false);
    }








    public void ShowPopup(int buttonIndex, M1Button m1Button)  
    {
        currentBeat = GameManager.Instance.Beats[buttonIndex];   // List samplù
        currentIndex = buttonIndex;     // Index
        m1ButtonScript = m1Button;      // Uloží script M1Buttonu

        UpdatePopupPanel();
    }



    public void UpdatePopupPanel()  // ClearSamplePanels(), UpdateSamplePanel(), updatne UI 
    {
        ClearSamplePanels();
        for (int i = 0; i < 4; i++)
        {
            var samplePanelScript = samplePanels[i].GetComponent<M1PopupSample>();
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
        foreach (var samplePanel in samplePanels)
        {
            var samplePanelScript = samplePanel.GetComponent<M1PopupSample>();
            samplePanelScript.ClearSamplePanel();
        }
    }



}
