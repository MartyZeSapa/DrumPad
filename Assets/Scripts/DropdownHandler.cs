using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject colorPanel;
    public GameObject barSelectPanel;


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
            barSelectPanel.SetActive(!barSelectPanel.activeInHierarchy);     // prepina mezi active a inactive

            colorPanel.SetActive(false);
            dropdown.SetValueWithoutNotify(-1);
        }


        if (index == 2)
        {
            colorPanel.SetActive(!colorPanel.activeInHierarchy);

            barSelectPanel.SetActive(false);
            dropdown.SetValueWithoutNotify(-1);
        }
    }
}