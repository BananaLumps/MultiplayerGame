using Base.Assets.Scripts.Server;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    public class UILogin : MonoBehaviour
    {
        [SerializeField]
        TMP_InputField usernameInput;
        [SerializeField]
        TMP_InputField passwordInput;
        [SerializeField]
        Button loginButton;
        public GameObject loginUI;

        private void Awake()
        {
            loginButton.onClick.AddListener(OnButtonClick);
        }
        private void Update()
        {
        }
        public void OnButtonClick()
        {
            AuthClient authClient = FindObjectOfType<AuthClient>();
            authClient.Username = usernameInput.text;
            authClient.Password = passwordInput.text;
            authClient.Login(authClient.Username, authClient.Password, onTokenReceived);
        }

        private void onTokenReceived(TokenBroadcast broadcast)
        {
        }
    }
}
