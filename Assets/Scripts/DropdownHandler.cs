using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown Dropdown;
    public GameObject colorPanel;
    public GameObject barSelectPanel;

    private bool resetDropdown;

    private void Start()
    {
        Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void Update()
    {
        if (resetDropdown)
        {
            Dropdown.SetValueWithoutNotify(-1);
            resetDropdown = false;
        }
    }

    public void OnDropdownValueChanged(int index)
    {
        if (index == 0)
        {
            barSelectPanel.SetActive(true);
            resetDropdown = true;
        }

        if (index == 2)
        {
            colorPanel.SetActive(true);
            resetDropdown = true;
        }
    }
}
