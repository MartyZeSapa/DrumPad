using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Added line to include the UnityEngine.UI namespace

public class DropReceiver : MonoBehaviour, IDropHandler
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
        GameObject droppedObject = eventData.pointerDrag; // This is the object that was dropped

        if (droppedObject != null)
        {
            
            // Transfer the sound data from the dropped object to this object
            AudioClip droppedClip = droppedObject.GetComponent<SoundData>()?.soundClip;
            GetComponent<SoundData>().soundClip = droppedClip;
            audioSource.clip = droppedClip;

            var droppedColor = droppedObject.GetComponent<Image>().color;
            GetComponent<Image>().color = droppedColor;

            

        }
    }
}
