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



    private void InitializeM1ButtonArray()   // Projede tlaèítka z M1, pøidá každé tlaèítko do m1Buttons pole a ve scriptu tlaèítka dá unikátní index
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

                    m1Button m1ButtonScript = button.GetComponent<m1Button>();  //Vytáhne m1Button script z tlaèítka
                    if (button != null && buttonIndex < m1Buttons.Length)
                    {
                        m1Buttons[buttonIndex] = button;
                        m1ButtonScript.buttonIndex = buttonIndex; // Nastaví buttonIndex v m1Button scriptu

                        buttonIndex++;
                    }
                }
            }
        }

        if (buttonIndex != m1Buttons.Length)
        {
            Debug.LogWarning("Našlo se pouze" + buttonIndex + "tlaèítek");
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









    private void ButtonClicked(m2Button m2ButtonScript, m2DropReceiver dropReceiver)   // Pøi kliknutí na tlaèítko zmìní jeho barvu, Pøidá/Odebere SampleData do daného Beatu
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
            AddSampleDataIfUnique(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData);  // Pokud Sample v Beatu není, pøidáme ho
            dropReceiver.activatedButtons.Add(m2ButtonScript);  // Pøidá m2ButtonScript do activatedButtons



            m2ButtonScript.GetComponent<Image>().color = m2ButtonScript.m2ButtonSampleData.color;   // Zmìna barvy
            m2ButtonScript.buttonClicked = true;


            LogBeatsListContents(m2ButtonScript.buttonIndex);
        }




        else
        {
            RemoveSampleData(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData);   // Odebere všechna SampleData z Beatu co jsou stejná jako m2ButtonSampleData
            dropReceiver.activatedButtons.Remove(m2ButtonScript);   // Odebere m2ButtonScript z activatedButtons



            m2ButtonScript.GetComponent<Image>().color = m2ButtonScript.defaultColor;   // Vrátí barvu na default
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






            if (BeatsSublist.Count == 1)    // Pokud je Sample 1, zmìní barvy všech kvadrantù na barvu tohoto samplu
            {
                foreach (var quadrantImage in buttonScript.quadrantImages)
                {
                    quadrantImage.color = BeatsSublist[0].color;
                }
            }
            else if (BeatsSublist.Count == 2)   // Pokud jsou 2, zbarví 2 kvadranty podle 1. samplu, a druhé 2 podle druhého samplu
            {
                buttonScript.quadrantImages[0].color = BeatsSublist[0].color;
                buttonScript.quadrantImages[1].color = BeatsSublist[0].color;

                buttonScript.quadrantImages[2].color = BeatsSublist[1].color;
                buttonScript.quadrantImages[3].color = BeatsSublist[1].color;
            }
            else if (BeatsSublist.Count == 3)   // Pokud jsou 3, zbarví první 3 kvadranty podle tìch 3 samplù a 4. kvadrantu dá barvu 3. samplu
            {
                for (int j = 0; j < BeatsSublist.Count && j < buttonScript.quadrantImages.Count; j++)
                {
                    buttonScript.quadrantImages[j].color = BeatsSublist[j].color;
                }
                buttonScript.quadrantImages[3].color = BeatsSublist[2].color;
            }
            else            // Pro více než 3 samply, zbarví každý kvadrant podle každého samplu
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



    public void AddNewM2Row() // Založí nový prefab m2 øady
    {
        GameObject newRow = Instantiate(m2RowPrefab, m2RowContainer.transform);
        InitializeM2Row(newRow);

        UpdateAddRowButtonPosition();
    }

    // Zavolat pokaždé co vytvoøíme novou øadu pro M2
    public void InitializeM2Row(GameObject rowGameObject)   // Dá m2DropReciever scriptu nový index, Založí nový list a vyplní ho M2 tlaèítky a pøidá list do listu M2 øad
    {                                                       // Nastaví m2Button Scriptu Index a zajistí zmìnu pøi stisku tlaèítka*



        var dropReceiver = rowGameObject.GetComponentInChildren<m2DropReceiver>();      // Vytáhne dropReceiver
        int newIndex = mode2Rows.Count; // Assuming this is called right before adding the new row

        dropReceiver.rowIndex = newIndex; // Nastaví rowIndex m2DropRecieveru

        m2DropReceivers.Add(dropReceiver);  // Pøidá dropReciever do listu






        List<Button> rowButtons = new List<Button>();   // Založí list tlaèítek pro tuto øadu

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







                        m2Button m2ButtonScript = button.GetComponent<m2Button>();   // Vytáhne m2Button script aktálního tlaèítka a nastaví mu Index
                        m2ButtonScript.buttonIndex = buttonIndex;

                        m2Button localButtonScript = m2ButtonScript;
                        button.onClick.AddListener(() => {          // Zajistí zmìnu pøi stisku tlaèítka
                            ButtonClicked(localButtonScript, dropReceiver);

                        });


                        rowButtons.Add(button); // Pøidá tlaèítko do listu
                        buttonIndex += 1;
                    }
                }
            }
        }



        mode2Rows.Add(rowButtons); // Pøidá list tlaèítek do listu M2 øad
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


















    public List<int> GetAllButtonIndexesForSample(SampleData sample) // Projede všechny beaty, pokud beat obsahuje daný sample, pøidá index beatu do buttonIndexes
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










    public void LogBeatsListContents(int beatIndex)      // Vypsání infa o beatu
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


