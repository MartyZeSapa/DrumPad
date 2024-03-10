using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class m2DropReceiver : MonoBehaviour, IDropHandler
{
    public AudioClip AudioClip { get; private set; }
    public Color PanelColor { get; private set; }
    private AudioSource audioSource;
    private TextMeshProUGUI buttonText;

    public Button[] buttons; // Array of buttons controlled by this sound panel

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => HandleButtonClick(btn));
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            AudioClip = droppedObject.GetComponent<SoundData>()?.soundClip;
            PanelColor = droppedObject.GetComponent<Image>().color;

            GetComponent<Image>().color = PanelColor;
            UpdateButtonName(AudioClip?.name);
        }
    }

    private void HandleButtonClick(Button clickedButton)
    {
        if (AudioClip != null)
        {
            AudioSource buttonAudioSource = clickedButton.GetComponent<AudioSource>();
            if (buttonAudioSource != null)
            {
                buttonAudioSource.clip = AudioClip;
            }

            clickedButton.GetComponent<Image>().color = PanelColor;
        }
    }

    private void UpdateButtonName(string newButtonName)
    {
        if (buttonText != null)
        {
            buttonText.text = newButtonName;
        }
    }
}
