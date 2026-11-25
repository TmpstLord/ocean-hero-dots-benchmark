using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class TrashSpawnerAuthoring : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject trashPrefab; // Backup
    public List<GameObject> trashVariations; // List 5-7 variasi sampah Kapten

    [Header("Settings")]
    public float spawnInterval = 2f;

    class Baker : Baker<TrashSpawnerAuthoring>
    {
        public override void Bake(TrashSpawnerAuthoring authoring)
        {
            // Membuat Entity untuk Spawner
            var entity = GetEntity(TransformUsageFlags.None);

            // 1. TAMBAHKAN KOMPONEN DATA
            AddComponent(entity, new TrashSpawnerComponent
            {
                // Mengonversi GameObjects menjadi Entity Prefabs
                Prefab = GetEntity(authoring.trashPrefab, TransformUsageFlags.Dynamic),
                SpawnInterval = authoring.spawnInterval,
                Timer = 0,
                AmountToSpawn = 1 // Default awal 1 agar spawn normal tidak langsung meledak
            });

            // 2. BUAT BUFFER (ARRAY) UNTUK VARIASI SAMPAH
            var buffer = AddBuffer<TrashBufferElement>(entity);

            // Masukkan semua variasi sampah dari List ke dalam Buffer ECS
            if (authoring.trashVariations != null)
            {
                foreach (var trashObj in authoring.trashVariations)
                {
                    if (trashObj != null)
                    {
                        buffer.Add(new TrashBufferElement
                        {
                            Prefab = GetEntity(trashObj, TransformUsageFlags.Dynamic)
                        });
                    }
                }
            }
        }
    }
}