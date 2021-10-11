using AutoMapper;

namespace SnAbp.TenantManagement
{
    public class SnAbpTenantManagementApplicationAutoMapperProfile : Profile
    {
        public SnAbpTenantManagementApplicationAutoMapperProfile()
        {
            CreateMap<Tenant, TenantDto>()
                .MapExtraProperties();
        }
    }
}