using AutoMapper;
using Volo.Abp.AutoMapper;
using SnAbp.TenantManagement.Web.Pages.TenantManagement.Tenants;

namespace SnAbp.TenantManagement.Web
{
    public class SnAbpTenantManagementWebAutoMapperProfile : Profile
    {
        public SnAbpTenantManagementWebAutoMapperProfile()
        {
            //List
            CreateMap<TenantDto, EditModalModel.TenantInfoModel>();

            //CreateModal
            CreateMap<CreateModalModel.TenantInfoModel, TenantCreateDto>()
                .Ignore(x => x.ExtraProperties);

            //EditModal
            CreateMap<EditModalModel.TenantInfoModel, TenantUpdateDto>()
                .Ignore(x => x.ExtraProperties);
        }
    }
}
