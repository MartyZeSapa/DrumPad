using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject colorPanel;
    public GameObject barSelectPanel;

    private bool resetDropdown;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void Update()
    {

    }

    public void OnDropdownValueChanged(int index)
    {
        if (index == 0)
        {
            barSelectPanel.SetActive(true);

            colorPanel.SetActive(false);
            dropdown.SetValueWithoutNotify(-1);
        }

        if (index == 2)
        {
            colorPanel.SetActive(true);

            barSelectPanel.SetActive(false);
            resetDropdown = true;
            dropdown.SetValueWithoutNotify(-1);
        }
    }
}