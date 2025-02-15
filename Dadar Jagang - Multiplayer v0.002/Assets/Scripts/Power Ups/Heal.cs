using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int heal; // Jumlah heal yang diberikan
    public float respawnTime = 60f; // Waktu untuk kembali aktif setelah nonaktif

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hostile")
        {
            HostileMark hostile = other.GetComponent<HostileMark>();
            if (hostile != null)
            {
                hostile.HealHostile(heal); // Panggil fungsi heal
                StartCoroutine(DeactivateAndReactivate()); // Nonaktifkan sementara
            }
            else
            {
                Debug.LogWarning("[Health] Objek dengan tag 'Hostile' tidak memiliki komponen HostileMark!");
            }
        }
    }

    private IEnumerator DeactivateAndReactivate()
    {
        // Nonaktifkan objek health
        gameObject.SetActive(false);

        // Tunggu selama respawnTime
        yield return new WaitForSeconds(respawnTime);

        // Aktifkan kembali objek health
        gameObject.SetActive(true);
        Debug.Log("[Health] Objek health telah kembali aktif.");
    }
}
