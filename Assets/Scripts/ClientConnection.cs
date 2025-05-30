using FishNet.Managing;
using FishNet.Transporting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientConnection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button connectButton;
    [SerializeField] private TMP_Text statusText;

    private NetworkManager networkManager;

    private void Awake()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager == null)
        {
            Debug.LogError("❌ No NetworkManager found in the scene.");
        }

        connectButton.onClick.AddListener(AttemptConnection);
        UpdateStatus("Ready to connect.");
    }

    private void AttemptConnection()
    {
        string ipAddress = "127.0.0.1"; // Default to localhost

        ushort port = 7777; // Default port


        var transport = networkManager.TransportManager.Transport;
        transport.SetClientAddress(ipAddress);
        transport.SetPort(port);

        networkManager.ClientManager.StartConnection();

        UpdateStatus($"Connecting to {ipAddress}:{port}...");
    }

    private void OnEnable()
    {
        if (networkManager != null)
        {
            networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }
    }

    private void OnDisable()
    {
        if (networkManager != null)
        {
            networkManager.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }
    }

    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        switch (args.ConnectionState)
        {
            case LocalConnectionState.Started:
                UpdateStatus("✅ Connected to server.");
                break;
            case LocalConnectionState.Stopped:
                UpdateStatus("❌ Disconnected from server.");
                break;
            case LocalConnectionState.Starting:
                UpdateStatus("⏳ Connecting...");
                break;
            case LocalConnectionState.Stopping:
                UpdateStatus("🔌 Disconnecting...");
                break;
        }
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
        Debug.Log(message);
    }
}
