using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject mode1;

    [SerializeField] 
    private GameObject defaultMode2Row;

    [SerializeField]
    private GameObject m2RowPrefab;

    [SerializeField]
    private GameObject m2RowContainer;




    public static GameManager Instance;



    public Button[] m1Buttons = new Button[64];


    public List<List<Button>> mode2Rows;    // Inicializuje list M2 �ad s tla��tky

    public List<m2DropReceiver> m2DropReceivers = new List<m2DropReceiver>(); // Inicializuje pole instanc� m2DropRecieveru



    public List<List<SampleData>> Beats = new List<List<SampleData>>();     


    void Awake()    // Game Manager Singleton,     Inicializace: SubList� Listu Beat�, Pole M1 tla��tek, List List� (M2�ady a jejich tla��tka)
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }          // O�et�en� aby v�dy byla jen 1 Instance gameManageru
        else
        {
            // Toto je jedin� instance gameManageru, p�i�a��me do Instance prom�nn�
            Instance = this;
        }




        InitializeBeatsList();

        InitializeM1ButtonArray();

        mode2Rows = new List<List<Button>>(); // Zalo�� list pro M2 �ady
    }


    private void InitializeBeatsList()  // P�id� 64 Sublist� pro SampleData do Listu beatu
    {
        for (int i = 0; i < 64; i++)
        {
            Beats.Add(new List<SampleData>());
        }
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



    // Zavolat poka�d� co vytvo��me novou �adu pro M2
    public void InitializeM2Row(GameObject rowGameObject)   // D� m2DropReciever scriptu nov� index, Zalo�� nov� list a vypln� ho M2 tla��tky a p�id� list do listu M2 �ad
    {                                                       // Nastav� m2Button Scriptu Index a zajist� zm�nu p�i stisku tla��tka*



        Transform samplePanelTransform = rowGameObject.transform.Find("Sample Panel");      // Vyt�hne dropReceiver
        m2DropReceiver dropReceiver = samplePanelTransform.GetComponent<m2DropReceiver>();  

        dropReceiver.rowIndex = mode2Rows.Count; // Nastav� rowIndex m2DropRecieveru

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
                        button.onClick.AddListener(() => {          // Zajist� zm�nu p�i stisku tla��tka
                            if (m2ButtonScript.m2ButtonSampleData != null)
                            {
                                ButtonClicked(m2ButtonScript, dropReceiver);
                            }
                            
                        });
   

                        rowButtons.Add(button); // P�id� tla��tko do listu
                        buttonIndex += 1;
                    }
                }
            }
        }




        if (rowButtons.Count != 64)
        {
            Debug.LogWarning("Na�lo se pouze " + rowButtons.Count + " tla��tek.");
        }
        else
        {
            Debug.Log($"Successfully initialized 64 buttons for {rowGameObject}");

            mode2Rows.Add(rowButtons); // P�id� list tla��tek do listu M2 �ad
        }
    }





    ////////////////////////////////////////////////////////////////////////////////////




    private void ButtonClicked(m2Button m2ButtonScript, m2DropReceiver dropReceiver)   // P�i kliknut� na tla��tko zm�n� jeho barvu, P�id�/Odebere SampleData do dan�ho Beatu
    {



        List<SampleData> currentBeatSamples = Beats[m2ButtonScript.buttonIndex];






        if (!m2ButtonScript.buttonClicked)
        {
            AddSampleDataIfUnique(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData);  // Pokud Sample v Beatu nen�, p�id�me ho
            dropReceiver.activatedButtons.Add(m2ButtonScript);  // P�id� m2ButtonScript do activatedButtons

            m2ButtonScript.GetComponent<Image>().color = m2ButtonScript.m2ButtonSampleData.color;   // Zm�na barvy


            m2ButtonScript.buttonClicked = true;
            LogBeatsListContents();
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





    public void UpdateMode1UI()
    {
        for (int i = 0; i < 64; i++)
        {
            var BeatsSublist = Beats[i];
            var buttonScript = m1Buttons[i].GetComponent<m1Button>();

            // Clear previous visuals
            buttonScript.ClearQuadrantColors();

            // Update quadrants based on samples
            for (int j = 0; j < BeatsSublist.Count && j < buttonScript.quadrantImages.Count; j++)
            {
                buttonScript.quadrantImages[j].color = BeatsSublist[j].color;
            }
        }
    }










   
    public void UpdateMode2UI()
    {
        // Identifikuje uniqueSamply ve v�ech Beatech
        HashSet<SampleData> uniqueSamples = new HashSet<SampleData>(new SampleDataComparer());
        foreach (var beat in Beats)
        {
            foreach (var sample in beat)
            {
                uniqueSamples.Add(sample);
            }
        }


        // Zajist� stejn� po�et �ad jako uniqueSampl�
        AdjustMode2RowsToMatchSamples(uniqueSamples);


        


        for (int rowIndex = 0; rowIndex < mode2Rows.Count; rowIndex++)
        {

            SampleData rowSample = uniqueSamples.ElementAt(rowIndex);


            // Z�sk� indexy v�ech tla��tek na kter�ch je dan� sample
            List<int> buttonIndexesForSample = GetAllButtonIndexesForSample(rowSample);





            // Update the UI elements for this row based on rowSample
            m2DropReceivers[rowIndex].UpdateSamplePanelUI(rowSample, buttonIndexesForSample);
        }
    }

    class SampleDataComparer : IEqualityComparer<SampleData>
    {
        public bool Equals(SampleData x, SampleData y)
        {
            // Porovn� 2 SampleData podle jm�na
            return x.audioClip.name == y.audioClip.name;
        }

        public int GetHashCode(SampleData obj)
        {
            // Provide a hash code based on the same unique identifier used in Equals
            return obj.audioClip.name.GetHashCode();
        }
    }


    private void AdjustMode2RowsToMatchSamples(HashSet<SampleData> uniqueSamples)   // P�i p�epnut� do mode2, zajist� stejn� po�et �ad jako uniqueSampl�
    {
        while (mode2Rows.Count > uniqueSamples.Count)   // Dokud je v�c �ad ne� uniqueSampl�, ub�r� je
        {
            var lastRowIndex = mode2Rows.Count - 1;
            var rowButtons = mode2Rows[lastRowIndex];


            if (rowButtons.Count > 0)
            {
                var rowGameObject = rowButtons[0].transform.parent.parent.parent.parent.gameObject;
                Destroy(rowGameObject);
            }

            mode2Rows.RemoveAt(lastRowIndex);
            m2DropReceivers.RemoveAt(lastRowIndex);
        }

        // P�id�v� �ady dokud jich nen� stejn� jako uniqueSampl�
        while (mode2Rows.Count < uniqueSamples.Count)
        {
            AddNewM2Row();
        }
    }


    public void AddNewM2Row() // Zalo�� nov� prefab m2 �ady
    {
        GameObject newRow = Instantiate(m2RowPrefab, m2RowContainer.transform);
        InitializeM2Row(newRow);
    }









    private List<int> GetAllButtonIndexesForSample(SampleData sample) // Projede v�echny beaty, pokud beat obsahuje dan� sample, p�id� index beatu do buttonIndexes
    {
        List<int> buttonIndexes = new List<int>();


        for (int i = 0; i < Beats.Count; i++)
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

        // Get the row buttons
        List<Button> rowButtons = mode2Rows[rowIndex];

        // Get the m2Button script from the button at buttonIndex
        m2Button buttonScript = rowButtons[buttonIndex].GetComponent<m2Button>();

        return buttonScript;
    }






    ////////////////////////////////////////////////////////////////////////////////////




    public void ReplaceSampleDataInBeat(int buttonIndex, SampleData oldSampleData, SampleData newSampleData)    // (M2 OnDrop) - Odstran� star� a p�id� nov� SampleData do Sublist� Beat�
    {
        if (Beats.Count > buttonIndex && Beats[buttonIndex] != null)
        {

            Beats[buttonIndex].RemoveAll(sd => sd.audioClip == oldSampleData.audioClip);
            Beats[buttonIndex].Add(newSampleData);
        }
    }


    public void UpdateM2ButtonsSampleData(int rowIndex, SampleData newSampleData)   // (M2 OnDrop) - Updatne SampleData v�ech tla��tek
    {
        foreach (var button in mode2Rows[rowIndex])
        {
            m2Button m2ButtonScript = button.GetComponent<m2Button>();

            m2ButtonScript.m2ButtonSampleData = newSampleData;
        }
    }       







    public void LogBeatsListContents()      // Vyps�n� infa o beatech
    {
        Debug.Log("");
        for (int i = 0; i < Beats.Count; i++)
        {
            if (Beats[i].Count > 0)
            {
               
                string beatInfo = $"Beat {i + 1} has {Beats[i].Count} Samples.";
                Debug.Log(beatInfo);
            }
            
        }
    }   

}
