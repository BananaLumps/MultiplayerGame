#if UNITY_SERVER || UNITY_EDITOR
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Server
{
    public static class TokenValidator
    {
        private const string SecretKey = "SuperSecretKey12345"; // Same as Auth Server

        public static bool Validate(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var payload = decoder.DecodeToObject<IDictionary<string, object>>(token, SecretKey, verify: true);

                if (payload.TryGetValue("sub", out object username))
                {
                    Debug.Log($"Token valid for user: {username}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Invalid token: {ex.Message}");
            }

            return false;
        }
    }
}
#endif