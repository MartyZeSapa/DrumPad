using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;



    #region Inicializace

    private int currentTimeSignature = 4; // Defaultn� 16/4 doby


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



    private void SetM1ButtonIndexes()   // Ka�d�mu tla��tku z m1Buttons d� unik�tn� index
    {

        int buttonIndex = 0;

        foreach (Button button in m1Buttons)
        {
            M1Button m1ButtonScript = button.GetComponent<M1Button>();  //Vyt�hne m1Button script z tla��tka

            m1ButtonScript.buttonIndex = buttonIndex; // Nastav� buttonIndex v m1Button scriptu
            buttonIndex++;
        }
    }

    private void InitializeBeatsList()
    {
        for (int i = 0; i < 64; i++)
        {
            Beats.Add(new List<SampleData>());
        }
    }   // P�id� 64 SubList� Beat�


    #endregion



    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////





    public void UpdateMode1UI() // Zm�na barev kvadrant�
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
        SortedSet<SampleData> uniqueSamples = IDUniqueSamplesInBeats();   // Se�ad� v�echny uniqueSamples podle sampleIndex�

        #region uniqueSamples Debug.Log
        //Debug.Log("");
        //Debug.Log("Unique Samples:");
        //foreach (var sample in uniqueSamples)
        //{
        //    Debug.Log($"{sample.audioClip.name}");
        //}
        //Debug.Log("");
        #endregion

        AssignSamplesToDropReceivers(uniqueSamples);    // P�id� ka�d�mu receiveru jeden z uniqueSampl�
                       
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
        while (mode2Rows.Count < uniqueSamples.Count)       // Pokud je m�n� �ad ne� uniqueSampl�
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
    public void AddNewM2Row() // Zalo�� nov� prefab m2 �ady
    {
        GameObject newRow = Instantiate(m2RowPrefab, m2RowContainer.transform);
        InitializeM2Row(newRow);

        UpdateAddRowButtonPosition();


        foreach (Button button in mode2Rows[mode2Rows.Count - 1])   // Aktivuje tla��tka podle currentTimeSignature
        {
            UpdateButtonActiveState(button, currentTimeSignature);
        }
    }


    // Zavolat poka�d� co vytvo��me novou �adu pro M2
    public void InitializeM2Row(GameObject rowGameObject)   // D� m2DropReciever scriptu nov� index, Zalo�� nov� list a vypln� ho M2 tla��tky a p�id� list do listu M2 �ad
    {                                                       // Nastav� m2Button Scriptu Index a zajist� zm�nu p�i stisku tla��tka*



        var dropReceiver = rowGameObject.GetComponentInChildren<M2DropReceiver>();      // Vyt�hne dropReceiver
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







                        M2Button m2ButtonScript = button.GetComponent<M2Button>();   // Vyt�hne m2Button script akt�ln�ho tla��tka a nastav� mu Index
                        m2ButtonScript.buttonIndex = buttonIndex;

                        button.onClick.AddListener(() => {          // Zajist� zm�nu p�i stisku tla��tka
                            m2ButtonScript.OnClick(dropReceiver);

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
        if (sampleToRemove != null)         // Odtsran� Sample ze v�ech Beat�
        {
            Beats.ForEach(beatList => beatList.RemoveAll(sample => sample.audioClip == sampleToRemove.audioClip));
        }

        var rowGameObject = m2DropReceivers[rowIndex].gameObject.transform.parent.gameObject;   // Zni�� Row
        Destroy(rowGameObject);

        m2DropReceivers.RemoveAt(rowIndex);     // Odebere �adu ze v�ech List�
        mode2Rows.RemoveAt(rowIndex);

        for (int i = rowIndex; i < m2DropReceivers.Count; i++)     // Zm�n� index na spr�vn� v�em n�sleduj�c�m dropReceiver�m
        {
            m2DropReceivers[i].rowIndex = i;
        }
    }
    #endregion

    #endregion




    /////////////////////////////////////////////////////////////////////////////////////////







    #region Metody volan� z m2DropReceiveru - OnDrop() nebo ReplaceActivatedButtons()

    public void ReplaceActivatedBeats(int buttonIndex, SampleData oldSampleData, SampleData newSampleData)    // V�em vym�n� nov� sample za star� pro ka�d� stisknut� tla��tko
    {


        RemoveSampleDataFromBeat(buttonIndex, oldSampleData);
;
        AddSampleDataToBeat(buttonIndex, newSampleData);
    }


    public void ReplaceM2ButtonSampleData(int rowIndex, SampleData newSampleData)   // Updatne SampleData m2tla��tek

    {


        foreach (var button in mode2Rows[rowIndex])
        {
            M2Button m2ButtonScript = button.GetComponent<M2Button>();

            m2ButtonScript.ReplaceButtonsSampleData(newSampleData); // newSampleData, Clicked = false, defaultColor
        }
    }


    #endregion


    /////////////////////////////


    #region Metody volan� z m2DropReceiver - ReplaceActivatedButtons()
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



    #region Metody volan� z m1Button - OnDrop() nebo m2Button - OnClick()

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

    #region Metody volan� ze SignatureChanger (nebo p�i AddNewM2Row() )
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


