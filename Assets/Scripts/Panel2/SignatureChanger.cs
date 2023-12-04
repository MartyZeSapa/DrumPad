using UnityEngine;
using UnityEngine.UI;

public class SignatureChanger : MonoBehaviour
{
    public Button oneFourButton;
    public Button twoFourButton;
    public Button fourFourButton;

    public GameObject mode1;
    public GameObject mode2content;

    void Start()
    {
        oneFourButton.onClick.AddListener(() => SetTimeSignature(4));
        twoFourButton.onClick.AddListener(() => SetTimeSignature(2));
        fourFourButton.onClick.AddListener(() => SetTimeSignature(1));
    }

    void SetTimeSignature(int activeBeats)
    {
        SetSignatureForMode1(activeBeats);
        SetSignatureForMode2 (activeBeats);
    }

    void SetSignatureForMode1(int activeBeats)
    {
        foreach (Transform row in mode1.transform)
        {
            foreach (Transform bar in row)
            {
                ToggleButtonsActive(bar.gameObject, activeBeats);
            }
        }
    }

    void SetSignatureForMode2(int activeBeats)
    {
        foreach (Transform row in mode2content.transform)
        {
            Transform beatPanel = row.Find("Beat Panel");     // Najde "Beat Panel" v "Row"
            if (beatPanel != null)
            {
                foreach (Transform section in beatPanel)
                {
                    foreach (Transform bar in section)
                    {
                        ToggleButtonsActive(bar.gameObject, activeBeats);
                    }
                }
            }
        }
    }



    void ToggleButtonsActive(GameObject bar, int activeBeats)
    {
        for (int i = 0; i < bar.transform.childCount; i++)
        {
            Transform buttonTransform = bar.transform.GetChild(i);
            Button button = buttonTransform.GetComponent<Button>();

            if (button != null)
            {
                bool shouldActivate = (activeBeats == 4 && i == 0) ||
                                      (activeBeats == 2 && (i == 0 || i == 2)) ||
                                      (activeBeats == 1);

                button.gameObject.SetActive(shouldActivate);
            }
        }
    }
}
