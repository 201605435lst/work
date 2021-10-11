using AutoMapper;
using SnAbp.Construction.Dtos;
using SnAbp.Construction.Entities;

namespace SnAbp.Construction
{
    public class ConstructionApplicationAutoMapperProfile : Profile
    {
        public ConstructionApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            //派工单
            CreateMap<Dispatch, DispatchDto>();
            CreateMap<DispatchDto, Dispatch>();
            CreateMap<Dispatch, DispatchCreateDto>();
            CreateMap<Dispatch, DispatchUpdateDto>();
            CreateMap<DispatchRltFile, DispatchRltFileDto>();
            CreateMap<DispatchRltFile, DispatchRltFileCreateDto>();
            CreateMap<DispatchRltMaterial, DispatchRltMaterialDto>();
            CreateMap<DispatchRltMaterial, DispatchRltMaterialCreateDto>();
            CreateMap<DispatchRltPlanContent, DispatchRltPlanContentDto>();
            CreateMap<DispatchRltPlanContent, DispatchRltPlanContentCreateDto>();
            CreateMap<DispatchRltSection, DispatchRltSectionDto>();
            CreateMap<DispatchRltSection, DispatchRltSectionCreateDto>();
            CreateMap<DispatchRltStandard, DispatchRltStandardDto>();
            CreateMap<DispatchRltStandard, DispatchRltStandardCreateDto>();
            CreateMap<DispatchRltWorker, DispatchRltWorkerDto>();
            CreateMap<DispatchRltWorker, DispatchRltWorkerCreateDto>();
            CreateMap<DispatchRltWorkFlow, DispatchRltWorkFlowDto>();
            CreateMap<DispatchRltWorkFlow, DispatchRltWorkFlowCreateDto>();

            CreateMap<Daily, DailyCreateDto>();
            CreateMap<DailyCreateDto, Daily>();
            CreateMap<Daily, DailyDto>();
            CreateMap<DailyDto, Daily>();
            CreateMap<Daily, DailyUpdateDto>();
            CreateMap<DailyUpdateDto, Daily>();

            CreateMap<DailyTemplate, DailyTemplateDto>();
            CreateMap<DailyTemplateDto, DailyTemplate>();
            CreateMap<DailyTemplate, DailyTemplateSimpleDto>();
            CreateMap<DailyTemplateSimpleDto, DailyTemplate>();

            CreateMap<DailyRltFile, DailyRltFileDto>();
            CreateMap<DailyRltFileDto, DailyRltFile>();
            CreateMap<DailyRltFile, DailyRltFileSimpleDto>();
            CreateMap<DailyRltFileSimpleDto, DailyRltFile>();

            CreateMap<DailyRltQuality, DailyRltQualityDto>();
            CreateMap<DailyRltQualityDto, DailyRltQuality>();
            CreateMap<DailyRltQuality, DailyRltQualitySimpleDto>();
            CreateMap<DailyRltQualitySimpleDto, DailyRltQuality>();

            CreateMap<DailyRltSafe, DailyRltSafeDto>();
            CreateMap<DailyRltSafeDto, DailyRltSafe>();
            CreateMap<DailyRltSafe, DailyRltSafeSimpleDto>();
            CreateMap<DailyRltSafeSimpleDto, DailyRltSafe>();

            CreateMap<DailyTemplate, DailyTemplateDto>();
            CreateMap<DailyTemplateDto, DailyTemplate>();
            CreateMap<DailyTemplate, DailyTemplateSimpleDto>();
            CreateMap<DailyTemplateSimpleDto, DailyTemplate>();

            CreateMap<UnplannedTask, UnplannedTaskDto>();
            CreateMap<UnplannedTaskDto, UnplannedTask>();
            CreateMap<UnplannedTask, UnplannedTaskSimpleDto>();
            CreateMap<UnplannedTaskSimpleDto, UnplannedTask>();

            CreateMap<DailyRltPlanMaterial, DailyRltPlanMaterialDto>();
            CreateMap<DailyRltPlanMaterialDto, DailyRltPlanMaterial>();
            CreateMap<DailyRltPlanMaterial, DailyRltPlanMaterialSimpleDto>();
            CreateMap<DailyRltPlanMaterialSimpleDto, DailyRltPlanMaterial>();
        }
    }
}