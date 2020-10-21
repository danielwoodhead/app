﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Web.Core.Models;

namespace MyHealth.Web.Core.AppApi
{
    public interface IAppApiClient
    {
        #region Health Record

        Task<SearchObservationsResponse> GetObservationsAsync();

        #endregion Health Record

        #region Integrations

        Task DeleteIntegrationAsync(string id);
        Task<GetIntegrationsResponse> GetAllIntegrationsAsync();
        Task CreateFitbitIntegrationAsync(string code, string redirectUri);
        Task<string> GetFitbitAuthenticationUriAsync(string redirectUri);
        Task CreateStravaIntegrationAsync(string code);
        Task<string> GetStravaAuthenticationUriAsync(string redirectUri);

        #endregion Integrations
    }
}