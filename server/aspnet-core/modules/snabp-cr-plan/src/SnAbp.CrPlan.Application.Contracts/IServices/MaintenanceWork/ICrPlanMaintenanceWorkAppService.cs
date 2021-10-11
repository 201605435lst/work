using Microsoft.AspNetCore.Mvc;
using SnAbp.Bpm;
using SnAbp.CrPlan.Dto;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Dtos.MaintenanceWork;
using SnAbp.File;
using SnAbp.File.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.CrPlan.IServices.MaintenanceWork
{
    public interface ICrPlanMaintenanceWorkAppService : IApplicationService
    {
        Task<PagedResultDto<MaintenanceWorkSimpleDto>> GetList(MaintenanceWorkSearchDto input);

        /// <summary>
        /// 获取第二阶段审批的维修作业
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MaintenanceWorkSimpleDto>> GetListForSecondStep(MaintenanceWorkSearchDto input);

        /// <summary>
        /// 提交维修计划的第二级审批
        /// </summary>
        /// <returns></returns>
        //Task<bool> SumbitSecondFlow(Guid id);

        /// <summary>
        /// 维修计划第二级审批 审批通过
        /// </summary>
        /// <param name="id">审批流id</param>
        /// <returns></returns>
        //Task<bool> FinishSecondFlow(Guid id, WorkflowState state);

        Task<List<MaintenanceWorkDto>> Get(Guid id);

        Task<MaintenanceWorkDto> Create(MaintenanceWorkCreateDto input);

        Task<bool> Delete(Guid id);

        Task<MaintenanceWorkDetailDto> GetMaintenanceWork(Guid workflowId);

        /// <summary>
        /// 提交维修计划第一阶段审批
        /// </summary>
        /// <returns></returns>
        Task<bool> SumbitFirsrFlow(Guid skylightPlanId, List<FileDomainDto> files);

        Task<bool> FirstFinshProcrss(Guid id, WorkflowState state);

        Task<bool> RemoveMaintenanceWorkRltSkylightPlan(RemoveMaintenanceWorkRltSkylightPlanDto input);

        /// <summary>
        /// 导出维修计划
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        Task<FileStreamResult> ExportMaintenanceWorkPlan(Guid workflowId);

        /// <summary>
        /// 导出工作票
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="isWorkflowId">True:工作流id False:天窗id</param>
        /// <returns></returns>
        Task<FileStreamResult> ExportWorkTicket(Guid workflowId, bool isWorkflowId);

        /// <summary>
        /// 导出计划方案
        /// </summary>
        /// <param name="worlflowId"></param>
        /// <returns></returns>
        Task<FileStreamResult> ExportPlan(Guid worlflowId);
    }
}
