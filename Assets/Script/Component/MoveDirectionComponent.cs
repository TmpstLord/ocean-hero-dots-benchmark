using Unity.Entities;
using Unity.Mathematics;

public struct MoveDirectionComponent : IComponentData
{
    // Menyimpan vektor arah. 
    // Ke Kanan = (1, 0, 0)
    // Ke Kiri = (-1, 0, 0)
    public float3 Value; 
}