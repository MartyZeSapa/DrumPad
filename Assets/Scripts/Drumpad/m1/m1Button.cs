using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class M1Button : MonoBehaviour, IDropHandler
{


    public int buttonIndex;

    public List<Image> quadrantImages;
    public List<Image> borderImages;


    private GameManager gameManager;
    private NotificationController notificationController;

    private M1Popup m1Popup;

    private List<SampleData> beat;

    void Start()    // Nastaví barvu kvadrantù na transparentní
    {
        ClearQuadrantColors();

        gameManager = GameManager.Instance;
        notificationController = NotificationController.Instance;
        m1Popup = M1Popup.Instance;


        beat = gameManager.Beats[buttonIndex];


        GetComponent<Button>().onClick.AddListener(OnClick);
    }











    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;   // Odkaz na dropntý objekt


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


        AudioClip droppedClip = soundData.soundClip;                     // Odkaz na clip dropnutého objektu
        int sampleIndex = soundData.sampleIndex;                         //        sampleIndex
        Color droppedColor = droppedObject.GetComponent<Image>().color;  //          barvu

        #region safety check   // Max poèet samplù na beat, Jestli Sample již v Beatu je


        if (beat.Count >= 4)
        {
            notificationController.ShowNotification($"Max samples reached on this beat.");
            return;
        }
        else if (beat.Exists(sd => sd.sampleIndex == sampleIndex)) // Pokud je Sample v beatu
        {
            notificationController.ShowNotification($"This sample is already assigned to this beat.");
            return;
        }

        #endregion





        SampleData newSampleData = new(droppedClip, droppedColor, sampleIndex);

        gameManager.AddSampleDataToBeat(buttonIndex, newSampleData);

        UpdateQuadrantAppearance();

    }



    public void UpdateQuadrantAppearance()
    {



        ClearQuadrantColors();
        foreach (var borderImage in borderImages)
        {
            borderImage.gameObject.SetActive(false);
        }


        var beatsCount = beat.Count;






        

        if (beatsCount == 1)    // Pokud je Sample 1, zmìní barvy všech kvadrantù na barvu tohoto samplu
        {
            foreach (var quadrantImage in quadrantImages)
            {
                quadrantImage.color = beat[0].color;
            }
            borderImages[3].gameObject.SetActive(true);
        }
        else if (beatsCount == 2)   // Pokud jsou 2, zbarví 2 kvadranty podle 1. samplu, a druhé 2 podle druhého samplu
        {
            quadrantImages[0].color = beat[0].color;
            quadrantImages[2].color = beat[0].color;

            quadrantImages[1].color = beat[1].color;
            quadrantImages[3].color = beat[1].color;

            borderImages[0].gameObject.SetActive(true);
            borderImages[1].gameObject.SetActive(true);

            borderImages[3].gameObject.SetActive(true);
        }
        else if (beatsCount == 3)       // Pokud jsou 3, zbarví první 3 kvadranty podle tìch 3 samplù a 4. kvadrantu dá barvu 3. samplu
        {
            for (int j = 0; j < beatsCount; j++)
            {
                quadrantImages[j].color = beat[j].color;
            }

            quadrantImages[3].color = beat[2].color;

            borderImages[0].gameObject.SetActive(true);

            borderImages[2].gameObject.SetActive(true);

            borderImages[3].gameObject.SetActive(true);
        }
        else if (beatsCount > 3)    // Pro více než 3 samply, zbarví každý kvadrant podle každého samplu
        {
            for (int j = 0; j < beatsCount && j < quadrantImages.Count; j++)
            {
                quadrantImages[j].color = beat[j].color;
            }

            foreach (var borderImage in borderImages)
            {
                borderImage.gameObject.SetActive(true);
            }
        }
    }




    public void OnClick()
    {
        m1Popup.ShowPopup(buttonIndex, this);
    }







    ////////////////////////////////////////////////////////////////////////////////////
    public void ClearQuadrantColors()
    {
        foreach (var image in quadrantImages)
        {
            image.color = Color.clear;
        }
    }   // Barvy kvadrantù na transparentní





}
