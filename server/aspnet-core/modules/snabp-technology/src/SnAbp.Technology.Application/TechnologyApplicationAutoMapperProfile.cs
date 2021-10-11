using AutoMapper;
using SnAbp.Technology.Dtos;
using SnAbp.Technology.Entities;


namespace SnAbp.Technology
{
    public class TechnologyApplicationAutoMapperProfile : Profile
    {
        public TechnologyApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<ConstructInterface, ConstructInterfaceDto>();
            CreateMap<ConstructInterfaceDto, ConstructInterface>();
            CreateMap<ConstructInterface, ConstructInterfaceCreateDto>();
            CreateMap<ConstructInterfaceCreateDto, ConstructInterface>();
            CreateMap<ConstructInterface, ConstructInterfaceUpdateDto>();
            CreateMap<ConstructInterfaceUpdateDto, ConstructInterface>();

            CreateMap<ConstructInterfaceInfo, ConstructInterfaceInfoDto>();
            CreateMap<ConstructInterfaceInfoDto, ConstructInterfaceInfo>();
            CreateMap<ConstructInterfaceInfo, ConstructInterfaceInfoReformDto>();
            CreateMap<ConstructInterfaceInfoReformDto, ConstructInterfaceInfo>();
            CreateMap<ConstructInterfaceInfo, ConstructInterfaceInfoCreateDto>();
            CreateMap<ConstructInterfaceInfoCreateDto, ConstructInterfaceInfo>();
            CreateMap<ConstructInterfaceInfo, ConstructInterfaceInfoUpdateDto>();
            CreateMap<ConstructInterfaceInfoUpdateDto, ConstructInterfaceInfo>();

            CreateMap<ConstructInterfaceInfoRltMarkFile, ConstructInterfaceInfoRltMarkFileDto>();
            CreateMap<ConstructInterfaceInfoRltMarkFileDto, ConstructInterfaceInfoRltMarkFile>();
            CreateMap<ConstructInterfaceInfoRltMarkFile, ConstructInterfaceInfoRltMarkFileSimpleDto>();
            CreateMap<ConstructInterfaceInfoRltMarkFileSimpleDto, ConstructInterfaceInfoRltMarkFile>();

            

            // 技术交底
            CreateMap<Disclose, DiscloseDto>();
            CreateMap<DiscloseDto, Disclose>();
            CreateMap<DiscloseCreateDto, Disclose>();
            CreateMap<Disclose, DiscloseCreateDto>();

            // 材料管理、用料计划
            CreateMap<MaterialPlan, MaterialCreateDto>();
            CreateMap<MaterialCreateDto, MaterialPlan>();
            CreateMap<MaterialPlan, MaterialPlanExportDto>();
            CreateMap<MaterialPlanExportDto, MaterialPlan>();
            CreateMap<MaterialPlan, MaterialPlanDto>();
            CreateMap<MaterialPlanDto, MaterialPlan>();

            CreateMap<Material, MaterialDto>();
            CreateMap<MaterialDto, Material>();

            CreateMap<Material, MaterialCreateDto>();
            CreateMap<MaterialCreateDto, Material>();
            CreateMap<MaterialPlanCreateDto, MaterialPlan>();
            CreateMap<MaterialPlan, MaterialPlanCreateDto>();
            CreateMap<MaterialPlan, MaterialPlanDto>();
            CreateMap<MaterialPlanDto, MaterialPlan>();
            CreateMap<MaterialPlanRltMaterialDto, MaterialPlanRltMaterial>();
            CreateMap<MaterialPlanRltMaterial, MaterialPlanRltMaterialDto>();






        }
    }

}