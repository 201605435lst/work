using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.OpenXmlFormats.Dml;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.Repositories;
using SnAbp.Identity;
using SnAbp.Message.Bpm;
using SnAbp.Message.MessageDefine;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Member = SnAbp.Message.Member;

namespace SnAbp.Bpm.Services
{
    public class BpmManager : DomainService
    {
        private readonly IWorkflowTemplateRepository _workflowTemplateRepository;
        private readonly IRepository<WorkflowData, Guid> _workflowDataRepository;
        private readonly IRepository<FormTemplate, Guid> _formTemplateRepository;
        private readonly IRepository<FlowTemplate, Guid> _flowTemplateRepository;
        private readonly IRepository<FlowTemplateNode, Guid> _flowTemplateNodeRepository;
        private readonly IRepository<FlowTemplateNodeRltMember, Guid> _flowTemplateNodeRltMemberRepository;
        private readonly IRepository<FlowTemplateStep, Guid> _flowTemplateStepRepository;
        private readonly IRepository<Workflow, Guid> _workflowRepository;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IUnitOfWorkManager _unitOfWorkManage;
        private readonly IRepository<WorkflowStateRltMember, Guid> _workflowStateRltMembers;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IMessageBpmProvider _messageBpmProvider;
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        private IdentityRoleManager _identityRoleManager;
        private OrganizationManager _organizationManager;

        public BpmManager(
            IWorkflowTemplateRepository workflowTemplateRepository,
            IRepository<WorkflowData, Guid> workflowDataRepository,
            IRepository<FormTemplate, Guid> formTemplateRepository,
            IRepository<FlowTemplate, Guid> flowTemplateRepository,
            IRepository<FlowTemplateNode, Guid> flowTemplateNodeRepository,
            IRepository<FlowTemplateNodeRltMember, Guid> flowTemplateNodeRltMemberRepository,
            IRepository<FlowTemplateStep, Guid> flowTemplateStepRepository,
            IRepository<Workflow, Guid> workflowRepository,
            IRepository<WorkflowStateRltMember, Guid> workflowStateRltMembers,
            IGuidGenerator guidGenerator,
            IdentityUserManager identityUserManager,
            IdentityRoleManager identityRoleManager,
            IUnitOfWorkManager unitOfWorkManage,
            IRepository<Organization> organizationRepository,
            ICancellationTokenProvider cancellationTokenProvider,
            IMessageBpmProvider messageBpmProvider,
            OrganizationManager organizationManager
        )
        {
            _workflowTemplateRepository = workflowTemplateRepository;
            _workflowDataRepository = workflowDataRepository;
            _formTemplateRepository = formTemplateRepository;
            _flowTemplateRepository = flowTemplateRepository;
            _flowTemplateNodeRepository = flowTemplateNodeRepository;
            _flowTemplateNodeRltMemberRepository = flowTemplateNodeRltMemberRepository;
            _flowTemplateStepRepository = flowTemplateStepRepository;
            _workflowRepository = workflowRepository;
            _workflowStateRltMembers = workflowStateRltMembers;
            _guidGenerator = guidGenerator;
            _identityUserManager = identityUserManager;
            _unitOfWorkManage = unitOfWorkManage;
            _messageBpmProvider = messageBpmProvider;
            CancellationTokenProvider = cancellationTokenProvider;
            _identityRoleManager = identityRoleManager;
            _organizationManager = organizationManager;
        }


        /// <summary>
        /// 创建工作流模板
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="isStatic">是否为静态，静态的工作流表单不能更改</param>
        /// <param name="key">唯一标识</param>
        /// <param name="formItems"></param>
        /// <param name="flowTemplateNodes"></param>
        /// <param name="flowTemplateSteps"></param>
        /// <returns></returns>
        public async Task<WorkflowTemplate> CreateWorkflowTemplate(
            Guid id,
            string name,
            bool isStatic = false,
            string key = null,
            string formItems = null,
            string formConfig = null,
            List<FlowTemplateNode> flowTemplateNodes = null,
            List<FlowTemplateStep> flowTemplateSteps = null,
            string webHookUrl = null
        )
        {
            // 创建工作流模板
            var template = new WorkflowTemplate(id)
            {
                Name = name,
                IsStatic = isStatic,
                Key = key,
                WebHookUrl = webHookUrl
            };
            FormTemplate formTemplate = null;
            FlowTemplate flowTemplate = null;

            // 创建表单模板
            if (formItems != null)
            {
                var formTemplateId = Guid.NewGuid();
                formTemplate = new FormTemplate(formTemplateId)
                {
                    WorkflowTemplateId = template.Id,
                    FormItems = formItems,
                    Config = formConfig ??
                             "{\"layout\":\"horizontal\",\"labelCol\":{\"span\":4},\"wrapperCol\":{\"span\":18},\"hideRequiredMark\":false,\"customStyle\":\"\"}",
                    Version = 1
                };


                flowTemplate = GetDefaultFlowtemplate();
                flowTemplate.FormTemplateId = formTemplateId;
                flowTemplate.Version = 1;
            }

            // 创建流程模板
            template.WorkflowTemplateGroupId = await _workflowTemplateRepository.GetGroupId();
            await _workflowTemplateRepository.InsertAsync(template);
            if (formTemplate != null && flowTemplate != null)
            {
                await _formTemplateRepository.InsertAsync(formTemplate);
                await _flowTemplateRepository.InsertAsync(flowTemplate);
            }

            return template;
        }

        /// <summary>
        /// 移除表单模板
        /// </summary>
        /// <param name="formTemplateId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveFormTemplate(Guid formTemplateId)
        {
            var flowTemplates = _flowTemplateRepository.Where(x => x.FormTemplateId == formTemplateId).ToList();
            // 删除流程模板
            foreach (var flowTemplate in flowTemplates)
            {
                await RemoveFlowTemplate(flowTemplate.Id);
            }

            // 删除表单模板
            await _formTemplateRepository.DeleteAsync(formTemplateId);

            return true;
        }

        public async Task<bool> RemoveFlowTemplate(Guid flowTemplateId)
        {
            // 删除节点成员
            await _flowTemplateNodeRltMemberRepository.DeleteAsync(x =>
                x.FlowTemplateNode.FlowTemplateId == flowTemplateId);
            // 删除节点
            await _flowTemplateNodeRepository.DeleteAsync(x => x.FlowTemplateId == flowTemplateId);
            // 删除流程
            await _flowTemplateStepRepository.DeleteAsync(x => x.FlowTemplateId == flowTemplateId);
            // 删除流程模板
            await _flowTemplateRepository.DeleteAsync(x => x.Id == flowTemplateId);
            return true;
        }


