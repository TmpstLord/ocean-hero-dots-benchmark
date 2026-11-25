using Unity.Entities;

public struct GameDataComponent : IComponentData
{
    // Data Utama
    public float CurrentTime;       // Waktu permainan berjalan
    public int Score;               // Skor pemain saat ini
    
    // Status Permainan
    public bool IsGameOver;         // Apakah kalah?
    public bool IsWin;              // Apakah menang? <--- INI YANG HILANG TADI
    
    // Settingan
    public int TargetScore;         // Skor target untuk menang <--- INI JUGA HILANG
    
    // Debugging
    public bool TriggerStressTest;  // Untuk tombol Debug ECS <--- INI JUGA
}