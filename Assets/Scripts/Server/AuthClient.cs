using Base.Assets.Scripts.Server;
using FishNet.Managing;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AuthClient : MonoBehaviour
{
    [Header("Auth Server Settings")]
    [SerializeField] private string authUrl = "https://yourserver.com/api/login";
    internal string Username { get; set; }
    internal string Password { get; set; }

    public TokenBroadcast CurrentToken;
    public void Login(string username, string password, Action<TokenBroadcast> onTokenReceived)
    {
        Debug.Log($"Attempting to login");
        StartCoroutine(LoginCoroutine(username, password, onTokenReceived));
    }

    private IEnumerator LoginCoroutine(string username, string password, Action<TokenBroadcast> onTokenReceived)
    {
        var json = JsonUtility.ToJson(new LoginRequest { Username = username, Password = password });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(authUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Login successful!");
            var response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            var tokenBroadcast = new TokenBroadcast { Token = response.Token };

            CurrentToken.Token = response.Token;

            onTokenReceived?.Invoke(tokenBroadcast);

            Debug.Log("Ready to connect to FishNet!");
            FindObjectOfType<NetworkManager>().ClientManager.StartConnection();
        }
        else
        {
            Debug.LogError($"Login failed: {request.error}");
            onTokenReceived?.Invoke(new TokenBroadcast());
        }
    }

    [Serializable]
    private struct LoginRequest
    {
        public string Username;
        public string Password;
    }

    [Serializable]
    private struct LoginResponse
    {
        public string Token;
    }
}
