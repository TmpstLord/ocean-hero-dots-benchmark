using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Membuat Singleton agar mudah dipanggil dari ECS System
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source untuk Efek Suara")]
    public AudioSource sfxSource;

    [Header("Koleksi Suara (Audio Clips)")]
    public AudioClip clickClip;
    public AudioClip collectTrashClip;
    public AudioClip hitObstacleClip;

    private void Awake()
    {
        // Setup Singleton & Jadikan Kebal Pindah Scene
        if (Instance == null)
        {
            Instance = this;
            // PERINTAH SAKTI: Objek ini tidak akan hancur saat pindah scene!
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // Jika pindah ke scene yang sudah ada AudioManager-nya, hancurkan yang baru
            // agar lagunya tidak tumpang tindih (double)
            Destroy(gameObject);
        }
    }

    // Fungsi ini yang akan dipanggil oleh ECS Collision System dan Tombol UI
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}