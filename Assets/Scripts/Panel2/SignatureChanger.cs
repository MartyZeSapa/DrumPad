using UnityEngine;
using UnityEngine.UI;

public class SignatureChanger : MonoBehaviour
{
    [SerializeField]
    private Button oneFourButton;
    [SerializeField]
    private Button twoFourButton;
    [SerializeField]
    private Button fourFourButton;

    private GameManager gameManager;

    void Start()
    {
        // Najde GameManager v instanci scény
        gameManager = FindObjectOfType<GameManager>();

        oneFourButton.onClick.AddListener(() => SetTimeSignature(1));
        twoFourButton.onClick.AddListener(() => SetTimeSignature(2));
        fourFourButton.onClick.AddListener(() => SetTimeSignature(4));
    }

    private void SetTimeSignature(int activeBeats)
    {
        SetSignatureForMode1(activeBeats);
        SetSignatureForMode2(activeBeats);
    }

    private void SetSignatureForMode1(int activeBeats)
    {
        // Projede tlaèítka z modu1
        foreach (Button button in gameManager.m1Buttons)
        {
            ToggleButtonActive(button, activeBeats);
        }
    }

    private void SetSignatureForMode2(int activeBeats)
    {
        // Projede øady v modu2
        foreach (var rowButtons in gameManager.mode2Rows)
        {   // Projede tlaèítka øady
            foreach (Button button in rowButtons)
            {
                ToggleButtonActive(button, activeBeats);
            }
        }
    }

    private void ToggleButtonActive(Button button, int activeBeats)
    {
        if (button != null)
        {
            // Zjistí index tlaèítka v kontextu baru
            int buttonIndex = button.transform.GetSiblingIndex();

            bool shouldActivate = (activeBeats == 1 && buttonIndex == 0) ||
                                  (activeBeats == 2 && (buttonIndex == 0 || buttonIndex == 2)) ||
                                  (activeBeats == 4);


            button.gameObject.SetActive(shouldActivate);
        }
    }
}
