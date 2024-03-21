using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class m2DropReceiver : MonoBehaviour, IDropHandler
{
    public Button removeButton;

    public int rowIndex;
    [SerializeField] private TextMeshProUGUI buttonText;


    public List<m2Button> activatedButtons = new List<m2Button>();

    public SampleData sampleData;







    void Awake()
    {         
        removeButton.onClick.AddListener(RemoveThisRow);
    }

    public void RemoveThisRow()
    {
        GameManager.Instance.RemoveRow(rowIndex);
    }










    public void OnDrop(PointerEventData eventData)  // Uloží do sebe droppedClip,   Zmìní barvu a text Sample Panelu
    {
        GameObject droppedObject = eventData.pointerDrag;   // Odkaz na dropnutý objekt

        if (droppedObject != null)
        {
            AudioClip droppedClip = droppedObject.GetComponent<SoundData>()?.soundClip;    // Odkaz na clip dropnutého objektu
            Color panelColor = droppedObject.GetComponent<Image>().color;                  //          barvu






            GetComponent<Image>().color = panelColor;    // Zmìní barvu Sample Panelu
            UpdateButtonText(droppedClip?.name);         //       text


            sampleData = new SampleData(droppedClip, panelColor);   // Založí globální SampleData


            foreach (var m2ButtonScript in activatedButtons)    
            {
                m2ButtonScript.GetComponent<Image>().color = sampleData.color;  // Zmìní barvu všech stisknutých tlaèítek

                if (m2ButtonScript.buttonClicked) // Odstraní stará a pøidá nová SampleData do Sublistù Beatù
                {
                    GameManager.Instance.ReplaceSampleDataInBeat(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData, sampleData);
                }
            }

            GameManager.Instance.UpdateM2ButtonsSampleData(rowIndex, sampleData);  // Updatne SampleData všech tlaèítek   



           
        }
    }

    private void UpdateButtonText(string newButtonName)
    {
        buttonText.text = newButtonName;
        
    }









    ////////////////////////////////////////////////////////////////////////////////////




    public void SetSampleData(SampleData newSampleData)
    {
        sampleData = newSampleData;

        // Update UI colors and texts
        UpdateSamplePanelUI();
    }



    public void UpdateSamplePanelUI()
    {
        GetComponent<Image>().color = sampleData.color; // Set panel color
        UpdateButtonText(sampleData.audioClip.name);    // Set panel text





        ResetActivatedButtons();
    }


    private void ResetActivatedButtons()
    {
        activatedButtons.Clear();
        GameManager.Instance.UpdateM2ButtonsSampleData(rowIndex, sampleData);

        var buttonIndexesForSample = GameManager.Instance.GetAllButtonIndexesForSample(sampleData);

        foreach (int index in buttonIndexesForSample)
        {
            var m2buttonScript = GameManager.Instance.GetM2ButtonScriptByIndex(rowIndex, index);
            if (m2buttonScript != null)
            {
                activatedButtons.Add(m2buttonScript);
                m2buttonScript.SetButtonState(true, sampleData.color);



                // You may also need to update buttonScript properties here
            }
        }

        
    }






}