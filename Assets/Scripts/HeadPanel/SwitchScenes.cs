using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitchScenes : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioPlaybackManager audioPlaybackManager;

    [SerializeField]
    private Button m1Button, m2Button, m3Button;

    [SerializeField]
    private Image m1ButtonImage, m2ButtonImage, m3ButtonImage;

    [SerializeField]
    private GameObject m1Drumpad, m2Drumpad, m3Drumpad;



    public M1Button[] m1ButtonScripts;

    public List<M2Button[]> m2RowsButtonScripts;
    public List<M2RowHandler> m2DropReceivers;



    public void Start()
    {
        m1Button.onClick.AddListener(m1ButtonClick);
        m2Button.onClick.AddListener(m2ButtonClick);
        m3Button.onClick.AddListener(m3ButtonClick);


        m1ButtonScripts = gameManager.m1ButtonScripts;

        m2RowsButtonScripts = gameManager.m2RowsButtonScripts;
        m2DropReceivers = gameManager.m2RowHandlers;


    }

    private void m1ButtonClick()
    {

        gameManager.UpdateMode1UI();

        m1Drumpad.SetActive(true);

        m2Drumpad.SetActive(false);
        m3Drumpad.SetActive(false);

        m1ButtonImage.color = new Color32(140, 140, 140, 255);
        m2ButtonImage.color = new Color32(80, 80, 80, 255);
        m3ButtonImage.color = new Color32(80, 80, 80, 255);


        if (audioPlaybackManager.isPlaying == true)
        {
            foreach (var rowButtons in m2RowsButtonScripts)
            {
                    M2Button lastButtonScript = rowButtons[gameManager.lastHighlightedBeatIndex];   
                    lastButtonScript?.Unhighlight();
            }

            foreach (var dropReceiver in m2DropReceivers)
            {
                dropReceiver.UnhighlightAllSections();
            }





            gameManager.HighlightM1Section(gameManager.currentSectionIndex);

            m1ButtonScripts[gameManager.currentBeatIndex].Highlight();

        }
    }

    private void m2ButtonClick()
    {
        gameManager.UpdateMode2UI();


        m2Drumpad.SetActive(true);

        m1Drumpad.SetActive(false);
        m3Drumpad.SetActive(false);

        m1ButtonImage.color = new Color32(80, 80, 80, 255);
        m2ButtonImage.color = new Color32(140, 140, 140, 255);
        m3ButtonImage.color = new Color32(80, 80, 80, 255);


        //Debug.Log(gameManager.lastHighlightedBeatIndex);

        if (audioPlaybackManager.isPlaying == true)
        {
            M1Button lastButtonScript = m1ButtonScripts[gameManager.lastHighlightedBeatIndex];
            lastButtonScript?.Unhighlight();

            gameManager.UnhighlightAllM1Sections();





            foreach (var rowHandler in m2DropReceivers)
            {
                rowHandler.HighlightSection(gameManager.currentSectionIndex);
            }

            foreach (var m2RowsButtonScripts in m2RowsButtonScripts)
            {
                m2RowsButtonScripts[gameManager.currentBeatIndex].Highlight();
            }


        }

    }

    private void m3ButtonClick()
    {
        m3Drumpad.SetActive(true);

        m1Drumpad.SetActive(false);
        m2Drumpad.SetActive(false);

        m1ButtonImage.color = new Color32(80, 80, 80, 255);
        m2ButtonImage.color = new Color32(80, 80, 80, 255);
        m3ButtonImage.color = new Color32(140, 140, 140, 255);

    }
}
