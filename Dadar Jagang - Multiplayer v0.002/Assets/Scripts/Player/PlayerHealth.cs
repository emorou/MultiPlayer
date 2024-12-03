using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    public static PlayerHealth instance;
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(100); // Gunakan NetworkVariable

    public int maxHealth = 100;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }
    }

    public void DamagePlayer(int damage)
    {
        if (IsServer) // Hanya server yang memproses damage
        {
            currentHealth.Value -= damage;
            if (currentHealth.Value <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
