using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class m1Button : MonoBehaviour, IDropHandler
{
    public int buttonIndex;

    public List<Image> quadrantImages;

    

    void Start()    // Nastaví barvu kvadrantù na transparentní
    {
        foreach (var image in quadrantImages)
        {
            image.color = Color.clear;
        }
    }










    public void OnDrop(PointerEventData eventData)    // Pøidá SampleData do beatu, zmìní barvu
    {
        GameObject droppedObject = eventData.pointerDrag; // Odkaz na dropnutý objekt

        if (droppedObject != null)
        {
            AudioClip droppedClip = droppedObject.GetComponent<SoundData>()?.soundClip; // Odkaz na clip dropnutého objektu
            Color droppedColor = droppedObject.GetComponent<Image>().color;             //          barvu


            SampleData sampleData = new SampleData(droppedClip, droppedColor);                                                // Pøidá SampleData do beatu pokud tam již není
            bool exists = GameManager.Instance.Beats[buttonIndex].Exists(sd => sd.audioClip == droppedClip);
            if (!exists)
            {
                GameManager.Instance.AddSampleDataIfUnique(buttonIndex, sampleData);


                UpdateQuadrantAppearance(droppedColor); // Zmìní barvu kvadrantu
                GameManager.Instance.LogBeatsListContents();
            }
            else
            {
                Debug.Log($"This Sample is already assigned to this beat.");
            }




        }
    }








    public void ClearQuadrantColors()
    {
        foreach (var image in quadrantImages)
        {
            image.color = Color.clear; // Sets the color to transparent
        }
    }



    private void UpdateQuadrantAppearance(Color color)
    {
        if (GameManager.Instance.Beats[buttonIndex].Count <= 4)
        {
            quadrantImages[GameManager.Instance.Beats[buttonIndex].Count - 1].color = color;
        }
    }
}
