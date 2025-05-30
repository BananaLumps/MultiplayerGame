using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

namespace Base
{
    public class ServerConnectionManager : MonoBehaviour
    {
        NetworkManager networkManager;
        public LocalConnectionState serverState = LocalConnectionState.Stopped;
        // Start is called before the first frame update
        void Start()
        {
            networkManager = FindObjectOfType<NetworkManager>();
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager not found in the scene. Please ensure it is present.");
                return;
            }
            networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
            ToggleServer();
        }

        private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs args)
        {
            serverState = args.ConnectionState;
        }
        public void ToggleServer()
        {
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager not found. Cannot toggle server connection.");
                return;
            }
            if (serverState == LocalConnectionState.Started)
            {
                networkManager.ServerManager.StopConnection(true);
            }
            else
            {
                networkManager.ServerManager.StartConnection();
            }
        }
        private void OnDestroy()
        {
            if (networkManager == null)
                return;

            networkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
        }
    }
}
