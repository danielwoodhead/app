using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string AccessTokenKey = "access_token";
        private const string AccessTokenExpiresUtcKey = "access_token_expires_utc";
        private const string IdentityTokenKey = "identity_token";
        private readonly string _accessTokenDefault = string.Empty;
        private readonly DateTime _accessTokenExpiresDefault = DateTime.MinValue;
        private readonly string _identityTokenDefault = string.Empty;

        public string AuthAccessToken
        {
            get => GetValueOrDefault(AccessTokenKey, _accessTokenDefault);
            set => AddOrUpdateValue(AccessTokenKey, value);
        }

        public DateTime AuthAccessTokenExpiresUtc
        {
            get => GetValueOrDefault(AccessTokenExpiresUtcKey, _accessTokenExpiresDefault);
            set => AddOrUpdateValue(AccessTokenExpiresUtcKey, value);
        }

        public string AuthIdentityToken
        {
            get => GetValueOrDefault(IdentityTokenKey, _identityTokenDefault);
            set => AddOrUpdateValue(IdentityTokenKey, value);
        }

        public Task AddOrUpdateValue(string key, bool value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, string value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, DateTime value) => AddOrUpdateValueInternal(key, value);
        public bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public string GetValueOrDefault(string key, string defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public DateTime GetValueOrDefault(string key, DateTime defaultValue) => GetValueOrDefaultInternal(key, defaultValue);

        private async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await Remove(key);
            }

            Application.Current.Properties[key] = value;
            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
            }
        }

        private T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T))
        {
            object value = null;
            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }
            return null != value ? (T)value : defaultValue;
        }

        private async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
            }
        }
    }
}
