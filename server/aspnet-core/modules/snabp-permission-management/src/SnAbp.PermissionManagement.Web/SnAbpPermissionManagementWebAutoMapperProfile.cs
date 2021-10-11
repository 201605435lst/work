using AutoMapper;
using Volo.Abp.AutoMapper;
using SnAbp.PermissionManagement.Web.Pages.AbpPermissionManagement;

namespace SnAbp.PermissionManagement.Web
{
    public class SnAbpPermissionManagementWebAutoMapperProfile : Profile
    {
        public SnAbpPermissionManagementWebAutoMapperProfile()
        {
            CreateMap<PermissionGroupDto, PermissionManagementModal.PermissionGroupViewModel>().Ignore(p=>p.IsAllPermissionsGranted);

            CreateMap<PermissionGrantInfoDto, PermissionManagementModal.PermissionGrantInfoViewModel>()
                .ForMember(p => p.Depth, opts => opts.Ignore());

            CreateMap<ProviderInfoDto, PermissionManagementModal.ProviderInfoViewModel>();
        }
    }
}
