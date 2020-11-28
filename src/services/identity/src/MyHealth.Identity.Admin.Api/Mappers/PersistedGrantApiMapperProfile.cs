using AutoMapper;
using MyHealth.Identity.Admin.Api.Dtos.PersistedGrants;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Dtos.Grant;

namespace MyHealth.Identity.Admin.Api.Mappers
{
    public class PersistedGrantApiMapperProfile : Profile
    {
        public PersistedGrantApiMapperProfile()
        {
            CreateMap<PersistedGrantDto, PersistedGrantApiDto>(MemberList.Destination);
            CreateMap<PersistedGrantDto, PersistedGrantSubjectApiDto>(MemberList.Destination);
            CreateMap<PersistedGrantsDto, PersistedGrantsApiDto>(MemberList.Destination);
            CreateMap<PersistedGrantsDto, PersistedGrantSubjectsApiDto>(MemberList.Destination);

            CreateMap<Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Grant.PersistedGrantDto, PersistedGrantApiDto>(MemberList.Destination);
            CreateMap<Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Grant.PersistedGrantsDto, PersistedGrantsApiDto>(MemberList.Destination);
        }
    }
}





