using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public static SimpleJoystick Instance; // Singleton biar bisa dipanggil System

    public RectTransform background;
    public RectTransform handle;
    
    [Header("Output")]
    public Vector2 InputDirection; // Ini yang akan dibaca oleh ECS

    private void Awake()
    {
        Instance = this; // Set diri sendiri sebagai boss
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / background.sizeDelta.x);
            pos.y = (pos.y / background.sizeDelta.y);

            InputDirection = new Vector2(pos.x * 2, pos.y * 2);
            InputDirection = (InputDirection.magnitude > 1.0f) ? InputDirection.normalized : InputDirection;

            // Gerakkan visual Handle
            handle.anchoredPosition = new Vector2(InputDirection.x * (background.sizeDelta.x / 2.5f), InputDirection.y * (background.sizeDelta.y / 2.5f));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}