using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine; 

public partial class TrashSpawnerSystem : SystemBase
{
    private Unity.Mathematics.Random _random;

    protected override void OnStartRunning()
    {
        uint seed = (uint)System.Diagnostics.Stopwatch.GetTimestamp();
        if (seed == 0) seed = 1; 
        _random = new Unity.Mathematics.Random(seed);
    }

    protected override void OnUpdate()
    {
        // 1. CEK GAME OVER DAN AMBIL DATA TIMER
        float waktuBermain = 0f;
        if (SystemAPI.TryGetSingleton<GameDataComponent>(out GameDataComponent gameDataCheck))
        {
            if (gameDataCheck.IsGameOver) return; 
            
            // --- BUG FIX: HITUNG WAKTU BERDASARKAN TIMER GAME ---
            // Asumsi game kamu dimulai dari 60 detik (menghitung mundur)
            waktuBermain = 60f - gameDataCheck.CurrentTime;
            
            // Jaga-jaga agar nilainya tidak minus saat baru mulai
            if (waktuBermain < 0) waktuBermain = 0;
        }

        // 2. CEK REQUEST BENCHMARK DARI UI (Menggantikan sistem TriggerStressTest lama)
        bool triggerBenchmark = false;
        int benchmarkSpawnCount = 0;

        // Cari Entity yang membawa pesan dari tombol (SpawnRequestComponent)
        var requestQuery = SystemAPI.QueryBuilder().WithAll<SpawnRequestComponent>().Build();
        if (!requestQuery.IsEmpty)
        {
            // Ambil data jumlah yang dikirim dari tombol
            var requests = requestQuery.ToComponentDataArray<SpawnRequestComponent>(Unity.Collections.Allocator.Temp);
            benchmarkSpawnCount = requests[0].SpawnAmount;
            triggerBenchmark = true;

            // Hancurkan entitas pesan agar tidak nge-spawn ribuan objek tiap frame!
            EntityManager.DestroyEntity(requestQuery);
            requests.Dispose();
            
            Debug.Log($"🚀 MENGAKTIFKAN STRESS TEST {benchmarkSpawnCount} OBJECT!");
        }

        // =========================================================
        // FITUR: PROGRESSIVE DIFFICULTY (DIJAMIN RESET SAAT RESTART)
        // =========================================================
        // Kecepatan Dasar = 2.0f
        // Kecepatan akan bertambah seiring berjalannya waktu game
        float kecepatanSaatIni = 2.0f + (waktuBermain * 0.05f); 
        // =========================================================

        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        foreach (var (spawner, buffer) in SystemAPI.Query<RefRW<TrashSpawnerComponent>, DynamicBuffer<TrashBufferElement>>())
        {
            spawner.ValueRW.Timer -= deltaTime;
            bool timeToSpawn = spawner.ValueRW.Timer <= 0;

            if (timeToSpawn || triggerBenchmark)
            {
                if (timeToSpawn) spawner.ValueRW.Timer = spawner.ValueRO.SpawnInterval;
                
                // Tentukan jumlah spawn: ambil angka dari tombol jika benchmark, atau 1 jika spawn normal
                int spawnCount = triggerBenchmark ? benchmarkSpawnCount : 1;

                if (buffer.Length > 0)
                {
                    for (int i = 0; i < spawnCount; i++)
                    {
                        int randomIndex = _random.NextInt(0, buffer.Length);
                        Entity newEntity = ecb.Instantiate(buffer[randomIndex].Prefab);

                        // --- SETTING ANTI-OFFSIDE (Sesuaikan dengan kameramu) ---
                        float batasBawahLayar = -4.0f; 
                        float batasAtasLayar = 4.0f;   
                        float randomY = _random.NextFloat(batasBawahLayar, batasAtasLayar);
                        
                        // Posisi Spawn Kanan
                        float spawnX = 14f; 
                        if (triggerBenchmark)
                        {
                            // Saat stress test, sebar juga posisi X-nya agar tidak menumpuk di satu garis vertikal lurus
                            spawnX = _random.NextFloat(14f, 40f); 
                        }

                        // Terapkan Posisi dan Kecepatan Baru
                        ecb.SetComponent(newEntity, LocalTransform.FromPosition(new float3(spawnX, randomY, -2f)));
                        ecb.SetComponent(newEntity, new MoveSpeedComponent { Value = kecepatanSaatIni });
                    }
                }
            }
        }
    }
}