        /// <summary>
        ///  注册工作流
        /// </summary>
        /// <param name="key">唯一标识</param>
        /// <param name="json">配置文件</param>
        /// <returns></returns>
        public async Task<WorkflowTemplate> RegisterWorkflowTemplate(string key, string json)
        {
            using var uwo = _unitOfWorkManage.Begin(true, false);
            var jsonToken = JToken.Parse(json);

            // 获取配置文件当前版本
            var version = (int)jsonToken["version"];
            var name = jsonToken["name"].ToString();
            var webHookUrl = jsonToken["webHookUrl"].ToString();
            var formItems = jsonToken["formItems"].ToString();
            var formConfig = jsonToken["formConfig"].ToString();

            // 根据 key 查询
            var template = (await _workflowTemplateRepository.GetListAsync(true)).FirstOrDefault(x => x.Key == key);

            if (template != null)
            {
                //var formTemplate = template.FormTemplates.OrderByDescending(x => x.Version).FirstOrDefault();
                var formTemplate = (await _formTemplateRepository.GetListAsync())
                    .Where(x => x.WorkflowTemplateId == template.Id).OrderByDescending(x => x.Version).FirstOrDefault();
                if (formTemplate != null)
                {
                    // 没有新版本，不需要升级
                    if (formTemplate.Version < version)
                    {
                        //var flowTemplate = formTemplate.FlowTemplates.OrderByDescending(x => x.Version).FirstOrDefault();
                        var flowTemplate = (await _flowTemplateRepository.GetListAsync())
                            .Where(x => x.FormTemplateId == formTemplate.Id).OrderByDescending(x => x.Version)
                            .FirstOrDefault();
                        if (flowTemplate != null)
                        {
                            // 检查当前版本是否有数据
                            //var workflows = _workflowRepository.Where(x => x.FlowTemplateId == flowTemplate.Id).ToList();
                            var workflows = (await _workflowRepository.GetListAsync())
                                .Where(x => x.FlowTemplateId == flowTemplate.Id).ToList();

                            //if (workflows.Count == 0)
                            //{
                            //    await _formTemplateRepository.DeleteAsync(x => x.Id == flowTemplate.FormTemplateId);
                            //}

                            // 有数据增加表单，
                            // 创建新版本，流程必须重新建立
                            var _formTemplateId = Guid.NewGuid();
                            var _formTemplate = new FormTemplate(_formTemplateId)
                            {
                                WorkflowTemplateId = template.Id,
                                FormItems = formItems,
                                Config = formConfig,
                                Version = version
                            };

                            var _flowTemplate = GetDefaultFlowtemplate();
                            _flowTemplate.FormTemplateId = _formTemplateId;
                            _flowTemplate.Version = 1;

                            await _formTemplateRepository.InsertAsync(_formTemplate);
                        }
                    }
                }
            }
            else
            {
                // 模板不存在，创建新模板
                template = await CreateWorkflowTemplate(Guid.NewGuid(), name, true, key, formItems, formConfig, null,
                    null, webHookUrl);
            }

            await uwo.CompleteAsync();
            return template;
        }


        public async Task<Workflow> CreateWorkflow(Guid workflowTemplateId, string formValues, Guid userId)
        {
            var workflowTemplate = await GetWorkflowTemplate(workflowTemplateId);
            if (workflowTemplate == null)
            {
                throw new UserFriendlyException("工作流不存在");
            }

            var formTemplate = workflowTemplate.FormTemplates.FirstOrDefault();
            var flowTemplate = formTemplate?.FlowTemplates.FirstOrDefault();

            //添加审批/抄送节点 添加人员信息判断
            if (flowTemplate == null)
            {
                return null;
            }

            foreach (var node in flowTemplate.Nodes)
            {
                switch (node.Type)
                {
                    case "bpmCc" when node.Members.Count <= 0:
                        throw new UserFriendlyException("抄送节点未添加人员信息");
                    case "bpmApprove" when node.Members.Count <= 0:
                        throw new UserFriendlyException("审批节点未添加人员信息");
                    default:
                        continue;
                }
            }

            using var unoww = _unitOfWorkManage.Begin(true, false);
            // 创建工作流实例
            var workflow = new Workflow(Guid.NewGuid());
            workflow.FlowTemplateId = flowTemplate.Id;
            workflow.State = WorkflowState.Waiting;


            // 添加实例数据
            var workflowData = new WorkflowData(Guid.NewGuid());
            workflowData.WorkflowId = workflow.Id;
            workflowData.FormValues = formValues;

            await _workflowRepository.InsertAsync(workflow);
            await _workflowDataRepository.InsertAsync(workflowData);
            await unoww.SaveChangesAsync();

            //新增：添加我发起的成员关联表
            var initialMember = new WorkflowStateRltMember(_guidGenerator.Create())
            {
                WorkflowId = workflow.Id,
                MemberId = userId,
                MemberType = MemberType.User,
                Group = UserWorkflowGroup.Initial,
                State = WorkflowState.Waiting
            };

            //添加workflowData
            await _workflowStateRltMembers.InsertAsync(initialMember);
            await unoww.SaveChangesAsync();
            //新增：添加待我审批的成员关联表
            var startNode = GetFlowStartNode(flowTemplate.Nodes, flowTemplate.Steps);
            var nextNodes = GetFlowNextNodes(flowTemplate.Nodes, flowTemplate.Steps, startNode);
            foreach (var node in nextNodes)
            {
                await InsertMembers(userId, node, workflow, true, false, false);
            }
            return workflow;
        }

        /// <summary>
        ///     通过 Key 发起流程
        /// </summary>
        /// <param name="key"></param>
        /// <param name="formValues"></param>
        /// <returns></returns>
        public async Task<Workflow> CreateWorkflowByWorkflowTemplateKey(string key, string formValues, Guid userId)
        {
            var json = JToken.Parse(formValues);

            var template = _workflowTemplateRepository.FirstOrDefault(x => x.Key == key);
            if (template == null)
            {
                throw new UserFriendlyException("工作流不存在");
            }

            return await CreateWorkflow(template.Id, formValues, userId);
        }


