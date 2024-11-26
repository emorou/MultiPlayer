using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class LobbyManager : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;

    private void Start()
    {
        hostButton.onClick.AddListener(OnHostButtonClicked);
        clientButton.onClick.AddListener(OnClientButtonClicked);
    }

    private void OnHostButtonClicked()
    {
        GameManager.instance.StartHost();  // Menggunakan metode StartHost dari GameManager
        // Scene load dilakukan di dalam GameManager, tidak perlu memanggil LoadGameScene di sini
    }

    private void OnClientButtonClicked()
    {
        GameManager.instance.StartClient();  // Menggunakan metode StartClient dari GameManager
        // Scene load dilakukan di dalam GameManager, tidak perlu memanggil LoadGameScene di sini
    }
}
