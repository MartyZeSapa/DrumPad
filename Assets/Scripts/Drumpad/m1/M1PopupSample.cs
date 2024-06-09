using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class M1PopupSample : MonoBehaviour, IDropHandler
{
    #region Inicializace

    [SerializeField] private GameManager gameManager;
    [SerializeField] private NotificationController notificationController;

    public M1Button m1Button;
    private SampleData sampleData;
    public int beatIndex;
    [SerializeField] private Color defaultColor;

    [SerializeField] private Image imageComponent;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Button removeSampleButton;



    void Start()
    {
        gameManager = GameManager.Instance;
        notificationController = NotificationController.Instance;

        removeSampleButton.onClick.AddListener(RemoveSampleData);
    }

    #endregion


    ////////////////////////////////////////////////
    


    public void UpdateSamplePanel(SampleData sample, int buttonIndex)
    {

        this.sampleData = sample;
        this.beatIndex = buttonIndex;
        textComponent.text = sample.audioClip.name;
        imageComponent.color = sample.color;
        removeSampleButton.gameObject.SetActive(true);
    }

    public void ClearSamplePanel()
    {
        sampleData = null;
        textComponent.text = "";
        imageComponent.color = defaultColor; 
        removeSampleButton.gameObject.SetActive(false);
    }

    public void RemoveSampleData()
    {
        gameManager.RemoveSampleDataFromBeat(beatIndex, sampleData);
        ClearSamplePanel();

        if (m1Button != null)
        {
            m1Button.UpdateQuadrantAppearance();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

        #region safety check    null, uniqueness
        SoundData soundData = eventData.pointerDrag.GetComponent<SoundData>();
        if (soundData == null) return;

        
        if (gameManager.BeatContainsSample(beatIndex, soundData.sampleIndex))
        {
            notificationController.ShowNotification($"This sample is already assigned to this beat.");
            return;
        }
        #endregion

        SampleData newSampleData = new(soundData.soundClip, soundData.sampleIndex, soundData.GetComponent<Image>().color);

        if (sampleData != null)
        {
            gameManager.ReplaceSampleDataInBeat(beatIndex, sampleData, newSampleData);
        }
        else
        {
            gameManager.AddSampleDataToBeat(beatIndex, newSampleData);
        }

        UpdateSamplePanel(newSampleData, beatIndex);

        if (m1Button != null)
        {
            m1Button.UpdateQuadrantAppearance();
        }
        else
        {
            Debug.LogError("M1Button is null after dropping sample data.");
        }
    }

}
