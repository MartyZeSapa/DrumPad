using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool[] playableBeats;  // Cache for playable beats

    #region Playback Inicializace

    [SerializeField] private AudioPlaybackManager audioPlaybackManager;

    public Dictionary<string, float> sampleVolumes = new Dictionary<string, float>();

    private bool isPlaying = false;
    public int currentTimeSignature; //  default 16 v inspectoru
    public int currentBpm;


    [SerializeField] private Image[] m1RowBorders = new Image[4];

    public int lastHighlightedBeatIndex = -1;
    public int lastHighlightedSectionIndex = -1;

    public int currentSectionIndex;
    public int currentBeatIndex;


    [SerializeField] private Button playButton;
    [SerializeField] private Image playButtonImage;
    [SerializeField] private Image iconImage;

    [SerializeField] private Sprite playSprite, pauseSprite;
    [SerializeField] private Color playColor = Color.green, pauseColor = Color.yellow;

    #endregion

    #region Inicializace

    public List<SampleData>[] Beats = new List<SampleData>[64];

    [SerializeField] public GameObject mode1, mode2;
    [SerializeField] private GameObject m2RowPrefab, m2RowContainer;
    [SerializeField] private Button addRowButton;

    public Button[] m1Buttons = new Button[64];
    public M1Button[] m1ButtonScripts = new M1Button[64];


    public List<Button[]> m2RowsButtons = new List<Button[]>();
    public List<M2Button[]> m2RowsButtonScripts = new List<M2Button[]>();

    public List<M2RowHandler> m2RowHandlers = new();





    void Awake()
    {
        SetupSingleton();

        InitializeBeatsList();
        SetM1ButtonIndexes();

        playButton.onClick.AddListener(TogglePlay);
        ClearAllSectionHighlights();

        playableBeats = new bool[64];  // Assuming 64 beats as the maximum
        SetCurrentTimeSignature(16);  // Default time signature setup
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
        for (int i = 0; i < Beats.Length; i++)
        {
            Beats[i] = new List<SampleData>();
        }
    }

    private void SetM1ButtonIndexes()
    {

        for (int i = 0; i < 64; i++)
        {
            M1Button m1ButtonScript = m1ButtonScripts[i];
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

            playButtonImage.color = pauseColor;
            iconImage.sprite = pauseSprite;
        }
        else
        {
            audioPlaybackManager.StopPlayback();

            playButtonImage.color = playColor;
            iconImage.sprite = playSprite;
        }
    }





    #region SetSampleVolume(), GetSampleVolume()
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

    #endregion


    #region Highlighting


    public void HighlightBeat(int beatIndex)
    {
        if (beatIndex < 0 || beatIndex >= 64) return;

        currentBeatIndex = beatIndex;
        currentSectionIndex = currentBeatIndex / 16;

        if (mode2.activeInHierarchy)
        {
            HighlightMode2();
        }
        else if (mode1.activeInHierarchy)
        {
            HighlightMode1();
        }

        lastHighlightedBeatIndex = currentBeatIndex;
    }

    private void HighlightMode2()
    {
        if (currentSectionIndex != lastHighlightedSectionIndex)
        {
            foreach (var rowHandler in m2RowHandlers)
            {
                rowHandler.HighlightSection(currentSectionIndex);
            }
            lastHighlightedSectionIndex = currentSectionIndex;
        }

        foreach (var rowButtons in m2RowsButtonScripts)
        {
            if (lastHighlightedBeatIndex != -1)
            {
                rowButtons[lastHighlightedBeatIndex]?.Unhighlight();
            }
            rowButtons[currentBeatIndex]?.Highlight();
        }
    }

    private void HighlightMode1()
    {
        if (currentSectionIndex != lastHighlightedSectionIndex)
        {
            UnhighlightAllM1Sections();
            HighlightM1Section(currentSectionIndex);
            lastHighlightedSectionIndex = currentSectionIndex;
        }

        if (lastHighlightedBeatIndex != -1)
        {
            m1ButtonScripts[lastHighlightedBeatIndex]?.Unhighlight();
        }
        m1ButtonScripts[currentBeatIndex]?.Highlight();
    }




    public void HighlightM1Section(int sectionIndex)
    {

        m1RowBorders[sectionIndex].enabled = true;
    }



    #region Clear Highlights

    public void UnhighlightAllM1Sections()
    {
        foreach (var rowBorder in m1RowBorders)
        {
            rowBorder.enabled = false;
        }
    }

    public void UnhighlightAll()
    {
        #region Unhighlight all buttons

        foreach (var rowButtons in m2RowsButtonScripts)
        {
            rowButtons[lastHighlightedBeatIndex]?.Unhighlight();
        }
        m1ButtonScripts[lastHighlightedBeatIndex]?.Unhighlight();

        #endregion

        ClearAllSectionHighlights();

        lastHighlightedSectionIndex = -1;
        lastHighlightedBeatIndex = -1; // Reset the last highlighted index after unhighlighting
    }
    public void ClearAllSectionHighlights()
    {
        UnhighlightAllM1Sections();

        
        foreach (M2RowHandler rowHandler in m2RowHandlers)
        {
            ClearM2SectionHighlights(rowHandler);
            
        }

    }

    public void ClearM2SectionHighlights(M2RowHandler rowHandler)
    {
        foreach (Image sectionBorder in rowHandler.m2SectionBorders)
        {
            sectionBorder.enabled = false;
        }
    }

    #endregion


    #endregion




    #region SetCurrentTimeSignature(), UpdateButtonActiveState()
    public void SetCurrentTimeSignature(int timeSignature)
    {
        currentTimeSignature = timeSignature;
        UpdatePlayableBeatsCache();

        foreach (var button in m1Buttons)
        {
            UpdateButtonActiveState(button);
        }

        foreach (var row in m2RowsButtons)
        {
            foreach (var button in row)
            {
                UpdateButtonActiveState(button);
            }
        }
    }



    public void UpdatePlayableBeatsCache()
    {
        for (int i = 0; i < 64; i++)
        {
            playableBeats[i] = ShouldPlayBeat(i);
        }
    }

    public bool ShouldPlayBeat(int beatIndex)
    {
        switch (currentTimeSignature)
        {
            case 16: return true;
            case 8: return beatIndex % 2 == 0;
            case 4: return beatIndex % 4 == 0;
            default: return false;
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

    #endregion




    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    #region UpdateUI
    public void ClearAllSamplesFromBeats()
    {
        foreach (var beatList in Beats)
        {
            beatList.Clear();
        }


        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateMode1UI();
        UpdateMode2UI();
    }




    public void UpdateMode1UI()
    {
        foreach (M1Button m1ButtonScript in m1ButtonScripts)
        {
            m1ButtonScript.UpdateQuadrantAppearance();
        }
    }




    #region UpdateMode2UI


    public void UpdateMode2UI()
    {
        SortedSet<SampleData> uniqueSamples = IDUniqueSamplesInBeats();   // Seøadí všechny uniqueSamples podle sampleIndexù


        AdjustMode2RowsToMatchSamples(uniqueSamples);

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
        while (m2RowsButtons.Count < uniqueSamples.Count)
        {
            AddNewM2Row();
        }


        while (m2RowsButtons.Count > uniqueSamples.Count)
        {
            RemoveRow(m2RowsButtons.Count - 1);
        }
    }


    private void UpdateRowsWithSamples(SortedSet<SampleData> uniqueSamples)
    {
        for (int i = 0; i < uniqueSamples.Count; i++)
        {
            m2RowHandlers[i].UpdateSamplePanel(uniqueSamples.ElementAt(i));
        }
    }

    #endregion





    #region AddNewM2Row(), InitializeM2Row(), UpdateAddRowButtonPosition(),    RemoveRow()
    public void AddNewM2Row()
    {
        GameObject newRow = Instantiate(m2RowPrefab, m2RowContainer.transform);

        var rowHandler = newRow.GetComponentInChildren<M2RowHandler>();
        ClearM2SectionHighlights(rowHandler);

        if (audioPlaybackManager.isPlaying == true)
        {
            rowHandler.m2SectionBorders[currentSectionIndex].enabled = true;
        }

        InitializeM2Row(newRow, rowHandler);


        


        addRowButton.transform.SetAsLastSibling();


        foreach (Button button in m2RowsButtons[^1])
        {
            UpdateButtonActiveState(button);
        }
    }

    public void InitializeM2Row(GameObject rowGameObject, M2RowHandler rowHandler)
    {

        
        rowHandler.rowIndex = m2RowsButtons.Count;
        m2RowHandlers.Add(rowHandler);

       


        Button[] rowButtons = rowHandler.Buttons;
        M2Button[] rowButtonScripts = rowHandler.ButtonScripts;


        for (int buttonIndex = 0; buttonIndex < 64; buttonIndex++)
        {
            M2Button m2ButtonScript = rowButtonScripts[buttonIndex];
            m2ButtonScript.buttonIndex = buttonIndex;

            var button = rowButtons[buttonIndex];
            button.onClick.AddListener(() =>
            {
                m2ButtonScript.OnClick(rowHandler);

            });

        }


        m2RowsButtons.Add(rowButtons);
        m2RowsButtonScripts.Add(rowButtonScripts);
    }






    public void RemoveRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= m2RowHandlers.Count)
            return;



        SampleData sampleToRemove = m2RowHandlers[rowIndex].sampleData;
        if (sampleToRemove != null)
        {
            foreach (var beatList in Beats)
            {
                beatList.RemoveAll(sample => sample.audioClip == sampleToRemove.audioClip);
            }
        }

        Destroy(m2RowHandlers[rowIndex].gameObject.transform.parent.parent.gameObject);

        m2RowHandlers.RemoveAt(rowIndex);
        m2RowsButtons.RemoveAt(rowIndex);
        m2RowsButtonScripts.RemoveAt(rowIndex);

        for (int i = rowIndex; i < m2RowHandlers.Count; i++)
        {
            m2RowHandlers[i].rowIndex = i;
        }
    }

    #endregion



    #endregion

    #endregion


    //////////////////////////////////////


    #region IsUniqueSamplePanel(), ReplaceSampleDataInBeat()


    public bool IsUniqueSamplePanel(AudioClip sampleClip)
    {
        foreach (var receiver in m2RowHandlers)
        {
            if (receiver.sampleData != null && receiver.sampleData.audioClip == sampleClip)
            {
                return false; // SamplePanel není unikátní
            }
        }
        return true; // SamplePanel je unikátní
    }


    public void ReplaceSampleDataInBeat(int buttonIndex, SampleData oldSampleData, SampleData newSampleData)
    {


        RemoveSampleDataFromBeat(buttonIndex, oldSampleData);

        AddSampleDataToBeat(buttonIndex, newSampleData);
    }


    #endregion


    //////////////////////////////////////


    #region ResetM2ButtonsAndReplaceSampleData(), GetAllButtonIndexesForSample()

    public void ResetM2ButtonsAndReplaceSampleData(int rowIndex, SampleData newSampleData)
    {
        if (rowIndex < 0 || rowIndex >= m2RowsButtonScripts.Count)
            return;

        foreach (M2Button m2ButtonScript in m2RowsButtonScripts[rowIndex])
        {
            m2ButtonScript.ResetButtonState();
            m2ButtonScript.SetButtonsSampleData(newSampleData);
        }
    }



    public List<int> GetAllButtonIndexesForSample(SampleData sample)
    {
        List<int> buttonIndexes = new();


        for (int i = 0; i < 64; i++)
        {
            if (Beats[i].Any(sd => sd.audioClip.name == sample.audioClip.name))
            {
                buttonIndexes.Add(i);
            }
        }

        return buttonIndexes;
    }


    #endregion


    /////////////////////////////////////


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


