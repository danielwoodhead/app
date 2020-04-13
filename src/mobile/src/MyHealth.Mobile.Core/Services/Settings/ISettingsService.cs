using System;
using System.Threading.Tasks;

namespace MyHealth.Mobile.Core.Services.Settings
{
    public interface ISettingsService
    {
        string AuthAccessToken { get; set; }
        DateTime AuthAccessTokenExpiresUtc { get; set; }
        string AuthIdentityToken { get; set; }

        bool GetValueOrDefault(string key, bool defaultValue);
        string GetValueOrDefault(string key, string defaultValue);
        Task AddOrUpdateValue(string key, bool value);
        Task AddOrUpdateValue(string key, string value);
    }
}
