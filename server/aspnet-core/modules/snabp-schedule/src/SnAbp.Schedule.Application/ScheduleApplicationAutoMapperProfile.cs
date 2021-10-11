using AutoMapper;
using SnAbp.Schedule.Dtos;
using SnAbp.Schedule.Entities;

namespace SnAbp.Schedule
{
    public class ScheduleApplicationAutoMapperProfile : Profile
    {
        public ScheduleApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Schedule, ScheduleDto>();
            CreateMap<ScheduleDto, Schedule>();

            CreateMap<ScheduleRltScheduleDto, ScheduleRltSchedule>();
            CreateMap<ScheduleRltSchedule, ScheduleRltScheduleDto>();

            CreateMap<Schedule, ScheduleRltScheduleDto>();
            CreateMap<ScheduleRltScheduleDto, Schedule>();

            CreateMap<Schedule, ScheduleSimpleDto>();
            CreateMap<ScheduleSimpleDto, Schedule>();

            CreateMap<ScheduleFlowTemplate, ScheduleFlowTemplateDto>();
            CreateMap<ScheduleFlowTemplateDto, ScheduleFlowTemplate>();

            CreateMap<Approval, ApprovalDto>();
            CreateMap<ApprovalDto, Approval>(); 
                 CreateMap<Approval, DiaryExportDto>();
            CreateMap<DiaryExportDto, Approval>(); 
            CreateMap<Approval, ApprovalMultipleDto>();
            CreateMap<ApprovalMultipleDto, Approval>();

            CreateMap<Diary, DiaryDto>();
            CreateMap<DiaryDto, Diary>();
            CreateMap<Diary, DiaryCreateDto>();
            CreateMap<DiaryCreateDto, Diary>();
            CreateMap<Diary, DiaryUpdateDto>();
            CreateMap<DiaryUpdateDto, Diary>();

            CreateMap<DiaryRltBuilder, DiaryRltBuilderDto>();
            CreateMap<DiaryRltBuilderDto, DiaryRltBuilder>();

            CreateMap<DiaryRltFileDto, DiaryRltFile>();
            CreateMap<DiaryRltFile, DiaryRltFileDto>();

            CreateMap<DiaryRltMaterialDto, DiaryRltMaterial>();
            CreateMap<DiaryRltMaterial, DiaryRltMaterialDto>();

            CreateMap<ApprovalRltMaterialDto, ApprovalRltMaterial>();
            CreateMap<ApprovalRltMaterial, ApprovalRltMaterialDto>();
        }
    }
}