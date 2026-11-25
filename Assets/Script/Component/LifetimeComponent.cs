using Unity.Entities;

public struct LifetimeComponent : IComponentData
{
    public float TimeRemaining; // Sisa waktu hidup
}