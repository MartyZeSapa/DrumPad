using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class M3DropReceiver : MonoBehaviour, IDropHandler
{

    // Use static to share these arrays among all instances of m3DropReceiver
    public static AudioSource[] audioSources;
    public static AudioClip[] audioClips;
    public static Image[] buttonImages;

    [SerializeField] private int buttonIndex; // Assign this in the inspector to match the button to an index

    private void Awake()
    {
        if (audioSources == null)
        {
            audioSources = new AudioSource[10];
            audioClips = new AudioClip[10];
            buttonImages = new Image[10];
        }

        audioSources[buttonIndex] = gameObject.AddComponent<AudioSource>();
        buttonImages[buttonIndex] = GetComponent<Image>();
    }


    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;   // Odkaz na dropnutý objekt

        if (droppedObject != null && droppedObject.GetComponent<SoundData>() != null)
        {
            audioClips[buttonIndex] = droppedObject.GetComponent<SoundData>().soundClip;        // Uloží audioclip a zmìní barvu
            buttonImages[buttonIndex].color = droppedObject.GetComponent<Image>().color;
        }
    }
}