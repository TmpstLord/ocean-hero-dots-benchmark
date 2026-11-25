using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        // Alat penghancur yang aman di akhir frame
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        // Hanya cari objek yang punya kecepatan (kecuali kapal player)
        foreach (var (transform, speed, entity) in 
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeedComponent>>()
                 .WithNone<PlayerInputComponent>()
                 .WithEntityAccess()) 
        {
            // GERAK SELALU KE KIRI
            transform.ValueRW.Position.x -= speed.ValueRO.Value * deltaTime;

            // HANCURKAN JIKA LEWAT BATAS KIRI (X < -16)
            if (transform.ValueRO.Position.x < -16f)
            {
                ecb.DestroyEntity(entity);
            }
        }
    }
}