        /// <summary>
        ///     获取默认流程模板
        /// </summary>
        /// <returns></returns>
        public FlowTemplate GetDefaultFlowtemplate()
        {
            var flowTemplate = new FlowTemplate(Guid.NewGuid());

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var id4 = Guid.NewGuid();

            var idStep1 = Guid.NewGuid();
            var idStep2 = Guid.NewGuid();
            var idStep3 = Guid.NewGuid();

            flowTemplate.Nodes = new List<FlowTemplateNode>();
            flowTemplate.Nodes.Add(new FlowTemplateNode(id1)
            {
                X = 300,
                Y = 50,
                Label = "开始节点",
                Type = "bpmStart",
                Active = false,
                FormItemPermisstions = new List<FlowNodeFormItemPermisstion>(),
                Members = new List<FlowTemplateNodeRltMember>()
            });
            flowTemplate.Nodes.Add(new FlowTemplateNode(id2)
            {
                X = 300,
                Y = 250,
                Label = "审批节点",
                Type = "bpmApprove",
                Active = false,
                FormItemPermisstions = new List<FlowNodeFormItemPermisstion>(),
                Members = new List<FlowTemplateNodeRltMember>()
            });
            flowTemplate.Nodes.Add(new FlowTemplateNode(id3)
            {
                X = 500,
                Y = 250,
                Label = "抄送节点",
                Type = "bpmCc",
                Active = false,
                FormItemPermisstions = new List<FlowNodeFormItemPermisstion>(),
                Members = new List<FlowTemplateNodeRltMember>()
            });
            flowTemplate.Nodes.Add(new FlowTemplateNode(id4)
            {
                X = 300,
                Y = 450,
                Label = "结束节点",
                Type = "bpmEnd",
                Active = false,
                FormItemPermisstions = new List<FlowNodeFormItemPermisstion>(),
                Members = new List<FlowTemplateNodeRltMember>()
            });


            flowTemplate.Steps = new List<FlowTemplateStep>();
            flowTemplate.Steps.Add(new FlowTemplateStep(idStep1)
            { Source = id1, Target = id2, SourceAnchor = 2, TargetAnchor = 4, Type = "flow" });
            flowTemplate.Steps.Add(new FlowTemplateStep(idStep2)
            { Source = id2, Target = id3, SourceAnchor = 1, TargetAnchor = 3, Type = "flow" });
            flowTemplate.Steps.Add(new FlowTemplateStep(idStep3)
            { Source = id2, Target = id4, SourceAnchor = 2, TargetAnchor = 4, Type = "flow" });

            return flowTemplate;
        }


        /// <summary>
        ///     获取工作流模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<WorkflowTemplate> GetWorkflowTemplate(Guid id)
        {
            var workflowTemplate = _workflowTemplateRepository
                .WithDetails()
                .Where(x => x.Id == id)
                .OrderByDescending(x => x.CreationTime)
                .FirstOrDefault();

            if (workflowTemplate == null)
            {
                return Task.FromResult((WorkflowTemplate)null);
            }

            workflowTemplate.FormTemplates =
                workflowTemplate.FormTemplates.OrderByDescending(form => form.Version).ToList();
            workflowTemplate.FormTemplates?.ForEach(a =>
            {
                a.FlowTemplates = a.FlowTemplates.OrderByDescending(x => x.Version).ToList();
            });

            return Task.FromResult(workflowTemplate);
        }


        /// <summary>
        ///     获取工作流模板表单模板
        /// </summary>
        /// <param name="workflowTemplateId"></param>
        /// <returns></returns>
        public async Task<FormTemplate> GetWorkflowTemplateFormTemplate(Guid workflowTemplateId)
        {
            var formTemplate = await GetWorkflowTemplate(workflowTemplateId);

            return formTemplate != null && formTemplate.FormTemplates.FirstOrDefault() != null
                ? formTemplate.FormTemplates.FirstOrDefault()
                : null;
        }


        /// <summary>
        ///     获取工作流模板流程模板
        /// </summary>
        /// <param name="workflowTemplateId"></param>
        /// <returns></returns>
        public async Task<FlowTemplate> GetWorkflowTemplateFlowTemplate(Guid workflowTemplateId)
        {
            var flowTemplate = await GetWorkflowTemplateFormTemplate(workflowTemplateId);

            return flowTemplate != null && flowTemplate.FlowTemplates.FirstOrDefault() != null
                ? flowTemplate.FlowTemplates.FirstOrDefault()
                : null;
        }


        /// <summary>
        ///     获取工作流实例包含简报信息
        /// </summary>
        /// <param name="workflow"></param>
        /// <returns></returns>
        public WorkflowSimple GetWorkflowSimple(Workflow workflow, Guid currentUserId)
        {
            var currentUserOrganizations = _identityUserManager.GetOrganizationsAsync(currentUserId).Result;
            var workflowDetail = new WorkflowSimple(workflow.Id)
            {
                FlowTemplateId = workflow?.FlowTemplateId,
                FlowTemplate = workflow?.FlowTemplate,
                WorkflowDatas = workflow?.WorkflowDatas,
                Name = workflow?.FlowTemplate.FormTemplate.WorkflowTemplate?.Name,
                CreationTime = workflow.CreationTime,
                LastModificationTime = workflow?.LastModificationTime,
                State = WorkflowState.Waiting
            };
            //
            workflowDetail.Infos = GetWorkflowInfos(workflow.FlowTemplate.Nodes,
                workflow.FlowTemplate.FormTemplate.FormItems, workflow.WorkflowDatas, currentUserId, currentUserOrganizations, workflow.CreatorId.GetValueOrDefault());

            return workflowDetail;
        }


