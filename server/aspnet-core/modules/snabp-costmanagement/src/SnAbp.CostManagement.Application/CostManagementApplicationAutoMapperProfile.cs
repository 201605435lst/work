using AutoMapper;
using SnAbp.CostManagement.Dtos;
using SnAbp.CostManagement.Entities;

namespace SnAbp.CostManagement
{
    public class CostManagementApplicationAutoMapperProfile : Profile
    {
        public CostManagementApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<PeopleCost, PeopleCostDto>();
            CreateMap<PeopleCostDto, PeopleCost>();

            CreateMap<PeopleCost, PeopleCostCreateDto>();
            CreateMap<PeopleCostCreateDto, PeopleCost>();

            CreateMap<PeopleCost, PeopleCostUpdateDto>();
            CreateMap<PeopleCostUpdateDto, PeopleCost>();


            CreateMap<CostOther, CostOtherDto>();
            CreateMap<CostOtherDto, CostOther>();

            CreateMap<CostOther, CostOtherCreateDto>();
            CreateMap<CostOtherCreateDto, CostOther>();

            CreateMap<CostOther, CostOtherUpdateDto>();
            CreateMap<CostOtherUpdateDto, CostOther>();

            //MoneyList
             CreateMap<MoneyList, MoneyListDto>();
            CreateMap<MoneyListDto, MoneyList>();

            CreateMap<MoneyList, MoneyListCreateDto>();
            CreateMap<MoneyListCreateDto, MoneyList>();

            CreateMap<MoneyList, MoneyListUpdateDto>();
            CreateMap<MoneyListUpdateDto, MoneyList>();

            //Contract
            CreateMap<Contract, CostContractDto>();
            CreateMap<CostContractDto, Contract>();

            CreateMap<Contract, CostContractCreateDto>();
            CreateMap<CostContractCreateDto, Contract>();

            CreateMap<Contract, CostContractUpdateDto>();
            CreateMap<CostContractUpdateDto, Contract>();
            //ContractRltFile
            CreateMap<ContractRltFile, CostContractRltFileDto>();
            CreateMap<CostContractRltFileDto, ContractRltFile>();
            CreateMap<ContractRltFile, CostContractRltFileSimpleDto>();
            CreateMap<CostContractRltFileSimpleDto, ContractRltFile>();

        }
    }
}