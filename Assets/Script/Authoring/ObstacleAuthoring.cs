using Unity.Entities;
using UnityEngine;

public class ObstacleAuthoring : MonoBehaviour
{
    public ObstacleType obstacleType; // Pilih di Inspector: Score atau Time?
    public int penaltyAmount = 10;    // Jumlah hukuman
    public float moveSpeed = 2f;      // Kecepatan hanyut
    public float lifeTime = 5f;       // Umur (detik)

    class Baker : Baker<ObstacleAuthoring>
    {
        public override void Bake(ObstacleAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // 1. Tambah Data Jebakan
            AddComponent(entity, new ObstacleComponent
            {
                Type = authoring.obstacleType,
                Value = authoring.penaltyAmount
            });

            // 2. Tambah Kecepatan (Biar Hanyut)
            AddComponent(entity, new MoveSpeedComponent
            {
                Value = authoring.moveSpeed
            });

            // 3. Tambah Umur (Biar Hilang dalam 5 detik)
            AddComponent(entity, new LifetimeComponent
            {
                TimeRemaining = authoring.lifeTime
            });
        }
    }
}