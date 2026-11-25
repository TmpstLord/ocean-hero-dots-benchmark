# Ocean Hero — Unity DOTS/ECS vs OOP Performance Benchmark

Game arcade edukasi lingkungan 2D berbasis Android yang dikembangkan menggunakan **Unity Engine (C#)** sebagai studi komparasi arsitektur performa antara **Data-Oriented Technology Stack (DOTS / Entities / Burst Compiler)** dan pendekatan tradisional **Object-Oriented Programming (OOP / MonoBehaviour)**.

## Fokus Penelitian & Arsitektur Teknis
* **Data-Oriented Design (ECS):** Pemisahan murni antara data komponen (`IComponentData`) dan logika pemrosesan (`ISystem` / `SystemBase`) untuk memaksimalkan *CPU cache locality* dan eksekusi paralel pada perangkat mobile Android.
* **OOP Baseline Implementation:** Implementasi pembanding berbasis `MonoBehaviour` dengan skenario *spawning* dan pergerakan entitas yang identik untuk mengukur selisih beban CPU, alokasi memori, serta stabilitas *frame rate* (FPS).
* **Benchmarking & Profiling:** Pengujian metrik performa (*Frame Time*, *FPS*, *Draw Calls*, dan *Memory Usage*) menggunakan Unity Profiler pada berbagai skenario kepadatan entitas.

## Struktur Proyek Unity
* `Assets/` — Kode sumber C# (ECS Components, Systems, Authoring/Baking, serta pembanding MonoBehaviour), *scene* permainan, prefab, dan aset visual 2D.
* `Packages/` — Daftar dependensi resmi Unity (`manifest.json` & `packages-lock.json`, termasuk paket `com.unity.entities` dan `com.unity.burst`).
* `ProjectSettings/` — Konfigurasi proyek Unity, *Physics 2D*, *Quality Settings*, dan target *build* Android.

## Cara Membuka Proyek
1. *Clone* atau unduh repositori ini.
2. Buka **Unity Hub**, klik **Add → Add project from disk**, dan arahkan ke folder proyek ini.
3. Gunakan versi Unity Editor yang sesuai dengan `ProjectSettings/ProjectVersion.txt` agar paket DOTS/Entities terpasang otomatis.
