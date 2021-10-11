using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.CrPlan.IServices.SkylightPlan
{
    /// <summary>
    /// 天窗、其他计划
    /// </summary>
    public interface ICrPlanSkylightPlanAppService : IApplicationService
    {
        /// <summary>
        /// 获取天窗计划列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SkylightPlanDto>> GetList(SkylightPlanSearchInputDto input);

        /// <summary>
        /// 获取详情（天窗、其他计划）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SkylightPlanDetailDto> Get(CommonGuidGetDto input);
        /// <summary>
        /// 获取最后添加的一条数据详情（天窗、其他计划）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SkylightPlanDetailDto> GetLastPlan(CommonGuidGetDto input);
        /// <summary>
        /// 获取详情(派工作业专用)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SkylightPlanDetailDto> GetInWork(CommonGuidGetDto input);
        ///// <summary>
        ///// 发布天窗计划
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<bool> PublishPlan(SkylightPlanSearchInputDto input);
        /// <summary>
        /// 撤销天窗计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> BackoutPlan(CommonGuidGetDto input);
        /// <summary>
        /// 添加(1:垂直 2:综合 3:点外 5:其他)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isOther">True:其他计划 False:天窗计划</param>
        /// <returns></returns>
        Task<bool> Create(SkylightPlanCreateDto input, bool isOther);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isOther">True:其他计划 False:天窗计划</param>
        /// <returns></returns>
        Task<bool> Update(SkylightPlanUpdateDto input, bool isOther);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Remove(CommonGuidGetDto input);

        //计划跨越变更
        Task<bool> CrossMonthChange(Guid? skylightPlanId);

        //天窗数据统计
        Task<SkylightPalnDataStatisticDto> GetSkylightDataStatistic(SkylightPlanSearchInputDto input);

        Task<SkylightPlanSpecificDataStatisticsDto> GetSkylightSpecificData(SkylightPlanSearchInputDto input);
        #region 其他计划
        /// <summary>
        /// 获取其他计划列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OtherPlanDto>> GetOtherPlanList(OtherPlanSearchInputDto input);
        /// <summary>
        /// 下发其他计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> PublishOtherPlan(OtherPlanSearchInputDto input);
        #endregion


        #region 工作票
        /// <summary>
        /// 添加工作票
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkTicketDto> CreateWorkTicket(WorkTicketCreateDto input);

        /// <summary>
        /// 获取工作票
        /// </summary>
        /// <param name="id">垂直天窗id</param>
        /// <returns></returns>
        Task<List<WorkTicketDto>> GetWorkTickets(Guid id);

        /// <summary>
        /// 获取工作票完成数量情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkTicketFinishInfoDto> GetWorkTicketFinishInfo(Guid id);

        /// <summary>
        /// 修改工作票
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkTicketDto> UpdateWorkTicket(WorkTicketUpdateDto input);

        /// <summary>
        /// 完成工作票
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkTicketDto> FinishWorkTicket(WorkTicketFinishDto input);

        /// <summary>
        /// 删除工作票
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> DeleteWorkTicket(WorkTicketDeleteDto input);

        //Task<Stream> Export(Guid id);

        Task<Stream> Mulexport();
        #endregion

        #region 代办通知
        Task<bool> ConfirmTodoMessage(Guid MeaasgeId);

        /// <summary>
        /// 获取配合作业列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<WorkTicketSimpleDto>> GetWorkTicketList(WorkTicketSearchDto input);

        /// <summary>
        /// 完成配合作业
        /// </summary>
        /// <param name="cooperationWorkId"></param>
        /// <returns></returns>
        Task<bool> FinishCooperationWork(FinsCooperationWorkDto input);

        /// <summary>
        /// 一键生成其他计划中的月表计划
        /// </summary>
        /// <param name="workUnitId"></param>
        /// <returns></returns>
        Task<bool> OneTouchMonthPlan(Guid workUnitId, DateTime month);

        #region 保存维修计划关联垂直天窗及作业方案

        Task<bool> SaveFileRltMaintenanceWork(List<Guid> fileIds, Guid skylightPlanId, string schemeCoverName);
        #endregion

        Task<bool> CancelPlan(Guid id, string type, string cancelReason, bool isWorkOrder);

        /// <summary>
        /// 下载计划方案封面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanCoverInfoDto> DownLoadPlanCover(Guid id);
    }
}
#endregion

