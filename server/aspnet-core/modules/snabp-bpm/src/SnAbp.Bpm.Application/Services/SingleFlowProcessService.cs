/**********************************************************************
*******命名空间： SnAbp.Bpm.Services
*******类 名 称： SingleFlowProcessService
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/15 15:22:25
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.IServices;
using SnAbp.Bpm.Repositories;
using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace SnAbp.Bpm.Services
{
    /// <summary>
    /// 单一流程接口实现
    /// </summary>
    public class SingleFlowProcessService : BpmAppService, ISingleFlowProcessService
    {
        BpmManager _bpmManager;
        IdentityUserManager _identityUser;
        ISingleFlowProcessRepository _singleFlowProcessRepository;
        IMemberAppService _appMemberAppServiceRepository;
        private IRepository<Workflow, Guid> _workflowRepository;
        private readonly IRepository<WorkflowStateRltMember, Guid> _workflowStateRltMembers;
        private readonly IdentityUserManager _identityUserManager;
        public SingleFlowProcessService(
            BpmManager bpmManager,
            IdentityUserManager identityUser,
            ISingleFlowProcessRepository singleFlowProcessRepository,
            IMemberAppService appMemberAppServiceRepository,
            IRepository<Workflow, Guid> workflowRepository
, IRepository<WorkflowStateRltMember, Guid> workflowStateRltMembers = null, IdentityUserManager identityUserManager = null)
        {
            _bpmManager = bpmManager;
            _identityUser = identityUser;
            _singleFlowProcessRepository = singleFlowProcessRepository;
            _appMemberAppServiceRepository = appMemberAppServiceRepository;
            _workflowRepository = workflowRepository;
            _workflowStateRltMembers = workflowStateRltMembers;
            _identityUserManager = identityUserManager;
        }

        public async Task<List<Guid>> GetMembers(Guid workflowId)
        {
            var workflow = await GetWorkflowDetail(workflowId);
            var nodes = workflow.FlowNodes;
            var result = nodes.SelectMany(a => a.Members.Select(a => a.MemberId)).ToList();
            return result;
        }
        public async Task<WorkflowDetail> GetWorkflowById(Guid id)
        {
            return await GetWorkflowDetail(id);
        }
        private async Task<WorkflowDetail> GetWorkflowDetail(Guid workflowId) => await _bpmManager.GetWorkflowDetail(workflowId, CurrentUser.Id);
        private async Task<List<FlowTemplateNode>> GetWorkflowNodeList(Guid workflowId) => (await GetWorkflowDetail(workflowId)).FlowNodes;
        private async Task<List<FlowTemplateStep>> GetWorkflowSteps(Guid workflowId) => (await GetWorkflowDetail(workflowId)).FlowSteps;

        public async Task<List<Guid>> GetMembersByNode(Guid workflowId, Guid nodeId)
        {
            var nodes = await GetWorkflowNodeList(workflowId);
            return nodes
                .Where(a => a.Id == nodeId)
                .SelectMany(a => a.Members.Select(b => b.MemberId))
                .ToList();
        }

        private async Task<List<FlowTemplateNode>> GetWorkflowNodes(Guid workflowId, Guid nodeId, bool backNode = true)
        {
            var workflow = await GetWorkflowDetail(workflowId);
            var nodes = workflow.FlowNodes;
            var steps = workflow.FlowSteps;
            var target = nodes.SingleOrDefault(a => a.Id == nodeId);
            var pnode = backNode ? _bpmManager.GetFlowBackNodes(nodes, steps, target) : _bpmManager.GetFlowNextNodes(nodes, steps, target);
            return pnode;
        }

        private async Task<List<FlowTemplateNode>> GetWorkflowNodes(WorkflowDetail workflowDetail, Guid nodeId, bool backNode = true)
        {
            var nodes = workflowDetail.FlowNodes;
            var steps = workflowDetail.FlowSteps;
            var target = nodes.SingleOrDefault(a => a.Id == nodeId);
            return backNode ? _bpmManager.GetFlowBackNodes(nodes, steps, target) : _bpmManager.GetFlowNextNodes(nodes, steps, target); ;
        }

        public async Task<List<Guid>> GetPreNodeMembers(Guid workflowId, Guid nodeId)
        {
            var pnode = await GetWorkflowNodes(workflowId, nodeId);
            return pnode.SelectMany(a => a.Members.Select(b => b.MemberId)).ToList();
        }

        public async Task<List<Guid>> GetNextNodeMember(Guid workflowId, Guid nodeId)
        {
            var nnode = await GetWorkflowNodes(workflowId, nodeId, false);
            return nnode.SelectMany(a => a.Members.Select(b => b.MemberId)).ToList();
        }

        public async Task<WorkflowStepState> GetStepState(Guid workflowId, Guid stepId)
        {
            var steps = await GetWorkflowSteps(workflowId);
            return steps.SingleOrDefault(a => a.Id == stepId).State ?? default;
        }
        /// <summary>
        /// 创建工作流
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public async Task<Workflow> CreateSingleWorkFlow(Guid templateId)
        {
            var workflow = await _bpmManager.CreateWorkflow(templateId, "", CurrentUser.Id.GetValueOrDefault());
            await CurrentUnitOfWork.SaveChangesAsync();
            return workflow;
        }

        /// <summary>
        /// 获取node树结构
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Task<List<SingleFlowNodeDto>> GetNodeTree(List<SingleFlowNodeDto> list)
        {
            var datas = GuidKeyTreeHelper<SingleFlowNodeDto>.GetTree(list);
            return Task.FromResult(datas);
        }

        /// <summary>
        /// 获取流程的节点信息
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public async Task<List<SingleFlowNodeDto>> GetWorkFlowNodes(Guid workflowId)
        {
            var nodes = await GetWorkflowNodeList(workflowId);
            var workFlow = _workflowRepository.Where(x => x.Id == workflowId).FirstOrDefault();
            var list = new List<SingleFlowNodeDto>();

            var guidList = new List<Guid>();
            guidList.Add((Guid)workFlow.CreatorId);

            nodes?.ForEach(a =>
            {
                var backNode = GetWorkflowNodes(workflowId, a.Id).Result;
                var dto = new SingleFlowNodeDto();
                dto.Id = a.Id;
                dto.ParentId = backNode.FirstOrDefault()?.Id;
                dto.Name = a.Label;
                dto.Type = a.Type;
                dto.Active = a.Active;
                dto.Approvers = a.Type == "bpmStart" ? GetUsers(guidList).Result : GetUsers(a.Members.Select(b => b.MemberId).ToList()).Result;
                list.Add(dto);
            });
            return list;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private async Task<List<IdentityUserDto>> GetUsers(IReadOnlyList<Guid> ids)
        {
            var user = await _identityUser.GetUserListAsync(a => ids.Contains(a.Id));
            return ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(user);
        }

        /// <summary>
        /// 通过审批
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="userId"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public async Task<WorkflowDetailDto> Approved(Guid workflowId, string comments, Guid? userId = null)
        {
            var detail = await Process(workflowId, userId, WorkflowStepState.Approved,comments);
            return detail;
        }
        /// <summary>
        /// 决绝审批
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="userId"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public async Task<WorkflowDetailDto> Rejected(Guid workflowId, string comments, Guid? userId = null)
        {
            var detail = await Process(workflowId, userId, WorkflowStepState.Rejected, comments);
            return detail;
        }
        /// <summary>
        /// 终止审批
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="userId"></param>
        /// <param name="comments"></param>
        /// <returns></returns>

        public async Task<WorkflowDetailDto> Stopped(Guid workflowId, string comments, Guid? userId = null)
        {
            var detail = await Process(workflowId, userId, WorkflowStepState.Stopped, comments);
            return detail;
        }

        /// <summary>
        /// 处理工作流
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="userId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<WorkflowDetailDto> Process(Guid workflowId, Guid? userId, WorkflowStepState state,string comments)
        {
            if (userId == null)
            {
                userId = CurrentUser.Id.GetValueOrDefault();
            }
            //userId = CurrentUser.Id.GetValueOrDefault();
            var members = (await _identityUser.GetUserMembers((Guid)userId)).Select(a => a.Id);
            // 先获取激活的状态
            var workflow = await GetWorkflowDetail(workflowId);
            // 过滤当前用户所在的节点
            var nodes = workflow.FlowNodes
                .Where(a => a.Members.Select(b => b.MemberId).Intersect(members).Any() && a.Type != "bpmCc" && a.Active)
                .ToList();
            var activeSteps = workflow.FlowSteps.Where(a =>
                nodes.Any(b => b.Id == a.Target)).ToList();
            activeSteps?.ForEach(a => _ = _bpmManager.ProcessWorkflow(
                workflowId,
                "",
                a.Source,
                a.Target,
                state,
                comments,
                (Guid)userId
            ).Result);
            return ObjectMapper.Map<WorkflowDetail, WorkflowDetailDto>(workflow);
        }

        /// <summary>
        /// 判断指定的流程是不是待我审批的流程，返回一个bool
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public async Task<bool> IsWaitingMyApproval(Guid workflowId)
        {
            var userId = CurrentUser.Id.GetValueOrDefault();
            var nodes = await GetWorkflowNodeList(workflowId);
            var activeNode = nodes.Where(a => a.Active).ToList();
            return activeNode.Any(a => a.Members.Any(b => b.MemberId == userId));
        }

        public async Task InsertSingleFlowRltContent<TEntity>(TEntity entity) where TEntity : SingleFlowRltEntity
        {
            await _singleFlowProcessRepository.Insert(entity);
        }

        public async Task<List<TEntity>> GetSingleFlowRltContent<TEntity>(Expression<Func<TEntity, bool>> condition) where TEntity : class
        {
            var list = await _singleFlowProcessRepository.GetList(condition);
            return list;
        }

        /// <summary>
        /// 获取我审批的工作流信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<Guid?>> GetMyApprovaledWorkflow()
        {
            var members = await _identityUserManager.GetUserMembers(CurrentUser.Id.Value);
            var memberIds = members.Select(x => x.Id).ToList();
            var query = _workflowStateRltMembers.WithDetails()
                            .Where(y => y.Group == UserWorkflowGroup.Approved && memberIds.Contains(y.MemberId))
                            .Select(z => z.Workflow)
                            .Distinct();

            var list = query.ToList();
            var result = new List<Guid?>();
            list?.ForEach(a =>
            {
                result.Add(a.Id);
            });
            return result;
            //var simples = GetWorkflowSimples(list);

            //foreach (var item in simples)
            //{
            //    var flow = list.FirstOrDefault(a => a.Id == item.Id);
            //    var lastData = flow?.WorkflowDatas
            //       .OrderBy(a => a.CreationTime)
            //       .LastOrDefault();
            //    // 需要考虑当前对应的节点有没有上级退回的，有则显示被退回
            //    if (lastData.CreatorId == CurrentUser.Id && lastData.StepState == WorkflowStepState.Rejected)
            //    {
            //        item.State = WorkflowState.Rejected;
            //    }
            //    else
            //    if (lastData.CreatorId != CurrentUser.Id &&
            //        lastData.StepState == WorkflowStepState.Rejected)
            //    {
            //        var laststeps = flow.FlowTemplate.Steps.Where(a => a.Target == lastData.TargetNodeId);
            //        var secondLastNodes = flow.FlowTemplate.Nodes
            //            .Where(a => laststeps.Any(b => b.Source == a.Id)).ToList();
            //        if (secondLastNodes.SelectMany(a => a.Members)
            //            .Any(a => a.MemberId == CurrentUser.Id.GetValueOrDefault()))
            //        {
            //            item.State = WorkflowState.Rejected;
            //        }
            //        else
            //        {
            //            // 如果最后一条数据和当前用户所在节点没有直接关系，则查询当前节点在记录中存在的最新一条记录的状态
            //            var currentUserNodes = flow.FlowTemplate.Nodes.Where(a =>
            //                a.Members.Any(b => b.MemberId == CurrentUser.Id) && a.Type != "bpmStart" && a.Type != "bpmEnd" && a.Type != "bpmCc");
            //            if (currentUserNodes.Any())
            //            {
            //                var flowData = item.WorkflowDatas.Where(a =>
            //                        currentUserNodes.Any(b => b.Id == a.TargetNodeId))
            //                    .OrderBy(a => a.CreationTime)
            //                    .LastOrDefault();
            //                if (flowData.StepState == WorkflowStepState.Rejected)
            //                {
            //                    item.State = WorkflowState.Rejected;
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        item.State = flow.State;
            //    }
            //}
        }
        private List<WorkflowSimple> GetWorkflowSimples(IEnumerable<Workflow> workflows)
        {
            var workflowSimples = new List<WorkflowSimple>();
            foreach (var item in workflows)
            {
                if (item.FlowTemplate == null) { }
                var isWorkflowExist = item.FlowTemplate?.FormTemplate.WorkflowTemplate;
                if (isWorkflowExist == null)
                {
                    continue;
                }
                var simple = _bpmManager.GetWorkflowSimple(item, CurrentUser.Id.Value);

                simple.FormTemplateVersion = item.FlowTemplate.FormTemplate.Version;
                simple.FlowTemplateVersion = item.FlowTemplate.Version;
                simple.State = item.State;
                simple.LastModificationTime = item.LastModificationTime;
                simple.IsStatic = item.FlowTemplate.FormTemplate?.WorkflowTemplate?.IsStatic ?? false;
                workflowSimples.Add(simple);
            };
            return workflowSimples;
        }
        ///// <summary>
        ///// 根据关联工作流的id获取工作流的节点及对应节点的审批信息
        ///// </summary>
        ///// <param name="entityId"></param>
        ///// <param name="condition"></param>
        ///// <returns></returns>
        //public async Task<List<SingleFlowNodeDto>> GetSingleFlowNodes(Guid entityId, Expression<Func<SingleFlowRltEntity, bool>> condition)
        //{
        //    var infos = await GetSingleFlowRltContent(condition);
        //    var nodes = await this.GetWorkFlowNodes(entityId);
        //    foreach (var node in nodes)
        //    {
        //        node.Comments ??= new List<CommentDto>();
        //        node.Approvers?.ForEach(a =>
        //        {
        //            var info = infos.FirstOrDefault(b => b.CreatorId == a.Id);
        //            var comment = new CommentDto()
        //            {
        //                User = a,
        //                Content = infos.Where(b => b.CreatorId == a.Id).Select(a => a.Content).ToList(),
        //                ApproveTime = info?.CreationTime ?? default
        //            };
        //            node.Comments.Add(comment);
        //        });
        //    }
        //    return await this.GetNodeTree(nodes);
        //    //return null;

        //}

    }
}
