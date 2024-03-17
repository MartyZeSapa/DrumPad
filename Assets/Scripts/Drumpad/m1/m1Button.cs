using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class m1Button : MonoBehaviour, IDropHandler
{
    public int buttonIndex;

    public List<Image> quadrantImages;

    

    void Start()    // Nastav� barvu kvadrant� na transparentn�
    {
        foreach (var image in quadrantImages)
        {
            image.color = Color.clear;
        }
    }










    public void OnDrop(PointerEventData eventData)    // P�id� SampleData do beatu, zm�n� barvu
    {
        GameObject droppedObject = eventData.pointerDrag; // Odkaz na dropnut� objekt

        if (droppedObject != null)
        {
            AudioClip droppedClip = droppedObject.GetComponent<SoundData>()?.soundClip; // Odkaz na clip dropnut�ho objektu
            Color droppedColor = droppedObject.GetComponent<Image>().color;             //          barvu


            SampleData sampleData = new SampleData(droppedClip, droppedColor);                                                // P�id� SampleData do beatu pokud tam ji� nen�
            bool exists = GameManager.Instance.Beats[buttonIndex].Exists(sd => sd.audioClip == droppedClip);
            if (!exists)
            {
                GameManager.Instance.AddSampleDataIfUnique(buttonIndex, sampleData);


                UpdateQuadrantAppearance(droppedColor); // Zm�n� barvu kvadrantu
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
