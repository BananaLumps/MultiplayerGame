using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

namespace Base
{
    public class ClientConnectionManager : MonoBehaviour
    {
        private NetworkManager networkManager;
        public LocalConnectionState clientState = LocalConnectionState.Stopped;

        private void Start()
        {
            networkManager = FindObjectOfType<NetworkManager>();
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager not found in the scene. Please ensure it is present.");
                return;
            }

            networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
            ToggleClient();
        }

        private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs args)
        {
            clientState = args.ConnectionState;
        }

        public void ToggleClient()
        {
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager not found. Cannot toggle client connection.");
                return;
            }

            if (clientState == LocalConnectionState.Started)
            {
                networkManager.ClientManager.StopConnection();
            }
            else
            {
                networkManager.ClientManager.StartConnection();
            }
        }

        private void OnDestroy()
        {
            if (networkManager == null)
                return;

            networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
        }
    }
}
