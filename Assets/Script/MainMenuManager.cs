using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ada untuk pindah scene

public class MainMenuManager : MonoBehaviour
{
    // Fungsi ini akan dipanggil saat tombol Play dipencet
    public void PlayGame()
    {
        // Memuat scene bernama "GameScene"
        SceneManager.LoadScene("GameScene");
    }
    public void BackMenu()
    {
        // Memuat scene bernama "GameScene"
        SceneManager.LoadScene("GameMenu");
    }
    public void StressTest()
    {
        // Memuat scene bernama "GameScene"
        SceneManager.LoadScene("Stress_Test");
    }
}