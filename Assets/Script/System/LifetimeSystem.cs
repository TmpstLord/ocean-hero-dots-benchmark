using Unity.Entities;

public partial class LifetimeSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (lifetime, entity) in SystemAPI.Query<RefRW<LifetimeComponent>>().WithEntityAccess())
        {
            // Kurangi waktu
            lifetime.ValueRW.TimeRemaining -= deltaTime;

            // Jika waktu habis, musnahkan!
            if (lifetime.ValueRO.TimeRemaining <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}