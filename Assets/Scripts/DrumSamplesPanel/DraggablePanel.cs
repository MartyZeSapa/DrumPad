using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private GameObject clonePanel;
    private Canvas canvas;
    private RectTransform canvasRectTransform;

    private float dragStartTime;
    private Vector3 originalPosition; // Store the original position for resetting

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = FindCanvas();
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        // Store the original position when the script starts
        originalPosition = rectTransform.position;
    }

    private Canvas FindCanvas()
    {
        Transform current = transform;
        while (current != null)
        {
            Canvas foundCanvas = current.GetComponent<Canvas>();
            if (foundCanvas != null)
            {
                return foundCanvas;
            }
            current = current.parent;
        }
        return null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartTime = Time.time;
        // Create a clone of this panel
        clonePanel = Instantiate(gameObject, canvas.transform, false);
        RectTransform cloneRectTransform = clonePanel.GetComponent<RectTransform>();
        cloneRectTransform.sizeDelta = rectTransform.sizeDelta;
        cloneRectTransform.localScale = rectTransform.localScale;

        var cloneCanvasGroup = clonePanel.GetComponent<CanvasGroup>();
        cloneCanvasGroup.alpha = 0.6f; // Make the clone semi-transparent
        cloneCanvasGroup.blocksRaycasts = false; // Allow raycasts to pass through the clone

        //canvasGroup.alpha = 0.6f; // Optionally make the original semi-transparent

        // Set initial position of the clone to the mouse position
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
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                clonePanel.transform.position = globalMousePos;
                // Optionally, you might want to adjust the z-position to match the UI plane
                clonePanel.transform.position = new Vector3(globalMousePos.x, globalMousePos.y, clonePanel.transform.position.z);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float dragDuration = Time.time - dragStartTime;

        if (dragDuration < 0.001f) // Threshold for considering it a click, adjust as needed
        {
            // Play sound directly when a click is detected
            GetComponent<SoundData>()?.PlaySound();
        }

        // Reset the original position

        // Destroy the clone
        if (clonePanel != null)
        {
            Destroy(clonePanel);
        }

        canvasGroup.alpha = 1f; // Restore original panel's appearance
    }
}
