using UnityEngine;

public class HostileMark : MonoBehaviour
{
    public int currentHealth = 100; // Health awal
    public int maxHealth = 100; // Health maksimum
    private RespawnManager respawnManager; // Referensi ke RespawnManager

    private void Start()
    {
        // Cari RespawnManager di scene
        respawnManager = RespawnManager.Instance;

        if (respawnManager == null)
        {
            Debug.LogError("[HostileMark] RespawnManager tidak ditemukan di scene!");
        }
    }

    // Fungsi untuk menerima damage
    public void DamageHostile(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"[HostileMark] Hostile menerima damage {damage}. Health tersisa: {currentHealth}");

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    // Fungsi untuk menerima heal
    public void HealHostile(int healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healAmount;

            // Pastikan health tidak melebihi batas maksimum
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            Debug.Log($"[HostileMark] Hostile menerima heal {healAmount}. Health sekarang: {currentHealth}");
        }
        else
        {
            Debug.Log("[HostileMark] Health sudah penuh, heal tidak diperlukan.");
        }
    }

    // Tangani kematian hostile
    private void HandleDeath()
    {
        if (respawnManager != null)
        {
            Debug.Log("[HostileMark] Hostile akan respawn.");

            // Panggil respawn melalui RespawnManager
            respawnManager.Respawn(gameObject, 3f);
        }
        else
        {
            Debug.LogWarning("[HostileMark] RespawnManager tidak tersedia, objek akan tetap nonaktif.");
            gameObject.SetActive(false);
        }
    }
}
