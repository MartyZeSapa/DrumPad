using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class m1Button : MonoBehaviour, IDropHandler
{

    public List<Image> quadrantImages;

    public int buttonIndex;

    void Start()
    {
        // Nastav� barvu kvadrant� na transparentn�
        foreach (var image in quadrantImages)
        {
            image.color = Color.clear;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag; // Odkaz na dropnut� objekt

        if (droppedObject != null)
        {
            AudioClip droppedClip = droppedObject.GetComponent<SoundData>()?.soundClip; // Odkazuje na clip co jsme dropli

            GameManager.Instance.Beats[buttonIndex].Add(droppedClip);

            GameManager.Instance.LogBeatsListContents();
            

            UpdateQuadrantAppearance(droppedObject.GetComponent<Image>().color); // Zm�n� barvu kvadrantu
        }
    }







    private void UpdateQuadrantAppearance(Color color)
    {
        if (GameManager.Instance.Beats[buttonIndex].Count <= quadrantImages.Count)
        {
            quadrantImages[GameManager.Instance.Beats[buttonIndex].Count - 1].color = color;
        }
    }
}
