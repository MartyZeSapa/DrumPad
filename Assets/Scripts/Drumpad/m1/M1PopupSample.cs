using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class M1PopupSample : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image imageComponent;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Button removeSampleButton;

    [SerializeField] private Color defaultColor;

    private SampleData sampleData;
    public int beatIndex;

    private GameManager gameManager;
    private NotificationController notificationController;

    public M1Button m1Button;

    void Start()
    {
        removeSampleButton.onClick.AddListener(RemoveSampleData);

        gameManager = GameManager.Instance;
        notificationController = NotificationController.Instance;
    }


    public void UpdateSamplePanel(SampleData sample, int buttonIndex)   // Nastaví SamplePanelu nový SampleData
    {

        this.sampleData = sample;
        this.beatIndex = buttonIndex;
        textComponent.text = sample.audioClip.name;
        imageComponent.color = sample.color;
        removeSampleButton.gameObject.SetActive(true);

        if (m1Button == null)
        {
            Debug.LogError("M1Button script is not set.");
        }

    }

    public void ClearSamplePanel()  // Resetuje Panel
    {
        sampleData = null;
        textComponent.text = "";
        imageComponent.color = defaultColor; 
        removeSampleButton.gameObject.SetActive(false);
    }

    public void RemoveSampleData()  // Odebere SampleData z Beats, resetuje se, Updatne UI M1Buttonu a PopupPanelu
    {
        GameManager.Instance.RemoveSampleDataFromBeat(beatIndex, sampleData);
        ClearSamplePanel();

        if (m1Button != null)
        {
            m1Button.UpdateQuadrantAppearance();
        }
        else
        {
            Debug.LogError("M1Button is null when trying to update quadrant appearance.");
        }
    }

    public void OnDrop(PointerEventData eventData)  // Nastaví nebo nahradí SampleData na daném Beatu, Updatje UI M1PopupPanelu a M1Buttonu
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

        SampleData newSampleData = new(soundData.soundClip, soundData.GetComponent<Image>().color, soundData.sampleIndex);

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
