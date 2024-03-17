using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class m2DropReceiver : MonoBehaviour, IDropHandler
{
    public int rowIndex;
    private AudioSource audioSource;
    private TextMeshProUGUI buttonText;
    public Color defaultButtonColor;

    public List<m2Button> activatedButtons = new List<m2Button>();

    public SampleData sampleData;

  

    void Awake()    // Vyt�hne AudioSource a Text Sample Panelu
    {
        audioSource = GetComponent<AudioSource>();                      
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

    }










    public void OnDrop(PointerEventData eventData)  // Ulo�� do sebe droppedClip,   Zm�n� barvu a text Sample Panelu
    {
        GameObject droppedObject = eventData.pointerDrag;   // Odkaz na dropnut� objekt

        if (droppedObject != null)
        {
            AudioClip droppedClip = droppedObject.GetComponent<SoundData>()?.soundClip;    // Odkaz na clip dropnut�ho objektu
            Color panelColor = droppedObject.GetComponent<Image>().color;                  //          barvu



            sampleData = new SampleData(droppedClip, panelColor);   // Zalo�� glob�ln� SampleData


            


            foreach (var m2ButtonScript in activatedButtons)    
            {
                m2ButtonScript.GetComponent<Image>().color = sampleData.color;  // Zm�n� barvu v�ech stisknut�ch tla��tek

                if (m2ButtonScript.buttonClicked) // Odstran� star� a p�id� nov� SampleData do Sublist� Beat�
                {
                    GameManager.Instance.ReplaceSampleDataInBeat(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData, sampleData);
                }
            }






            GameManager.Instance.UpdateM2ButtonsSampleData(rowIndex, sampleData);  // Updatne SampleData v�ech tla��tek   



            GetComponent<Image>().color = panelColor;    // Zm�n� barvu Sample Panelu
            UpdateButtonText(droppedClip?.name);         //       text
        }
    }

    private void UpdateButtonText(string newButtonName)
    {
        if (buttonText != null)
        {
            buttonText.text = newButtonName;
        }
    }






    ////////////////////////////////////////////////////////////////////////////////////






    public void UpdateSamplePanelUI(SampleData newSampleData, List<int> buttonIndexesForSample)
    {
        // Zm�n� barvu a  text samplePanelu
        GetComponent<Image>().color = newSampleData.color;
        UpdateButtonText(newSampleData.audioClip.name);

        foreach (var button in GameManager.Instance.mode2Rows[rowIndex])
        {
            m2Button m2ButtonScript = button.GetComponent<m2Button>();

            button.GetComponent<Image>().color = m2ButtonScript.defaultColor;
            m2ButtonScript.buttonClicked = false;
        }

        ResetActivatedButtons(buttonIndexesForSample, newSampleData);


        GameManager.Instance.UpdateM2ButtonsSampleData(rowIndex, newSampleData);  // Updatne SampleData v�ech tla��tek
    }





    private void ResetActivatedButtons(List<int> buttonIndexesForSample, SampleData newSampleData)
    {
        activatedButtons.Clear();

        foreach (int index in buttonIndexesForSample)
        {
            var m2buttonScript = GameManager.Instance.GetM2ButtonScriptByIndex(rowIndex, index);
            if (m2buttonScript != null)
            {
                activatedButtons.Add(m2buttonScript);
                m2buttonScript.GetComponent<Image>().color = newSampleData.color;
                m2buttonScript.buttonClicked = true;


                // You may also need to update buttonScript properties here
            }
        }
    }













   
}
