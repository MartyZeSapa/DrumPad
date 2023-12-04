using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; // Originálního panelu
    private CanvasGroup canvasGroup; // -//- (pro zmìnu Alphy)

    private GameObject clonePanel;
    private Canvas canvas;
    private RectTransform canvasRectTransform;

    private float dragStartTime;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>(); // Projede všechny rodièe dokud nenajde Canvas
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }
    }





    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartTime = Time.time;  // Nastaví dragSTartTime na aktuální èas pøo mìøení délky dragu
        
        clonePanel = Instantiate(gameObject, canvas.transform, false); // Vytvoøí klon originálu (Instantiate (jaký gameObject, pod jakým rodièem, pozice relative to parent)) 
        RectTransform cloneRectTransform = clonePanel.GetComponent<RectTransform>();

        cloneRectTransform.sizeDelta = rectTransform.sizeDelta; // Nastaví velikost a scale jako orig. panel
        cloneRectTransform.localScale = rectTransform.localScale;

        CanvasGroup cloneCanvasGroup = clonePanel.GetComponent<CanvasGroup>();
        cloneCanvasGroup.alpha = 0.6f; // Klon semi-transparentní
        cloneCanvasGroup.blocksRaycasts = false; // Neblokuje raycasty, aby i UI elementy pod klonem zaznamenali user input

        canvasGroup.alpha = 0.6f; // Originál semi-transparentní

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
            // Pøevede pozici myši na obrazovce na lokalní pozici relativní k rectTransform canvasu (uloží se do globalMousePos)
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localMousePos))
            {
                clonePanel.transform.localPosition = localMousePos;
            }
        }
    }
    

    public void OnEndDrag(PointerEventData eventData)
    {

        float dragDuration = Time.time - dragStartTime;

        if (dragDuration < 0.1f) // Pokud pouze kliknu, pøehraje zvuk
        {
            GetComponent<SoundData>()?.PlaySound();
        }


        // Znièí klon
        if (clonePanel != null)
        {
            Destroy(clonePanel);
        }

        canvasGroup.alpha = 1f; // Vrátí alphu originálního prvku
    }
}