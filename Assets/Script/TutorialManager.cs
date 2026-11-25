using UnityEngine;
using TMPro; // Wajib untuk TextMeshPro
using System.Collections; // Wajib untuk Coroutine

public class TutorialManager : MonoBehaviour
{
    [Header("UI Utama")]
    [Tooltip("Panel kayu besar yang berisi slide panduan")]
    public GameObject infoPanelUtama; 
    
    [Tooltip("Teks angka 3, 2, 1 yang muncul di tengah layar")]
    public TextMeshProUGUI teksHitungMundur; 

    [Header("Daftar Halaman Panduan")]
    [Tooltip("Masukkan semua objek slide/halaman panduan ke sini")]
    public GameObject[] slides;

    [Header("Tombol Kontrol")]
    public GameObject tombolKiri;
    public GameObject tombolKanan;
    public GameObject tombolMulai;

    private int currentIndex = 0;

    void Start()
    {
        // 1. BEKUKAN WAKTU (Agar game ECS di belakang tidak jalan dulu)
        Time.timeScale = 0f; 
        
        // 2. Sembunyikan teks hitung mundur di awal agar tidak menghalangi
        if (teksHitungMundur != null) 
        {
            teksHitungMundur.gameObject.SetActive(false);
        }

        // 3. Reset index slide ke nol
        currentIndex = 0;
        UpdateUI();
    }

    public void NextSlide()
    {
        // Cek apakah array slides valid
        if (slides == null || slides.Length == 0) return;

        if (currentIndex < slides.Length - 1)
        {
            PlayClickSound();
            currentIndex++;
            UpdateUI();
        }
    }

    public void PrevSlide()
    {
        // Cek apakah array slides valid
        if (slides == null || slides.Length == 0) return;

        if (currentIndex > 0)
        {
            PlayClickSound();
            currentIndex--;
            UpdateUI();
        }
    }

    // FUNGSI INI DIPANGGIL SAAT TOMBOL 'START' DI SLIDE TERAKHIR DITEKAN
    public void MulaiGame()
    {
        PlayClickSound();

        // Matikan panel panduan
        if (infoPanelUtama != null) 
        {
            infoPanelUtama.SetActive(false);
        }
        
        // Mulai coroutine hitung mundur
        StartCoroutine(HitungMundurCoroutine());
    }

    // Fungsi pembantu agar kode lebih rapi dan tidak duplikasi
    private void PlayClickSound()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.clickClip != null) 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.clickClip);
        }
    }

    // --- LOGIKA HITUNG MUNDUR (3... 2... 1... MULAI!) ---
    private IEnumerator HitungMundurCoroutine()
    {
        if (teksHitungMundur == null)
        {
            Debug.LogWarning("Peringatan: Teks Hitung Mundur belum dipasang di Inspector!");
            Time.timeScale = 1f; // Langsung jalankan game saja
            yield break;
        }

        teksHitungMundur.gameObject.SetActive(true);

        // Angka 3
        teksHitungMundur.text = "3";
        yield return new WaitForSecondsRealtime(1f); // Pakai Realtime karena timescale masih 0

        // Angka 2
        teksHitungMundur.text = "2";
        yield return new WaitForSecondsRealtime(1f);

        // Angka 1
        teksHitungMundur.text = "1";
        yield return new WaitForSecondsRealtime(1f);

        // MULAI!
        teksHitungMundur.text = "MULAI!";
        yield return new WaitForSecondsRealtime(0.5f);

        teksHitungMundur.gameObject.SetActive(false);

        // JALANKAN WAKTU GAME (ECS akan mulai memproses pergerakan)
        Time.timeScale = 1f;
    }

    private void UpdateUI()
    {
        if (slides == null || slides.Length == 0) return;

        // Loop untuk menyalakan slide yang aktif saja
        for (int i = 0; i < slides.Length; i++)
        {
            if (slides[i] != null)
            {
                slides[i].SetActive(i == currentIndex);
            }
        }

        // Atur visibilitas tombol navigasi
        if (tombolKiri != null) tombolKiri.SetActive(currentIndex > 0);
        
        if (currentIndex == slides.Length - 1)
        {
            if (tombolKanan != null) tombolKanan.SetActive(false); 
            if (tombolMulai != null) tombolMulai.SetActive(true);  
        }
        else 
        {
            if (tombolKanan != null) tombolKanan.SetActive(true);  
            if (tombolMulai != null) tombolMulai.SetActive(false); 
        }
    }
}