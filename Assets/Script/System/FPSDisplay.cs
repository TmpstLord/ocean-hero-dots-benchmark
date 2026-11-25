using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    void Update()
    {
        // Hitung rata-rata waktu per frame agar angkanya stabil
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width;
        int h = Screen.height;

        GUIStyle style = new GUIStyle();

        // --- SETTING POSISI & FONT ---
        // Bikin kotak seukuran lebar layar, tinggi 10% layar
        Rect rect = new Rect(0, 0, w, h * 0.1f); 
        
        // Ratakan Tengah Atas
        style.alignment = TextAnchor.UpperCenter; 
        
        // Ukuran font responsif (3.5% dari tinggi layar)
        style.fontSize = h * 4 / 100; 
        
        // Warna Teks (Kuning terang biar kontras)
        style.normal.textColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);

        // --- HITUNG FPS ---
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} FPS)", msec, fps);

        // --- EFEK BAYANGAN (SHADOW) ---
        // Biar kebaca jelas walau backgroundnya botol warna-warni
        GUIStyle shadowStyle = new GUIStyle(style);
        shadowStyle.normal.textColor = Color.black;
        
        // Gambar bayangan dulu (digeser sedikit ke bawah kanan)
        Rect shadowRect = new Rect(rect);
        shadowRect.x += 2;
        shadowRect.y += 2;
        GUI.Label(shadowRect, text, shadowStyle);

        // --- GAMBAR TEKS ASLI ---
        GUI.Label(rect, text, style);
    }
}