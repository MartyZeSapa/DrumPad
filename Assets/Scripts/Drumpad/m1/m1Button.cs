using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class M1Button : MonoBehaviour, IDropHandler
{


    public int buttonIndex;

    public List<Image> quadrantImages;


    private GameManager gameManager;
    private List<SampleData> beat;

    void Start()    // Nastav� barvu kvadrant� na transparentn�
    {
        ClearQuadrantColors();

        gameManager = GameManager.Instance;
        beat = gameManager.Beats[buttonIndex];


        GetComponent<Button>().onClick.AddListener(LogBeatsContent);
    }











    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;   // Odkaz na dropnt� objekt


        #region safety check    // Jestli je droppedObject nebo soundData null
        if (droppedObject == null)
        {
            Debug.LogError($"Dropped object is null.");
            return;
        }

        SoundData soundData = droppedObject.GetComponent<SoundData>();
        if (soundData == null)
        {
            Debug.LogError($"SoundData component on droppedObject not found.");
            return;
        }


        #endregion


        AudioClip droppedClip = soundData.soundClip;                     // Odkaz na clip dropnut�ho objektu
        int sampleIndex = soundData.sampleIndex;                         //        sampleIndex
        Color droppedColor = droppedObject.GetComponent<Image>().color;  //          barvu

        #region safety check   // Max po�et sampl� na beat, Jestli Sample ji� v Beatu je


        if (beat.Count >= 4)
        {
            Debug.LogWarning($"Max samples reached on Beat {buttonIndex + 1}.");
            return;
        }
        else if (beat.Exists(sd => sd.sampleIndex == sampleIndex)) // Pokud je Sample v beatu
        {
            Debug.LogWarning($"This Sample is already assigned to Beat {buttonIndex + 1}.");
            return;
        }

        #endregion





        SampleData newSampleData = new SampleData(droppedClip, droppedColor, sampleIndex);

        gameManager.AddSampleDataToBeat(buttonIndex, newSampleData);

        UpdateQuadrantAppearance();

    }



    public void UpdateQuadrantAppearance()
    {



        ClearQuadrantColors();



        var beatsCount = beat.Count;


        if (beatsCount == 1)    // Pokud je Sample 1, zm�n� barvy v�ech kvadrant� na barvu tohoto samplu
        {
            foreach (var quadrantImage in quadrantImages)
            {
                quadrantImage.color = beat[0].color;
            }
        }
        else if (beatsCount == 2)   // Pokud jsou 2, zbarv� 2 kvadranty podle 1. samplu, a druh� 2 podle druh�ho samplu
        {
            quadrantImages[0].color = beat[0].color;
            quadrantImages[2].color = beat[0].color;

            quadrantImages[1].color = beat[1].color;
            quadrantImages[3].color = beat[1].color;
        }
        else if (beatsCount == 3)       // Pokud jsou 3, zbarv� prvn� 3 kvadranty podle t�ch 3 sampl� a 4. kvadrantu d� barvu 3. samplu
        {
            for (int j = 0; j < beatsCount; j++)
            {
                quadrantImages[j].color = beat[j].color;
            }

            quadrantImages[3].color = beat[2].color;
        }
        else if (beatsCount > 3)    // Pro v�ce ne� 3 samply, zbarv� ka�d� kvadrant podle ka�d�ho samplu
        {
            for (int j = 0; j < beatsCount && j < quadrantImages.Count; j++)
            {
                quadrantImages[j].color = beat[j].color;
            }
        }
    }










    ////////////////////////////////////////////////////////////////////////////////////
    public void ClearQuadrantColors()
    {
        foreach (var image in quadrantImages)
        {
            image.color = Color.clear;
        }
    }   // Barvy kvadrant� na transparentn�

    private void LogBeatsContent()  // DebugLog Sampl� v beatu
    {

        if (beat.Count > 0)
        {
            Debug.Log($"");
            Debug.Log($"Beat {buttonIndex + 1} contains:");
            foreach (var sample in beat)
            {
                Debug.Log(sample.audioClip.name);
            }
        }
        else
        {
            Debug.Log($"Beat {buttonIndex + 1} contains no samples.");
        }
    }

}
