using SnAbp.Account.Web.Pages.Account;
using SnAbp.Identity;
using AutoMapper;

namespace SnAbp.Account.Web
{
    public class AbpAccountWebAutoMapperProfile : Profile
    {
        public AbpAccountWebAutoMapperProfile()
        {
            CreateMap<ProfileDto, PersonalSettingsInfoModel>();
        }
    }
}
