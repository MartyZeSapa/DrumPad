using UnityEngine;
using UnityEngine.UI;

public class SampleChanger : MonoBehaviour
{   
    [SerializeField]
    private GameObject[] scrollViews;

    private void ActivateViewport(int scrollViewIndex)
    {
        foreach (GameObject scrollView in scrollViews)
        {
            scrollView.SetActive(false);
        }

        scrollViews[scrollViewIndex].SetActive(true);
    }
}
