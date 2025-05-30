using Base.Server;
using FishNet.Authenticating;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Example.Authenticating;
using FishNet.Managing;
using FishNet.Transporting;
using System;
using UnityEngine;

namespace Base.Assets.Scripts.Server
{
    public class TokenAuthenticator : HostAuthenticator
    {
        public override event Action<NetworkConnection, bool> OnAuthenticationResult;
        public override void InitializeOnce(NetworkManager networkManager)
        {
            base.InitializeOnce(networkManager);

            //Listen for connection state change as client.
            base.NetworkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
            //Listen for broadcast from client. Be sure to set requireAuthentication to false.
#if UNITY_SERVER  
            base.NetworkManager.ServerManager.RegisterBroadcast<TokenBroadcast>(OnTokenBroadcast, false);
#endif
            //Listen to response from server.
            base.NetworkManager.ClientManager.RegisterBroadcast<ResponseBroadcast>(OnResponseBroadcast);
        }
#if UNITY_SERVER
        private void OnTokenBroadcast(NetworkConnection connection, TokenBroadcast broadcast, Channel channel)
        {
            if (connection.IsAuthenticated)
            {
                connection.Disconnect(true);
                return;
            }
            bool validToken = TokenValidator.Validate(broadcast.Token);
            SendAuthenticationResponse(connection, validToken);
            OnAuthenticationResult?.Invoke(connection, validToken);
        }
#endif
        private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs args)
        {
            if (args.ConnectionState != LocalConnectionState.Started)
                return;
            //Authentication was sent as host, no need to authenticate normally.
            if (AuthenticateAsHost())
                return;
            Debug.Log("Client connected, starting authentication process.");
            AuthClient authClient = FindObjectOfType<AuthClient>();
            authClient.Login(authClient.Username, authClient.Password, onTokenReceived);


        }

        private void onTokenReceived(TokenBroadcast broadcast)
        {
            base.NetworkManager.ClientManager.Broadcast(broadcast);
        }


        /// <summary>
        /// Received on client after server sends an authentication response.
        /// </summary>
        /// <param name="rb"></param>
        private void OnResponseBroadcast(ResponseBroadcast rb, Channel channel)
        {
            string result = (rb.Passed) ? "Authentication complete." : "Authenitcation failed.";
            NetworkManager.Log(result);
            // Load game scene
        }

        /// <summary>
        /// Sends an authentication result to a connection.
        /// </summary>
        private void SendAuthenticationResponse(NetworkConnection conn, bool authenticated)
        {
            /* Tell client if they authenticated or not. This is
            * entirely optional but does demonstrate that you can send
            * broadcasts to client on pass or fail. */
            ResponseBroadcast rb = new ResponseBroadcast()
            {
                Passed = authenticated
            };
            base.NetworkManager.ServerManager.Broadcast(conn, rb, false);
        }
        /// <summary>
        /// Called after handling a host authentication result.
        /// </summary>
        /// <param name="conn">Connection authenticating.</param>
        /// <param name="authenticated">True if authentication passed.</param>
        protected override void OnHostAuthenticationResult(NetworkConnection conn, bool authenticated)
        {
            SendAuthenticationResponse(conn, authenticated);
            OnAuthenticationResult?.Invoke(conn, authenticated);
        }

    }
    public struct TokenBroadcast : IBroadcast
    {
        public string Token;
    }
}
