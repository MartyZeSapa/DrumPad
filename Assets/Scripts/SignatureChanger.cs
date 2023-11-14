using UnityEngine;
using UnityEngine.UI;

public class SignatureChanger : MonoBehaviour
{
    public Button oneFourButton;
    public Button twoFourButton;
    public Button fourFourButton;

    public GameObject mode1;
    public GameObject mode2;

    void Start()
    {
        oneFourButton.onClick.AddListener(() => SetTimeSignature(4));
        twoFourButton.onClick.AddListener(() => SetTimeSignature(2));
        fourFourButton.onClick.AddListener(() => SetTimeSignature(1));
    }

    void SetTimeSignature(int activeBeats)
    {
        SetSignatureForMode(mode1, activeBeats);
        SetSignatureForMode(mode2, activeBeats);
    }

    void SetSignatureForMode(GameObject mode, int activeBeats)
    {
        foreach (Transform row in mode.transform)
        {
            foreach (Transform section in row)
            {
                ToggleButtonsActive(section.gameObject, activeBeats);
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
