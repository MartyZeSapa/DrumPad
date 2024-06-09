using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel2Changer : MonoBehaviour
{
    [SerializeField]
    private Button MetronomeButton, SignatureButton, ColorButton;

    [SerializeField]
    private Image MetronomeButtonImage, SignatureButtonImage, ColorButtonImage;

    [SerializeField]
    private GameObject signaturePanel;
    [SerializeField]
    private GameObject colorPanel;
   

    void Start()
    {
        MetronomeButton.onClick.AddListener(MetronomeButtonClick);
        SignatureButton.onClick.AddListener(SignatureButtonClick);
        ColorButton.onClick.AddListener(ColorButtonClick);
    }


    private void MetronomeButtonClick()
    {
        signaturePanel.SetActive(false);
        colorPanel.SetActive(false);

        MetronomeButtonImage.color = new Color32(140, 140, 140, 255);
        SignatureButtonImage.color = new Color32(80, 80, 80, 255);
        ColorButtonImage.color = new Color32(80, 80, 80, 255);

    }

    private void SignatureButtonClick()
    {
        signaturePanel.SetActive(true);
        colorPanel.SetActive(false);

        MetronomeButtonImage.color = new Color32(80, 80, 80, 255);
        SignatureButtonImage.color = new Color32(140, 140, 140, 255);
        ColorButtonImage.color = new Color32(80, 80, 80, 255);
    }

    private void ColorButtonClick()
    {
        colorPanel.SetActive(true);
        signaturePanel.SetActive(false);

        MetronomeButtonImage.color = new Color32(80, 80, 80, 255);
        SignatureButtonImage.color = new Color32(80, 80, 80, 255);
        ColorButtonImage.color = new Color32(140, 140, 140, 255);
    }
}
