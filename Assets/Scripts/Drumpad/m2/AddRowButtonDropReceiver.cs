using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddRowButtonDropReceiver : MonoBehaviour, IDropHandler
{


    [SerializeField] private GameManager gameManager;
    [SerializeField] private NotificationController notificationController;

    public List<M2RowHandler> m2DropReceivers;


    void Awake()
    {
        m2DropReceivers = gameManager.m2RowHandlers;
    }

    ////////////////////////////////////////////////


    public void OnDrop(PointerEventData eventData)
    {
        SoundData soundData = eventData.pointerDrag.GetComponent<SoundData>();

        #region safety check    // null


        if (soundData == null)
        {
            return;
        }
        #endregion


        #region safety check    // Jestli existuje samplePanel s tímto Samplem

        if (!gameManager.IsUniqueSamplePanel(soundData.soundClip))
        {
            notificationController.ShowNotification("A row with this sample already exists.");
            return;
        }

        #endregion




        gameManager.AddNewM2Row();


        var m2DropReceiver = m2DropReceivers[^1]; // ^1 je stejné jako gameManager.m2DropReceiver.Count - 1

        m2DropReceiver.sampleData = new SampleData(soundData.soundClip, soundData.sampleIndex, soundData.GetComponent<Image>().color);

        m2DropReceiver.UpdateSamplePanelUI();

        

        gameManager.ResetM2ButtonsAndReplaceSampleData(m2DropReceiver.rowIndex, m2DropReceiver.sampleData); 
    }
}
