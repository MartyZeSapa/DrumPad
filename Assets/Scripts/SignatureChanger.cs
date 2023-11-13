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
        // Set time signature for both Mode1 and Mode2
        SetSignatureForMode(mode1, "Row", "Bar", 4, activeBeats);
        SetSignatureForMode(mode2, "Row", "Beat Panel/Section", 6, activeBeats);
    }

    void SetSignatureForMode(GameObject mode, string rowPrefix, string sectionPrefix, int rowsCount, int activeBeats)
    {

        // Iterate through each Row
        for (int i = 1; i <= rowsCount; i++)
        {
            GameObject row = FindChildByName(mode, $"{rowPrefix}{i}");
            if (row == null) continue;

            // Iterate through each Section/Bar within the Row
            for (int j = 1; j <= 4; j++)
            {
                GameObject section = FindChildByName(row, $"{sectionPrefix}{j}");
                if (section == null) continue;

                // If in Mode2, we find each Bar inside the Section
                if (sectionPrefix.Contains("Beat Panel"))
                {
                    for (int k = 1; k <= 4; k++)
                    {
                        GameObject bar = FindChildByName(section, $"Bar{k}");
                        if (bar == null) continue;
                        ToggleButtonsActive(bar, activeBeats);
                    }
                }
                else
                {
                    // If in Mode1, the section itself is the Bar
                    ToggleButtonsActive(section, activeBeats);
                }
            }
        }
    }

    GameObject FindChildByName(GameObject parent, string name)
    {
        Transform child = parent.transform.Find(name);
        if (child == null)
        {
            Debug.LogError($"{name} not found under {parent.name}");
            return null;
        }
        return child.gameObject;
    }

    void ToggleButtonsActive(GameObject bar, int activeBeats)
    {
        // Iterate over the buttons within the Bar
        for (int l = 1; l <= 4; l++)
        {
            Button button = bar.transform.Find("Button" + l.ToString())?.GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError($"Button {l} not found under {bar.name}");
                continue;
            }

            bool shouldActivate = (activeBeats == 4 && l == 1) ||
                                  (activeBeats == 2 && (l == 1 || l == 3)) ||
                                  (activeBeats == 1);

            button.gameObject.SetActive(shouldActivate);
        }
    }


}
