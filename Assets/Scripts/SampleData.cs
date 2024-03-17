using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleData
{
    public AudioClip audioClip;
    public Color color;

    public SampleData(AudioClip clip, Color color)
    {
        this.audioClip = clip;
        this.color = color;
    }
}
