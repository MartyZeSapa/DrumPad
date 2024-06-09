using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class M1Button : MonoBehaviour, IDropHandler
{
    #region Inicializace

    [SerializeField] private GameManager gameManager;
    [SerializeField] private NotificationController notificationController;
    [SerializeField] private M1Popup m1Popup;

    public int buttonIndex;
    private List<SampleData> beat;



    public Image[] quadrantImages;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color inactiveHighlightColor;



    [SerializeField] private Image[] borderImages;
    [SerializeField] private Image edgeBorder;

    [SerializeField] private Color borderColor;
    [SerializeField] private Color borderHighlightColor;



    void Start()
    {
        gameManager = GameManager.Instance;
        notificationController = NotificationController.Instance;
        m1Popup = M1Popup.Instance;


        beat = gameManager.Beats[buttonIndex];

        ClearQuadrantColors();

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    #endregion

    ////////////////////////////////////////////////


    #region Button Highlight
    public void Highlight()
    {
        if (isActiveAndEnabled == false)
        {
            return;
        }



        if (beat.Count == 0)
        {
            foreach (var quadrantImage in quadrantImages)
            {
                HighlightColor(quadrantImage, inactiveHighlightColor);
            }
        }
        else
        {

            foreach (var quadrantImage in quadrantImages)
            {
                Color quadrantHighlightColor = CalculateActiveHighlightColor(quadrantImage);

                HighlightColor(quadrantImage, quadrantHighlightColor);
            }
        }
    }


    private Color CalculateActiveHighlightColor(Image quadrantImage)
    {
        // Neon colors are fully saturated and very bright, so we max out these values.
        float h, s, v;
        Color.RGBToHSV(quadrantImage.color, out h, out s, out v); // Convert RGB to HSV
        s = 1.0f; // Max out saturation for neon effect
        v = 1.0f; // Max out brightness for neon effect
        return Color.HSVToRGB(h, s, v, false); // Convert back to RGB with no HDR
    }

    private void HighlightColor(Image quadrantImage, Color highlightColor)
    {
        quadrantImage.color = highlightColor;

        edgeBorder.color = borderHighlightColor;
    }


    public void Unhighlight()
    {
        UpdateQuadrantAppearance();

        edgeBorder.color = borderColor;
    }

    #endregion


    ////////////////////////////////////////////////



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
        }
        else if (beatsCount == 2)   // Pokud jsou 2, zbarví 2 kvadranty podle 1. samplu, a druhé 2 podle druhého samplu
        {
            quadrantImages[0].color = beat[0].color;
            quadrantImages[2].color = beat[0].color;

            quadrantImages[1].color = beat[1].color;
            quadrantImages[3].color = beat[1].color;

            borderImages[0].gameObject.SetActive(true);
            borderImages[1].gameObject.SetActive(true);
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
        }
        else if (beatsCount > 3)    // Pro více než 3 samply, zbarví každý kvadrant podle každého samplu
        {
            for (int j = 0; j < beatsCount && j < 4; j++)
            {
                quadrantImages[j].color = beat[j].color;
            }

            foreach (var borderImage in borderImages)
            {
                borderImage.gameObject.SetActive(true);
            }
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        SoundData soundData = eventData.pointerDrag.GetComponent<SoundData>();


        #region safety check    null, IsUnique

        if (soundData == null)
        {
            Debug.LogError($"SoundData is null.");
            return;
        }


        if (beat.Count >= 4)
        {
            notificationController.ShowNotification($"Max samples reached on this beat.");
            return;
        }
        else if (gameManager.BeatContainsSample(buttonIndex, soundData.sampleIndex))
        {
            notificationController.ShowNotification($"This sample is already assigned to this beat.");
            return;
        }

        #endregion


        SampleData newSampleData = new(soundData.soundClip, soundData.sampleIndex, soundData.GetComponent<Image>().color);

        gameManager.AddSampleDataToBeat(buttonIndex, newSampleData);

        UpdateQuadrantAppearance();
    }




    public void ClearQuadrantColors()
    {
        foreach (var image in quadrantImages)
        {
            image.color = defaultColor;
        }
    }





    public void OnClick()
    {
        m1Popup.ShowPopup(buttonIndex, this);
    }


}
