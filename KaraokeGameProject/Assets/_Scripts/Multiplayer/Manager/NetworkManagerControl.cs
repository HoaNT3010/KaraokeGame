using Unity.Netcode;
using UnityEngine;

public class NetworkManagerControl : MonoBehaviour
{
    [ContextMenu("Start Host")]
    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    [ContextMenu("Start Client")]
    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    [ContextMenu("Start Server")]
    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    [ContextMenu("Shutdown")]
    private void ShutdownConnection()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
