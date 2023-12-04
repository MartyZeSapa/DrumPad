using UnityEngine;
using UnityEngine.EventSystems;

public class BpmButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BpmHandler bpmHandler;
    public bool raiseValue;

    private bool isPressed = false;
    private float timeSinceLastChange = 0f;
    private float delay = 0.05f; // Delay in seconds between value changes
    private float initialDelay = 0.2f; // Initial delay to avoid immediate increment/decrement on click

    void Update()
    {
        if (isPressed)
        {
            timeSinceLastChange += Time.deltaTime;

            if (timeSinceLastChange >= initialDelay && timeSinceLastChange - Time.deltaTime < initialDelay)
            {
                // First update after initial delay - increment/decrement once
                ChangeValue();
            }
            else if (timeSinceLastChange >= delay + initialDelay)
            {
                // Subsequent updates - increment/decrement continuously
                ChangeValue();
                timeSinceLastChange = initialDelay; // Reset time to maintain delay
            }
        }
    }

    private void ChangeValue()
    {
        if (raiseValue)
            bpmHandler.IncrementBpm();
        else
            bpmHandler.DecrementBpm();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        timeSinceLastChange = 0f; // Reset timer on pointer down
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        timeSinceLastChange = 0f;
    }
}
