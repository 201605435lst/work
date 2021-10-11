using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace SnAbp.Identity
{
    public class SnAbpIdentityApplicationModuleAutoMapperProfile : Profile
    {
        public SnAbpIdentityApplicationModuleAutoMapperProfile()
        {
            CreateMap<IdentityUser, IdentityUserDto>()
                .MapExtraProperties();

            CreateMap<IdentityRole, IdentityRoleDto>();

            CreateMap<IdentityUser, ProfileDto>()
                .MapExtraProperties();

            CreateMap<OrganizationInputDto, Organization>()
                .Ignore(a => a.TenantId)
                .Ignore(a => a.Roles)
                .Ignore(a => a.IsDeleted)
                .Ignore(a => a.DeleterId)
                .Ignore(a => a.DeletionTime)
                .Ignore(a => a.LastModificationTime)
                .Ignore(a => a.LastModifierId)
                .Ignore(a => a.CreationTime)
                .Ignore(a => a.CreatorId)
                .Ignore(a => a.ConcurrencyStamp)
                .Ignore(a => a.Id);
            CreateMap<OrganizationInputDto, OrganizationDto>()
                //.Ignore(a => a.Parent)
                .Ignore(a => a.Children)
                .Ignore(a => a.Id);

            CreateMap<OrganizationInputDto, Organization>();
            CreateMap<OrganizationUpdateDto, Organization>();
            CreateMap<Organization, OrganizationDto>();
            CreateMap<OrganizationSelectDto, OrganizationSelectSimpleDto>();

            CreateMap<DataDictionary, DataDictionaryDto>();
            CreateMap<DataDictionaryDto, DataDictionary>();

            CreateMap<DataDictionaryCreateDto, DataDictionary>();
            CreateMap<DataDictionary, DataDictionaryCreateDto>();

            CreateMap<DataDictionaryUpdateDto, DataDictionary>();
            CreateMap<DataDictionary, DataDictionaryUpdateDto>();
            CreateMap<OrganizationDto, Organization>();
            CreateMap<Organization, OrganizationDto>();
            CreateMap<OrganizationImportTemplate, Organization>();
            CreateMap<Organization, OrganizationImportTemplate > ();

            
            // Member
            CreateMap<Member, MemberDto>();
            CreateMap<MemberDto, Member>();
            CreateMap<Organization, MemberDto>();
            CreateMap<IdentityRole, MemberDto>();
            CreateMap<IdentityUser, MemberDto>();
            CreateMap<IdentityUserCreateDto, IdentityUser>();
            CreateMap<IdentityUser, IdentityUserCreateDto>();
            CreateMap<IdentityUser, UserImportTemplate>();
            CreateMap<UserImportTemplate, IdentityUser>();
        }
    }
}