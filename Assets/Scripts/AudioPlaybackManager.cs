using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioPlaybackManager : MonoBehaviour
{
    #region  Initialization


    public static AudioPlaybackManager Instance;


    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private GameManager gameManager;

    private int bpm;
    private int currentTimeSignature = 16;

    private AudioSource[] audioSources;
    private Coroutine playbackCoroutine;
    private bool isPlaying = false;

    void Start()
    {
        SetupSingleton();
        InitializeAudioSources();
        gameManager = GameManager.Instance;
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


    private void InitializeAudioSources()
    {
        audioSources = new AudioSource[64]; // Increasing audio sources to handle more simultaneous samples
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i] = Instantiate(audioSourcePrefab, transform);
            audioSources[i].playOnAwake = false;
            audioSources[i].loop = false;
        }
    }

    #endregion


    public void SetBPM(int newBpm)
    {
        // Clamp BPM to be within the allowed range
        bpm = Mathf.Clamp(newBpm, 40, 420);  // Set the minimum BPM to 40 and maximum to 420
    }



    public void SetCurrentTimeSignature(int newTimeSignature)
    {
        currentTimeSignature = newTimeSignature;
    }


    //////////////////////////////////////////////////////////

    #region StartPlayback(), PlaybackCouroutine(), PlayBeatSamples()
    public void StartPlayback()
    {
        if (isPlaying) return;
        isPlaying = true;
        playbackCoroutine = StartCoroutine(PlaybackCoroutine());
    }


    private IEnumerator PlaybackCoroutine()
    {
        int beatCount = 64; // Total number of beats
        int currentBeatIndex = 0; // Current beat index
        double nextTime = AudioSettings.dspTime; // For more precise timing

        while (isPlaying)
        {
            double currentTime = AudioSettings.dspTime;
            if(currentTime >= nextTime)
            {
                if (ShouldPlayBeat(currentBeatIndex))
                {
                    PlayBeatSamples(gameManager.Beats[currentBeatIndex]);
                    nextTime += 60f / bpm; // Calculate next beat time
                }
                currentBeatIndex = (currentBeatIndex + 1) % beatCount;
            }
            yield return null; // Wait until next frame before checking again



        }
    }

    private void PlayBeatSamples(List<SampleData> samplesForThisBeat)
    {
        foreach (SampleData sampleData in samplesForThisBeat)
        {
            AudioSource availableSource = audioSources.FirstOrDefault(source => !source.isPlaying);
            if (availableSource != null)
            {
                availableSource.clip = sampleData.audioClip;
                availableSource.volume = gameManager.GetSampleVolume(sampleData.audioClip.name);
                availableSource.Play();
            }
            else
            {
                Debug.LogWarning("No available audio sources to play sample: " + sampleData.audioClip.name);
            }
        }
    }





    private bool ShouldPlayBeat(int beatIndex)
    {
        // Simplified time signature interpretation
        return (currentTimeSignature == 16) || // Play every beat for 16/4 time
               (currentTimeSignature == 8 && (beatIndex % 4 == 0 || beatIndex % 4 == 2)) || // Play beats 0 and 2 for 8/4 time
               (currentTimeSignature == 4 && beatIndex % 4 == 0); // Play beat 0 for 4/4 time
    }

    #endregion

    //////////////////////////////////////////////////////////

    #region StopPlayback(), StopAllAudioSources()

    public void StopPlayback()
    {
        if (!isPlaying) return;
        isPlaying = false;
        if (playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
            playbackCoroutine = null;
        }
        StopAllAudioSources();
    }
    private void StopAllAudioSources()
    {
        foreach (var source in audioSources)
        {
            source.Stop();
        }
    }

    #endregion
}
