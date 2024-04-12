using UnityEngine;
using UnityEngine.EventSystems;

public class BpmButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private BpmHandler bpmHandler;

    public bool raiseValue;

    private bool isPressed = false;
    private float timeSinceLastChange = 0f;
    private float delay = 0.05f; // Delay v sekund�ch mezi zm�nou hodnoty
    private float initialDelay = 0.2f; // Po��t�n� delay aby jsme se vyhnuli instantn�mu zv��en�/sn�en� hodnoty p�i kliknut�

    void Update()
    {
        if (isPressed)
        {
            timeSinceLastChange += Time.deltaTime;

            if (timeSinceLastChange >= initialDelay && timeSinceLastChange - Time.deltaTime < initialDelay)
            {
                // Prvn� update po po��te�n�m delayi - zv���/sn�� jednou
                ChangeValue();
            }
            else if (timeSinceLastChange >= delay + initialDelay)
            {
                // N�sleduj�c� updaty - pokra�uje ve zvy�ov�n�/sni�ov�n� hodnoty
                ChangeValue();
                timeSinceLastChange = initialDelay; // Resetuje �as aby z�stal delay
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
        timeSinceLastChange = 0f; // Resetuje timer p�i kliknut�
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        timeSinceLastChange = 0f;
    }
}
