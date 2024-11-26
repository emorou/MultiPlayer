using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // Tambahkan ini untuk Netcode

public class Bullet : NetworkBehaviour
{
    public float bulletSpeed, lifetime;
    public Rigidbody theRigidbody;
    public int damage;

    void Start()
    {
        if (IsServer) // Hanya server yang menghancurkan peluru setelah waktu habis
        {
            StartCoroutine(DestroyAfterLifetime());
        }
    }

    void Update()
    {
        theRigidbody.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer && other.gameObject.tag == "Player") // Hanya server yang menangani damage
        {
            other.gameObject.GetComponent<HostileMark>().DamageHostile(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
