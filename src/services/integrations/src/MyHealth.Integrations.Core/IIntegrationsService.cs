﻿using System.Threading.Tasks;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Requests;
using MyHealth.Integrations.Models.Response;

namespace MyHealth.Integrations.Core
{
    public interface IIntegrationsService
    {
        Task<SearchIntegrationsResponse> SearchIntegrationsAsync();
        Task<Integration> GetIntegrationAsync(string id);
        Task<Integration> CreateIntegrationAsync(CreateIntegrationRequest request);
        Task<Integration> UpdateIntegrationAsync(string id, UpdateIntegrationRequest request);
        Task DeleteIntegrationAsync(string id);
    }
}
