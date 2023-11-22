using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundData : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    public TextMeshProUGUI buttonText;

    void Start()
    {
        // Add an AudioSource component if not already attached
        if (!TryGetComponent(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the AudioSource clip
        audioSource.clip = soundClip;

        UpdateButtonName();
    }

    public void PlaySound()
    {
        if (soundClip != null)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }

    void UpdateButtonName()
    {
        if (audioSource.clip != null)
        {
            buttonText.text = audioSource.clip.name;
        }
    }
}
