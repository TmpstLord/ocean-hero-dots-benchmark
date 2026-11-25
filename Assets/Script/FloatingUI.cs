using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public float speed = 2f;
    public float height = 10f;
    Vector3 startPos;

    void Start() { startPos = transform.position; }

    void Update()
    {
        // Gerakan naik turun pakai Sinus (Matematika Ombak)
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}