        /// <summary>
        ///     获取工作流实例详细信息
        /// </summary>
        /// <param name="workflow"></param>
        /// <returns></returns>
        public async Task<WorkflowDetail> GetWorkflowDetail(Guid workflowId, Guid? currentUserId)
        {
            using var uow = _unitOfWorkManage.Begin(true);
            // 如果当前用户是发起人，则只能查看详情
            // 如果到当前用户审批环节，则处于审批模式
            // 如果为审批，则可编辑，否则只能查看
            var workflow = _workflowRepository
                .WithDetails()
                .FirstOrDefault(x => x.Id == workflowId);

            // 出现的问题，由于由于服务完成后框架自动提交修改后的事务，需要将查询的工作流在内存中复制一份，不能执行同一个指针
            var currentUserOrganizations = await _identityUserManager.GetOrganizationsAsync(currentUserId.GetValueOrDefault());
            var creatorOrganizations = await _identityUserManager.GetOrganizationsAsync(workflow.CreatorId.GetValueOrDefault());
            var workflowDetail = new WorkflowDetail(workflowId)
            {
                Name = workflow.FlowTemplate?.FormTemplate?.WorkflowTemplate?.Name,
                FormItems = workflow.FlowTemplate.FormTemplate.FormItems,
                FormConfig = workflow.FlowTemplate.FormTemplate.Config,
                FlowTemplate = workflow.FlowTemplate,
                FlowNodes = workflow.FlowTemplate.Nodes,
                FlowSteps = workflow.FlowTemplate.Steps,
                WorkflowDatas = workflow.WorkflowDatas,
                CreationTime = workflow.CreationTime,
                LastModificationTime = workflow.LastModificationTime,
                FormValue = JsonConvert.SerializeObject(
                    GetWorkflowFormValue(JToken.Parse(workflow.FlowTemplate.FormTemplate.FormItems),
                        workflow.WorkflowDatas)),
                Infos = GetWorkflowInfos(workflow.FlowTemplate.Nodes, workflow.FlowTemplate.FormTemplate.FormItems,
                    workflow.WorkflowDatas, currentUserId.GetValueOrDefault(), currentUserOrganizations, workflow.CreatorId.GetValueOrDefault())
            };

            workflowDetail.ActivedSteps = GetWorkflowActivedSteps(workflowDetail.FlowNodes, workflowDetail.FlowSteps,
                workflow.WorkflowDatas);
            workflowDetail.State = GetWorkflowState(workflowDetail.ActivedSteps, workflowDetail.WorkflowDatas);

            workflowDetail.IsStatic = (bool)(workflow.FlowTemplate.FormTemplate.WorkflowTemplate?.IsStatic);
            // 逻辑修改（easten)：已拒绝的数据全部为只读数据
            if (currentUserId.HasValue)
            {
                // 设置激活节点
                foreach (var node in workflowDetail.FlowNodes.Where(node =>
                    workflowDetail.ActivedSteps.Exists(x => x.Target == node.Id)))
                {
                    node.Active = true;
                }

                var memberIds = (await _identityUserManager.GetUserMembers(currentUserId.Value)).Select(x => x.Id).ToList();

                // 获取当前用户可以执行的流程 step
                workflowDetail.CurrentUserActivedStep = workflowDetail.ActivedSteps.FirstOrDefault(a =>
                    workflowDetail.FlowNodes.Any(a =>
                        a.Members != null && a.Members.Any(b => memberIds.Exists(c => c == b.MemberId) ||
                            CheckCurrentUserInCreatorOrg(currentUserOrganizations, creatorOrganizations, (int)b.Type)))
                    );
                if (workflowDetail.CurrentUserActivedStep != null)
                {
                    // 根据当前节点权限设置表单权限（编辑、查看）
                    workflowDetail.FormItems = JsonConvert.SerializeObject(MergeFormPermission(
                        JToken.Parse(workflowDetail.FormItems),
                        GetNodeById(workflow.FlowTemplate.Nodes, workflowDetail.CurrentUserActivedStep.Target)
                            .FormItemPermisstions));
                }
                else
                {
                    // 全部设置为只读
                    var formItems = JToken.Parse(workflowDetail.FormItems);
                    foreach (var item in formItems)
                    {
                        item["options"]["disabled"] = true;
                    }

                    workflowDetail.FormItems = JsonConvert.SerializeObject(formItems);
                }
            }

            return workflowDetail;
        }


        /// <summary>
        ///     合并表单权限
        /// </summary>
        /// <param name="formItems"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public JToken MergeFormPermission(JToken formItems, List<FlowNodeFormItemPermisstion> permissions)
        {
            foreach (var item in formItems)
            {
                var key = item["key"].ToString();
                foreach (var permission in permissions.Where(permission => permission.Key == key))
                {
                    item["options"]["disabled"] = !permission.Edit;
                }
            }

            return formItems;
        }


        /// <summary>
        ///     获取工作流状态
        /// </summary>
        /// <param name="activedSteps"></param>
        /// <param name="workflowDatas"></param>
        /// <returns></returns>
        public WorkflowState GetWorkflowState(List<FlowTemplateStep> activedSteps, List<WorkflowData> workflowDatas)
        {
            var state = WorkflowState.Waiting;
            // 新增逻辑处理，当前的流程被第一级拒绝时，工作流的状态修改为 已终止，流程结束
            // 当任何一级拒绝时，整个流程结束
            if (!activedSteps.Any())
            {
                state = workflowDatas.Exists(x => x.StepState == WorkflowStepState.Stopped)
                    ? WorkflowState.Stopped
                    : WorkflowState.Finished;
            }
            else
            {
                state = WorkflowState.Waiting;
            }
            return state;
        }


