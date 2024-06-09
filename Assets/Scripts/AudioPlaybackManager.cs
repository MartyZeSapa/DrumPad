using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlaybackManager : MonoBehaviour
{
    public static AudioPlaybackManager Instance;

    [SerializeField] private AudioSourcePool audioSourcePool; // Pool Manager
    [SerializeField] private GameManager gameManager;

    [SerializeField] private MetronomeHandler[] metronomeHandlers;
    [SerializeField] private Slider volumeSlider; // Reference to the volume slider

    private int currentBeatIndex = 0;
    private int bpm;
    private int currentTimeSignature = 16;
    int beatCount = 64;
    private Coroutine playbackCoroutine;
    public bool isPlaying = false;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolumeForAllMetronomes);
        volumeSlider.value = 1.0f; // Default volume to max
    }

    public void StartPlayback()
    {
        if (isPlaying) return;
        isPlaying = true;
        playbackCoroutine = StartCoroutine(PlaybackCoroutine());
    }

    private IEnumerator PlaybackCoroutine()
    {
        
        double nextTime = AudioSettings.dspTime;

        while (isPlaying)
        {
            float delayBetweenBeats = 60f / bpm;
            double currentTime = AudioSettings.dspTime;
            if (currentTime >= nextTime)
            {
                if (gameManager.ShouldPlayBeat(currentBeatIndex))
                {
                    PlayBeatSamples(gameManager.Beats[currentBeatIndex]);
                    gameManager.HighlightBeat(currentBeatIndex);

                    PlayMetronome(currentBeatIndex, delayBetweenBeats);

                    nextTime += delayBetweenBeats;
                }

               
                currentBeatIndex = (currentBeatIndex + 1) % beatCount;
            }
            yield return null;
        }
    }


    private void PlayMetronome(int beatIndex, float delayBetweenBeats)
    {
        MetronomeHandler metronomeHandler = null;

        if (beatIndex == 0 || beatIndex == 16 || beatIndex == 32 || beatIndex == 48)
        {
            metronomeHandler = metronomeHandlers[0];
        }
        else if (beatIndex == 4 || beatIndex == 20 || beatIndex == 36 || beatIndex == 52)
        {
            metronomeHandler = metronomeHandlers[1];
        }
        else if (beatIndex == 8 || beatIndex == 24 || beatIndex == 40 || beatIndex == 56)
        {
            metronomeHandler = metronomeHandlers[2];
        }
        else if (beatIndex == 12 || beatIndex == 28 || beatIndex == 44 || beatIndex == 60)
        {
            metronomeHandler = metronomeHandlers[3];
        }

        if (metronomeHandler != null && !metronomeHandler.isReset)
        {
            StartCoroutine(FlashMetronomeButton(metronomeHandler, delayBetweenBeats));
            metronomeHandler.PlayMetronome();
        }
    }


    private IEnumerator FlashMetronomeButton(MetronomeHandler metronomeHandler, float delayBetweenBeats)
    {
        metronomeHandler.HighlightButton();
        yield return new WaitForSeconds(delayBetweenBeats); // Flash duration
        metronomeHandler.UnhighlightButton();
    }



    private void PlayBeatSamples(List<SampleData> samplesForThisBeat)
    {
        foreach (SampleData sampleData in samplesForThisBeat)
        {
            AudioSource src = audioSourcePool.GetSource();
            if (src != null)
            {
                src.clip = sampleData.audioClip;
                src.volume = gameManager.GetSampleVolume(sampleData.audioClip.name);
                src.PlayScheduled(AudioSettings.dspTime);
                StartCoroutine(ReturnSourceAfterPlaying(src, src.clip.length));
            }
        }
    }

    private IEnumerator ReturnSourceAfterPlaying(AudioSource src, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        audioSourcePool.ReturnSource(src);
    }

    public void StopPlayback()
    {
        isPlaying = false;
        if (playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
            playbackCoroutine = null;
        }
        gameManager.UnhighlightAll();
        currentBeatIndex = 0;
    }

    public void SetBPM(int newBpm)
    {
        bpm = Mathf.Clamp(newBpm, 40, 420);  // Set the minimum BPM to 40 and maximum to 420
    }

    public void SetCurrentTimeSignature(int newTimeSignature)
    {
        currentTimeSignature = newTimeSignature;
        gameManager.SetCurrentTimeSignature(newTimeSignature);
    }

    public void SetVolumeForAllMetronomes(float volume)
    {
        foreach (var metronomeHandler in metronomeHandlers)
        {
            metronomeHandler.SetVolume(volume);
        }
    }

}
