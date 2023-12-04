using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundData : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    private TextMeshProUGUI buttonText;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        
    }
    void Start()
    {
        UpdateButtonName();
    }

    public void PlaySound()
    {
        if (soundClip != null)
        {
            audioSource.Play();
        }
    }

    void UpdateButtonName()
    {
        if (audioSource.clip != null && buttonText != null)
        {
            buttonText.text = audioSource.clip.name;
        }
    }
}
