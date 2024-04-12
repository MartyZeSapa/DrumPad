using UnityEngine;
using UnityEngine.EventSystems;

public class BpmButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private BpmHandler bpmHandler;

    public bool raiseValue;

    private bool isPressed = false;
    private float timeSinceLastChange = 0f;
    private float delay = 0.05f; // Delay v sekundách mezi zmìnou hodnoty
    private float initialDelay = 0.2f; // Poèátèní delay aby jsme se vyhnuli instantnímu zvýšení/snížení hodnoty pøi kliknutí

    void Update()
    {
        if (isPressed)
        {
            timeSinceLastChange += Time.deltaTime;

            if (timeSinceLastChange >= initialDelay && timeSinceLastChange - Time.deltaTime < initialDelay)
            {
                // První update po poèáteèním delayi - zvýší/sníží jednou
                ChangeValue();
            }
            else if (timeSinceLastChange >= delay + initialDelay)
            {
                // Následující updaty - pokraèuje ve zvyšování/snižování hodnoty
                ChangeValue();
                timeSinceLastChange = initialDelay; // Resetuje èas aby zùstal delay
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
        timeSinceLastChange = 0f; // Resetuje timer pøi kliknutí
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        timeSinceLastChange = 0f;
    }
}
