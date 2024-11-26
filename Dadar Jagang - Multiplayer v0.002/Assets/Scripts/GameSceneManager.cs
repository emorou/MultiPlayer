using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class GameSceneManager : NetworkBehaviour
{
    public static GameSceneManager instance;
    public GameObject playerPrefab; 
    public GameObject bullet;
    public List<Transform> spawnPost; 
    private int currentSpawnIndex = 0;

private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
{
    if (IsHost)
    {
        SpawnPlayerForHost();
        Debug.Log("isHost");
    }
    else if (IsClient)
    {
        RequestSpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        Debug.Log("isClient");
    }

    GameManager.instance.StartGame();
}

[ServerRpc(RequireOwnership = false)]
private void RequestSpawnPlayerServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
{
    Debug.Log($"Spawn request received for client {clientId}");
    SpawnPlayer(clientId);
}

private void SpawnPlayer(ulong clientId)
{
    if (playerPrefab != null)
    {
        Debug.Log($"Spawning player for client {clientId}");
        Vector3 spawnPosition = GetSpawnPost().position + Vector3.up * 1f;
        Quaternion spawnRotation = GetSpawnPost().rotation;
        GameObject player = Instantiate(playerPrefab, spawnPosition, spawnRotation);

        Debug.Log(spawnPosition.ToString());
        // Assign ownership and spawn player object
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        
        currentSpawnIndex+=1;
    }
}

    private void SpawnPlayerForHost()
    {
        if (playerPrefab != null)
        {
            Debug.Log("host enter");
            Vector3 spawnPosition = GetSpawnPost().position + Vector3.up * 1f;
            Quaternion spawnRotation = GetSpawnPost().rotation;
            GameObject player = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            Debug.Log(spawnPosition.ToString());
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
            
        currentSpawnIndex+=1;
        }
    }
[ServerRpc(RequireOwnership = false)]
public void RequestSpawnBulletServerRpc(Vector3 post, Vector4 rotation, ulong clientId, ServerRpcParams serverRpcParams = default)
{
    SpawnBullet(post, rotation);
}

private void SpawnBullet(Vector3 post, Vector4 rotation)
{
    Quaternion rot = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);

        GameObject player = Instantiate(bullet, post, rot);

        player.GetComponent<NetworkObject>().Spawn(false);
        
        currentSpawnIndex+=1;
  
}

    private Transform GetSpawnPost()
    {
        return spawnPost[currentSpawnIndex];
    }
}
