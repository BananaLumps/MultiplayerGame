
#if UNITY_SERVER || UNITY_EDITOR
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Base.Server
{

    public class AuthServer : MonoBehaviour
    {
        private HttpListener _httpListener;
        private const string SecretKey = "SuperSecretKey12345"; // TODO: Store securely

        private void Start()
        {
            StartHttpServer();
        }

        private async void StartHttpServer()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://*:2340/"); // Use HTTPS in production!
            _httpListener.Start();
            Debug.Log("Auth server listening on http://localhost:2340/");

            while (true)
            {
                var context = await _httpListener.GetContextAsync();
                _ = HandleRequest(context);
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            if (context.Request.HttpMethod != "POST")
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
                return;
            }

            string body;
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                body = await reader.ReadToEndAsync();
            }

            var credentials = JsonUtility.FromJson<LoginRequest>(body);

            if (ValidateCredentials(credentials.Username, credentials.Password))
            {
                var token = GenerateJwt(credentials.Username);
                var response = JsonUtility.ToJson(new LoginResponse { Token = token });

                var buffer = Encoding.UTF8.GetBytes(response);
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            else
            {
                context.Response.StatusCode = 401;
            }

            context.Response.Close();
        }

        private bool ValidateCredentials(string username, string password)
        {
            // Replace with real validation (e.g., database lookup)
            return true; // For demo purposes, always return true
        }

        private string GenerateJwt(string username)
        {
            var payload = new Dictionary<string, object>
        {
            { "sub", username },
            { "exp", DateTimeOffset.UtcNow.AddMinutes(3).ToUnixTimeSeconds() }
        };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, SecretKey);
        }

        [Serializable]
        public class LoginRequest
        {
            public string Username;
            public string Password;
        }

        [Serializable]
        public class LoginResponse
        {
            public string Token;
        }
    }
}
#endif // Unity_Server && !UNITY_EDITOR

