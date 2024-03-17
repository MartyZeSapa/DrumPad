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


    public List<List<Button>> mode2Rows;    // Inicializuje list M2 øad s tlaèítky

    public List<m2DropReceiver> m2DropReceivers = new List<m2DropReceiver>(); // Inicializuje pole instancí m2DropRecieveru



    public List<List<SampleData>> Beats = new List<List<SampleData>>();     


    void Awake()    // Game Manager Singleton,     Inicializace: SubListù Listu Beatù, Pole M1 tlaèítek, List Listù (M2øady a jejich tlaèítka)
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }          // Ošetøení aby vždy byla jen 1 Instance gameManageru
        else
        {
            // Toto je jediná instance gameManageru, pøiøaïíme do Instance promìnné
            Instance = this;
        }




        InitializeBeatsList();

        InitializeM1ButtonArray();

        mode2Rows = new List<List<Button>>(); // Založí list pro M2 øady
    }


    private void InitializeBeatsList()  // Pøidá 64 Sublistù pro SampleData do Listu beatu
    {
        for (int i = 0; i < 64; i++)
        {
            Beats.Add(new List<SampleData>());
        }
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



    // Zavolat pokaždé co vytvoøíme novou øadu pro M2
    public void InitializeM2Row(GameObject rowGameObject)   // Dá m2DropReciever scriptu nový index, Založí nový list a vyplní ho M2 tlaèítky a pøidá list do listu M2 øad
    {                                                       // Nastaví m2Button Scriptu Index a zajistí zmìnu pøi stisku tlaèítka*



        Transform samplePanelTransform = rowGameObject.transform.Find("Sample Panel");      // Vytáhne dropReceiver
        m2DropReceiver dropReceiver = samplePanelTransform.GetComponent<m2DropReceiver>();  

        dropReceiver.rowIndex = mode2Rows.Count; // Nastaví rowIndex m2DropRecieveru

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
                        button.onClick.AddListener(() => {          // Zajistí zmìnu pøi stisku tlaèítka
                            if (m2ButtonScript.m2ButtonSampleData != null)
                            {
                                ButtonClicked(m2ButtonScript, dropReceiver);
                            }
                            
                        });
   

                        rowButtons.Add(button); // Pøidá tlaèítko do listu
                        buttonIndex += 1;
                    }
                }
            }
        }




        if (rowButtons.Count != 64)
        {
            Debug.LogWarning("Našlo se pouze " + rowButtons.Count + " tlaèítek.");
        }
        else
        {
            Debug.Log($"Successfully initialized 64 buttons for {rowGameObject}");

            mode2Rows.Add(rowButtons); // Pøidá list tlaèítek do listu M2 øad
        }
    }





    ////////////////////////////////////////////////////////////////////////////////////




    private void ButtonClicked(m2Button m2ButtonScript, m2DropReceiver dropReceiver)   // Pøi kliknutí na tlaèítko zmìní jeho barvu, Pøidá/Odebere SampleData do daného Beatu
    {



        List<SampleData> currentBeatSamples = Beats[m2ButtonScript.buttonIndex];






        if (!m2ButtonScript.buttonClicked)
        {
            AddSampleDataIfUnique(m2ButtonScript.buttonIndex, m2ButtonScript.m2ButtonSampleData);  // Pokud Sample v Beatu není, pøidáme ho
            dropReceiver.activatedButtons.Add(m2ButtonScript);  // Pøidá m2ButtonScript do activatedButtons

            m2ButtonScript.GetComponent<Image>().color = m2ButtonScript.m2ButtonSampleData.color;   // Zmìna barvy


            m2ButtonScript.buttonClicked = true;
            LogBeatsListContents();
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
        // Identifikuje uniqueSamply ve všech Beatech
        HashSet<SampleData> uniqueSamples = new HashSet<SampleData>(new SampleDataComparer());
        foreach (var beat in Beats)
        {
            foreach (var sample in beat)
            {
                uniqueSamples.Add(sample);
            }
        }


        // Zajistí stejný poèet øad jako uniqueSamplù
        AdjustMode2RowsToMatchSamples(uniqueSamples);


        


        for (int rowIndex = 0; rowIndex < mode2Rows.Count; rowIndex++)
        {

            SampleData rowSample = uniqueSamples.ElementAt(rowIndex);


            // Získá indexy všech tlaèítek na kterých je daný sample
            List<int> buttonIndexesForSample = GetAllButtonIndexesForSample(rowSample);





            // Update the UI elements for this row based on rowSample
            m2DropReceivers[rowIndex].UpdateSamplePanelUI(rowSample, buttonIndexesForSample);
        }
    }

    class SampleDataComparer : IEqualityComparer<SampleData>
    {
        public bool Equals(SampleData x, SampleData y)
        {
            // Porovná 2 SampleData podle jména
            return x.audioClip.name == y.audioClip.name;
        }

        public int GetHashCode(SampleData obj)
        {
            // Provide a hash code based on the same unique identifier used in Equals
            return obj.audioClip.name.GetHashCode();
        }
    }


    private void AdjustMode2RowsToMatchSamples(HashSet<SampleData> uniqueSamples)   // Pøi pøepnutí do mode2, zajistí stejný poèet øad jako uniqueSamplù
    {
        while (mode2Rows.Count > uniqueSamples.Count)   // Dokud je víc øad než uniqueSamplù, ubírá je
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

        // Pøidává øady dokud jich není stejnì jako uniqueSamplù
        while (mode2Rows.Count < uniqueSamples.Count)
        {
            AddNewM2Row();
        }
    }


    public void AddNewM2Row() // Založí nový prefab m2 øady
    {
        GameObject newRow = Instantiate(m2RowPrefab, m2RowContainer.transform);
        InitializeM2Row(newRow);
    }









    private List<int> GetAllButtonIndexesForSample(SampleData sample) // Projede všechny beaty, pokud beat obsahuje daný sample, pøidá index beatu do buttonIndexes
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




    public void ReplaceSampleDataInBeat(int buttonIndex, SampleData oldSampleData, SampleData newSampleData)    // (M2 OnDrop) - Odstraní stará a pøidá nová SampleData do Sublistù Beatù
    {
        if (Beats.Count > buttonIndex && Beats[buttonIndex] != null)
        {

            Beats[buttonIndex].RemoveAll(sd => sd.audioClip == oldSampleData.audioClip);
            Beats[buttonIndex].Add(newSampleData);
        }
    }


    public void UpdateM2ButtonsSampleData(int rowIndex, SampleData newSampleData)   // (M2 OnDrop) - Updatne SampleData všech tlaèítek
    {
        foreach (var button in mode2Rows[rowIndex])
        {
            m2Button m2ButtonScript = button.GetComponent<m2Button>();

            m2ButtonScript.m2ButtonSampleData = newSampleData;
        }
    }       







    public void LogBeatsListContents()      // Vypsání infa o beatech
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
