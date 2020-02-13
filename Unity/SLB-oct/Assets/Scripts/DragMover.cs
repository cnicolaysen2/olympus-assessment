using UnityEngine;
using UnityEngine.EventSystems;

public class DragMover : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RectTransform Target;

    public Vector2 Sensivity;
    private Vector2 PointPosition;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset = (eventData.position - PointPosition) * Sensivity;

        Target.position += new Vector3(offset.x, offset.y);
        PointPosition = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointPosition = eventData.position;
    }
}
