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
    public class ApiResourceRepository : IApiResourceRepository
    {
        private readonly ConfigurationDbContext _context;

        public ApiResourceRepository(ConfigurationDbContext context)
        {
            _context = context;
        }

        public async Task CreateApiResourceAsync(ApiResource apiResource)
        {
            _context.ApiResources.Add(apiResource.ToEntity());
            await _context.SaveChangesAsync();
        }

        public async Task DeleteApiResourceAsync(string name)
        {
            Entities.ApiResource apiResource = await _context.ApiResources.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name);

            if (apiResource != null)
            {
                _context.ApiResources.Remove(apiResource);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ApiResource>> GetAllApiResourcesAsync()
        {
            IQueryable<Entities.ApiResource> apis = _context.ApiResources
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims)
                .Include(x => x.Properties)
                .AsNoTracking();

            return (await apis.ToArrayAsync()).Select(x => x.ToModel());
        }

        public async Task<ApiResource> GetApiResourceAsync(string name)
        {
            IQueryable<Entities.ApiResource> query =
                from apiResource in _context.ApiResources
                where apiResource.Name == name
                select apiResource;

            IQueryable<Entities.ApiResource> apis = query
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims)
                .Include(x => x.Properties)
                .AsNoTracking();

            Entities.ApiResource api = await apis.FirstOrDefaultAsync();

            return api.ToModel();
        }

        public async Task<ApiResource> UpdateApiResourceAsync(ApiResource apiResource)
        {
            if (apiResource == null)
                throw new ArgumentNullException(nameof(apiResource));

            IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();

            Entities.ApiResource entity = await _context.ApiResources
                .Include(x => x.Scopes)
                .FirstOrDefaultAsync(x => x.Name == apiResource.Name);

            if (entity == null)
            {
                Entities.ApiResource newEntity = mapper.Map<ApiResource, Entities.ApiResource>(apiResource);
                _context.ApiResources.Add(newEntity);
                await _context.SaveChangesAsync();

                return newEntity.ToModel();
            }

            _context.Entry(entity).CurrentValues.SetValues(apiResource);

            foreach (Scope scope in apiResource.Scopes)
            {
                Entities.ApiScope existingScope = entity.Scopes
                    .FirstOrDefault(x => x.Name == scope.Name);

                if (existingScope == null)
                {
                    entity.Scopes.Add(mapper.Map<Scope, Entities.ApiScope>(scope));
                }
                else
                {
                    _context.Entry(existingScope).CurrentValues.SetValues(scope);
                }
            }

            foreach (Entities.ApiScope scope in entity.Scopes)
            {
                if (!apiResource.Scopes.Any(x => x.Name == scope.Name))
                {
                    _context.Remove(scope);
                }
            }

            await _context.SaveChangesAsync();

            return entity.ToModel();
        }
    }
}
