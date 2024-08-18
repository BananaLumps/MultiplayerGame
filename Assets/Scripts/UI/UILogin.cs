using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Transporting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    public class UILogin : MonoBehaviour
    {
        TMP_InputField inputField;
        public GameObject loginUI;
        private LocalConnectionState _clientState = LocalConnectionState.Stopped;
        private NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = InstanceFinder.NetworkManager;
            inputField = loginUI.GetComponentInChildren<TMP_InputField>();
            loginUI.GetComponentInChildren<Button>().onClick.AddListener(OnButtonClick);
            _networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
            _networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
        }
        private void Update()
        {
            if (_clientState == LocalConnectionState.Stopped) ConnectClient();
        }
        private void SceneManager_OnClientLoadedStartScenes(NetworkConnection connection, bool arg2)
        {

        }
        public void ConnectClient()
        {
            if (_networkManager == null)
                return;

            if (_clientState != LocalConnectionState.Stopped)
                _networkManager.ClientManager.StopConnection();
            else
                _networkManager.ClientManager.StartConnection();
        }
        private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs args)
        {
            _clientState = args.ConnectionState;
        }
        public void OnButtonClick()
        {
            Login(_networkManager.ClientManager.Connection, inputField.text);
            loginUI.SetActive(false);
        }
        public void Login(NetworkConnection nc, string userName)
        {
            Core.Instance.Server.Login(nc, userName);
        }
        private void OnDestroy()
        {
            _clientState = LocalConnectionState.Stopped;
            _networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
            _networkManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
        }
    }
}
