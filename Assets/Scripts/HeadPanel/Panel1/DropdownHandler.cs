using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;


    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }


    private void OnDropdownValueChanged(int index)
    {
        if (index == 0) // Clear All
        {

            GameManager.Instance.ClearAllSamplesFromBeats();

            dropdown.SetValueWithoutNotify(-1);
        }

        if (index == 2) // Saved Loops
        {

            dropdown.SetValueWithoutNotify(-1);
        }

        if (index == 2) // Export
        {

            dropdown.SetValueWithoutNotify(-1);
        }
    }
}