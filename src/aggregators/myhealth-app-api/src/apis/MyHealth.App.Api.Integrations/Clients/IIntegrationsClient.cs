﻿using System.Net.Http;
using System.Threading.Tasks;
using MyHealth.App.Api.Integrations.Models;

namespace MyHealth.App.Api.Integrations.Clients
{
    public interface IIntegrationsClient
    {
        Task<HttpResponseMessage> DeleteIntegrationAsync(string id);
        Task<HttpResponseMessage> GetIntegrationAsync(string id);
        Task<HttpResponseMessage> GetIntegrationsAsync();

        #region Fitbit

        Task<HttpResponseMessage> CreateFitbitIntegrationAsync(CreateFitbitIntegrationRequest request);
        Task<HttpResponseMessage> GetFitbitAuthenticationUri(string redirectUri);

        #endregion Fitbit

        #region Strava

        Task<HttpResponseMessage> CreateStravaIntegrationAsync(CreateStravaIntegrationRequest request);
        Task<HttpResponseMessage> GetStravaAuthenticationUri(string redirectUri);

        #endregion Strava
    }
}
