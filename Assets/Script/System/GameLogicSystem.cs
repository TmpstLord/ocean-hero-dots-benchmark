using Unity.Entities;
using UnityEngine;

public partial class GameLogicSystem : SystemBase
{
    protected override void OnStartRunning()
    {
        // --- PAKSA RESET DATA SAAT GAME MULAI ---
        if (SystemAPI.TryGetSingletonRW<GameDataComponent>(out RefRW<GameDataComponent> gameData))
        {
            gameData.ValueRW.IsGameOver = false;
            gameData.ValueRW.IsWin = false;
            gameData.ValueRW.CurrentTime = 60f; // Reset waktu (sesuaikan maumu)
            gameData.ValueRW.Score = 0;         // Reset skor
            
            Debug.Log("Game Data Di-Reset!");
        }
    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonRW<GameDataComponent>(out RefRW<GameDataComponent> gameData))
            return;

        // Jangan jalankan logika kalau sudah Game Over
        if (gameData.ValueRO.IsGameOver) return;

        float dt = SystemAPI.Time.DeltaTime;

        // 1. Kurangi Waktu
        gameData.ValueRW.CurrentTime -= dt;

        // 2. Cek Kalah (Waktu Habis)
        if (gameData.ValueRO.CurrentTime <= 0)
        {
            gameData.ValueRW.CurrentTime = 0;
            gameData.ValueRW.IsGameOver = true;
            Debug.Log("GAME OVER: Waktu Habis!");
        }

        // 3. Cek Menang (Skor Tercapai - Opsional)
        if (gameData.ValueRO.Score >= gameData.ValueRO.TargetScore && gameData.ValueRO.TargetScore > 0)
        {
            gameData.ValueRW.IsGameOver = true;
            gameData.ValueRW.IsWin = true;
            Debug.Log("YOU WIN!");
        }
    }
}