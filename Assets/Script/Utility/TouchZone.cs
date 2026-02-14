using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchZone : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private float sensitivity = 0.15f;
    [SerializeField] private bool invertY = false;

    public UnityEvent<Vector2> onTouchLook;

    private Vector2 lastPosition;
    private bool isTouching;

    public void OnPointerDown(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        isTouching = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isTouching)
            return;

        Vector2 currentPosition = eventData.position;

        Vector2 delta = currentPosition - lastPosition;
        lastPosition = currentPosition;

        // Apply sensitivity
        delta *= sensitivity;

        // Fix vertical inversion
        if (!invertY)
            delta.y *= -1f;

        onTouchLook?.Invoke(delta);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;

        // VERY IMPORTANT: reset look input
        onTouchLook?.Invoke(Vector2.zero);
    }

    public void ListnToOnSensetivityValueChanged(Component sender,object data)
    {
        float value = (float)data;

        sensitivity = value;
    }
}
