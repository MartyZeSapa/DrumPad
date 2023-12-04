using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class m3DropReceiver : MonoBehaviour, IDropHandler
{
    private AudioSource audioSource;
    void Start()
    {
        if (!TryGetComponent(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag; // Odkazuje na objekt který drag and dropujeme
        if (droppedObject != null)
        {

            AudioClip droppedClip = droppedObject.GetComponent<SoundData>()?.soundClip; // Nastaví soundclip na button
            GetComponent<SoundData>().soundClip = droppedClip;
            audioSource.clip = droppedClip;
            Color droppedColor = droppedObject.GetComponent<Image>().color; // Nastaví barvu tlaèítka na barvu tlaèítka který jsme dropli
            GetComponent<Image>().color = droppedColor;

        }
    }
}