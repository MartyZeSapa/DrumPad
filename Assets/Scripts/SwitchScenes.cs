using UnityEngine;
using UnityEngine.UI;
public class SwitchScenes : MonoBehaviour
{
    public Button m1Button;
    public Button m2Button;
    public Button m3Button;

    public GameObject m1Drumpad;
    public GameObject m2Drumpad;
    public GameObject m3Drumpad;

    public void Start()
    {
        m1Button.onClick.AddListener(m1ButtonClick);
        m2Button.onClick.AddListener(m2ButtonClick);
        m3Button.onClick.AddListener(m3ButtonClick);
    }

    public void m1ButtonClick()
    {
        m1Drumpad.SetActive(true);

        m2Drumpad.SetActive(false);
        m3Drumpad.SetActive(false);
    }

    public void m2ButtonClick()
    {
        m2Drumpad.SetActive(true);

        m1Drumpad.SetActive(false);
        m3Drumpad.SetActive(false);
    }

    public void m3ButtonClick()
    {
        m3Drumpad.SetActive(true);

        m1Drumpad.SetActive(false);
        m2Drumpad.SetActive(false);
    }
}
