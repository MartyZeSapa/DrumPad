using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    #region Inicializace

    public Dictionary<string, float> sampleVolumes = new Dictionary<string, float>();

    [SerializeField] private AudioPlaybackManager audioPlaybackManager;
    private bool isPlaying = false;
    public int currentTimeSignature; //  default 16 v inspectoru


    [SerializeField] private Button playButton;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite playSprite, pauseSprite;
    [SerializeField] private Color playColor = Color.green, pauseColor = Color.yellow;



    [SerializeField] private GameObject mode1, m2RowPrefab, m2RowContainer;
    [SerializeField] private Button addRowButton;

    public Button[] m1Buttons = new Button[64];
    public List<List<Button>> mode2Rows = new List<List<Button>>();
    public List<M2DropReceiver> m2DropReceivers = new();
    public List<List<SampleData>> Beats = new List<List<SampleData>>(64);


    void Awake()
    {
        SetupSingleton();
        playButton.onClick.AddListener(TogglePlay);

        InitializeBeatsList();
        SetM1ButtonIndexes();
    }

    private void SetupSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



    private void InitializeBeatsList()
    {
        for (int i = 0; i < 64; i++)
        {
            Beats.Add(new List<SampleData>());
        }
    }



    private void SetM1ButtonIndexes()
    {

        for (int i = 0; i < m1Buttons.Length; i++)
        {
            M1Button m1ButtonScript = m1Buttons[i].GetComponent<M1Button>();
            if (m1ButtonScript != null)
            {
                m1ButtonScript.buttonIndex = i;
            }
        }
    }

    #endregion



    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////





    #region Playback
    public void TogglePlay()
    {
        isPlaying = !isPlaying; // Toggle the playing state

        if (isPlaying)
        {
            audioPlaybackManager.StartPlayback();

            playButton.GetComponent<Image>().color = pauseColor;
            iconImage.sprite = pauseSprite;
        }
        else
        {
            audioPlaybackManager.StopPlayback();

            playButton.GetComponent<Image>().color = playColor;
            iconImage.sprite = playSprite;
        }
    }




    public void SetSampleVolume(string sampleName, float volume)
    {
        if (sampleVolumes.ContainsKey(sampleName))
        {
            sampleVolumes[sampleName] = volume;
        }
        else
        {
            sampleVolumes.Add(sampleName, volume);
        }
    }

    public float GetSampleVolume(string sampleName)
    {
        if (sampleVolumes.TryGetValue(sampleName, out float volume))
        {
            return volume;
        }
        return 0.5f; // Default volume if not set
    }





    public void SetCurrentTimeSignature(int timeSignature)
    {

        currentTimeSignature = timeSignature;



        foreach (Button button in m1Buttons)
        {
            UpdateButtonActiveState(button);
        }


        foreach (var row in mode2Rows)
        {
            foreach (Button button in row)
            {
                UpdateButtonActiveState(button);
            }
        }
    }


    private void UpdateButtonActiveState(Button button)
    {
        if (button != null)
        {
            int buttonIndex = button.transform.GetSiblingIndex();
            bool shouldActivate = (currentTimeSignature == 4 && buttonIndex == 0) ||
                                  (currentTimeSignature == 8 && (buttonIndex == 0 || buttonIndex == 2)) ||
                                  (currentTimeSignature == 16);
            button.gameObject.SetActive(shouldActivate);
        }
    }

    #endregion






    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region UpdateUI
    public void ClearAllSamplesFromBeats()
    {
        Beats.ForEach(sublist => sublist.Clear());


        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateMode1UI();
        UpdateMode2UI();
    }

    public void UpdateMode1UI() // Zmìna barev kvadrantù
    {
        foreach (Button button in m1Buttons)
        {
            m1Buttons.ToList().ForEach(button => button.GetComponent<M1Button>().UpdateQuadrantAppearance());
        }
    }





    #region UpdateMode2UI


    public void UpdateMode2UI()
    {
        SortedSet<SampleData> uniqueSamples = IDUniqueSamplesInBeats();   // Seøadí všechny uniqueSamples podle sampleIndexù


        AdjustMode2RowsToMatchSamples(uniqueSamples);    // Pøidá každému receiveru jeden z uniqueSamplù

        UpdateRowsWithSamples(uniqueSamples);
    }



    #region IDUniqueSamplesInBeats(), AdjustMode2RowsToMatchSamples(),  UpdateRowsWithSamples()
    private SortedSet<SampleData> IDUniqueSamplesInBeats()
    {                     
        SortedSet<SampleData> uniqueSamples = new SortedSet<SampleData>(new SampleDataComparer());
        foreach (List<SampleData> beat in Beats)
        {
            foreach (SampleData sample in beat)
            {
                uniqueSamples.Add(sample);
            }
        }
        return uniqueSamples;
    }





    private void AdjustMode2RowsToMatchSamples(SortedSet<SampleData> uniqueSamples)
    {
        while (mode2Rows.Count < uniqueSamples.Count)       // Pokud je ménì øad než uniqueSamplù
        {
            AddNewM2Row();
        }


        // Remove excess rows if any
        while (mode2Rows.Count > uniqueSamples.Count)
        {
            RemoveRow(mode2Rows.Count - 1);
        }
    }


    private void UpdateRowsWithSamples(SortedSet<SampleData> uniqueSamples)
    {
        for (int i = 0; i < uniqueSamples.Count; i++)
        {
            m2DropReceivers[i].UpdateSamplePanel(uniqueSamples.ElementAt(i));
        }
    }

    #endregion







    #region AddNewM2Row(), InitializeM2Row(), UpdateAddRowButtonPosition(),    RemoveRow()
    public void AddNewM2Row() // Založí nový prefab m2 øady
    {
        GameObject newRow = Instantiate(m2RowPrefab, m2RowContainer.transform);
        InitializeM2Row(newRow);


        addRowButton.transform.SetAsLastSibling(); // Updatne pozici addRowButton


        foreach (Button button in mode2Rows[^1])   // Aktivuje tlaèítka podle currentTimeSignature
        {
            UpdateButtonActiveState(button);
        }
    }
    // Zavolat pokaždé co vytvoøíme novou øadu pro M2
    public void InitializeM2Row(GameObject rowGameObject)   // Dá m2DropReciever scriptu nový index, Založí nový list a vyplní ho M2 tlaèítky a pøidá list do listu M2 øad
    {                                                       // Nastaví m2Button Scriptu Index a zajistí zmìnu pøi stisku tlaèítka*



        var dropReceiver = rowGameObject.GetComponentInChildren<M2DropReceiver>();      // Vytáhne dropReceiver
        dropReceiver.rowIndex = mode2Rows.Count; // Nastaví rowIndex m2DropRecieveru

        m2DropReceivers.Add(dropReceiver);  // Pøidá dropReciever do listu






        List<Button> rowButtons = new();   // Založí list tlaèítek pro tuto øadu

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

                        button.onClick.AddListener(() =>
                        {          // Zajistí zmìnu pøi stisku tlaèítka
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

    




    public void RemoveRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= m2DropReceivers.Count) return; // Safety Check



        SampleData sampleToRemove = m2DropReceivers[rowIndex].sampleData;
        if (sampleToRemove != null)         // Odtsraní Sample ze všech Beatù
        {
            Beats.ForEach(beatList => beatList.RemoveAll(sample => sample.audioClip == sampleToRemove.audioClip));
        }

        Destroy(m2DropReceivers[rowIndex].gameObject.transform.parent.parent.gameObject);

        m2DropReceivers.RemoveAt(rowIndex);     // Odebere øadu ze všech Listù
        mode2Rows.RemoveAt(rowIndex);

        for (int i = rowIndex; i < m2DropReceivers.Count; i++)     // Zmìní index na správný všem následujícím dropReceiverùm
        {
            m2DropReceivers[i].rowIndex = i;
        }
    }

    #endregion







    #endregion

    #endregion


    //////////////////////////////////////////////////




    #region IsUniqueSamplePanel(), ReplaceSampleDataInBeat()




    public bool IsUniqueSamplePanel(AudioClip sampleClip)
    {
        foreach (var receiver in m2DropReceivers)
        {
            if (receiver.sampleData != null && receiver.sampleData.audioClip == sampleClip)
            {
                return false; // SamplePanel není unikátní
            }
        }
        return true; // SamplePanel je unikátní
    }


    public void ReplaceSampleDataInBeat(int buttonIndex, SampleData oldSampleData, SampleData newSampleData)    // vymìní nový sample za starý
    {


        RemoveSampleDataFromBeat(buttonIndex, oldSampleData);

        AddSampleDataToBeat(buttonIndex, newSampleData);
    }






    #endregion


    #region  ResetRowsButtonState(), ReplaceM2ButtonSampleData()


    public void ResetRowsButtonState(int rowIndex)
    {
        foreach (var button in mode2Rows[rowIndex])
        {
            M2Button m2ButtonScript = button.GetComponent<M2Button>();

            m2ButtonScript.ResetButtonState();
        }
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



    #region GetAllButtonIndexesForSample(), GetM2ButtonScriptByIndex()
    public List<int> GetAllButtonIndexesForSample(SampleData sample) // Projede všechny beaty, pokud beat obsahuje daný sample, pøidá index beatu do buttonIndexes
    {
        List<int> buttonIndexes = new();


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




    ////////////////////////////////////////////////




    #region AddSampleDataToBeat(), RemoveSampleDataFromBeat(), BeatContainsSample()
   
    public void AddSampleDataToBeat(int buttonIndex, SampleData sampleData)
    {
        var beatList = Beats[buttonIndex];
        beatList.Add(sampleData);


        beatList.Sort((sample1, sample2) => sample1.sampleIndex.CompareTo(sample2.sampleIndex));
    }

    public void RemoveSampleDataFromBeat(int buttonIndex, SampleData sampleData)
    {
        var beatList = Beats[buttonIndex];

        beatList.RemoveAll(sd => sd.audioClip == sampleData.audioClip);
    }




    public bool BeatContainsSample(int buttonIndex, int sampleIndex)
    {
        return Beats[buttonIndex].Any(sd => sd.sampleIndex == sampleIndex);
    }

    #endregion









}


