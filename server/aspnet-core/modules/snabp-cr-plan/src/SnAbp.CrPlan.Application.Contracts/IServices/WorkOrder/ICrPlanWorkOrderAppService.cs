using Microsoft.AspNetCore.Http;
using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Dto.WorkOrder;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.CrPlan.IServices.WorkOrder
{
    /// <summary>
    /// 派工单相关接口
    /// </summary>
    public interface ICrPlanWorkOrderAppService : IApplicationService
    {

        /// <summary>
        /// 根据查询条件，获取已派工模块的主页显示数据列表
        /// 使用范围：车间及以上组织机构人员
        /// </summary>
        /// <param name="input">查询条件实体</param>
        /// <returns></returns>
        Task<PagedResultDto<WorkOrderSimpleDto>> GetListSentedWorkOrders(SkylightSearchInputDto input);

        /// <summary>
        /// 根据查询条件，获取派工作业、已完成模块的主页显示数据列表
        /// </summary>
        /// <param name="input">查询条件实体</param>
        /// <returns></returns>
        Task<PagedResultDto<WorkOrderSimpleDto>> GetList(WorkOrderSearchInputDto input);

        /// <summary>
        /// 获取其他作业列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OtherHomeworkDto>> GetOtherHomeworkList(OtherPlanSearchInputDto input);


        /// <summary>
        /// 根据派工单ID获取派工单实体（无设备测试项）
        /// </summary>
        /// <param name="id">派工单ID</param>
        /// <returns></returns>
        Task<WorkOrderDto> Get(CommonGuidGetDto input);

        /// <summary>
        /// 根据派工单ID获取派工单实体（有设备测试项）
        /// </summary>
        /// <param name="id">派工单ID</param>
        /// <returns></returns>
        Task<WorkOrderDetailDto> GetDetail(CommonGuidGetDto input);

        /// <summary>
        /// 添加派工单（状态默认未完成）
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <returns></returns>
        Task<WorkOrderCreateDto> Create(WorkOrderCreateDto input, bool isOtherPlan);

        /// <summary>
        /// 修改派工单（无天窗相关）
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <returns></returns>
        Task<WorkOrderUpdateDto> Update(WorkOrderUpdateDto input);

        /// <summary>
        /// 完成派工单
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <param name="issave">是否只保存数据</param>
        /// <param name="isOtherPlan">是否其他计划</param>
        /// <returns></returns>
        Task<WorkOrderFinishDto> Finish(WorkOrderFinishDto input, bool issave, bool isOtherPlan);

        /// <summary>
        /// 验收派工单
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <param name="issave">是否只保存数据</param>
        /// <returns></returns>
        Task<WorkOrderAcceptanceDto> Acceptance(WorkOrderAcceptanceDto input, bool issave);

        /// <summary>
        /// 编辑派工单（已完成模块使用）
        /// </summary>
        /// <param name="input">派工单实体</param>
        /// <returns></returns>
        Task<WorkOrderFinishDto> UpdateDetail(WorkOrderFinishDto input);

        /// <summary>
        /// 撤销派工单
        /// 删除派工单数据
        /// 对应天窗计划状态改为已发布
        /// 设备测试项清理检修结果、验收结果数据
        /// 相关设备检修人员数据删除
        /// </summary>
        /// <param name="id">派工单ID</param>
        /// <returns></returns>
        Task<bool> Delete(WorkOrderDeleteDto input);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id">派工单Id</param>
        /// <param name="ismaintenance">是否导出检修表</param>
        /// <returns></returns>
        Task<Stream> Export(Guid id, bool ismaintenance, string repairTagKey);

        /// <summary>
        /// 根据天窗计划id查询复验人员
        /// </summary>
        /// <param name="skylightPlanId"></param>
        /// <returns></returns>
        Task<MemberDto> CheckPlatformLiaisonOfficer(Guid skylightPlanId);

        /// <summary>
        /// 获取派工单测试项的其他附加测试项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> GetWorkOrderTestAdditional(WorkOrderTestAdditionalDto input);

        /// <summary>
        /// 创建派工单测试项的其他附加测试项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CreateWorkOrderTestAdditional(WorkOrderTestAdditionalDto input);

        /// <summary>
        /// 测试项目文件上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UploadWorkOrderTestAdditional(IFormFile file, Guid workOrderId, string number);
    }
}
