using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk fungsi tombol BACK

public class DebugSpawner : MonoBehaviour
{
    // =========================================================
    // 1. OBAT UNTUK BUG KAPAL DIAM (Fungsi ini otomatis jalan saat scene dibuka)
    // =========================================================
    void Awake()
    {
        // Memaksa waktu berjalan normal (1.0) setiap kali scene ini dimuat
        // Ini mengatasi bug kapal diam dari Main Menu
        Time.timeScale = 1f;
    }

    // =========================================================
    // 2. FUNGSI ASLI ANDA UNTUK TOMBOL "DEBUG ECS"
    // =========================================================
    public void SpawnMassiveTrash()
    {
        // 1. Ambil World ECS Default
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // 2. Buat Query untuk mencari GameData
        var query = entityManager.CreateEntityQuery(typeof(GameDataComponent));

        // 3. Cek apakah GameData ada?
        if (!query.IsEmptyIgnoreFilter)
        {
            var entity = query.GetSingletonEntity();
            var data = entityManager.GetComponentData<GameDataComponent>(entity);

            // 4. HIDUPKAN TRIGGER!
            data.TriggerStressTest = true;

            // 5. Simpan perubahan
            entityManager.SetComponentData(entity, data);

            Debug.Log("⚠️ BENCHMARK STARTED: 500 Entitas sedang dikirim...");
        }
        else
        {
            Debug.LogWarning("❌ GameData tidak ditemukan! Pastikan Game sudah Play.");
        }
    }

    // =========================================================
    // 3. FUNGSI BARU UNTUK TOMBOL "BACK" (KEMBALI KE MENU)
    // =========================================================
    public void BackToMainMenu()
    {
        // Pastikan waktu normal sebelum pindah scene
        Time.timeScale = 1f; 
        
        // Ganti "MainMenu" dengan nama persis scene menu utama Anda (perhatikan huruf besar/kecilnya)
        SceneManager.LoadScene("MainMenu"); 
    }
}