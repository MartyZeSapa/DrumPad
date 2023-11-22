using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;


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

            dropdown.SetValueWithoutNotify(-1);
        }


        if (index == 2)
        {

            dropdown.SetValueWithoutNotify(-1);
        }
    }
}