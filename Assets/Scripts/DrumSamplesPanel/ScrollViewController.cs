using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour
{
    public GameObject[] scrollViews;

    public void ActivateViewport(int scrollViewIndex)
    {
        foreach (GameObject scrollView in scrollViews)
        {
            scrollView.SetActive(false);
        }

        scrollViews[scrollViewIndex].SetActive(true);
    }
}
