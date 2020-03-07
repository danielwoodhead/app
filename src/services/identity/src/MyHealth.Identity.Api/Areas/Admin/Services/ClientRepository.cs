using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Entities = IdentityServer4.EntityFramework.Entities;

namespace MyHealth.Identity.Api.Areas.Admin.Services
{
    public class ClientRepository : IClientRepository
    {
        private readonly ConfigurationDbContext _context;

        public ClientRepository(ConfigurationDbContext context)
        {
            _context = context;
        }

        public async Task CreateClientAsync(Client client)
        {
            _context.Clients.Add(client.ToEntity());
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(string clientId)
        {
            Entities.Client client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == clientId);

            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            IQueryable<Entities.Client> baseQuery = _context.Clients
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.AllowedScopes)
                .Include(x => x.Claims)
                .Include(x => x.ClientSecrets)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.Properties)
                .Include(x => x.RedirectUris)
                .AsNoTracking();

            return (await baseQuery.ToArrayAsync()).Select(x => x.ToModel());
        }

        public async Task<Client> GetClientAsync(string clientId)
        {
            IQueryable<Entities.Client> baseQuery = _context.Clients
                .Where(x => x.ClientId == clientId)
                .Take(1);

            var client = await baseQuery.FirstOrDefaultAsync();
            if (client == null)
                return null;

            await baseQuery.Include(x => x.AllowedCorsOrigins).SelectMany(c => c.AllowedCorsOrigins).LoadAsync();
            await baseQuery.Include(x => x.AllowedGrantTypes).SelectMany(c => c.AllowedGrantTypes).LoadAsync();
            await baseQuery.Include(x => x.AllowedScopes).SelectMany(c => c.AllowedScopes).LoadAsync();
            await baseQuery.Include(x => x.Claims).SelectMany(c => c.Claims).LoadAsync();
            await baseQuery.Include(x => x.ClientSecrets).SelectMany(c => c.ClientSecrets).LoadAsync();
            await baseQuery.Include(x => x.IdentityProviderRestrictions).SelectMany(c => c.IdentityProviderRestrictions).LoadAsync();
            await baseQuery.Include(x => x.PostLogoutRedirectUris).SelectMany(c => c.PostLogoutRedirectUris).LoadAsync();
            await baseQuery.Include(x => x.Properties).SelectMany(c => c.Properties).LoadAsync();
            await baseQuery.Include(x => x.RedirectUris).SelectMany(c => c.RedirectUris).LoadAsync();

            return client.ToModel();
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();

            Entities.Client entity = await _context.Clients
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Where(x => x.ClientId == client.ClientId)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                Entities.Client newEntity = mapper.Map<Client, Entities.Client>(client);
                _context.Clients.Add(newEntity);
                await _context.SaveChangesAsync();

                return newEntity.ToModel();
            }

            _context.Entry(entity).CurrentValues.SetValues(client);
            UpdateAllowedGrantTypes(entity, client, mapper);
            UpdateAllowedScopes(entity, client, mapper);
            UpdateClientSecrets(entity, client, mapper);

            await _context.SaveChangesAsync();

            return entity.ToModel();
        }

        private void UpdateAllowedGrantTypes(Entities.Client entity, Client client, IMapper mapper)
        {
            foreach (string allowedGrantType in client.AllowedGrantTypes)
            {
                Entities.ClientGrantType existingAllowedGrantType = entity.AllowedGrantTypes
                    .FirstOrDefault(x => x.GrantType == allowedGrantType);

                if (existingAllowedGrantType == null)
                {
                    entity.AllowedGrantTypes.Add(mapper.Map<string, Entities.ClientGrantType>(allowedGrantType));
                }
            }

            foreach (Entities.ClientGrantType allowedGrantType in entity.AllowedGrantTypes)
            {
                if (!client.AllowedGrantTypes.Any(x => x == allowedGrantType.GrantType))
                {
                    _context.Remove(allowedGrantType);
                }
            }
        }

        private void UpdateAllowedScopes(Entities.Client entity, Client client, IMapper mapper)
        {
            foreach (string allowedScope in client.AllowedScopes)
            {
                Entities.ClientScope existingAllowedScope = entity.AllowedScopes
                    .FirstOrDefault(x => x.Scope == allowedScope);

                if (existingAllowedScope == null)
                {
                    entity.AllowedScopes.Add(mapper.Map<string, Entities.ClientScope>(allowedScope));
                }
            }

            foreach (Entities.ClientScope allowedScope in entity.AllowedScopes)
            {
                if (!client.AllowedScopes.Any(x => x == allowedScope.Scope))
                {
                    _context.Remove(allowedScope);
                }
            }
        }

        private void UpdateClientSecrets(Entities.Client entity, Client client, IMapper mapper)
        {
            foreach (Secret clientSecret in client.ClientSecrets)
            {
                Entities.ClientSecret existingClientSecret = entity.ClientSecrets
                    .FirstOrDefault(x => x.Value == clientSecret.Value);

                if (existingClientSecret == null)
                {
                    entity.ClientSecrets.Add(mapper.Map<Secret, Entities.ClientSecret>(clientSecret));
                }
            }

            foreach (Entities.ClientSecret clientSecret in entity.ClientSecrets)
            {
                if (!client.ClientSecrets.Any(x => x.Value == clientSecret.Value))
                {
                    _context.Remove(clientSecret);
                }
            }
        }
    }
}
