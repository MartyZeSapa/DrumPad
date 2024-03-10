using UnityEngine;
using UnityEngine.UI;
public class SwitchScenes : MonoBehaviour
{
    [SerializeField]
    private Button m1Button;
    [SerializeField]
    private Button m2Button;
    [SerializeField]
    private Button m3Button;

    [SerializeField]
    private GameObject m1Drumpad;
    [SerializeField]
    private GameObject m2Drumpad;
    [SerializeField]
    private GameObject m3Drumpad;


    public void Start()
    {
        m1Button.onClick.AddListener(m1ButtonClick);
        m2Button.onClick.AddListener(m2ButtonClick);
        m3Button.onClick.AddListener(m3ButtonClick);
    }

    private void m1ButtonClick()
    {
        m1Drumpad.SetActive(true);

        m2Drumpad.SetActive(false);
        m3Drumpad.SetActive(false);

        m1Button.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        m2Button.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        m3Button.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
    }

    private void m2ButtonClick()
    {
        m2Drumpad.SetActive(true);

        m1Drumpad.SetActive(false);
        m3Drumpad.SetActive(false);

        m1Button.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        m2Button.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
        m3Button.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
    }

    private void m3ButtonClick()
    {
        m3Drumpad.SetActive(true);

        m1Drumpad.SetActive(false);
        m2Drumpad.SetActive(false);

        m1Button.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        m2Button.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        m3Button.GetComponent<Image>().color = new Color32(140, 140, 140, 255);
    }
}
