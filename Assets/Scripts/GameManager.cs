using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;



    #region Inicializace

    private int currentTimeSignature = 4; // Defaultní 16/4 doby


    [SerializeField] private GameObject mode1;

    [SerializeField] private GameObject m2RowPrefab;
    [SerializeField] private GameObject m2RowContainer;
    [SerializeField] private Button addRowButton;


    [SerializeField] private Button logButton;

    


    public Button[] m1Buttons = new Button[64];

    public List<List<Button>> mode2Rows = new List<List<Button>>();
    public List<M2DropReceiver> m2DropReceivers = new List<M2DropReceiver>();

    public List<List<SampleData>> Beats = new List<List<SampleData>>();


    void Awake()
    {
        if (Instance != null && Instance != this)   // GameManager singleton
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


        SetM1ButtonIndexes();
        InitializeBeatsList();
        mode2Rows = new List<List<Button>>();


        logButton.onClick.AddListener(LogButtonClick);
    }

    public void LogButtonClick()
    {
        Debug.Log($"m2DropReceivers.Count {m2DropReceivers.Count}");
    }



    private void SetM1ButtonIndexes()   // Každému tlaèítku z m1Buttons dá unikátní index
    {

        int buttonIndex = 0;

        foreach (Button button in m1Buttons)
        {
            M1Button m1ButtonScript = button.GetComponent<M1Button>();  //Vytáhne m1Button script z tlaèítka

            m1ButtonScript.buttonIndex = buttonIndex; // Nastaví buttonIndex v m1Button scriptu
            buttonIndex++;
        }
    }

    private void InitializeBeatsList()
    {
        for (int i = 0; i < 64; i++)
        {
            Beats.Add(new List<SampleData>());
        }
    }   // Pøidá 64 SubListù Beatù


    #endregion



    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////





    public void UpdateMode1UI() // Zmìna barev kvadrantù
    {
        for (int i = 0; i < 64; i++)
        {
            var buttonScript = m1Buttons[i].GetComponent<M1Button>();

            buttonScript.UpdateQuadrantAppearance();
        }
    }





    #region UpdateMode2UI


    public void UpdateMode2UI()
    {
        SortedSet<SampleData> uniqueSamples = IDUniqueSamplesInBeats();   // Seøadí všechny uniqueSamples podle sampleIndexù

        #region uniqueSamples Debug.Log
        //Debug.Log("");
        //Debug.Log("Unique Samples:");
        //foreach (var sample in uniqueSamples)
        //{
        //    Debug.Log($"{sample.audioClip.name}");
        //}
        //Debug.Log("");
        #endregion

        AssignSamplesToDropReceivers(uniqueSamples);    // Pøidá každému receiveru jeden z uniqueSamplù
                       
        RemoveUnusedSampleRows(uniqueSamples);  
    }

    private SortedSet<SampleData> IDUniqueSamplesInBeats()
    {
        SortedSet<SampleData> uniqueSamples = new SortedSet<SampleData>(new SampleDataComparer());
        foreach (var beat in Beats)
        {
            foreach (var sample in beat)
            {
                uniqueSamples.Add(sample);
            }
        }
        return uniqueSamples;
    }





    private void AssignSamplesToDropReceivers(SortedSet<SampleData> uniqueSamples)
    {
        while (mode2Rows.Count < uniqueSamples.Count)       // Pokud je ménì øad než uniqueSamplù
        {
            AddNewM2Row();
        }

        

        int index = 0;

        foreach (var sample in uniqueSamples)
        {
            if (index < m2DropReceivers.Count)
            {
                m2DropReceivers[index].UpdateSamplePanel(sample);
            }
            index++;
        }
    }
    
    #region AddNewM2Row(), InitializeM2Row(), UpdateAddRowButtonPosition()
    public void AddNewM2Row() // Založí nový prefab m2 øady
    {
        GameObject newRow = Instantiate(m2RowPrefab, m2RowContainer.transform);
        InitializeM2Row(newRow);

        UpdateAddRowButtonPosition();


        foreach (Button button in mode2Rows[mode2Rows.Count - 1])   // Aktivuje tlaèítka podle currentTimeSignature
        {
            UpdateButtonActiveState(button, currentTimeSignature);
        }
    }


    // Zavolat pokaždé co vytvoøíme novou øadu pro M2
    public void InitializeM2Row(GameObject rowGameObject)   // Dá m2DropReciever scriptu nový index, Založí nový list a vyplní ho M2 tlaèítky a pøidá list do listu M2 øad
    {                                                       // Nastaví m2Button Scriptu Index a zajistí zmìnu pøi stisku tlaèítka*



        var dropReceiver = rowGameObject.GetComponentInChildren<M2DropReceiver>();      // Vytáhne dropReceiver
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







                        M2Button m2ButtonScript = button.GetComponent<M2Button>();   // Vytáhne m2Button script aktálního tlaèítka a nastaví mu Index
                        m2ButtonScript.buttonIndex = buttonIndex;

                        button.onClick.AddListener(() => {          // Zajistí zmìnu pøi stisku tlaèítka
                            m2ButtonScript.OnClick(dropReceiver);

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

    #endregion

    


    #region RemoveUnusedSampleRows(), RemoveRow()
    private void RemoveUnusedSampleRows(SortedSet<SampleData> uniqueSamples)
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
        if (rowIndex < 0 || rowIndex >= m2DropReceivers.Count) return; // Safety Check



        SampleData sampleToRemove = m2DropReceivers[rowIndex].sampleData;
        if (sampleToRemove != null)         // Odtsraní Sample ze všech Beatù
        {
            Beats.ForEach(beatList => beatList.RemoveAll(sample => sample.audioClip == sampleToRemove.audioClip));
        }

        var rowGameObject = m2DropReceivers[rowIndex].gameObject.transform.parent.gameObject;   // Znièí Row
        Destroy(rowGameObject);

        m2DropReceivers.RemoveAt(rowIndex);     // Odebere øadu ze všech Listù
        mode2Rows.RemoveAt(rowIndex);

        for (int i = rowIndex; i < m2DropReceivers.Count; i++)     // Zmìní index na správný všem následujícím dropReceiverùm
        {
            m2DropReceivers[i].rowIndex = i;
        }
    }
    #endregion

    #endregion




    /////////////////////////////////////////////////////////////////////////////////////////







    #region Metody volané z m2DropReceiveru - OnDrop() nebo ReplaceActivatedButtons()

    public void ReplaceActivatedBeats(int buttonIndex, SampleData oldSampleData, SampleData newSampleData)    // Všem vymìní nový sample za starý pro každé stisknuté tlaèítko
    {


        RemoveSampleDataFromBeat(buttonIndex, oldSampleData);
;
        AddSampleDataToBeat(buttonIndex, newSampleData);
    }


    public void ReplaceM2ButtonSampleData(int rowIndex, SampleData newSampleData)   // Updatne SampleData m2tlaèítek

    {


        foreach (var button in mode2Rows[rowIndex])
        {
            M2Button m2ButtonScript = button.GetComponent<M2Button>();

            m2ButtonScript.ReplaceButtonsSampleData(newSampleData); // newSampleData, Clicked = false, defaultColor
        }
    }


    #endregion


    /////////////////////////////


    #region Metody volané z m2DropReceiver - ReplaceActivatedButtons()
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


    public M2Button GetM2ButtonScriptByIndex(int rowIndex, int buttonIndex)
    {



        if (rowIndex < mode2Rows.Count && buttonIndex < mode2Rows[rowIndex].Count)
        {
            return mode2Rows[rowIndex][buttonIndex].GetComponent<M2Button>();
        }
        return null;
    }


    #endregion







    ////////////////////////////////////////////////////////////////////



    #region Metody volané z m1Button - OnDrop() nebo m2Button - OnClick()

    public void AddSampleDataToBeat(int buttonIndex, SampleData sampleData)
    {
        var beatList = Beats[buttonIndex];
        beatList.Add(sampleData);


        // Sort the beat list by sampleIndex before updating the appearance
        beatList.Sort((sample1, sample2) => sample1.sampleIndex.CompareTo(sample2.sampleIndex));
    }

    public void RemoveSampleDataFromBeat(int buttonIndex, SampleData sampleData)
    {
        var beatList = Beats[buttonIndex];

        beatList.RemoveAll(sd => sd.audioClip == sampleData.audioClip);
    }


    #endregion












    ////////////////////////////////////////////////////////////////////

    #region Metody volané ze SignatureChanger (nebo pøi AddNewM2Row() )
    public void SetCurrentTimeSignature(int timeSignature)
    {
        currentTimeSignature = timeSignature;

        foreach (Button button in m1Buttons)
        {
            UpdateButtonActiveState(button, currentTimeSignature);
        }


        foreach (var row in mode2Rows)
        {
            foreach (Button button in row)
            {
                UpdateButtonActiveState(button, currentTimeSignature);
            }
        }
    }

    private void UpdateButtonActiveState(Button button, int timeSignature)
    {
        if (button != null)
        {
            int buttonIndex = button.transform.GetSiblingIndex();
            bool shouldActivate = (timeSignature == 1 && buttonIndex == 0) ||
                                  (timeSignature == 2 && (buttonIndex == 0 || buttonIndex == 2)) ||
                                  (timeSignature == 4);
            button.gameObject.SetActive(shouldActivate);
        }
    }

    #endregion




    ////////////////////////////////////////////////////////////////////

    public void ClearAllSamplesFromBeats()
    {
        foreach (var beatSublist in Beats)
        {
            beatSublist.Clear();
        }


        UpdateMode1UI();
        UpdateMode2UI();
    }
}


