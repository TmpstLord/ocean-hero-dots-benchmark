using Unity.Entities;
using Unity.Mathematics;

public struct PlayerInputComponent : IComponentData
{
    public float2 MovementInput; // x untuk kiri/kanan
    public bool IsMoving;        // Sesuai atribut {isMoving} di diagram
}