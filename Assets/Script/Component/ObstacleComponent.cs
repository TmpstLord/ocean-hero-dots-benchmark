using Unity.Entities;

public enum ObstacleType
{
    ScorePenalty, // Untuk Ikan (Kurang Skor)
    TimePenalty   // Untuk Batu (Kurang Waktu)
}

public struct ObstacleComponent : IComponentData
{
    public ObstacleType Type; // Jenis jebakan
    public int Value;         // Berapa poin/detik yang dikurangi
}