        /// <summary>
        ///     处理流程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formValues"></param>
        /// <param name="sourceNodeId"></param>
        /// <param name="targetNodeId"></param>
        /// <param name="stepState"></param>
        /// <param name="comments"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WorkflowDetail> ProcessWorkflow(Guid id, string formValues, Guid sourceNodeId,
            Guid targetNodeId, WorkflowStepState stepState, string comments, Guid userId)
        {
            // 判断工作流是否存在
            var workflowDetail = await GetWorkflowDetail(id, userId);
            if (workflowDetail == null)
            {
                throw new UserFriendlyException("工作流不存在");
            }
            var workflow = _workflowRepository.WithDetails().FirstOrDefault(x => x.Id == workflowDetail.Id);

            //清除关联表信息
            await _workflowStateRltMembers.DeleteAsync(x => x.WorkflowId == workflow.Id && x.Group == UserWorkflowGroup.Waiting);
            //获取当前处理节点信息
            foreach (var node in workflowDetail.FlowNodes.Where(node => node.Id == targetNodeId))
            {
                var nextNodes = GetFlowNextNodes(workflowDetail.FlowNodes, workflowDetail.FlowSteps, node);//查找指定节点的下一步节点集合
                var backNodes = GetFlowBackNodes(workflowDetail.FlowNodes, workflowDetail.FlowSteps, node);//查找指定节点的上一步节点集合
                switch (stepState)
                {
                    case WorkflowStepState.Approved:
                        await InsertMembers(userId, node, workflow, false, false, false);
                        foreach (var nextNode in nextNodes)
                        {
                            switch (nextNode.Type)
                            {
                                case "bpmCc":
                                    await InsertMembers(userId, nextNode, workflow, false, false, true);
                                    break;
                                case "bpmEnd":
                                    break;
                                default:
                                    await InsertMembers(userId, nextNode, workflow, true, false, false);
                                    break;
                            }
                        }
                        break;
                    case WorkflowStepState.Rejected:
                        await InsertMembers(userId, node, workflow, false, true, false);
                        foreach (var backNode in backNodes)
                        {
                            await InsertMembers(userId, backNode, workflow, true, false, false);
                        }
                        break;
                    case WorkflowStepState.Stopped:
                        await InsertMembers(userId, node, workflow, false, false, false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(stepState), stepState, null);
                }
            }
            var workflowData = new WorkflowData(Guid.NewGuid())
            {
                WorkflowId = workflowDetail.Id,
                FormValues = formValues ?? "",
                StepState = stepState,
                Comments = comments ?? "",
                SourceNodeId = sourceNodeId,
                TargetNodeId = targetNodeId,
                CreationTime = DateTime.Now
            };

            // 假设把最新数据加入，计算工作流下一步状态
            workflowDetail.WorkflowDatas.Add(workflowData);
            var activedSteps = GetWorkflowActivedSteps(workflowDetail.FlowNodes, workflowDetail.FlowSteps,
                workflowDetail.WorkflowDatas);
            var state = GetWorkflowState(activedSteps, workflowDetail.WorkflowDatas);
            workflow.State = state;
            if ((workflow.State == WorkflowState.Finished || workflow.State == WorkflowState.Stopped) &&
                workflowDetail.FlowTemplate.FormTemplate.WorkflowTemplate.WebHookUrl != null)
            {
                var webHookUrl = workflowDetail.FlowTemplate.FormTemplate.WorkflowTemplate.WebHookUrl;
                var appsettings = File.ReadAllText("appsettings.json");
                var appsettingsJson = JToken.Parse(appsettings);
                var selfUrl = appsettingsJson["App"]["SelfUrl"];
                webHookUrl = webHookUrl.Contains("Http") ? webHookUrl : selfUrl + webHookUrl;

                //此处为为http请求url 
                var uri = new Uri($"{webHookUrl}?id={workflow.Id}&state={workflow.State}");
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "Post";
                _ = request.GetResponseAsync();
            }

            await NoticeWorkFlowCreator(workflow, stepState, userId);
            await _workflowRepository.UpdateAsync(workflow);
            await _workflowDataRepository.InsertAsync(workflowData);
            return workflowDetail;
        }

        /// <summary>
        /// 通知流程的发起人
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="state"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private async Task NoticeWorkFlowCreator(Workflow workflow, WorkflowStepState state, Guid userid)
        {
            /* 说明：使用消息通知功能
             * 思路：1、判断当前的处理状态，任何状态都得通知一次流程得发起者
             *       2、如果通过，通知下面节点的人
             * 作者：easten
             * */
            var creatorMessage = GetCreatorMessageContent(workflow.CreatorId.GetValueOrDefault());
            var mstate = 0;
            if (state == WorkflowStepState.Rejected) mstate = 4;
            if (state == WorkflowStepState.Approved) mstate = 1;
            if (state == WorkflowStepState.Stopped) mstate = 3;

            var creatorContent = new BpmMessageContent()
            {

                State = mstate,
                WorkFlowId = workflow.Id,
                ProcessorId = userid,
                Type = BpmMessageType.Notice,
                SponsorId = workflow.CreatorId.GetValueOrDefault()
            };
            creatorMessage.SetContent(creatorContent);
            if (workflow.FlowTemplate.FormTemplate.WorkflowTemplate.Type == WorkflowTemplateType.Default)
            {
                await _messageBpmProvider.PushAsync(creatorMessage.GetBinary());
            }
        }

        private BpmMessage GetCreatorMessageContent(Guid id)
        {
            var message = new BpmMessage();
            message.SendType = SendModeType.User;
            message.SetUserId(id);
            return message;
        }


        ///  <summary>
        /// 添加关联表信息
        ///  </summary>
        ///  <param name="node"></param>
        ///  <param name="workflow"></param>
        ///  <param name="isPending"></param>
        ///  <param name="isRejected"></param>
        ///  <param name="isCc"></param>
        ///  <returns></returns>
        private async Task InsertMembers(Guid userId, FlowTemplateNode node, Workflow workflow, bool isPending, bool isRejected, bool isCc)
        {
            if (node.Members.Count <= 0 || node.Members == null)
            {
                if (node.Type == "bpmApprove")
                {
                    throw new UserFriendlyException("审批节点未添加审批人");
                }
                return;
            }
            //遍历关联表已经存在数据
            var workflowRltInfos = _workflowStateRltMembers.Where(x => x.WorkflowId == workflow.Id).ToList();

            using var unoww = _unitOfWorkManage.Begin(true, false);
            var ccMemberList = new List<Member>();
            var proMemberList = new List<Member>();
            foreach (var member in node.Members)
            {
                var memberInfo = new WorkflowStateRltMember(_guidGenerator.Create());
                memberInfo.WorkflowId = workflow.Id;
                memberInfo.MemberId = member.MemberId;
                memberInfo.MemberType = member.Type;
                if (isCc)
                {
                    //判断是否存在重复抄送数据
                    if (workflowRltInfos.Any(workflowRltInfo =>
                        workflowRltInfo.MemberId == member.MemberId && workflowRltInfo.Group == UserWorkflowGroup.Cc))
                    {
                        continue;
                    }
                    memberInfo.Group = UserWorkflowGroup.Cc;
                    memberInfo.State = WorkflowState.Waiting;
                    await _workflowStateRltMembers.InsertAsync(memberInfo);
                    await unoww.SaveChangesAsync();
                    // 创建给抄送者的消息通知
                    ccMemberList.Add(new Member() { Id = member.MemberId, Type = (Message.MemberType)member.Type });
                    await CreateCcMessage(ccMemberList, workflow, WorkflowState.All, userId);
                    //return;
                }
                if (isPending)//待审批状态
                {
                    memberInfo.Group = UserWorkflowGroup.Waiting;
                    memberInfo.State = WorkflowState.Waiting;
                    //if (isRejected)
                    //{
                    //    memberInfo.State = WorkflowState.Rejected;
                    //}
                    proMemberList.Add(new Member() { Id = member.MemberId, Type = (Message.MemberType)member.Type });
                }
                else
                {
                    if (isRejected)//退回状态
                    {
                        memberInfo.Group = UserWorkflowGroup.Approved;
                        memberInfo.State = WorkflowState.Rejected;
                    }
                    else
                    {
                        //判断是否存在我已审批重复数据
                        if (workflowRltInfos.Any(workflowRltInfo => member.MemberId == workflowRltInfo.MemberId && workflowRltInfo.Group == UserWorkflowGroup.Approved))
                        {
                            continue;
                        }
                        //添加当前处理人的信息
                        memberInfo.WorkflowId = workflow.Id;
                        memberInfo.MemberId = userId;
                        memberInfo.MemberType = MemberType.User;
                        memberInfo.Group = UserWorkflowGroup.Approved;
                        memberInfo.State = WorkflowState.Waiting;
                    }
                }
                if (!isCc)
                {
                    await _workflowStateRltMembers.InsertAsync(memberInfo);
                    await unoww.SaveChangesAsync();
                }

            }
            await CreateApprovalMessage(proMemberList, workflow, WorkflowState.All, userId);

        }
        /// <summary>
        /// 创建一条发送给抄送者的消息
        /// </summary>
        /// <param name="members"></param>
        /// <param name="workflow"></param>
        /// <param name="state"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task CreateCcMessage(List<Member> members, Workflow workflow, WorkflowState state, Guid userId)
        {
            if (members.Any())
            {
                var message = CreateDefaultMessage(members);
                var content = new BpmMessageContent
                {
                    State = (int)state,
                    Type = BpmMessageType.Cc,
                    SponsorId = workflow.CreatorId.GetValueOrDefault(),
                    WorkFlowId = workflow.Id,
                    ProcessorId = userId
                };
                message.SetContent(content);
                if (workflow.FlowTemplate.FormTemplate.WorkflowTemplate.Type == WorkflowTemplateType.Default)
                {
                    await _messageBpmProvider.PushAsync(message.GetBinary());
                }
            }
        }
        /// <summary>
        /// 创建一条发送给审批人的消息
        /// </summary>
        /// <param name="members"></param>
        /// <param name="workflow"></param>
        /// <param name="state"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task CreateApprovalMessage(List<Member> members, Workflow workflow, WorkflowState state, Guid userId)
        {
            var message = CreateDefaultMessage(members);
            var content = new BpmMessageContent
            {
                State = (int)state,
                Type = BpmMessageType.Approval,
                SponsorId = workflow.CreatorId.GetValueOrDefault(),
                WorkFlowId = workflow.Id,
                ProcessorId = userId
            };
            message.SetContent(content);
            if (workflow.FlowTemplate?.FormTemplate?.WorkflowTemplate.Type == WorkflowTemplateType.Default)
            {
                await _messageBpmProvider.PushAsync(message.GetBinary());
            }
        }

