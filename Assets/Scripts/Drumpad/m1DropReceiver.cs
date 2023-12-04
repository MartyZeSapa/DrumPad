using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class m1DropReceiver : MonoBehaviour, IDropHandler
{
    private List<AudioClip> soundClips = new List<AudioClip>();
    private AudioSource audioSource;
    public List<Image> quadrantImages;

    void Start()
    {
        if (!TryGetComponent(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Nastaví barvu kvadrantù na transparentní
        foreach (var image in quadrantImages)
        {
            image.color = Color.clear;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag; // Odkazuje na objekt který drag and dropujeme

        if (droppedObject != null && soundClips.Count < 4) // Max 4 soundClipy na button
        {
            AudioClip droppedClip = droppedObject.GetComponent<SoundData>()?.soundClip;
            if (droppedClip != null)
            {
                soundClips.Add(droppedClip); // Pøidá soundClip do listu soundclipù
                UpdateQuadrantAppearance(droppedObject.GetComponent<Image>().color); // Zmìní barvu kvadrantu
            }
        }
    }

    private void UpdateQuadrantAppearance(Color color)
    {
        if (soundClips.Count <= quadrantImages.Count)
        {
            quadrantImages[soundClips.Count - 1].color = color;
        }
    }

    public void PlaySounds()  // Pøehraje všechny zvuky z tlaèítka najednou
    {
        foreach (var clip in soundClips)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
