using Unity.Entities;
using UnityEngine;

public class ShipAuthoring : MonoBehaviour
{
    public float MoveSpeed = 5f;
    
    [Header("Game Rules")]
    public float GameDuration = 60f; // Waktu main 60 detik
    public int WinScoreTarget = 100; // Harus kumpul 100 poin

    class Baker : Baker<ShipAuthoring>
    {
        public override void Bake(ShipAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // Komponen Gerak
            AddComponent(entity, new MoveSpeedComponent { Value = authoring.MoveSpeed });
            AddComponent(entity, new PlayerInputComponent());

            // Komponen Skor (Dompet)
            AddComponent(entity, new ScoreComponent { Value = 0 });

            // --- DATA ATURAN GAME BARU ---
            AddComponent(entity, new GameDataComponent 
            { 
                CurrentTime = authoring.GameDuration,
                TargetScore = authoring.WinScoreTarget,
                IsGameOver = false,
                IsWin = false
            });
        }
    }
}