        private BpmMessage CreateDefaultMessage(List<Member> members)
        {
            var message = new BpmMessage { SendType = SendModeType.Member };
            message.SetMembers(members);
            return message;
        }


        /// <summary>
        ///     获取当前激活的流程
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<FlowTemplateStep> GetWorkflowActivedSteps(List<FlowTemplateNode> nodes,
            List<FlowTemplateStep> steps, List<WorkflowData> workflowData)
        {
            // 当前激活 steps
            var activedSteps = new List<FlowTemplateStep>();
            // 根据最新的流程数据判断获取激活的流程
            // 情况1：流程通过  2、流程退回  3、流程拒绝  4、流程终止

            var datalist = workflowData
                .OrderBy(a => a.CreationTime)
                .ToList();

            var startNode = GetFlowStartNode(nodes, steps);
            if (datalist.Count == 1)
            {
                // 起始状态
                var initSteps = steps.Where(a => a.Source == startNode.Id);
                activedSteps.AddRange(initSteps);
                return activedSteps;
            }
            else
            {
                datalist = datalist
                    .Where(a => a.TargetNodeId != null && a.SourceNodeId != null)
                    .OrderBy(a => a.CreationTime)
                    .ToList();
            }


            // 获取最后一条处理数据
            var lastData = datalist.LastOrDefault();
            if (lastData == null) return activedSteps;
            var endNode = GetFlowEndNode(nodes, steps);
            switch (lastData.StepState)
            {
                case WorkflowStepState.Approved: //最后一条是通过
                    // 1、流程是否到最后一步
                    if (lastData.TargetNodeId == endNode.Id)
                    {
                        return activedSteps;
                    }
                    else
                    {
                        // 流程不是最后一步，进行计算，拿到目标节点，再查找目标节点起始的流程，同时排除流程节点目标为抄送节点的。
                        var targetNode = nodes.FirstOrDefault(a => a.Id == lastData.TargetNodeId);
                        // && nodes.Select(b => b.Id).All(c => c != a.Target)
                        var targetSteps = steps.Where(a => a.Source == targetNode.Id && nodes.Where(b => b.Type == "bpmCc" || b.Type == "bpmEnd").All(b => b.Id != a.Target));
                        activedSteps.AddRange(targetSteps);
                    }
                    break;
                case WorkflowStepState.Rejected: // 退回
                    // 找到源节点，再查找源节点为目标节点的流程
                    var sourceNode = nodes.FirstOrDefault(a => a.Id == lastData.SourceNodeId);
                    var sourceSteps = steps.Where(a => a.Target == sourceNode.Id);
                    activedSteps.AddRange(sourceSteps);
                    break;
                case WorkflowStepState.Stopped:
                case null:
                    break;
                default:
                    return null;
            }
            return activedSteps;
        }


