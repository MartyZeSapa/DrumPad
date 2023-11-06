using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown Dropdown;

    public GameObject m1Drumpad;
    public GameObject m2Drumpad;
    public GameObject m3Drumpad;
    public GameObject colorPanel;
    public GameObject barSelectPanel;


    public void Start()
    {
        Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnDropdownValueChanged(int index)
    {
        if (index == 0)
        {
            barSelectPanel.SetActive(true);
        }

        if (index == 1)
        {

        }

        if (index == 2)
        {
            colorPanel.SetActive(true);

        }

        if (index == 3)
        {

        }

        if (index == 4)
        {

        }

        Dropdown.SetValueWithoutNotify(-1);
    }
}
