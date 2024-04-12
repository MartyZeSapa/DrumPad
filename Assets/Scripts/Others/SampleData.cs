using System.Collections.Generic;
using UnityEngine;
public class SampleData
{
    public AudioClip audioClip;
    public Color color;
    public int sampleIndex;
    public float volume = 1.0f;
    // Konstruktor
    public SampleData(AudioClip clip, Color color, int sampleIndex, float vol = 1.0f)
    {
        this.audioClip = clip;
        this.color = color;
        this.sampleIndex = sampleIndex;
        this.volume = vol;
    }
}