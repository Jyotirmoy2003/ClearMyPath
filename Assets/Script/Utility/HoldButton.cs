using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onReleased?.Invoke();
    }
}
