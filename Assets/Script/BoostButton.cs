using UnityEngine;
using UnityEngine.EventSystems; // Wajib ada

public class BoostButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Singleton biar bisa dipanggil dari ECS
    public static BoostButton Instance;
    
    public bool IsPressed = false; // Status tombol

    void Awake()
    {
        Instance = this;
    }

    // Saat tombol ditekan
    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
    }

    // Saat tombol dilepas
    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
    }
}