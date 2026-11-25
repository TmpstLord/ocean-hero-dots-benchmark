using Unity.Entities;

public struct TrashSpawnerComponent : IComponentData
{
    public Entity Prefab;       // Data Entity sampah (buat backup)
    public float SpawnInterval; // Jeda waktu spawn
    public float Timer;         // Penghitung waktu mundur
    public int AmountToSpawn; // <--- TAMBAHKAN BARIS INI (Wajib!)
}