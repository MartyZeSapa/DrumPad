using UnityEngine;
using UnityEngine.UI;

public class MetronomeHandler : MonoBehaviour
{
    [SerializeField]
    private Button button1;
    [SerializeField]
    private Button button2;
    [SerializeField]
    private Button button3;

    void Start()
    {
        button1.onClick.AddListener(Button1Click);
        button2.onClick.AddListener(Button2Click);
        button3.onClick.AddListener(Button3Click);
    }

    private void Button1Click()
    {
        button1.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
        button2.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
        button3.GetComponent<Image>().color = new Color32(200, 200, 200, 255);

    }

    private void Button2Click()
    {
        button1.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        button2.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
        button3.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
    }

    private void Button3Click()
    {
        button1.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        button2.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        button3.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
    }
}
