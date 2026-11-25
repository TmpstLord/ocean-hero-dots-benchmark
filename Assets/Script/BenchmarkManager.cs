using UnityEngine;
using TMPro;
using Unity.Entities;

public class BenchmarkManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject resultPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI spawnAmountText; 
    public TextMeshProUGUI resultFpsText;
    public TextMeshProUGUI resultMsText;

    [Header("Benchmark Settings")]
    public float testDuration = 20f;

    private bool isTesting = false;
    private float timer = 0f;
    private int frameCount = 0;
    private float totalTime = 0f;

    void Start()
    {
        if (resultPanel != null) resultPanel.SetActive(false);
        if (timerText != null) timerText.text = "0.00";
        if (spawnAmountText != null) spawnAmountText.text = "0"; 
    }

    void Update()
    {
        if (isTesting)
        {
            timer -= Time.deltaTime;
            frameCount++;
            totalTime += Time.deltaTime;

            if (timerText != null) timerText.text = Mathf.Max(0, timer).ToString("F2");

            if (timer <= 0)
            {
                EndBenchmark();
            }
        }
    }

    public void StartBenchmarkTest(int amount)
    {
        if (isTesting) return; 

        isTesting = true;
        timer = testDuration;
        frameCount = 0;
        totalTime = 0f;

        if (resultPanel != null) resultPanel.SetActive(false);

        if (spawnAmountText != null) 
        {
            spawnAmountText.text = amount.ToString(); 
        }

        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var requestEntity = entityManager.CreateEntity();
        entityManager.AddComponentData(requestEntity, new SpawnRequestComponent { SpawnAmount = amount });

        Debug.Log($"[ECS] Memulai tes dengan {amount} entitas!");
    }

    private void EndBenchmark()
    {
        isTesting = false;

        float avgFps = frameCount / totalTime;
        float avgMs = (totalTime / frameCount) * 1000f;

        if (resultPanel != null) resultPanel.SetActive(true);
        if (resultFpsText != null) resultFpsText.text = $"FPS: {Mathf.RoundToInt(avgFps)}";
        if (resultMsText != null) resultMsText.text = $"MS: {avgMs.ToString("F2")}";
    }

    // ====================================================
    // FUNGSI BARU UNTUK TOMBOL "KEMBALI"
    // ====================================================
    public void CloseResultPanel()
    {
        // Sembunyikan panel hasil
        if (resultPanel != null) resultPanel.SetActive(false);
        
        // Reset teks di layar (opsional, agar rapi saat dites lagi)
        if (timerText != null) timerText.text = "0.00";
        if (spawnAmountText != null) spawnAmountText.text = "0";
        
        // Pastikan status testing mati
        isTesting = false;
    }
}