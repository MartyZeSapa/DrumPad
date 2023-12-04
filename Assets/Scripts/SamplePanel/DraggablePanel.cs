using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; // Origin�ln�ho panelu
    private CanvasGroup canvasGroup; // -//- (pro zm�nu Alphy)

    private GameObject clonePanel;
    private Canvas canvas;
    private RectTransform canvasRectTransform;

    private float dragStartTime;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>(); // Projede v�echny rodi�e dokud nenajde Canvas
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }
    }





    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartTime = Time.time;  // Nastav� dragSTartTime na aktu�ln� �as p�o m��en� d�lky dragu
        
        clonePanel = Instantiate(gameObject, canvas.transform, false); // Vytvo�� klon origin�lu (Instantiate (jak� gameObject, pod jak�m rodi�em, pozice relative to parent)) 
        RectTransform cloneRectTransform = clonePanel.GetComponent<RectTransform>();

        cloneRectTransform.sizeDelta = rectTransform.sizeDelta; // Nastav� velikost a scale jako orig. panel
        cloneRectTransform.localScale = rectTransform.localScale;

        CanvasGroup cloneCanvasGroup = clonePanel.GetComponent<CanvasGroup>();
        cloneCanvasGroup.alpha = 0.6f; // Klon semi-transparentn�
        cloneCanvasGroup.blocksRaycasts = false; // Neblokuje raycasty, aby i UI elementy pod klonem zaznamenali user input

        canvasGroup.alpha = 0.6f; // Origin�l semi-transparentn�

        UpdateClonePosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateClonePosition(eventData);
    }

    private void UpdateClonePosition(PointerEventData eventData)
    {
        if (clonePanel != null && canvasRectTransform != null)
        {
            // P�evede pozici my�i na obrazovce na lokaln� pozici relativn� k rectTransform canvasu (ulo�� se do globalMousePos)
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localMousePos))
            {
                clonePanel.transform.localPosition = localMousePos;
            }
        }
    }
    

    public void OnEndDrag(PointerEventData eventData)
    {

        float dragDuration = Time.time - dragStartTime;

        if (dragDuration < 0.1f) // Pokud pouze kliknu, p�ehraje zvuk
        {
            GetComponent<SoundData>()?.PlaySound();
        }


        // Zni�� klon
        if (clonePanel != null)
        {
            Destroy(clonePanel);
        }

        canvasGroup.alpha = 1f; // Vr�t� alphu origin�ln�ho prvku
    }
}