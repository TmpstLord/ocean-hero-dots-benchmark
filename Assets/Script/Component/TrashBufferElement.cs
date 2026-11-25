using Unity.Entities;

// Ini adalah wadah untuk menyimpan daftar Prefab Sampah
[InternalBufferCapacity(8)] // Kapasitas awal buffer
public struct TrashBufferElement : IBufferElementData
{
    public Entity Prefab; // Setiap elemen isinya 1 Entity Prefab
}