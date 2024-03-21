using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    #region initialization
    [SerializeField] private GameObject mode1;

    [SerializeField] private GameObject m2RowPrefab;
    [SerializeField] private GameObject m2RowContainer;
    [SerializeField] private Button addRowButton;

    [SerializeField] private Button logButton;







    public Button[] m1Buttons = new Button[64];

    public List<List<Button>> mode2Rows = new List<List<Button>>();
    public List<m2DropReceiver> m2DropReceivers = new List<m2DropReceiver>();
    public List<List<SampleData>> Beats = new List<List<SampleData>>();


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializeM1ButtonArray();
        InitializeBeatsList();
        mode2Rows = new List<List<Button>>();

        logButton.onClick.AddListener(LogButtonClick);
    }



    private void InitializeM1ButtonArray()   // Projede tla��tka z M1, p�id� ka�d� tla��tko do m1Buttons pole a ve scriptu tla��tka d� unik�tn� index
    {

        int buttonIndex = 0;
        for (int row = 1; row <= 4; row++)
        {
            Transform rowPanel = mode1.transform.Find("Row" + row);
            if (rowPanel == null) continue;

            for (int bar = 1; bar <= 4; bar++)
            {
                Transform barPanel = rowPanel.Find("Bar" + bar);
                if (barPanel == null) continue;

                for (int btn = 1; btn <= 4; btn++)
                {





                    Button button = barPanel.GetChild(btn - 1).GetComponent<Button>();

                    m1Button m1ButtonScript = button.GetComponent<m1Button>();  //Vyt�hne m1Button script z tla��tka
                    if (button != null && buttonIndex < m1Buttons.Length)
                    {
                        m1Buttons[buttonIndex] = button;
                        m1ButtonScript.buttonIndex = buttonIndex; // Nastav� buttonIndex v m1Button scriptu

                        buttonIndex++;
                    }
                }
            }
        }

        if (buttonIndex != m1Buttons.Length)
        {
            Debug.LogWarning("Na�lo se pouze" + buttonIndex + "tla��tek");
        }
    }

    private void InitializeBeatsList()
    {
        for (int i = 0; i < 64; i++)
        {
            Beats.Add(new List<SampleData>());
        }
    }


    #endregion



    ////////////////////////////////////////////////////////////////////////////////////









    private void ButtonClicked(m2Button m2ButtonScript, m2DropReceiver dropReceiver)   // P�i kliknut� na tla��tko zm�n� jeho barvu, P�id�/Odebere SampleData do dan�ho Beatu
    {

        Debug.LogError("Click!");

        // Check if m2ButtonScript is not null
        if (m2ButtonScript == null)
        {
            Debug.LogError("ButtonClicked: m2ButtonScript is null.");
            return;
        }

        // Check if m2ButtonScript's m2ButtonSampleData is not null
        if (m2ButtonScript.m2ButtonSampleData == null)
        {
            Debug.LogError("ButtonClicked: m2ButtonScript's m2ButtonSampleData is null.");
            return;
        }

        // Check if dropReceiver is not null
        if (dropReceiver == null)
        {
            Debug.LogError("ButtonClicked: dropReceiver is null.");
            return;
        }
       





        if (!m2ButtonScript.buttonClicked)
        {
            AddSampleDataIfUnique(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData);  // Pokud Sample v Beatu nen�, p�id�me ho
            dropReceiver.activatedButtons.Add(m2ButtonScript);  // P�id� m2ButtonScript do activatedButtons



            m2ButtonScript.GetComponent<Image>().color = m2ButtonScript.m2ButtonSampleData.color;   // Zm�na barvy
            m2ButtonScript.buttonClicked = true;


            LogBeatsListContents(m2ButtonScript.buttonIndex);
        }




        else
        {
            RemoveSampleData(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData);   // Odebere v�echna SampleData z Beatu co jsou stejn� jako m2ButtonSampleData
            dropReceiver.activatedButtons.Remove(m2ButtonScript);   // Odebere m2ButtonScript z activatedButtons



            m2ButtonScript.GetComponent<Image>().color = m2ButtonScript.defaultColor;   // Vr�t� barvu na default
            m2ButtonScript.buttonClicked = false;
        }
    }

    public void AddSampleDataIfUnique(int buttonIndex, SampleData newSample)
    {
        var beatsList = Beats[buttonIndex];
        if (!beatsList.Exists(sd => sd.audioClip == newSample.audioClip))
        {
            beatsList.Add(newSample);
        }
    }

    public void RemoveSampleData(int buttonIndex, SampleData sampleToRemove)
    {
        Beats[buttonIndex].RemoveAll(sd => sd.audioClip == sampleToRemove.audioClip && sd.color == sampleToRemove.color);
    }

















    ////////////////////////////////////////////////////////////////////////////////////



    #region updateUI

    public void UpdateMode1UI()
    {
        for (int i = 0; i < 64; i++)
        {

            var BeatsSublist = Beats[i];
            var buttonScript = m1Buttons[i].GetComponent<m1Button>();


            buttonScript.ClearQuadrantColors();






            if (BeatsSublist.Count == 1)    // Pokud je Sample 1, zm�n� barvy v�ech kvadrant� na barvu tohoto samplu
            {
                foreach (var quadrantImage in buttonScript.quadrantImages)
                {
                    quadrantImage.color = BeatsSublist[0].color;
                }
            }
            else if (BeatsSublist.Count == 2)   // Pokud jsou 2, zbarv� 2 kvadranty podle 1. samplu, a druh� 2 podle druh�ho samplu
            {
                buttonScript.quadrantImages[0].color = BeatsSublist[0].color;
                buttonScript.quadrantImages[1].color = BeatsSublist[0].color;

                buttonScript.quadrantImages[2].color = BeatsSublist[1].color;
                buttonScript.quadrantImages[3].color = BeatsSublist[1].color;
            }
            else if (BeatsSublist.Count == 3)   // Pokud jsou 3, zbarv� prvn� 3 kvadranty podle t�ch 3 sampl� a 4. kvadrantu d� barvu 3. samplu
            {
                for (int j = 0; j < BeatsSublist.Count && j < buttonScript.quadrantImages.Count; j++)
                {
                    buttonScript.quadrantImages[j].color = BeatsSublist[j].color;
                }
                buttonScript.quadrantImages[3].color = BeatsSublist[2].color;
            }
            else            // Pro v�ce ne� 3 samply, zbarv� ka�d� kvadrant podle ka�d�ho samplu
            {
                for (int j = 0; j < BeatsSublist.Count && j < buttonScript.quadrantImages.Count; j++)
                {
                    buttonScript.quadrantImages[j].color = BeatsSublist[j].color;
                }
            }
        }
    }












    public void UpdateMode2UI()
    {
        HashSet<SampleData> uniqueSamples = IdentifyUniqueSamplesInBeats();

        #region uniqueSamples Debug.Log
        Debug.Log("");
        Debug.Log("Unique Samples:");
        foreach (var sample in uniqueSamples)
        {
            Debug.Log($"{sample.audioClip.name}");
        }
        Debug.Log("");
        #endregion


        RemoveUnusedSampleRows(uniqueSamples);

        AssignSamplesToDropReceivers(uniqueSamples);
    }

    private HashSet<SampleData> IdentifyUniqueSamplesInBeats()
    {
        HashSet<SampleData> uniqueSamples = new HashSet<SampleData>(new SampleDataComparer());
        foreach (var beat in Beats)
        {
            foreach (var sample in beat)
            {
                uniqueSamples.Add(sample);
            }
        }
        return uniqueSamples;
    }   // Funguje




    private void RemoveUnusedSampleRows(HashSet<SampleData> uniqueSamples)
    {
        for (int i = m2DropReceivers.Count - 1; i >= 0; i--)
        {
            var sampleData = m2DropReceivers[i].sampleData;
            if (sampleData == null || !uniqueSamples.Contains(sampleData))
            {
                RemoveRow(i);
            }
        }
    }
    public void RemoveRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= m2DropReceivers.Count) return; // Validate index

        SampleData sampleToRemove = m2DropReceivers[rowIndex].sampleData;
        if (sampleToRemove != null)
        {
            Beats.ForEach(beatList => beatList.RemoveAll(sample => sample.audioClip == sampleToRemove.audioClip));
        }

        var rowGameObject = m2DropReceivers[rowIndex].gameObject.transform.parent.gameObject;
        Destroy(rowGameObject);

        m2DropReceivers.RemoveAt(rowIndex);
        mode2Rows.RemoveAt(rowIndex);

        for (int i = rowIndex; i < m2DropReceivers.Count; i++)
        {
            m2DropReceivers[i].rowIndex = i;
        }
    }



    public void AddNewM2Row() // Zalo�� nov� prefab m2 �ady
    {
        GameObject newRow = Instantiate(m2RowPrefab, m2RowContainer.transform);
        InitializeM2Row(newRow);

        UpdateAddRowButtonPosition();
    }

    // Zavolat poka�d� co vytvo��me novou �adu pro M2
    public void InitializeM2Row(GameObject rowGameObject)   // D� m2DropReciever scriptu nov� index, Zalo�� nov� list a vypln� ho M2 tla��tky a p�id� list do listu M2 �ad
    {                                                       // Nastav� m2Button Scriptu Index a zajist� zm�nu p�i stisku tla��tka*



        var dropReceiver = rowGameObject.GetComponentInChildren<m2DropReceiver>();      // Vyt�hne dropReceiver
        int newIndex = mode2Rows.Count; // Assuming this is called right before adding the new row

        dropReceiver.rowIndex = newIndex; // Nastav� rowIndex m2DropRecieveru

        m2DropReceivers.Add(dropReceiver);  // P�id� dropReciever do listu






        List<Button> rowButtons = new List<Button>();   // Zalo�� list tla��tek pro tuto �adu

        int buttonIndex = 0;
        Transform beatPanel = rowGameObject.transform.Find("Beat Panel");

        for (int sec = 1; sec <= 4; sec++)
        {
            Transform sectionPanel = beatPanel.Find("Section" + sec);
            if (sectionPanel == null) continue;

            for (int bar = 1; bar <= 4; bar++)
            {
                Transform barPanel = sectionPanel.Find("Bar" + bar);
                if (barPanel == null) continue;

                for (int btn = 1; btn <= 4; btn++)
                {
                    Button button = barPanel.GetChild(btn - 1).GetComponent<Button>();
                    if (button != null)
                    {







                        m2Button m2ButtonScript = button.GetComponent<m2Button>();   // Vyt�hne m2Button script akt�ln�ho tla��tka a nastav� mu Index
                        m2ButtonScript.buttonIndex = buttonIndex;

                        m2Button localButtonScript = m2ButtonScript;
                        button.onClick.AddListener(() => {          // Zajist� zm�nu p�i stisku tla��tka
                            ButtonClicked(localButtonScript, dropReceiver);

                        });


                        rowButtons.Add(button); // P�id� tla��tko do listu
                        buttonIndex += 1;
                    }
                }
            }
        }



        mode2Rows.Add(rowButtons); // P�id� list tla��tek do listu M2 �ad
    }


    private void UpdateAddRowButtonPosition()
    {
        // This sets the add row button to be the last element in its parent container
        addRowButton.transform.SetSiblingIndex(m2RowContainer.transform.childCount - 1);
    }





    private void AssignSamplesToDropReceivers(HashSet<SampleData> uniqueSamples)
    {
        while (mode2Rows.Count < uniqueSamples.Count)
        {
            AddNewM2Row();
        }

        // Reset and assign new samples to each receiver
        int index = 0;


        foreach (var sample in uniqueSamples)
        {
            if (index < m2DropReceivers.Count)
            {
                m2DropReceivers[index].SetSampleData(sample);
            }
            index++;
        }
    }


















    public List<int> GetAllButtonIndexesForSample(SampleData sample) // Projede v�echny beaty, pokud beat obsahuje dan� sample, p�id� index beatu do buttonIndexes
    {
        List<int> buttonIndexes = new List<int>();


        for (int i = 0; i < 64; i++)
        {

            bool containsSample = Beats[i].Any(sd => sd.audioClip.name == sample.audioClip.name);


            if (containsSample)
            {
                buttonIndexes.Add(i);
            }
        }

        return buttonIndexes;
    }









    public m2Button GetM2ButtonScriptByIndex(int rowIndex, int buttonIndex)
    {



        if (rowIndex < mode2Rows.Count && buttonIndex < mode2Rows[rowIndex].Count)
        {
            return mode2Rows[rowIndex][buttonIndex].GetComponent<m2Button>();
        }
        return null;
    }


    #endregion



    ////////////////////////////////////////////////////////////////////////////////////








    public void ReplaceSampleDataInBeat(int buttonIndex, SampleData oldSampleData, SampleData newSampleData)
    {
        var beatSamples = Beats[buttonIndex];
        beatSamples.RemoveAll(sd => sd.audioClip == oldSampleData.audioClip);
        beatSamples.Add(newSampleData);
    }


    public void UpdateM2ButtonsSampleData(int rowIndex, SampleData newSampleData)
    {

        
        foreach (var button in mode2Rows[rowIndex])
        {
            m2Button m2ButtonScript = button.GetComponent<m2Button>();

            m2ButtonScript.ResetButtonState(newSampleData);
        }
    }










    public void LogBeatsListContents(int beatIndex)      // Vyps�n� infa o beatu
    {
        Debug.Log("");
        string beatInfo = $"Beat {beatIndex + 1} has {Beats[beatIndex].Count} Samples.";
        Debug.Log(beatInfo);
    }



    public void LogButtonClick()
    {
        Debug.Log($"m2DropReceivers.Count {m2DropReceivers.Count}");
    }
}


