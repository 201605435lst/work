using AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace SnAbp.TenantManagement
{
    public class SnAbpTenantManagementDomainMappingProfile : Profile
    {
        public SnAbpTenantManagementDomainMappingProfile()
        {
            CreateMap<Tenant, TenantConfiguration>()
                .ForMember(ti => ti.ConnectionStrings, opts =>
                {
                    opts.MapFrom((tenant, ti) =>
                    {
                        var connStrings = new ConnectionStrings();

                        foreach (var connectionString in tenant.ConnectionStrings)
                        {
                            connStrings[connectionString.Name] = connectionString.Value;
                        }

                        return connStrings;
                    });
                });

            CreateMap<Tenant, TenantEto>();
        }
    }
}
