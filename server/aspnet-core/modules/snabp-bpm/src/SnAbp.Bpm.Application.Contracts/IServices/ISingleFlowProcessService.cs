/**********************************************************************
*******命名空间： SnAbp.Bpm.Repositories
*******接口名称： ISingleFlowProcessRepository
*******接口说明： 单一流程出进度处理接口，其他使用单一流程的模块使用
*******作    者： 东腾 Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/15 13:45:21
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2019-2020. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Entities;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace SnAbp.Bpm.IServices
{
    public interface ISingleFlowProcessService : IApplicationService
    {
        /// <summary>
        /// 根据流程获取成员信息
        /// </summary>
        /// <param name="workFlowId"></param>
        /// <returns></returns>

        Task<List<Guid>> GetMembers(Guid workFlowId);
        /// <summary>
        /// 根据流程节点获取改节点的成员信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<List<Guid>> GetMembersByNode(Guid workflowId, Guid nodeId);

        /// <summary>
        /// 获取制定节点上一个或多个节点的成员id
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<List<Guid>> GetPreNodeMembers(Guid workflowId, Guid nodeId);
        /// <summary>
        /// 获取指定节点的下一个或多个节点的成员id
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<List<Guid>> GetNextNodeMember(Guid workflowId, Guid nodeId);
        /// <summary>
        /// 根据工作流id获取流程的所有节点
        /// </summary>
        /// <param name="workFlowId"></param>
        /// <returns></returns>
        Task<List<SingleFlowNodeDto>> GetWorkFlowNodes(Guid workflowId);
        /// <summary>
        /// 获取工作流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkflowDetail> GetWorkflowById(Guid id);
        /// <summary>
        /// 获取当前流程的状态
        /// </summary>
        /// <param name="stepId"></param>
        /// <returns></returns>
        Task<WorkflowStepState> GetStepState(Guid workflowId, Guid stepId);

        /// <summary>
        /// 根据模板id创建一条工作流数据
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        Task<Workflow> CreateSingleWorkFlow(Guid templateId);

        Task<List<SingleFlowNodeDto>> GetNodeTree(List<SingleFlowNodeDto> list);

        /// <summary>
        /// 通过审批
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        Task<WorkflowDetailDto> Approved(
            Guid workflowId,// 工作流id
            string comments,
            Guid? userId //审批人员ID
            );
        /// <summary>
        /// 决绝审批
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        Task<WorkflowDetailDto> Rejected(
            Guid workflowId,// 工作流id
             string comments,
            Guid? userId //审批人员ID
        );
        /// <summary>
        /// 终止审批
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        Task<WorkflowDetailDto> Stopped(
            Guid workflowId,// 工作流id
              string comments,
            Guid? userId //审批人员ID
        );
        /// <summary>
        /// 是否是我审批
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        Task<bool> IsWaitingMyApproval(Guid workflowId);

        /// <summary>
        /// 获取我审批的工作流id
        /// </summary>
        /// <returns></returns>
        Task<List<Guid?>> GetMyApprovaledWorkflow();

        /// <summary>
        /// 创建关联了单一流程的审批内容
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertSingleFlowRltContent<TEntity>(TEntity entity) where TEntity : SingleFlowRltEntity;

        /// <summary>
        /// 根据条件获取评论的结果及状态数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetSingleFlowRltContent<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : class;

        //Task<List<SingleFlowNodeDto>> GetSingleFlowNodes(Guid entityId, Expression<Func<SingleFlowRltEntity, bool>> condition);
    }
}
