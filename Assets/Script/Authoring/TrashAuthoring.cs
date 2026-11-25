using Unity.Entities;
using Unity.Mathematics; // Wajib ditambahkan untuk float3
using UnityEngine;

public class TrashAuthoring : MonoBehaviour
{
    public float MoveSpeed = 2f;      // Kecepatan
    public int ScoreValue = 10;       // Skor

    class Baker : Baker<TrashAuthoring>
    {
        public override void Bake(TrashAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // 1. Tambah Komponen Kecepatan (INI WAJIB ADA!)
            AddComponent(entity, new MoveSpeedComponent { Value = authoring.MoveSpeed });

            // 2. Tambah Komponen Arah (Default gerak ke kiri, nanti dioverride sama Spawner)
            AddComponent(entity, new MoveDirectionComponent { Value = new float3(-1, 0, 0) });

            // 3. Tambah Komponen Tag Sampah (Supaya bisa diambil)
            AddComponent(entity, new TrashComponent { ScoreValue = authoring.ScoreValue });

            // 4. (Opsional) Tambah Umur jika mau hilang sendiri
            // AddComponent(entity, new LifetimeComponent { Value = 10.0f });
        }
    }
}