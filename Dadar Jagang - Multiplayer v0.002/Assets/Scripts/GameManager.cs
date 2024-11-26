using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool IsHost { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartHost()
    {
        IsHost = true;
        NetworkManager.Singleton.StartHost();
        // Pindah ke GameScene
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void StartClient()
    {
        IsHost = false;
        NetworkManager.Singleton.StartClient();
        // Pindah ke GameScene
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToLobby()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
