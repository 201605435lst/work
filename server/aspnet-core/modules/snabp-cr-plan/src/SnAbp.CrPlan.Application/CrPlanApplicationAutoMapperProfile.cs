using AutoMapper;
using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Dto;
using SnAbp.CrPlan.Dto.AlterRecord;
using SnAbp.CrPlan.Entities;
//using SnAbp.CrPlan.Dto;
//using SnAbp.CrPlan.Entities;
//using Volo.Abp.AutoMapper;
using SnAbp.CrPlan.Dto.WorkOrder;
using SnAbp.CrPlan.Dto.PlanTodo;
using SnAbp.CrPlan.Dto.Statistical;
using SnAbp.CrPlan.Dtos;

namespace SnAbp.CrPlan
{
    public class CrPlanApplicationAutoMapperProfile : Profile
    {
        public CrPlanApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            //CreateMap<OrganizationInputDto, Organization>();
            //CreateMap<OrganizationUpdateDto, Organization>();
            //CreateMap<Organization, OrganizationDto>();
            CreateMap<SkylightPlanDto, SkylightPlan>();
            CreateMap<SkylightPlan, SkylightPlanDto>();
            CreateMap<SkylightPlanCreateDto, SkylightPlan>();
            CreateMap<SkylightPlan, SkylightPlanCreateDto>();
            CreateMap<SkylightPlanUpdateDto, SkylightPlan>();
            CreateMap<SkylightPlan, SkylightPlanUpdateDto>();
            CreateMap<SkylightPlan, SkylightPlanDetailDto>();

            CreateMap<WorkOrder, WorkOrderDto>();
            CreateMap<WorkOrder, WorkOrderUpdateDto>();
            CreateMap<WorkOrder, WorkOrderCreateDto>();
            CreateMap<WorkOrder, WorkOrderDetailDto>();
            CreateMap<WorkOrder, WorkOrderFinishDto>();
            CreateMap<WorkOrder, WorkOrderAcceptanceDto>();
            CreateMap<WorkOrderTestAdditional, WorkOrderTestAdditionalDto>();
            CreateMap<WorkOrderTestAdditionalDto, WorkOrderTestAdditional>();


            CreateMap<WorkOrderDto, WorkOrder>();
            CreateMap<WorkOrderUpdateDto, WorkOrder>();
            CreateMap<WorkOrderCreateDto, WorkOrder>();
            CreateMap<WorkOrderDetailDto, WorkOrder>();
            CreateMap<WorkOrderFinishDto, WorkOrder>();
            CreateMap<WorkOrderAcceptanceDto, WorkOrder>();

            CreateMap<SkylightPlan, PlanTodoDto>(); //��λ����

            CreateMap<PlanDetail, PlanDetailDto>();
            CreateMap<PlanDetailCreateDto, PlanDetail>();
            CreateMap<PlanDetailUpdateDto, PlanDetail>();

            CreateMap<PlanRelateEquipment, PlanRelateEquipmentDto>();
            CreateMap<YearMonthPlan, YearMonthPlanChangeDto>();
            CreateMap<YearMonthPlan, YearMonthPlanDto>();
            CreateMap<YearMonthPlanDto, YearMonthPlan>();
            CreateMap<YearMonthPlan, YearMonthPlanYearDetailDto>();
            CreateMap<YearMonthPlan, YearMonthPlanYearStatisticalDto>();
            CreateMap<YearMonthPlan, YearMonthPlanMonthDetailDto>();
            CreateMap<YearMonthPlanAlter, YearMonthPlanAlterDto>();

            CreateMap<YearMonthAlterRecord, YearMonthAlterRecordDto>();
            CreateMap<YearMonthAlterRecordDto, YearMonthAlterRecord>();
            //�¼ƻ�������
            CreateMap<YearMonthPlan, MonthCompletionDto>();

            CreateMap<YearMonthPlanDto, YearMonthPlanYearDetailDto>();
            CreateMap<YearMonthPlanYearDetailDto, YearMonthPlanDto>();

            CreateMap<YearMonthPlanDto, YearMonthPlanMonthDetailDto>();
            CreateMap<YearMonthPlanMonthDetailDto, YearMonthPlanDto>();

            CreateMap<AlterRecord, AlterRecordDetailDto>();
            CreateMap<AlterRecordDetailDto, AlterRecord>();
            CreateMap<AlterRecord, AlterRecordDto>();
            CreateMap<AlterRecordDto, AlterRecord>();
            CreateMap<AlterRecord, AlterRecordUpdateDto>();
            CreateMap<AlterRecordUpdateDto, AlterRecord>();
            CreateMap<AlterRecord, AlterRecordSimpleDto>();
            CreateMap<AlterRecordSimpleDto, AlterRecord>();
            CreateMap<DailyPlanAlter, DailyPlanAlterDto>();
            CreateMap<DailyPlanAlterDto, DailyPlanAlter>();
            CreateMap<DailyPlan, DailyPlanSelectableDto>();
            CreateMap<DailyPlanSelectableDto, DailyPlan>();
            CreateMap<DailyPlan, DailyPlanDto>();
            CreateMap<DailyPlanDto, DailyPlan>();
            CreateMap<PlanRelateEquipmentCreateDto, PlanRelateEquipment>();
            CreateMap<PlanRelateEquipmentUpdateDto, PlanRelateEquipment>();
            CreateMap<MaintenanceWorkSimpleDto, MaintenanceWork>();
            CreateMap<MaintenanceWork, MaintenanceWorkSimpleDto>();
            CreateMap<MaintenanceWorkDto, MaintenanceWork>();
            CreateMap<MaintenanceWork, MaintenanceWorkDto>();
            CreateMap<MaintenanceWorkRltSkylightPlanDto, MaintenanceWorkRltSkylightPlan>();
            CreateMap<MaintenanceWorkRltSkylightPlan, MaintenanceWorkRltSkylightPlanDto>();
            CreateMap<MaintenanceWorkCreateDto, MaintenanceWorkDto>();
            CreateMap<MaintenanceWorkRltFile, MaintenanceWorkRltFileDto>();
            CreateMap<MaintenanceWorkRltFileDto, MaintenanceWorkRltFile>();

            CreateMap<WorkTicket, WorkTicketDto>();
            CreateMap<WorkTicketDto, WorkTicket>();
            CreateMap<WorkTicket, WorkTicketCreateDto>();
            CreateMap<WorkTicketCreateDto, WorkTicket>();
            CreateMap<WorkTicket, WorkTicketUpdateDto>();
            CreateMap<WorkTicketUpdateDto, WorkTicket>();
            CreateMap<WorkTicketRltCooperationUnit, WorkTicketRltCooperationUnitDto>();
            CreateMap<WorkTicketRltCooperationUnitDto, WorkTicketRltCooperationUnit>();
            CreateMap<WorkTicketSimpleDto, WorkTicket>();
            CreateMap<WorkTicket, WorkTicketSimpleDto>();

            CreateMap<StatisticsEquipmentWorker, GetEquipmentStatisticDto>();
            CreateMap<GetEquipmentStatisticDto, StatisticsEquipmentWorker>();

            CreateMap<File.FileDomainDto, File.Entities.File>();
            CreateMap<File.Entities.File, File.FileDomainDto>();

        }
    }
}