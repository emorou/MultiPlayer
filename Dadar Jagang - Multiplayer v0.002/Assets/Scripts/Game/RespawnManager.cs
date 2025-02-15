using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class RespawnManager : NetworkBehaviour
{
    public static RespawnManager Instance;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Respawn(GameObject playerObject, float delay = 3f)
    {
        if (!IsServer)
        {
            Debug.LogWarning("[RespawnManager] Hanya server yang dapat memicu respawn.");
            return;
        }

        Transform randomSpawnPoint = GetRandomSpawnPoint();

        if (randomSpawnPoint != null)
        {
            StartCoroutine(RespawnCoroutine(playerObject, randomSpawnPoint, delay));
        }
        else
        {
            Debug.LogError("[RespawnManager] Tidak ada spawn points yang tersedia.");
        }
    }

    private IEnumerator RespawnCoroutine(GameObject playerObject, Transform spawnPoint, float delay)
    {
        if (playerObject == null || spawnPoint == null)
        {
            Debug.LogError("[RespawnManager] PlayerObject atau SpawnPoint null!");
            yield break;
        }

        playerObject.SetActive(false);

        yield return new WaitForSeconds(delay);

        // Pindahkan objek di server
        playerObject.transform.position = spawnPoint.position;
        playerObject.transform.rotation = spawnPoint.rotation;

        // Aktifkan kembali di server
        playerObject.SetActive(true);

        // Sinkronisasi posisi dan status ke client
        RespawnClientRpc(playerObject.GetComponent<NetworkObject>().NetworkObjectId, spawnPoint.position, spawnPoint.rotation);

        Debug.Log($"[RespawnManager] {playerObject.name} berhasil respawn di {spawnPoint.position}.");
    }

    [ClientRpc]
    private void RespawnClientRpc(ulong networkObjectId, Vector3 position, Quaternion rotation)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject networkObject))
        {
            GameObject playerObject = networkObject.gameObject;

            playerObject.transform.position = position;
            playerObject.transform.rotation = rotation;

            if (!playerObject.activeSelf)
            {
                playerObject.SetActive(true);
            }

            Debug.Log($"[RespawnManager] {playerObject.name} disinkronkan ke posisi respawn {position} di client.");
        }
        else
        {
            Debug.LogWarning($"[RespawnManager] Tidak dapat menemukan NetworkObject dengan ID {networkObjectId}.");
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0) return null;

        int randomIndex = Random.Range(0, spawnPoints.Count);
        return spawnPoints[randomIndex];
    }
}
