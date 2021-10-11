using AutoMapper;
using SnAbp.Users;
using Volo.Abp.Users;

namespace SnAbp.Identity
{
    public class IdentityDomainMappingProfile : Profile
    {
        public IdentityDomainMappingProfile()
        {
            CreateMap<IdentityUser, UserEto>();
            CreateMap<IdentityClaimType, IdentityClaimTypeEto>();
            CreateMap<IdentityRole, IdentityRoleEto>();
            CreateMap<Organization, OrganizationEto>();
        }
    }
}