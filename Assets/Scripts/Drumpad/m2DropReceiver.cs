using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class m2DropReceiver : MonoBehaviour, IDropHandler
{
    private AudioClip soundClip;
    private AudioSource audioSource;
    private TextMeshProUGUI buttonText;

    private Color originalColor;
    private Button targetButton; // Reference to the target button
    private bool isClipSet = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        originalColor = GetComponent<Image>().color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            AudioClip droppedClip = droppedObject.GetComponent<SoundData>()?.soundClip;
            soundClip = droppedClip;

            Color droppedColor = droppedObject.GetComponent<Image>().color;
            GetComponent<Image>().color = droppedColor;

            UpdateButtonName(droppedClip.name);

            isClipSet = true;
        }
    }

    // Call this method to set the target button
    public void SetTargetButton(Button button)
    {
        targetButton = button;
        if (targetButton != null)
        {
            buttonText = targetButton.GetComponentInChildren<TextMeshProUGUI>();
            originalColor = targetButton.GetComponent<Image>().color;
        }
    }

    // Public method to handle the button click
    public void HandleButtonClick()
    {
        if (isClipSet)
        {
            ResetButton();
        }
        else
        {
            SetAudioClip();
        }
    }

    private void SetAudioClip()
    {
        if (soundClip != null && targetButton != null)
        {
            audioSource.clip = soundClip;
            audioSource.Play(); // Play the sound clip
            targetButton.GetComponent<Image>().color = GetComponent<Image>().color; // Change the button's color
            isClipSet = true;
        }
    }

    private void ResetButton()
    {
        if (targetButton != null)
        {
            audioSource.clip = null; // Remove the sound clip
            targetButton.GetComponent<Image>().color = originalColor; // Reset to original color
            if (buttonText != null)
            {
                buttonText.text = "Default Text"; // Reset to default button text
            }
            isClipSet = false;
        }
    }

    private void UpdateButtonName(string newButtonName)
    {
        if (soundClip != null && buttonText != null)
        {
            buttonText.text = newButtonName;
        }
    }
}
