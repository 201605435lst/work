using AutoMapper;
using SnAbp.Oa.Dtos;
using SnAbp.Oa.Dtos.Seal;
using SnAbp.Oa.Entities;

namespace SnAbp.Oa
{
    public class OaApplicationAutoMapperProfile : Profile
    {
        public OaApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<DutySchedule, DutyScheduleDto>();
            CreateMap<DutyScheduleDto, DutySchedule>();
            CreateMap<DutyScheduleRltUser, DutyScheduleRltUserDto>();
            CreateMap<DutyScheduleRltUserDto, DutyScheduleRltUser>();

            CreateMap<DutyScheduleRltUserDto, DutyScheduleRltUser>();
            CreateMap<DutyScheduleRltUserDto, DutyScheduleRltUser>();
 
            CreateMap<Contract, ContractDto>();
            CreateMap<ContractDto, Contract>();
            CreateMap<ContractCreateDto, Contract>();
            CreateMap<Contract, ContractCreateDto>();

            ///ContractRltFile
            CreateMap<ContractRltFile, ContractRltFileDto>();
            CreateMap<ContractRltFileDto, ContractRltFile>();

            CreateMap<Seal, SealDto>();
            CreateMap<SealDto, Seal>();
            CreateMap<Seal, SealSimpleDto>();
            CreateMap<SealSimpleDto, Seal>();
            CreateMap<SealRltMember, SealRltMemberDto>();
            CreateMap<SealRltMemberDto, SealRltMember>();


        }
    }
}