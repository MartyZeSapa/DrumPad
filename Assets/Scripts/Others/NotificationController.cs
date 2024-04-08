using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    public static NotificationController Instance;
    public GameObject popupPrefab;

    public Color backgroundColor;

    [SerializeField]
    private Image Background;

    public Canvas canvas;



    void Awake()
    {
        if (Instance != null && Instance != this)   // Singleton
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        backgroundColor = Background.color;
            
    }



    public void ShowNotification(string message)   // Vytvo�� a uk�e Popup se specifickou zpr�vou
    {
        
        GameObject newPopupInstance = Instantiate(popupPrefab, canvas.transform);  // Zalo�� novou instanci popupu z prefabu


        TextMeshProUGUI popupText = newPopupInstance.GetComponentInChildren<TextMeshProUGUI>();    // Nastav� text popupu
        popupText.text = message;

        
        Transform stripeImageTransform = newPopupInstance.transform.Find("stripeImage");   // Nastav� barvu pruhu na barvu pozad�
        if (stripeImageTransform != null)
        {
            Image stripeImage = stripeImageTransform.GetComponent<Image>();
            if (stripeImage != null)
            {
                stripeImage.color = backgroundColor;
            }
        }




        
        Animator popupAnimator = newPopupInstance.GetComponent<Animator>();    // Spust� animaci objeven� popupu
        popupAnimator.Play("PopupAppear");




        StartCoroutine(DismissPopup(newPopupInstance, popupAnimator));     // Spust� Coroutine kter� odebere popupu po delayi
    }

    private IEnumerator DismissPopup(GameObject popupInstance, Animator popupAnimator)
    {

        yield return new WaitForSeconds(2.0f);

        popupAnimator.Play("PopupDisappear");   // Spust� animaci zmizen� popupu

        yield return new WaitForSeconds(1.0f);

        Destroy(popupInstance); // Zni�� instanci popupu
    }
}
