using AutoMapper;
using SnAbp.Emerg.Dtos;
using SnAbp.Emerg.Dtos;
using SnAbp.Emerg.Dtos;
using SnAbp.Emerg.Dtos;
using SnAbp.Emerg.Entities;

namespace SnAbp.Emerg
{
    public class EmergApplicationAutoMapperProfile : Profile
    {
        public EmergApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<EmergPlan, EmergPlanDto>();
            CreateMap<EmergPlanDto, EmergPlan>();
            CreateMap<EmergPlan, EmergPlanSimpleDto>();
            CreateMap<EmergPlanSimpleDto, EmergPlan>();
            CreateMap<EmergPlan, EmergPlanUpdateDto>();
            CreateMap<EmergPlanUpdateDto, EmergPlan>();

            CreateMap<EmergPlanRltFile, EmergPlanRltFileDto>();
            CreateMap<EmergPlanRltFileDto, EmergPlanRltFile>();
            CreateMap<EmergPlanRltFile, EmergPlanRltFileSimpleDto>();
            CreateMap<EmergPlanRltFileSimpleDto, EmergPlanRltFile>();

            CreateMap<EmergPlanRltComponentCategory, EmergPlanRltComponentCategorySimpleDto>();
            CreateMap<EmergPlanRltComponentCategorySimpleDto, EmergPlanRltComponentCategory>();

            CreateMap<EmergPlanRecord, EmergPlanRecordDto>();
            CreateMap<EmergPlanRecordDto, EmergPlanRecord>();
            /*CreateMap<EmergPlanRecord, EmergPlanRecordSimpleDto>();
            CreateMap<EmergPlanRecordSimpleDto, EmergPlanRecord>();*/

            CreateMap<EmergPlanRecordRltFile, EmergPlanRecordRltFileDto>();
            CreateMap<EmergPlanRecordRltFileDto, EmergPlanRecordRltFile>();
            CreateMap<EmergPlanRecordRltFile, EmergPlanRecordRltFileSimpleDto>();
            CreateMap<EmergPlanRecordRltFileSimpleDto, EmergPlanRecordRltFile>();

            CreateMap<EmergPlanRecordRltComponentCategory, EmergPlanRecordRltComponentCategorySimpleDto>();
            CreateMap<EmergPlanRecordRltComponentCategorySimpleDto, EmergPlanRecordRltComponentCategory>();


            CreateMap<Fault, FaultDto>();
            CreateMap<FaultDto, Fault>();
            CreateMap<Fault, FaultSimpleDto>();
            CreateMap<FaultSimpleDto, Fault>();
            CreateMap<FaultDto, FaultSearchDto>();
            CreateMap<FaultSearchDto, Fault>();
            CreateMap<FaultDto, FaultUpdateDto>();
            CreateMap<FaultUpdateDto, Fault>();

            CreateMap<FaultRltComponentCategory, FaultRltComponentCategoryCreateDto>();
            CreateMap<FaultRltComponentCategoryCreateDto, FaultRltComponentCategory>();
            CreateMap<FaultRltComponentCategory, FaultRltComponentCategorySimpleDto>();
            CreateMap<FaultRltComponentCategorySimpleDto, FaultRltComponentCategory>();


            CreateMap<FaultRltEquipment, FaultRltEquipmentCreateDto>();
            CreateMap<FaultRltEquipmentCreateDto, FaultRltEquipment>();
            CreateMap<FaultRltEquipment, FaultRltEquipmentSimpleDto>();
            CreateMap<FaultRltEquipmentSimpleDto, FaultRltEquipment>();

            CreateMap<EmergPlanProcessRecord, EmergPlanProcessRecordDto>();
            CreateMap<EmergPlanProcessRecordDto, EmergPlanProcessRecord>();
        }
    }
}