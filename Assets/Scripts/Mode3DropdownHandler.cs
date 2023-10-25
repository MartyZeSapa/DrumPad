using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Mode3DropdownHandler : MonoBehaviour
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
            if (Script.lastMode == true)
            {
                Mode1.SetActive(true);
                Mode3.SetActive(false);
            }
            else
            {
                Mode2.SetActive(true);
                Mode3.SetActive(false);
            }
        }
    }
}
