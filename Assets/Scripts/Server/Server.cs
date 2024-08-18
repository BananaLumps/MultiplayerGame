// Ignore Spelling: username

using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Server
{
    public class Server : NetworkBehaviour
    {
        public static readonly Dictionary<NetworkConnection, string> Users = new();
        public static Players Players = new();
        private void Awake()
        {
            InstanceFinder.ServerManager.OnRemoteConnectionState += ServerManager_OnRemoteConnectionState;
        }
        [Server]
        private void ServerManager_OnRemoteConnectionState(NetworkConnection arg1, RemoteConnectionStateArgs arg2)
        {
            if (arg2.ConnectionState == RemoteConnectionState.Stopped) PlayerDisconnect(arg1);
        }
        public void PlayerDisconnect(NetworkConnection connection)
        {
            Users.Remove(connection);
        }
        [ServerRpc(RequireOwnership = false)]
        public void Login(NetworkConnection nc, string username)
        {
            Users.Add(nc, username);
            GameObject temp = Instantiate(Core.Instance.PlayerPrefab);
            temp.transform.parent = GameObject.Find("Players").transform;
            temp.name = username;
            temp.SetActive(true);
            Players.PlayerList.Add(username, temp.GetComponent<Client.Player.Player>());
            InstanceFinder.NetworkManager.ServerManager.Spawn(temp, nc);
            Players.PlayerList[username].PlayerName.Value = username;
            Players.PlayerList[username].UserID.Value = username;
        }
        private void OnDestroy()
        {
            Users.Clear();
            Players.PlayerList.Clear();
            InstanceFinder.ServerManager.OnRemoteConnectionState -= ServerManager_OnRemoteConnectionState;
        }
    }
}
