using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Math.EC.Rfc7748;

using SnAbp.Bpm.Authorization;
using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.IServices;
using SnAbp.Identity;
using SnAbp.Message.Bpm;
using SnAbp.Message.MessageDefine;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace SnAbp.Bpm.Services
{
    public class BpmWorkflowAppService : BpmAppService, IBpmWorkflowAppService
    {
        protected IMemberAppService MemberManager { get; }
        private readonly IRepository<Workflow, Guid> workflowRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IMessageBpmProvider _messageBpmProvider;
        private readonly IRepository<WorkflowTemplate, Guid> _workflowTemplates;
        private readonly BpmWorkflowTemplateAppService _bpmWorkflowTemplateAppServices;
        private readonly BpmManager bpmManager;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IRepository<WorkflowStateRltMember, Guid> _workflowStateRltMembers;
        public IEventBus EventBus { get; set; }
        public BpmWorkflowAppService(
            IRepository<Workflow, Guid> workflowRepository,
            BpmManager bpmManager,
            IMemberAppService memberManager,
            IdentityUserManager identityUserManager,
            IUnitOfWorkManager unitOfWorkManager,
            IIdentityUserRepository userRepository,
            IMessageBpmProvider messageBpmProvider,
            IRepository<WorkflowStateRltMember, Guid> workflowStateRltMembers,
            IRepository<WorkflowTemplate, Guid> workflowTemplates,
            BpmWorkflowTemplateAppService bpmWorkflowTemplateAppServices
            )
        {
            this.workflowRepository = workflowRepository;
            this.bpmManager = bpmManager;
            this.MemberManager = memberManager;
            this._identityUserManager = identityUserManager;
            _workflowStateRltMembers = workflowStateRltMembers;
            _unitOfWorkManager = unitOfWorkManager;
            _userRepository = userRepository;
            _messageBpmProvider = messageBpmProvider;
            _workflowTemplates = workflowTemplates;
            _bpmWorkflowTemplateAppServices = bpmWorkflowTemplateAppServices;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WorkflowDetailDto> Get([Required] Guid id)
        {
            //var memberIds = (await systemManager.GetUserMembers(CurrentUser.Id.Value)).Select(x => x.Id).ToList();

            using var uow = _unitOfWorkManager.Begin(true);
            var workflow = await bpmManager.GetWorkflowDetail(id, CurrentUser.Id.GetValueOrDefault());
            return ObjectMapper.Map<WorkflowDetail, WorkflowDetailDto>(workflow);
        }
        /// <summary>
        /// 获取当前工作流的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<WorkflowDto> GetWorkflow(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var workflow = workflowRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (workflow == null) throw new UserFriendlyException("此工作流不存在");
            return Task.FromResult(ObjectMapper.Map<Workflow, WorkflowDto>(workflow));
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns> 
        public async Task<PagedResultDto<WorkflowSimpleDto>> GetList(WorkflowSearchInputDto input)
        {
            var members = await _identityUserManager.GetUserMembers(CurrentUser.Id.Value);
            var memberIds = members.Select(x => x.Id).ToList();
            var currentUserOrganizations = _identityUserManager.GetOrganizationsAsync(CurrentUser.Id.GetValueOrDefault()).Result;
            //var users = _userRepository.Where(x => userIds.Contains(x.Id)).ToList();
            var list = new List<Workflow>();
            var rst = new PagedResultDto<WorkflowSimpleDto>();
            switch (input.Group)
            {
                // 我发起的
                case UserWorkflowGroup.Initial:
                    {
                        list = _workflowStateRltMembers
                            .WithDetails()
                            .Where(y =>
                                    y.Group == UserWorkflowGroup.Initial
                                 && y.MemberId == CurrentUser.Id.Value 
                                 && y.MemberType == MemberType.User
                                 && input.SubmitStartTime <= y.CreationTime 
                                 && input.SubmitEndTime >= y.CreationTime)
                            .Select(z => z.Workflow)
                            .ToList();

                        if (!input.Name.IsNullOrEmpty())
                        {
                            list = list.Where(x =>
                                x.FlowTemplate.FormTemplate.WorkflowTemplate.Name.Contains(input.Name)).ToList();
                        }
                        if (input.State != WorkflowState.All)
                        {
                            list = list.Where(x => x.State == input.State).Distinct().ToList();
                        }
                        // 数据过滤，不查询单一流程的发起记录。
                        list = list.Where(a => a.FlowTemplate.FormTemplate.WorkflowTemplate.Type == WorkflowTemplateType.Default)
                            .ToList();
                        break;
                    }
                // 待我审批
                case UserWorkflowGroup.Waiting:
                    {

                        //重构 2020.10.10
                        var query = _workflowStateRltMembers
                            .WithDetails()
                            .Where(y => (memberIds.Contains(y.MemberId) || (int)y.MemberType >= 21)
                                && y.Group == UserWorkflowGroup.Waiting
                                && input.SubmitStartTime <= y.CreationTime && input.SubmitEndTime >= y.CreationTime)
                            .Select(z => z.Workflow)
                            .Distinct();

                        list = await GetCheckLevelWorkflow(currentUserOrganizations, query, true);
                     
                        // 数据过滤，不查询单一流程的发起记录。
                        list = list.Where(a => a.FlowTemplate.FormTemplate.WorkflowTemplate.Type == WorkflowTemplateType.Default)
                            .ToList();
                        // 先找到待审批的流程和激活的节点，判断当前用户是否属于该节点
                        if (!input.Name.IsNullOrEmpty())
                        {
                            list = list.Where(x =>
                                x.FlowTemplate.FormTemplate.WorkflowTemplate.Name.Contains(input.Name)).ToList();
                        }

                        var approveWorlflows = input.IsAll ? list.ToList() : list.OrderByDescending(s => s.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                        var simples = GetWorkflowSimples(approveWorlflows);
                        foreach (var item in simples)
                        {
                            item.State = GetSimpleFlowState(list.FirstOrDefault(a => a.Id == item.Id));
                        }
                        rst.TotalCount = list.Count();
                        rst.Items = ObjectMapper.Map<List<WorkflowSimple>, List<WorkflowSimpleDto>>(simples);
                        return rst;
                    }
                // 我审批的
                case UserWorkflowGroup.Approved:
                    {
                        var query = _workflowStateRltMembers.WithDetails()
                            .Where(y => y.Group == UserWorkflowGroup.Approved && memberIds.Contains(y.MemberId)
                            && input.SubmitStartTime <= y.CreationTime && input.SubmitEndTime >= y.CreationTime)
                            .Select(z => z.Workflow)
                            .Distinct();
                        list = query
                            .Where(a => a.FlowTemplate.FormTemplate.WorkflowTemplate.Type == WorkflowTemplateType.Default)
                            .ToList();
                        if (!input.Name.IsNullOrEmpty())
                        {
                            list = list.Where(x =>
                                x.FlowTemplate.FormTemplate.WorkflowTemplate.Name.Contains(input.Name)).ToList();
                        }

                        if (input.State != WorkflowState.All)
                        {
                            list = list.Where(x => x.State == input.State).ToList();
                        }

                        var approveWorlflows = input.IsAll ? list.ToList() : list.OrderByDescending(s => s.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                        var simples = GetWorkflowSimples(approveWorlflows);

                        foreach (var item in simples)
                        {
                            var flow = list.FirstOrDefault(a => a.Id == item.Id);
                            var lastData = flow?.WorkflowDatas
                               .OrderBy(a => a.CreationTime)
                               .LastOrDefault();
                            // 需要考虑当前对应的节点有没有上级退回的，有则显示被退回
                            if (lastData.CreatorId == CurrentUser.Id && lastData.StepState == WorkflowStepState.Rejected)
                            {
                                item.State = WorkflowState.Rejected;
                            }
                            else
                            if (lastData.CreatorId != CurrentUser.Id &&
                                lastData.StepState == WorkflowStepState.Rejected)
                            {
                                var laststeps = flow.FlowTemplate.Steps.Where(a => a.Target == lastData.TargetNodeId);
                                var secondLastNodes = flow.FlowTemplate.Nodes
                                    .Where(a => laststeps.Any(b => b.Source == a.Id)).ToList();
                                if (secondLastNodes.SelectMany(a => a.Members)
                                    .Any(a => a.MemberId == CurrentUser.Id.GetValueOrDefault()))
                                {
                                    item.State = WorkflowState.Rejected;
                                }
                                else
                                {
                                    // 如果最后一条数据和当前用户所在节点没有直接关系，则查询当前节点在记录中存在的最新一条记录的状态
                                    var currentUserNodes = flow.FlowTemplate.Nodes.Where(a =>
                                        a.Members.Any(b => b.MemberId == CurrentUser.Id) && a.Type != "bpmStart" && a.Type != "bpmEnd" && a.Type != "bpmCc");
                                    if (currentUserNodes.Any())
                                    {
                                        var flowData = item.WorkflowDatas.Where(a =>
                                                currentUserNodes.Any(b => b.Id == a.TargetNodeId))
                                            .OrderBy(a => a.CreationTime)
                                            .LastOrDefault();
                                        if (flowData.StepState == WorkflowStepState.Rejected)
                                        {
                                            item.State = WorkflowState.Rejected;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                item.State = flow.State;
                            }
                        }
                        rst.TotalCount = list.Count();
                        rst.Items = ObjectMapper.Map<List<WorkflowSimple>, List<WorkflowSimpleDto>>(simples);
                        return rst;
                    }
                // 抄送我的
                case UserWorkflowGroup.Cc:
                    {
                        var query = _workflowStateRltMembers.WithDetails()
                            .Where(y => y.Group == UserWorkflowGroup.Cc && (memberIds.Contains(y.MemberId) || (int)y.MemberType >= 21)
                            && input.SubmitStartTime <= y.CreationTime && input.SubmitEndTime >= y.CreationTime)
                            .Select(z => z.Workflow)
                            .Distinct();

                        list = await GetCheckLevelWorkflow(currentUserOrganizations, query, false);
                        list = query
                           .Where(a => a.FlowTemplate.FormTemplate.WorkflowTemplate.Type == WorkflowTemplateType.Default)
                           .ToList();
                        if (!input.Name.IsNullOrEmpty())
                        {
                            list = list.Where(x =>
                                x.FlowTemplate.FormTemplate.WorkflowTemplate.Name.Contains(input.Name)).ToList();
                        }

                        if (input.State != WorkflowState.All)
                        {
                            list = list.Where(x => x.State == input.State).ToList();
                        }

                        var approveWorlflows = input.IsAll ? list.ToList() : list.OrderByDescending(s => s.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                        var simples = GetWorkflowSimples(approveWorlflows);
                        foreach (var item in simples)
                        {
                            var flow = list.FirstOrDefault(a => a.Id == item.Id);
                            var lastData = flow?.WorkflowDatas
                                .OrderBy(a => a.CreationTime)
                                .LastOrDefault();
                            item.State = lastData?.StepState == WorkflowStepState.Rejected ? WorkflowState.Rejected : flow.State;
                        }
                        rst.TotalCount = list.Count();
                        rst.Items = ObjectMapper.Map<List<WorkflowSimple>, List<WorkflowSimpleDto>>(simples);
                        return rst;
                    }
                case UserWorkflowGroup.All:
                    break;
                default:
                    throw new UserFriendlyException("数据处理异常");
            }
            rst.TotalCount = list.Count();
            var workflows = input.IsAll ? list.ToList() : list.OrderByDescending(s => s.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var workflowSimples = GetWorkflowSimples(workflows);
            rst.Items = ObjectMapper.Map<List<WorkflowSimple>, List<WorkflowSimpleDto>>(workflowSimples);
            return rst;
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
                var simple = bpmManager.GetWorkflowSimple(item, CurrentUser.Id.Value);

                simple.FormTemplateVersion = item.FlowTemplate.FormTemplate.Version;
                simple.FlowTemplateVersion = item.FlowTemplate.Version;
                simple.State = item.State;
                simple.LastModificationTime = item.LastModificationTime;
                simple.IsStatic = item.FlowTemplate.FormTemplate?.WorkflowTemplate?.IsStatic ?? false;
                workflowSimples.Add(simple);
            };
            return workflowSimples;
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private WorkflowState GetSimpleFlowState(Workflow item)
        {
            var flowData = item.WorkflowDatas.OrderBy(a => a.CreationTime);
            var lastData = flowData.LastOrDefault();
            return lastData?.StepState switch
            {
                WorkflowStepState.Rejected => WorkflowState.Rejected,
                WorkflowStepState.Stopped => WorkflowState.Stopped,
                null => WorkflowState.Waiting,
                WorkflowStepState.Approved => WorkflowState.Waiting,
                _ => WorkflowState.Waiting
            };
        }

        /// <summary>
        /// 获取待我审批的数据
        /// </summary>
        /// <param name="currentUserOrganizations"></param>
        /// <param name="workflows"></param>
        /// <returns></returns>

        private async Task<List<Workflow>> GetCheckLevelWorkflow(List<Organization> currentUserOrganizations, IQueryable<Workflow> workflows, bool isWaiting)
        {
            var result = new List<Workflow>();
            foreach (var workflow in workflows)
            {
                var detial = await bpmManager.GetWorkflowDetail(workflow.Id, CurrentUser.Id.GetValueOrDefault());
                var creatorOrgs = bpmManager.GetCreatorOrganizations(workflow.CreatorId.GetValueOrDefault());
                var members = await _identityUserManager.GetUserMembers(CurrentUser.Id.Value);
                List<FlowTemplateNode> nodes = null;
                if (isWaiting)
                {
                    nodes = detial.FlowNodes.Where(x => x.Active).ToList();
                }
                else
                {
                    nodes = detial.FlowNodes.Where(x => x.Type == "bpmCc").ToList();
                }

                foreach (var node in nodes)
                {
                    var has = false;
                    foreach (var member in node.Members)
                    {
                        // 判断动态成员
                        // 判断非动态成员 
                        if ((int)member.Type < 21 && members.Select(x => x.Id).Contains(member.MemberId) ||
                            bpmManager.CheckCurrentUserInCreatorOrg(currentUserOrganizations, creatorOrgs,
                                (int)member.Type))
                        {
                            has = true;
                            break;
                        }
                    }
                    if (has)
                    {
                        result.Add(workflow);
                        break;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 根据工作流id获取工作流模板的id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns
        public Task<Guid> GetWorkflowTemplateId(Guid id)
        {
            var temp = workflowRepository
                .WithDetails()
                .FirstOrDefault(a => a.Id == id)
                ?.FlowTemplate.FormTemplate.WorkflowTemplateId;
            return Task.FromResult(temp.GetValueOrDefault());
        }

        /// <summary>
        /// 获取流程简报数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IOrderedEnumerable<WorkflowImformationDto>> GetWorkflowData(Guid id)
        {
            var currentId = CurrentUser.Id.GetValueOrDefault();//currenGuid;

            using var uow = _unitOfWorkManager.Begin(true);
            var workflow = await bpmManager.GetWorkflowDetail(id, currentId);

            List<WorkflowImformationDto> infoList = new List<WorkflowImformationDto>();


            var currentUserOrganizations = _identityUserManager.GetOrganizationsAsync(currentId).Result;

            List<WorkflowData> newWorkflowData = new List<WorkflowData>();

            //获取开始节点
            var starNode =
                    bpmManager.GetFlowStartNode(workflow.FlowNodes, workflow.FlowSteps);

            var creatorId = workflowRepository.FirstOrDefault(x => x.Id == id)?.CreatorId;

            var user = await _identityUserManager.GetByIdAsync(creatorId.GetValueOrDefault());
            //添加开始节点
            var startNodeInfo = new WorkflowImformationDto
            {
                Comments = "",
                Infos = bpmManager.GetWorkflowInfos(workflow.FlowTemplate.Nodes, workflow.FlowTemplate.FormTemplate.FormItems, workflow.WorkflowDatas, currentId, currentUserOrganizations, creatorId.GetValueOrDefault()),//处理简报
                Time = workflow.CreationTime,
                NodeLabel = starNode.Label,
                State = null,
                UserName = user.Name,//null
                NodeType = starNode.Type
            };

            infoList.AddFirst(startNodeInfo);

            var startNextNodes = bpmManager.GetFlowNextNodes(workflow.FlowTemplate.Nodes,
                workflow.FlowTemplate.Steps, starNode);
            //待处理人员信息
            var nextNodeInfo = new WorkflowImformationDto();
            //获取待处理节点成员
            var memberListInfos = new MemberSearchInputDto();

            startNextNodes?.ForEach(async startNextNode =>
            {
                if (!startNextNode.Active)
                {
                    return;
                }

                nextNodeInfo = await NextNodeInfos(startNextNode, nextNodeInfo);
                //nextNodeInfo.NodeLabel = startNextNode.Label;
                //nextNodeInfo.IsBpmApprove = true;
                //nextNodeInfo.Time = DateTime.Now;
                //nextNodeInfo.NodeType = startNextNode.Type;
                //startNextNode.Members?.ForEach(member =>
                //{
                //    var memberInfos = new MemberInfoDto
                //    {
                //        Id = member.MemberId,
                //        Type = member.Type
                //    };
                //    memberListInfos.MemberInfos.Add(memberInfos);
                //});

            });


            //添加其他节点
            var nodes = workflow.FlowNodes;
            var data = workflow.WorkflowDatas;

            foreach (var node in nodes)
            {
                if (node.Type != "bpmApprove")
                {
                    continue;
                }
                foreach (var d in data)
                {
                    if (d.TargetNodeId != node.Id || d.Comments == d.WorkflowId.ToString())
                    {
                        continue;
                    }

                    var workflowInformations = new WorkflowImformationDto();

                    newWorkflowData.Add(d);

                    var infos = bpmManager.GetWorkflowInfos(workflow.FlowNodes, workflow.FormItems, newWorkflowData, currentId, currentUserOrganizations, creatorId.GetValueOrDefault());

                    workflowInformations.Comments = d.Comments;//审批建议
                    workflowInformations.Time = d.CreationTime;//处理时间;
                    workflowInformations.Infos = infos;//处理简报
                    workflowInformations.NodeLabel = node.Label;//节点名称
                    workflowInformations.State = d.StepState;//节点状态
                    workflowInformations.UserId = d.CreatorId;
                    workflowInformations.NodeType = node.Type;
                    infoList.Add(workflowInformations);

                    //查找目标节点的上节点与下节点
                    var nextNodes = bpmManager.GetFlowNextNodes(workflow.FlowTemplate.Nodes,
                        workflow.FlowTemplate.Steps, node);
                    var backNodes = bpmManager.GetFlowBackNodes(workflow.FlowTemplate.Nodes,
                        workflow.FlowTemplate.Steps, node);

                    switch (d.StepState)
                    {
                        //节点为通过状态
                        case WorkflowStepState.Approved:
                            foreach (var nextNode in nextNodes)
                            {
                                nextNodeInfo = await NextNodeInfos(nextNode, nextNodeInfo);
                                //if (nextNode.Active)
                                //{
                                //    nextNodeInfo.NodeLabel = nextNode.Label;
                                //    nextNodeInfo.IsBpmApprove = true;
                                //    nextNodeInfo.Time = DateTime.Now;
                                //    nextNodeInfo.NodeType = nextNode.Type;
                                //    //添加待审批成员
                                //    nextNode.Members?.ForEach(member =>
                                //    {
                                //        var memberInfos = new MemberInfoDto
                                //        {
                                //            Id = member.MemberId,
                                //            Type = member.Type
                                //        };
                                //        memberListInfos.MemberInfos.Add(memberInfos);
                                //    });
                                //}
                                ////添加节点
                                //if (nextNode.Type == "bpmEnd")
                                //{
                                //    nextNodeInfo.NodeLabel = nextNode.Label;
                                //    nextNodeInfo.IsBpmEnd = true;
                                //    nextNodeInfo.Time = DateTime.Now;
                                //    nextNodeInfo.NodeType = nextNode.Type;
                                //}
                            }
                            break;
                        //节点为退回状态
                        case WorkflowStepState.Rejected:
                            foreach (var backNode in backNodes)
                            {
                                nextNodeInfo = await NextNodeInfos(backNode, nextNodeInfo);
                                //nextNodeInfo.NodeLabel = backNode.Label;
                                //nextNodeInfo.IsBpmApprove = true;
                                //nextNodeInfo.Time = DateTime.Now;
                                //nextNodeInfo.NodeType = backNode.Type;

                                //backNode.Members?.ForEach(member =>
                                //{
                                //    var memberInfos = new MemberInfoDto
                                //    {
                                //        Id = member.MemberId,
                                //        Type = member.Type
                                //    };
                                //    memberListInfos.MemberInfos.Add(memberInfos);

                                //});
                            }
                            break;
                    }
                }
            }
            //nextNodeInfo.PendingUserInfos = await MemberManager.Search(memberListInfos);
            infoList.Add(nextNodeInfo);

            //获取当前节点处理用户的信息
            var userIds = infoList.Select(x => x.UserId).ToList();

            var users = _userRepository.Where(x => userIds.Contains(x.Id)).ToList();

            foreach (var info in infoList)
            {
                var target = users.Find(x => x.Id == info.UserId);
                if (target != null)
                {
                    info.UserName = target.Name;
                }
            }
            //获取下一节点待处理的人员信息

            return infoList.OrderBy(x => x.Time);
        }

        private async Task<WorkflowImformationDto> NextNodeInfos(FlowTemplateNode nextNode, WorkflowImformationDto nextNodeInfo)
        {
            var memberListInfos = new MemberSearchInputDto();
            var isLevelNode = true;
            if (nextNode.Active)
            {
                nextNodeInfo.NodeLabel = nextNode.Label;
                nextNodeInfo.IsBpmApprove = true;
                nextNodeInfo.Time = DateTime.Now;
                nextNodeInfo.NodeType = nextNode.Type;
                //添加待审批成员
                nextNode.Members?.ForEach(member =>
                {
                    if ((int)member.Type > 20)
                    {
                        var level = (int)member.Type - 20;
                        var list = new List<MemberDto>();

                        var pendingMember = new MemberDto
                        {
                            Name = "第 " + level + " 级",
                            Type = member.Type,
                            Id = member.MemberId
                        };

                        list.Add(pendingMember);

                        isLevelNode = false;
                        nextNodeInfo.PendingUserInfos = list;
                    }
                    var memberInfos = new MemberInfoDto
                    {
                        Id = member.MemberId,
                        Type = member.Type
                    };
                    memberListInfos.MemberInfos.Add(memberInfos);
                });
                if (isLevelNode)
                {
                    nextNodeInfo.PendingUserInfos = await MemberManager.Search(memberListInfos);

                }
            }
            //添加节点
            if (nextNode.Type == "bpmEnd")
            {
                nextNodeInfo.NodeLabel = nextNode.Label;
                nextNodeInfo.IsBpmEnd = true;
                nextNodeInfo.Time = DateTime.Now;
                nextNodeInfo.NodeType = nextNode.Type;
            }
            return nextNodeInfo;
        }

        /// <summary>
        /// 判断当前用户所在的节4点
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="ids"></param>
        /// <returns></returns>

        private bool CheckMembers(Workflow workflow, List<Guid> ids)
        {
            return workflow.FlowTemplate.Steps
                .Where(a => a.State == null || a.State == WorkflowStepState.Rejected)
                .Select(b => bpmManager.GetNodeById(workflow.FlowTemplate.Nodes, b.Target)
                ).Any(c => c != null
                           && (c.Type == FlowNodeTypes.BpmApprove || c.Type == FlowNodeTypes.BpmEnd)
                           && c.Members != null
                           && c.Members.Exists(d => ids.Contains(d.MemberId)));
        }

        /// <summary>
        /// 发起工作流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkflowDetailDto> Create(WorkflowCreateDto input)
        {
            var workflow = await bpmManager.CreateWorkflow(input.WorkflowTemplateId, input.FormValues, CurrentUser.Id.Value);

            return ObjectMapper.Map<Workflow, WorkflowDetailDto>(workflow);

        }


        /// <summary>
        /// 处理流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// 
        public async Task<WorkflowDetailDto> Process(WorkflowProcessDto input)
        {
            // 判断工作流是否存在
            var workflowDetail = await bpmManager.ProcessWorkflow(input.Id, input.FormValues, input.SourceNodeId, input.TargetNodeId, input.StepState, input.Comments, CurrentUser.Id.Value);
            return ObjectMapper.Map<Workflow, WorkflowDetailDto>(workflowDetail);
        }

        /// <summary>
        /// 判断当前节点是否有回退的权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CanReturnStep(WorkflowProcessDto input)
        {
            if (input.TargetNodeId == Guid.Empty || input.Id == Guid.Empty)
            {
                return true;
            }
            var workFlow = await workflowRepository.GetAsync(input.Id);
            if (workFlow == null)
            {
                return true;
            }
            // 根据目标id获取到当前正在进行流程的线
            var currentStep = bpmManager.GetStepsByTargetId(workFlow.FlowTemplate.Steps, input.TargetNodeId);
            if (!currentStep.Any())
            {
                return true;
            }

            // 根据当前流程获取到流程的起始节点
            var startNode =
                bpmManager.GetFlowStartNode(workFlow.FlowTemplate.Nodes, workFlow.FlowTemplate.Steps);
            if (startNode == null)
            {
                return true;
            }
            // 如果当前线的起点id是开始节点的id，就返回false
            return startNode.Id != currentStep[0].Source;
        }

        public async Task BpmSignalRTest()
        {
            var message = new BpmMessage();
            message.SendType = SendModeType.Default;
            var content = new BpmMessageContent();
            content.Title = "工作流666";
            message.SetContent(content);
            await _messageBpmProvider.PushAsync(message.GetBinary());
        }

        /// <summary>
        /// 统计全部数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetTotalSimpleDto> GetTotal(WorkflowSearchInputDto input)
        {
            var totalSimpleDto = new GetTotalSimpleDto();

            input.Group = UserWorkflowGroup.Approved;
            input.State = WorkflowState.All;
            input.IsAll = true;

            var list = await GetList(input);
            totalSimpleDto.ApprovedTotal = list.Items.Count();

            input.Group = UserWorkflowGroup.Initial;
            list = await GetList(input);
            totalSimpleDto.InitialTotal = list.Items.Count();

            input.Group = UserWorkflowGroup.Cc;
            list = await GetList(input);
            totalSimpleDto.CcTotal = list.Items.Count();

            input.Group = UserWorkflowGroup.Waiting;
            list = await GetList(input);
            totalSimpleDto.WaitingTotal = list.Items.Count();

            return await Task.FromResult(totalSimpleDto);
        }

        public Task<List<string>> GetWorkFlowNames()
        {
            var workflowNames = _workflowTemplates.Where(x => !x.IsDeleted).Select(x => x.Name.Replace("表单", "")).OrderBy(x => x.Length).ToList();

            return Task.FromResult(workflowNames);
        }

        //一键审批
        public async Task<bool> OneTouchArrove(string opinion, bool isPass, List<Guid> workflowIds)
        {
            if (workflowIds.Count == 0) throw new UserFriendlyException("请选择流程");
            foreach (var item in workflowIds)
            {
                var workflowTemplate = await Get(item);

                //var workflowProcessDto = ObjectMapper.Map<WorkflowDetailDto, WorkflowProcessDto>(workflowTemplate);

                var workflowProcessDto = new WorkflowProcessDto()
                {
                    FormValues = workflowTemplate.FormValue,
                    SourceNodeId = workflowTemplate.CurrentUserActivedStep.Source,
                    TargetNodeId = workflowTemplate.CurrentUserActivedStep.Target,
                    StepState = isPass ? WorkflowStepState.Approved : WorkflowStepState.Stopped,
                    Comments = opinion,
                    Id = item
                };


                var workflowDetailDto = await Process(workflowProcessDto);

                if (workflowDetailDto == null) continue;

            }

            return true;
        }
    }
}
