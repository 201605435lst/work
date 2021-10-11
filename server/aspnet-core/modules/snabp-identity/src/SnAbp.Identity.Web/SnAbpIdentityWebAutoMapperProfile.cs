using AutoMapper;
using Volo.Abp.AutoMapper;
using SnAbp.Identity.Web.Pages.Identity.Roles;
using CreateUserModalModel = SnAbp.Identity.Web.Pages.Identity.Users.CreateModalModel;
using EditUserModalModel = SnAbp.Identity.Web.Pages.Identity.Users.EditModalModel;

namespace SnAbp.Identity.Web
{
    public class SnAbpIdentityWebAutoMapperProfile : Profile
    {
        public SnAbpIdentityWebAutoMapperProfile()
        {
            CreateUserMappings();
            CreateRoleMappings();
            CreateOrganizationMappings();
        }

        protected virtual void CreateUserMappings()
        {
            //List
            CreateMap<IdentityUserDto, EditUserModalModel.UserInfoViewModel>()
                .Ignore(x => x.Password);

            //CreateModal
            CreateMap<CreateUserModalModel.UserInfoViewModel, IdentityUserCreateDto>()
                .Ignore(x => x.ExtraProperties)
                .ForMember(dest => dest.RoleIds, opt => opt.Ignore());

            CreateMap<IdentityRoleDto, CreateUserModalModel.AssignedRoleViewModel>()
                .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());

            //EditModal
            CreateMap<EditUserModalModel.UserInfoViewModel, IdentityUserUpdateDto>()
                .Ignore(x => x.ExtraProperties)
                .ForMember(dest => dest.RoleIds, opt => opt.Ignore());

            CreateMap<IdentityRoleDto, EditUserModalModel.AssignedRoleViewModel>()
                .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());
        }

        protected virtual void CreateRoleMappings()
        {
            //List
            CreateMap<IdentityRoleDto, EditModalModel.RoleInfoModel>();

            //CreateModal
            CreateMap<CreateModalModel.RoleInfoModel, IdentityRoleCreateDto>()
                .Ignore(x => x.ExtraProperties);

            //EditModal
            CreateMap<EditModalModel.RoleInfoModel, IdentityRoleUpdateDto>()
                .Ignore(x => x.ExtraProperties);
        }

        protected virtual void CreateOrganizationMappings()
        {
            CreateMap<Organization, IdentityRoleUpdateDto>()
                .Ignore(x => x.ExtraProperties);
        }
    }
}