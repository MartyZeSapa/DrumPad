using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel2Changer : MonoBehaviour
{
    [SerializeField]
    private Button MetronomeButton;
    [SerializeField]
    private Button SignatureButton;
    [SerializeField]
    private Button ColorButton;

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


    void Update()
    {
        
    }

    private void MetronomeButtonClick()
    {
        signaturePanel.SetActive(false);
        colorPanel.SetActive(false);

        MetronomeButton.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        SignatureButton.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        ColorButton.GetComponent<Image>().color = new Color32(80, 80, 80, 255);

    }

    private void SignatureButtonClick()
    {
        signaturePanel.SetActive(true);
        colorPanel.SetActive(false);

        MetronomeButton.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        SignatureButton.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        ColorButton.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
    }

    private void ColorButtonClick()
    {
        colorPanel.SetActive(true);
        signaturePanel.SetActive(false);

        MetronomeButton.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        SignatureButton.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        ColorButton.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
    }
}
