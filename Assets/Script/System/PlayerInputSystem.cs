using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine; 

public partial class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // --- BAGIAN 1: BACA INPUT (JOYSTICK & BOOST) ---
        
        // A. Ambil Input Arah dari Joystick
        float2 moveInput = float2.zero;
        if (SimpleJoystick.Instance != null)
        {
            Vector2 joyDir = SimpleJoystick.Instance.InputDirection;
            moveInput = new float2(joyDir.x, joyDir.y);
        }

        // B. Ambil Status Tombol Boost (Ditekan atau Tidak)
        bool isBoosting = false;
        if (BoostButton.Instance != null)
        {
            isBoosting = BoostButton.Instance.IsPressed;
        }

        float deltaTime = SystemAPI.Time.DeltaTime;

        // --- BAGIAN 2: TERAPKAN KE KAPAL ---

        foreach (var (transform, speed, inputComponent) in 
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeedComponent>, RefRW<PlayerInputComponent>>())
        {
            // Cek apakah ada input gerak?
            if (math.lengthsq(moveInput) > 0.01f)
            {
                // -- HITUNG KECEPATAN --
                // Jika tombol boost ditekan, kecepatan dikali 2.5. Jika tidak, pakai kecepatan normal.
                float speedMultiplier = isBoosting ? 2.5f : 1.0f;
                float finalSpeed = speed.ValueRO.Value * speedMultiplier;

                // -- PROSES GERAK --
                float3 movement = new float3(moveInput.x, moveInput.y, 0) * finalSpeed * deltaTime;
                transform.ValueRW.Position += movement;

                // -- PROSES ROTASI --
                float angle = math.atan2(moveInput.y, moveInput.x) - math.radians(90);
                transform.ValueRW.Rotation = quaternion.Euler(0, 0, angle);

                inputComponent.ValueRW.IsMoving = true;
            }
            else
            {
                inputComponent.ValueRW.IsMoving = false;
            }

            // --- BAGIAN 3: TEMPLATE PEMBATAS LAYAR (CLAMPING) ---
            
            float3 pos = transform.ValueRW.Position;

            // ==========================================================
            // [AREA ISI DATA] -> MASUKKAN ANGKA BATAS LAYAR DI SINI
            // ==========================================================
            
            float batasKiri   = -5.06f;  // Ganti dengan angka X paling KIRI
            float batasKanan  =  5.11f;  // Ganti dengan angka X paling KANAN
            float batasBawah  = -2.63f;  // Ganti dengan angka Y paling BAWAH
            float batasAtas   =  2.69f;  // Ganti dengan angka Y paling ATAS

            // ==========================================================

            // Penerapan Rumus Matematika (Jangan Diubah)
            pos.x = math.clamp(pos.x, batasKiri, batasKanan);
            pos.y = math.clamp(pos.y, batasBawah, batasAtas);

            // Kembalikan posisi yang sudah dibatasi ke Entity
            transform.ValueRW.Position = pos;
        }
    }
}