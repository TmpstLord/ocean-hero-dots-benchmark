using Unity.Entities;

// Pastikan namanya TrashComponent (bukan TrashTag)
public struct TrashComponent : IComponentData
{
    public int ScoreValue;
}