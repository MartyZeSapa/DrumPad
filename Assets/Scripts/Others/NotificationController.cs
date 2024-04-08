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



    public void ShowNotification(string message)   // Vytvoøí a ukáže Popup se specifickou zprávou
    {
        
        GameObject newPopupInstance = Instantiate(popupPrefab, canvas.transform);  // Založí novou instanci popupu z prefabu


        TextMeshProUGUI popupText = newPopupInstance.GetComponentInChildren<TextMeshProUGUI>();    // Nastaví text popupu
        popupText.text = message;

        
        Transform stripeImageTransform = newPopupInstance.transform.Find("stripeImage");   // Nastaví barvu pruhu na barvu pozadí
        if (stripeImageTransform != null)
        {
            Image stripeImage = stripeImageTransform.GetComponent<Image>();
            if (stripeImage != null)
            {
                stripeImage.color = backgroundColor;
            }
        }




        
        Animator popupAnimator = newPopupInstance.GetComponent<Animator>();    // Spustí animaci objevení popupu
        popupAnimator.Play("PopupAppear");




        StartCoroutine(DismissPopup(newPopupInstance, popupAnimator));     // Spustí Coroutine která odebere popupu po delayi
    }

    private IEnumerator DismissPopup(GameObject popupInstance, Animator popupAnimator)
    {

        yield return new WaitForSeconds(2.0f);

        popupAnimator.Play("PopupDisappear");   // Spustí animaci zmizení popupu

        yield return new WaitForSeconds(1.0f);

        Destroy(popupInstance); // Znièí instanci popupu
    }
}
