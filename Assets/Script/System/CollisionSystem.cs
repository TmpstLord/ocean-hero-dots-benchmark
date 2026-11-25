using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
// using Unity.Burst; // KITA HAPUS ATAU MATIKAN INI

// HAPUS [BurstCompile] di atas struct ini agar kita bisa memanggil AudioManager (Managed Code)
public partial struct CollisionSystem : ISystem
{
    // HAPUS [BurstCompile] di sini juga
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonRW<GameDataComponent>(out RefRW<GameDataComponent> gameData)) return;
        if (!SystemAPI.TryGetSingletonEntity<PlayerInputComponent>(out Entity playerEntity)) return;

        float3 playerPos3D = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        RefRW<ScoreComponent> playerScore = SystemAPI.GetComponentRW<ScoreComponent>(playerEntity);

        // Alat penghancur yang aman
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        float hitRadius = 0.8f; 

        // =========================================================
        // 1. NABRAK SAMPAH
        // =========================================================
        foreach (var (transform, trashData, entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<TrashComponent>>().WithEntityAccess())
        {
            if (math.distance(playerPos3D.xy, transform.ValueRO.Position.xy) < hitRadius)
            {
                // Tambah Skor
                playerScore.ValueRW.Value += trashData.ValueRO.ScoreValue;
                gameData.ValueRW.Score += trashData.ValueRO.ScoreValue;

                // ---> PANGGIL EFEK SUARA AMBIL SAMPAH <---
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.collectTrashClip);
                }

                // Hancurkan Entitas
                ecb.DestroyEntity(entity);
            }
        }

        // =========================================================
        // 2. NABRAK RINTANGAN (IKAN/BATU)
        // =========================================================
        foreach (var (transform, obstacleData, entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<ObstacleComponent>>().WithEntityAccess())
        {
            if (math.distance(playerPos3D.xy, transform.ValueRO.Position.xy) < hitRadius)
            {
                // Hitung Penalti
                if (obstacleData.ValueRO.Type == ObstacleType.TimePenalty)
                {
                    gameData.ValueRW.CurrentTime -= obstacleData.ValueRO.Value;
                }
                else if (obstacleData.ValueRO.Type == ObstacleType.ScorePenalty)
                {
                    playerScore.ValueRW.Value -= obstacleData.ValueRO.Value;
                    gameData.ValueRW.Score -= obstacleData.ValueRO.Value;

                    if (playerScore.ValueRW.Value < 0) playerScore.ValueRW.Value = 0;
                    if (gameData.ValueRW.Score < 0) gameData.ValueRW.Score = 0;
                }

                // ---> PANGGIL EFEK SUARA NABRAK JEBAKAN <---
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.hitObstacleClip);
                }

                // Hancurkan Entitas
                ecb.DestroyEntity(entity);
            }
        }
    }
}