        /// <summary>
        ///     合并流程节点数据到流程模板
        /// </summary>
        /// GetWorkflowDetail
        /// <param name="templateSteps"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<FlowTemplateStep> MergeWorkflowStepsData(List<FlowTemplateStep> templateSteps,
            List<WorkflowData> data)
        {
            return (from step in templateSteps
                    let lastData = data.Where(x => x.SourceNodeId == step.Source && x.TargetNodeId == step.Target)
                        .OrderByDescending(x => x.CreationTime)
                        .FirstOrDefault()
                    select new FlowTemplateStep(step.Id)
                    {
                        Type = step.Type,
                        Source = step.Source,
                        Target = step.Target,
                        SourceAnchor = step.SourceAnchor,
                        TargetAnchor = step.TargetAnchor,
                        Active = step.Active,
                        Comments = lastData?.Comments,
                        State = lastData?.StepState
                    }).ToList();
        }


        /// <summary>
        ///     根据全部流程数据获取工作流表单值
        /// </summary>
        /// <param name="formItems"></param>
        public JObject GetWorkflowFormValue(JToken formItems, List<WorkflowData> workflowDatas)
        {
            var _workflowDatas = workflowDatas.OrderBy(x => x.CreationTime).AsEnumerable().ToList();
            var value = new JObject();

            foreach (var formitem in formItems)
            {
                var key = formitem["key"].ToString();

                if (!string.IsNullOrEmpty(_workflowDatas[0].FormValues))
                {
                    foreach (var formValues in from workflowData in _workflowDatas
                                               where workflowData.FormValues != null
                                               select JToken.Parse(workflowData.FormValues))
                    {
                        foreach (var jToken in formValues)
                        {
                            var item = (JProperty)jToken;
                            if (item.Name != key)
                            {
                                continue;
                            }

                            value[key] = item.Value;
                            break;
                        }
                    }
                }

            }

            return value;
        }


        /// <summary>
        ///     获取流程简报列表,新增了简报权限信息
        /// </summary>
        /// <param name="nodes">工作流节点</param>
        /// <param name="formItems">表单项</param>
        /// <param name="workflowDatas">表单值</param>
        /// <returns></returns>
        public List<WorkflowInfo> GetWorkflowInfos(List<FlowTemplateNode> nodes, string formItems,
            List<WorkflowData> workflowDatas, Guid currentUserId, List<Organization> currentUserOrganizations, Guid creatorId)
        {
            // 逻辑修改：easten
            // 获取流程简报需要考虑节点配置的权限信息
            // 不同的节点对应的权限可能不同

            var memberIds = _identityUserManager
                .GetUserMembers(currentUserId)
                .Result
                .Select(x => x.Id)
                .AsEnumerable();
            // 先获取当前用户所在的节点
            FlowTemplateNode currentNode = null;
            if (memberIds.Any())
            {
                currentNode = nodes
                    .FirstOrDefault(a => a.Members.Exists(b => memberIds.Contains(b.MemberId)) || a.Members.Any(b =>
                          CheckCurrentUserInCreatorOrg(currentUserOrganizations, GetCreatorOrganizations(creatorId), (int)b.Type)));
            }

            var infos = new List<WorkflowInfo>();
            var formItemsJToken = JToken.Parse(formItems);

            foreach (var formitem in formItemsJToken)
            {
                var info = new WorkflowInfo { Label = formitem["label"].ToString(), Key = formitem["key"].ToString(), Type = formitem["type"].ToString() };

                // 动态表格跳过，数据量太大
                if (info.Key.Contains("batch"))
                {
                    info.Info = "动态表格";
                }
                else
                {
                    foreach (var jToken in formitem["options"])
                    {
                        var option = (JProperty)jToken;
                        if (option.Name != "defaultValue")
                        {
                            continue;
                        }

                        info.Info = option.Value.ToString();
                        break;
                    }

                    if (!string.IsNullOrEmpty(workflowDatas[0].FormValues))
                    {
                        foreach (var formValues in from data in workflowDatas
                                                   where data.FormValues != null
                                                   select JToken.Parse(data.FormValues))
                        {
                            foreach (var jToken in formValues)
                            {
                                var item = (JProperty)jToken;
                                if (item.Name != info.Key)
                                {
                                    continue;
                                }

                                info.Info = item.Value.ToString();
                                break;
                            }
                        }
                    }

                }

                if (currentNode != null)
                {
                    var formItemPermission =
                        currentNode.FormItemPermisstions.FirstOrDefault(a => a.Key == formitem["key"].ToString());
                    if (formItemPermission.Info)
                    {
                        infos.Add(info);
                    }
                }
                else
                {
                    infos.Add(info);
                }
            }

            return infos;
        }


        /// <summary>
        ///     获取正在执行程的 Step
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<FlowTemplateStep> GetActivedWorkflowSteps(List<FlowTemplateStep> steps,
            List<FlowTemplateNode> nodes)
        {
            var activedSteps = steps.Where(x => !nodes.Any(node =>
                node.Id == x.Target && (node.Type == FlowNodeTypes.BpmCc || node.Type == FlowNodeTypes.BpmEnd)));
            foreach (var step in activedSteps)
            {
                step.Active = true;
            }

            return activedSteps.ToList();
        }


        /// <summary>
        ///     获取流程的开始节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public FlowTemplateNode GetFlowStartNode(List<FlowTemplateNode> nodes, List<FlowTemplateStep> steps)
        {
            var sourceIds = steps.Select(x => x.Source);
            var targetIds = steps.Select(x => x.Target);

            FlowTemplateNode target = null;

            foreach (var item in nodes)
            {
                if (sourceIds.Contains(item.Id) && !targetIds.Contains(item.Id))
                {
                    target = item;
                }
            }

            return target;
        }


        /// <summary>
        ///     获取流程的指定节点下一步节点集合
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="steps"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public List<FlowTemplateNode> GetFlowNextNodes(List<FlowTemplateNode> nodes, List<FlowTemplateStep> steps,
            FlowTemplateNode target)
        {
            var sourceIds = steps.Select(x => x.Source);
            var targetIds = steps.Select(x => x.Target);

            var nextNodeIds = new List<Guid>();

            foreach (var item in steps)
            {
                if (item.Source == target.Id)
                {
                    nextNodeIds.Add(item.Target);
                }
            }

            return nodes.Where(x => nextNodeIds.Contains(x.Id)).ToList();
        }
        /// <summary>
        /// 获取上一步节点集合
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="steps"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public List<FlowTemplateNode> GetFlowBackNodes(List<FlowTemplateNode> nodes, List<FlowTemplateStep> steps,
            FlowTemplateNode target)
        {
            var sourceIds = steps.Select(x => x.Source);
            var targetIds = steps.Select(x => x.Target);

            var backNodeIds = new List<Guid>();

            foreach (var item in steps)
            {
                if (item.Target == target.Id)
                {
                    backNodeIds.Add(item.Source);
                }
            }

            return nodes.Where(x => backNodeIds.Contains(x.Id)).ToList();
        }

        /// <summary>
        ///     获取流程的结束节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public FlowTemplateNode GetFlowEndNode(List<FlowTemplateNode> nodes, List<FlowTemplateStep> steps)
        {
            var sourceIds = steps.Select(x => x.Source);
            var targetIds = steps.Select(x => x.Target);

            FlowTemplateNode target = null;

            foreach (var item in nodes)
            {
                if (targetIds.Contains(item.Id) && !sourceIds.Contains(item.Id))
                {
                    target = item;
                }
            }

            return target;
        }


        /// <summary>
        ///     通过 Id 获取节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public FlowTemplateNode GetNodeById(List<FlowTemplateNode> nodes, Guid id)
        {
            return nodes.Find(x => x.Id == id);
        }


        /// <summary>
        ///     通过 Ids 获取节点集合
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<FlowTemplateNode> GetNodesByIds(List<FlowTemplateNode> nodes, List<Guid> ids)
        {
            return nodes.Where(x => ids.Contains(x.Id)).ToList();
        }


        /// <summary>
        ///     通过 目标节点 Id 获取流程步骤
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public List<FlowTemplateStep> GetStepsByTargetId(List<FlowTemplateStep> steps, Guid targetId)
        {
            return steps.Where(x => x.Target == targetId).ToList();
        }


        /// <summary>
        ///     通过 起始节点 Id 获取流程步骤
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public List<FlowTemplateStep> GetStepsBySourceId(List<FlowTemplateStep> steps, Guid sourceId)
        {
            return steps.Where(x => x.Source == sourceId).ToList();
        }

        public bool CheckCurrentUserInCreatorOrg(List<Organization> currentUserOrganizations, List<Organization> creatorOrganizations, int type)
        {
            // 判断 当前发起者组织结构的 等级问题。
            var tag = false;
            var level = type - 20;
            foreach (var organization in creatorOrganizations)
            {
                var codeArr = organization.Code.Split('.');
                if (codeArr.Length > level)
                {
                    var code = codeArr.Take(codeArr.Length - level).JoinAsString(".");
                    if (currentUserOrganizations.Any(a => a.Code == code))
                    {
                        return true;
                    }
                }
            }
            return tag;
        }

        private static void GetCreatorOrganizations(OrganizationHandler handler, string code)
        {
            // 根据当前组织机构获取他的上层级数的组织id;
            var arr = code.Split('.');
            for (var i = 1; i <= arr.Length; i++)
            {
                handler.Add(arr.Take(i).JoinAsString("."));
            }
        }

        public List<Organization> GetCreatorOrganizations(Guid userid) => _identityUserManager.GetOrganizationsAsync(userid).Result;

        /// <summary>
        /// 高铁工作票优化 工作流节点添加code:GTAQK:安全科审核  GTJSK:技术科审核 GTZGDZ:主管段长审核
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        //返回指定节点人员信息
        public async Task<WorkflowGetProcessMember> ProcessMember(Guid workflowId)
        {
            var processMember = new WorkflowGetProcessMember();

            var workflowState = _workflowRepository.FirstOrDefault(x => x.Id == workflowId).State;
            if (workflowState != WorkflowState.Finished)
            {
                return await Task.FromResult(processMember);
            }

            //获取已经处理过的人员信息
            var processedMemberIds = _workflowStateRltMembers.Where(x => x.WorkflowId == workflowId && x.Group == UserWorkflowGroup.Approved).Select(x => x.MemberId).ToList();

            //获取节点信息
            var nodes = _workflowRepository.Where(x => x.Id == workflowId).Select(y => y.FlowTemplate.Nodes).FirstOrDefault();

            foreach (var node in nodes)
            {
                if (node.Code != "GTJSK" && node.Code != "GTAQK" && node.Code != "GTZGDZ")
                {
                    continue;
                }
                Guid memberId;
                var nodeRltMembers = _flowTemplateNodeRltMemberRepository.Where(x => x.FlowTemplateNodeId == node.Id).ToList();
                foreach (var nodeRltMember in nodeRltMembers)
                {
                    //判断用户类型
                    if (nodeRltMember.Type == MemberType.User && processedMemberIds.Contains(nodeRltMember.MemberId))
                    {
                        memberId = nodeRltMember.MemberId;
                    }
                    else if (nodeRltMember.Type == MemberType.Role)
                    {
                        var role = await _identityRoleManager.GetByIdAsync(nodeRltMember.MemberId);
                        var users = await _identityUserManager.GetUsersInRoleAsync(role?.Name);
                        var member = users.FirstOrDefault(x => processedMemberIds.Contains(x.Id));
                        if (member == null) continue;
                        memberId = member.Id;
                    }
                    else if (nodeRltMember.Type == MemberType.Organization)
                    {
                        var organization = await _organizationManager.GetAsync(nodeRltMember.MemberId);
                        var users = await _identityUserManager.GetUsersInOrganizationAsync(organization);
                        var member = users.FirstOrDefault(x => processedMemberIds.Contains(x.Id));
                        if (member == null) continue;
                        memberId = member.Id;
                    }
                }
                switch (node.Code)
                {
                    case "GTJSK":
                        processMember.TechnicalMemberId = memberId;
                        break;
                    case "GTAQK":
                        processMember.SafeMemberId = memberId;
                        break;
                    case "GTZGDZ":
                        processMember.ChiefMemberId = memberId;
                        break;
                    default:
                        break;
                }
                memberId = Guid.Empty;
            }

            return await Task.FromResult(processMember);
        }

        public DateTime? ProcessTime(Guid workflowId, Guid userId)
        {
            return _workflowStateRltMembers
                .Where(x => x.WorkflowId == workflowId && x.MemberId == userId && x.Group == UserWorkflowGroup.Approved)
                .FirstOrDefault()?
                .CreationTime;
        }


        private WorkflowGetProcessMember GetProcessMember(FlowTemplateNode node, FlowTemplateNodeRltMember nodeRltMember)
        {
            var processMember = new WorkflowGetProcessMember();
            switch (node.Code)
            {
                case "GTJSK":
                    processMember.TechnicalMemberId = nodeRltMember.MemberId;
                    break;
                case "GTAQK":
                    processMember.SafeMemberId = nodeRltMember.MemberId;
                    break;
                case "GTZGDZ":
                    processMember.ChiefMemberId = nodeRltMember.MemberId;
                    break;
                default:
                    break;
            }

            return processMember;
        }

        class OrganizationHandler
        {
            /// <summary>
            /// 查询的组织结构的编码集合
            /// </summary>
            private List<string> Codes { get; set; }

            public OrganizationHandler() => Codes = new List<string>();

            public void Add(string code)
            {
                Codes.Add(code);
            }
            /// <summary>
            /// 最高级别
            /// </summary>
            /// <returns></returns>
            private int Level()
            {
                return Codes.OrderByDescending(a => a.Length).FirstOrDefault()?.Split('.').Length ?? 0;
            }
            public List<string> GetCodeByLevel(int level)
            {
                return Codes.Any() ? Codes.Where(a => a.Split('.').Length == level).ToList() : null;
            }
        }

        public class WorkflowGetProcessMember
        {
            /// <summary>
            /// 安全科审核
            /// </summary>
            public Guid SafeMemberId { get; set; }

            /// <summary>
            /// 技术科审核
            /// </summary>
            public Guid TechnicalMemberId { get; set; }

            /// <summary>
            /// 主管段长
            /// </summary>
            public Guid ChiefMemberId { get; set; }
        }
    }
}