﻿using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class TokenResponse
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_at")]
        public long ExpiresAt { get; set; } // unix timestamp

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; } // seconds

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("athlete")]
        public Athlete Athlete { get; set; }
    }

    public class Athlete
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
