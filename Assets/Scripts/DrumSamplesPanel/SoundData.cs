using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundData : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    private TextMeshProUGUI buttonText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;

        // Find the child TextMeshProUGUI component and assign it to buttonText
        Transform textTransform = transform.Find("Text (TMP)");
        if (textTransform != null)
        {
            buttonText = textTransform.GetComponent<TextMeshProUGUI>();
        }

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
