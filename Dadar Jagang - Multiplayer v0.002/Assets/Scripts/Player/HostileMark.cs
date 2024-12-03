using UnityEngine;

public class HostileMark : MonoBehaviour
{
    public int currentHealth = 100; // Health awal
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
