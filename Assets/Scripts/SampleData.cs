using System.Collections.Generic;
using UnityEngine;
public class SampleData
{
    public AudioClip audioClip;
    public Color color;
    public int sampleIndex;

    // Konstruktor
    public SampleData(AudioClip clip, Color color, int sampleIndex)
    {
        this.audioClip = clip;
        this.color = color;
        this.sampleIndex = sampleIndex;
    }
}