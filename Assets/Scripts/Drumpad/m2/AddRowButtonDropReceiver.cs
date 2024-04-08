using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddRowButtonDropReceiver : MonoBehaviour, IDropHandler
{


    private GameManager gameManager;
    private NotificationController notificationController;

    void Start()
    {
        gameManager = GameManager.Instance;
        notificationController = NotificationController.Instance;
    }






    public void OnDrop(PointerEventData eventData)  // Zm�n� barvu a text Sample Panelu
                                                    // zm�n� SampleData v�ech m2Buttons


    {
        GameObject droppedObject = eventData.pointerDrag;   // Odkaz na dropnut� objekt

        #region safety check    // Jestli je droppedObject nebo soundData null

        if (droppedObject == null)
        {
            Debug.LogError($"Dropped object is null.");
            return;
        }

        SoundData soundData = droppedObject.GetComponent<SoundData>();
        if (soundData == null)
        {
            return;
        }
        #endregion


        AudioClip droppedClip = soundData.soundClip;                        // Odkaz na clip dropnut�ho objektu
        int sampleIndex = soundData.sampleIndex;                            //        sampleIndex
        Color panelColor = droppedObject.GetComponent<Image>().color;       //          barvu


        #region safety check    // Jestli existuje samplePanel s t�mto Samplem

        if (!gameManager.IsUniqueSamplePanel(droppedClip))
        {
            notificationController.ShowNotification("A row with this sample already exists.");
            return;
        }

        #endregion


        gameManager.AddNewM2Row();




        var m2DropReceiver = gameManager.m2DropReceivers[^1]; // ^1 je stejn� jako gameManager.m2DropReceiver.Count - 1



        m2DropReceiver.UpdateSamplePanelUI(droppedClip.name, panelColor);         // Zm�n� barvu a text Sample Panelu

        m2DropReceiver.sampleData = new SampleData(droppedClip, panelColor, sampleIndex);   // Zalo�� glob�ln� SampleData

        gameManager.ReplaceM2ButtonSampleData(m2DropReceiver.rowIndex, m2DropReceiver.sampleData);  // Updatne SampleData v�ech tla��tek   



    }
}
