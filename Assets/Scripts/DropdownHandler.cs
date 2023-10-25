using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject Mode1;
    public GameObject Mode2;
    public GameObject Mode3;
    public SwitchScenes Script;

    public void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnDropdownValueChanged(int index)
    {
        if (index == 0)
        {
            
        }

        if (index == 1)
        {

        }

        if (index == 2)
        {

        }

        if (index == 3)
        {
            if (Script.lastMode == true)
            {
                Mode3.SetActive(true);
                Mode1.SetActive(false);
            }
            else
            {
                Mode3.SetActive(true);
                Mode2.SetActive(false);
            }
        }

        if (index == 4)
        {

        }

        if (index == 5)
        {

        }
    }
}
