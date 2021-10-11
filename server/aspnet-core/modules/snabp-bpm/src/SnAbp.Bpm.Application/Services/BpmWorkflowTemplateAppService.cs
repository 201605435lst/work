using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SnAbp.Bpm.Authorization;
using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Dtos.Test;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.IServices;
using SnAbp.Bpm.Repositories;
using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Bpm.Services
{
    [Authorize]
    public class BpmWorkflowTemplateAppService : BpmAppService, IBpmWorkflowTemplateAppService
    {
        private readonly WorkflowTemplateRepository _workflowTemplateRepository;
        private readonly IRepository<FormTemplate, Guid> _formTemplateRepository;
        private readonly IRepository<FlowTemplate, Guid> _flowTemplateRepository;
        private IRepository<WorkflowTemplateGroup, Guid> _workflowTemplateGroups;
        private IRepository<FlowTemplateNodeRltMember, Guid> _flowTemplateNodeRltMemberRepository;
        private IRepository<FlowTemplateStep, Guid> _flowTemplateStepRepository;
        private readonly IRepository<Workflow, Guid> _workflowRepository;
        private readonly BpmManager _bpmManager;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IRepository<WorkflowTemplateRltMember, Guid> _workflowTemplateRltMemberRepository;
        readonly IDataFilter _dataFilter;
        public BpmWorkflowTemplateAppService(
            IRepository<FormTemplate, Guid> formTemplateRepository,
            IRepository<FlowTemplate, Guid> flowTemplateRepository,
            IRepository<WorkflowTemplateGroup, Guid> workflowTemplateGroups,
            IRepository<FlowTemplateNodeRltMember, Guid> flowTemplateNodeRltMemberRepository,
            IRepository<FlowTemplateStep, Guid> flowTemplateStepRepository,
            IRepository<Workflow, Guid> workflowRepository,
            IRepository<WorkflowTemplateRltMember, Guid> workflowTemplateRltMemberRepository,
            BpmManager bpmManager,
            IdentityUserManager identityUserManager,
            WorkflowTemplateRepository workflowTemplateRepository,
            IDataFilter dataFilter
        )
        {
            _workflowTemplateRepository = workflowTemplateRepository;
            _formTemplateRepository = formTemplateRepository;
            _flowTemplateRepository = flowTemplateRepository;
            _workflowTemplateGroups = workflowTemplateGroups;
            _flowTemplateNodeRltMemberRepository = flowTemplateNodeRltMemberRepository;
            _flowTemplateStepRepository = flowTemplateStepRepository;
            _workflowRepository = workflowRepository;
            _bpmManager = bpmManager;
            _identityUserManager = identityUserManager;
            _workflowTemplateRltMemberRepository = workflowTemplateRltMemberRepository;
            _dataFilter = dataFilter;
        }


        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WorkflowTemplateDetailDto> Get([Required] Guid id)
        {
            var workflowTemplate = await _bpmManager.GetWorkflowTemplate(id);
            if (workflowTemplate != null)
            {
                var workflowTemplateDetailDto =
                    ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDetailDto>(workflowTemplate);
                workflowTemplateDetailDto.FormTemplate =
                    ObjectMapper.Map<FormTemplate, FormTemplateDetailDto>(
                        workflowTemplate.FormTemplates.FirstOrDefault());
                if (workflowTemplateDetailDto.FormTemplate != null)
                {
                    workflowTemplateDetailDto.FormTemplate.FlowTemplate =
                        ObjectMapper.Map<FlowTemplate, FlowTemplateDetailDto>(workflowTemplate.FormTemplates
                            .FirstOrDefault()?.FlowTemplates.FirstOrDefault());
                }

                return workflowTemplateDetailDto;
            }

            return null;
        }


        /// <summary>
        ///     获取首次发起表单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WorkflowDetailDto> GetWorkflowForInitial([Required] Guid id)
        {
            var workfowTemplate = await _bpmManager.GetWorkflowTemplate(id);

            if (workfowTemplate == null)
            {
                throw new UserFriendlyException("该工作流不存在");
            }

            if (
                workfowTemplate.FormTemplates.FirstOrDefault() == null ||
                workfowTemplate.FormTemplates.FirstOrDefault()?.FlowTemplates.FirstOrDefault() == null
            )
            {
                throw new UserFriendlyException("该工作流未设计");
            }

            // 判断当前用户是否有权限、
            var memberIds = (await _identityUserManager.GetUserMembers(CurrentUser.Id.GetValueOrDefault())).Select(x => x.Id)
                .ToList();
            if (!workfowTemplate.Members.Where(x => memberIds.Contains(x.MemberId)).Any())
            {
                throw new UserFriendlyException("无该工作流发起权限");
            }

            // 查询当前工作流模板的表单
            var formTemplate = workfowTemplate.FormTemplates.FirstOrDefault();
            var flowTemplate = formTemplate.FlowTemplates.FirstOrDefault();

            // 查询该表单流程其实节点的字段权限，并设置表单权限
            var flowStartNode = flowTemplate.Nodes.Find(n => n.Type == FlowNodeTypes.BpmStart);
            // TODO: 检验流程数据是否合法
            var formItems = JToken.Parse(formTemplate.FormItems);
            foreach (var item in formItems)
            {
                // 逻辑修改，需要处理是否为卡片布局和列表布局 2020-09-27
                if (item["type"].ToString() == "input")
                {
                    var key = item["key"].ToString();
                    var permission = flowStartNode.FormItemPermisstions.Find(x => x.Key == key);
                    item["options"]["disabled"] = !permission?.Edit;
                    item["options"]["visible"] = permission?.View;
                }
                else if (item["type"].ToString() == "card")
                {
                    foreach (var lis in item["list"])
                    {
                        var key = lis["key"].ToString();
                        var permission = flowStartNode.FormItemPermisstions.Find(x => x.Key == key);
                        lis["options"]["disabled"] = !permission?.Edit;
                        lis["options"]["visible"] = permission?.View;
                    }
                }
            }
            var workflowDetail = new WorkflowDetail
            {
                Name = workfowTemplate.Name,
                FormItems = JsonConvert.SerializeObject(formItems),
                FormConfig = formTemplate.Config,
                FlowNodes = flowTemplate.Nodes,
                FlowSteps = flowTemplate.Steps
            };

            return ObjectMapper.Map<WorkflowDetail, WorkflowDetailDto>(workflowDetail);
        }


        /// <summary>
        ///     查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<WorkflowTemplateDto>> GetList(WorkflowTemplateSearchInputDto input)
        {
            var query = _workflowTemplateRepository
                .WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name))
                .WhereIf(input.GroupId.HasValue, a => a.WorkflowTemplateGroupId == input.GroupId);

            // 当前用户成员组可发起的
            if (input.ForCurrentUser)
            {
                var memberIds = (await _identityUserManager.GetUserMembers(CurrentUser.Id.GetValueOrDefault()))
                    .Select(x => x.Id).ToList();
                if (input.Select)
                {
                    // 选择模式
                    query = query.Where(a => a.Type == WorkflowTemplateType.Single);
                }
                query = query
                    .Where(x => x.Published)
                    .Where(x => x.Members != null && x.Members.Any(m => memberIds.Contains(m.MemberId)))
                    .Where(a=>a.Type== WorkflowTemplateType.Default)
                    .Where(a => !a.IsStatic).OrderByDescending(x => x.CreationTime);
            }
            // 所有的
            else
            {
                query = query.WhereIf(input.Published.HasValue, x => x.Published == input.Published);
            }
            var result = new PagedResultDto<WorkflowTemplateDto>();
            result.TotalCount = query.Count();
            result.Items = input.IsAll ?
                ObjectMapper.Map<List<WorkflowTemplate>, List<WorkflowTemplateDto>>(query.OrderByDescending(s => s.CreationTime).ToList()) :
                ObjectMapper.Map<List<WorkflowTemplate>, List<WorkflowTemplateDto>>(query.OrderByDescending(s => s.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());

            foreach (var workflow in result.Items)
            {
                workflow.FormTemplates = workflow.FormTemplates.OrderBy(x => x.CreationTime).ToList();
                foreach (var form in workflow.FormTemplates)
                {
                    form.FlowTemplates = form.FlowTemplates.OrderBy(x => x.CreationTime).ToList();
                }
            }

            return result;
        }


        /// <summary>
        ///     创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BpmPermissions.WorkflowTemplate.Create)]
        public async Task<WorkflowTemplateDetailDto> Create(WorkflowTemplateCreateDto input)
        {
            var workflow = new WorkflowTemplate(Guid.NewGuid());
            workflow.Name = input.Name;
            workflow.WorkflowTemplateGroupId = input.GroupId;
            workflow.Type = input.Type;
            await _workflowTemplateRepository.InsertAsync(workflow);
            return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDetailDto>(workflow);
        }


        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BpmPermissions.WorkflowTemplate.Update)]
        public async Task<WorkflowTemplateDetailDto> Update(WorkflowTemplateUpdateDto input)
        {
            var workflow = await _workflowTemplateRepository.GetAsync(input.Id);

            if (workflow == null)
            {
                throw new UserFriendlyException("工作流不存在");
            }

            workflow.Name = input.Name;
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDetailDto>(workflow);
        }

        /// <summary>
        ///     编辑流程模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BpmPermissions.WorkflowTemplate.Update)]
        public async Task<WorkflowTemplateDetailDto> UpdateFlowTemplate(
            WorkflowTemplateUpdateFlowTemplateInputDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty)
            {
                throw new UserFriendlyException("工作流模板 Id 无效");
            }

            // 获取当前工作流的表单模板Id，进行对比保证表单模板没有被更改过
            var workflowTemplate = await Get(input.Id);
            if (workflowTemplate.FormTemplate.Id != input.FormTemplateId)
            {
                throw new UserFriendlyException("表单模板已发生变化，请刷新重新编辑");
            }

            foreach (var node in input.FlowNodes)
            {
                var id = Guid.NewGuid();
                foreach (var step in input.FlowSteps)
                {
                    if (step.Target == node.Id)
                    {
                        step.Target = id;
                    }

                    if (step.Source == node.Id)
                    {
                        step.Source = id;
                    }

                    step.Id = Guid.NewGuid();
                }

                node.Id = id;
            }

            var flowTemplate = workflowTemplate.FormTemplate.FlowTemplate;

            // 判断当前数据是否发起过流程（即产生了数据），如果没有则直接更新，否则增加一条
            var count = _workflowRepository.Count(x => x.FlowTemplateId == flowTemplate.Id);
            var _nodes = ObjectMapper.Map<List<FlowTemplateNodeDto>, List<FlowTemplateNode>>(input.FlowNodes);
            var _steps = ObjectMapper.Map<List<FlowTemplateStepDto>, List<FlowTemplateStep>>(input.FlowSteps);
            var flowVersion = 1;

            if (count == 0)
            {
                var flowTemplateId = workflowTemplate.FormTemplate.FlowTemplate.Id;
                await _bpmManager.RemoveFlowTemplate(flowTemplateId);
                flowVersion = flowTemplate.Version;
            }
            else
            {
                flowVersion = flowTemplate.Version + 1;
            }

            var _flowTemplate = new FlowTemplate(Guid.NewGuid())
            {
                FormTemplateId = flowTemplate.FormTemplateId,
                Nodes = _nodes,
                Steps = _steps,
                Version = flowVersion
            };

            await _flowTemplateRepository.InsertAsync(_flowTemplate);

            return await Get(input.Id);
        }

        /// <summary>
        ///     编辑表单模板和流程模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BpmPermissions.WorkflowTemplate.Update)]
        public async Task<WorkflowTemplateDetailDto> UpdateFormTemplateAndFlowTemplate(
            WorkflowTemplateUpdateFormTemplateAndFlowTemplateInputDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty)
            {
                throw new UserFriendlyException("工作流模板 Id 无效");
            }

            // 静态模板不能编辑
            var workflowTemplate = await _bpmManager.GetWorkflowTemplate(input.Id);
            if (workflowTemplate.IsStatic)
            {
                throw new UserFriendlyException("静态模板不能编辑");
            }

            foreach (var node in input.FlowNodes)
            {
                var id = Guid.NewGuid();
                foreach (var step in input.FlowSteps)
                {
                    if (step.Target == node.Id)
                    {
                        step.Target = id;
                    }

                    if (step.Source == node.Id)
                    {
                        step.Source = id;
                    }

                    step.Id = Guid.NewGuid();
                }

                node.Id = id;
            }

            // 当前【表单模板】和【流程模板】是否存在，如果存在并没有发起过流程，则新建，否则直接修改
            var _formTemplate = workflowTemplate.FormTemplates.FirstOrDefault();
            var _flowTemplate = _formTemplate?.FlowTemplates.FirstOrDefault();

            var _nodes = ObjectMapper.Map<List<FlowTemplateNodeDto>, List<FlowTemplateNode>>(input.FlowNodes);
            var _steps = ObjectMapper.Map<List<FlowTemplateStepDto>, List<FlowTemplateStep>>(input.FlowSteps);


            var formVersion = 1;
            if (_formTemplate != null && _flowTemplate != null)
            {
                // 检查最后一个版本流程模板是否被使用，

                if (_workflowRepository.Where(x => x.FlowTemplateId == _flowTemplate.Id).Count() == 0)
                {
                    await _bpmManager.RemoveFlowTemplate(_flowTemplate.Id);
                }

                // 表单已经被使用，版本增加
                if (_workflowRepository.Where(x => x.FlowTemplate.FormTemplate.Id == _formTemplate.Id).Count() > 0)
                {
                    formVersion = _formTemplate.Version + 1;
                }
                else
                // 删除之前的模板，版本不变
                {
                    await _bpmManager.RemoveFormTemplate(_formTemplate.Id);
                    formVersion = _formTemplate.Version;
                }
            }

            var formTemplate = new FormTemplate(Guid.NewGuid())
            {
                WorkflowTemplateId = input.Id,
                FormItems = input.FormItems,
                Config = input.FormConfig,
                Version = formVersion,
                FlowTemplates = new List<FlowTemplate>
                {
                    new FlowTemplate(Guid.NewGuid())
                    {
                        Nodes = _nodes,
                        Steps = _steps,
                        Version = 1
                    }
                }
            };
            await _formTemplateRepository.InsertAsync(formTemplate);
            return await Get(input.Id);
        }


        /// <summary>
        ///     更新发布对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BpmPermissions.WorkflowTemplate.Update)]
        public async Task<WorkflowTemplateDetailDto> UpdateMembers(WorkflowTemplateUpdateMembersDto input)
        {
            var workflowTemplate = await _workflowTemplateRepository.GetAsync(input.Id);
            if (workflowTemplate == null)
            {
                throw new UserFriendlyException("工作流不存在");
            }

            await _workflowTemplateRltMemberRepository.DeleteAsync(x => x.WorkflowTemplateId == input.Id);
            foreach (var member in input.Members)
            {
                await _workflowTemplateRltMemberRepository.InsertAsync(
                    new WorkflowTemplateRltMember(Guid.NewGuid())
                    {
                        WorkflowTemplateId = input.Id,
                        MemberId = member.Id,
                        Name = member.Name,
                        Type = member.Type
                    });
            }

            return await Get(input.Id);
        }


        /// <summary>
        ///     更改发布状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkflowTemplateDetailDto> UpdatePublishState(
            WorkflowTemplateChangePublishStateInputDto input)
        {
            var workflow = _workflowTemplateRepository.WithDetails(x => x.FormTemplates).Where(x => x.Id == input.Id)
                .FirstOrDefault();

            if (workflow == null)
            {
                throw new UserFriendlyException("工作流不存在");
            }

            if (workflow.Published == input.Published)
            {
                throw new UserFriendlyException("状态已存在");
            }

            // 判断是否有表单模板及流程模板，如果没有则不允许发布
            if (workflow.FormTemplates.Count == 0 && input.Published)
            {
                throw new UserFriendlyException("请先设计表单及流程，然后发布");
            }


            workflow.Published = input.Published;
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDetailDto>(workflow);
        }


        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(BpmPermissions.WorkflowTemplate.Delete)]
        public async Task<bool> Delete([Required] Guid id)
        {
            await _workflowTemplateRepository.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// 获取工作流模板分组信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkflowTemplateGroupDetailDto>> GetGroupList()
        {
            var data = await _workflowTemplateGroups.GetListAsync();
            var list = ObjectMapper.Map<List<WorkflowTemplateGroup>, List<WorkflowTemplateGroupDetailDto>>(data);
            return GuidKeyTreeHelper<WorkflowTemplateGroupDetailDto>.GetTree(list);
        }

        /// <summary>
        /// 创建一个分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CreateGroup(WorkflowTemplateGroupCreateDto input)
        {
            if (input.Name == "系统流程")
            {
                throw new UserFriendlyException("分组名称不能为【系统流程】");
            }
            var model = ObjectMapper.Map<WorkflowTemplateGroupCreateDto, WorkflowTemplateGroup>(input);
            model.SetId(GuidGenerator.Create());
            await _workflowTemplateGroups.InsertAsync(model);
            return true;
        }

        /// <summary>
        /// 更新分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateGroup(WorkflowTemplateGroupCreateDto input)
        {
            if (input.Name.IsNullOrEmpty()) throw new UserFriendlyException("分组名称不能为空");
            var model = await _workflowTemplateGroups.GetAsync(input.Id);
            model.Name = input.Name;
            await _workflowTemplateGroups.UpdateAsync(model);
            return true;
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<bool> DeleteGroup(Guid id)
        {
            // 删除分组修改，查询分组下中的流程模板中是否为空，不为空进行提示，为空时判断流程模板是否被软删除，对软删除的文件删除分组id
            var templates = _workflowTemplateRepository.Where(a => a.WorkflowTemplateGroupId == id);
            if (templates.Any())
            {
                throw new UserFriendlyException("该分组含有子流程信息，请先删除分组下的流程！");
            }
            else
            {
                using (_dataFilter.Disable<ISoftDelete>())
                {
                    var list = _workflowTemplateRepository.Where(a => a.WorkflowTemplateGroupId == id && a.IsDeleted).ToList();
                    list?.ForEach(a =>
                    {
                        a.WorkflowTemplateGroupId = null;
                        _workflowTemplateRepository.UpdateAsync(a);
                    });
                    await UnitOfWorkManager.Current.SaveChangesAsync();
                }
            }
            await _workflowTemplateGroups.DeleteAsync(id);
            return true;
        }


        public async Task<List<Test>> Test(WorkflowTestInputDto input)
        {
            var list = new List<Test>();
            list.Add(new TestA
            {
                Id = 1,
                Name = "TestA",
                PropA = "TestA_1_PropA"
            });

            list.Add(new TestB
            {
                Id = 2,
                Name = "TestB",
                PropB = 2
            });


            //// 工作流唯一标识，YearMonthPlan：代表年月计划工作流
            //var key = "YearMonthPlan";
            //// 获取年月表计划工作流配置文件，包含表单版本 Version、表单项 FromItems、表单配置不包含工作流程（流程可以在Web端进行设计）
            //var jsonString = File.ReadAllText(key + ".json");

            //var test = await _bpmManager.RegisterWorkflowTemplate(key, jsonString);

            //var test2 = workflowTemplateRepository.LocalGetList();

            await _bpmManager.RemoveFlowTemplate(new Guid("33fa672c-0e8b-4376-8e7a-f5cf0895b4f6"));

            return ObjectMapper.Map<List<object>, List<Test>>(input.Tests);
        }
    }
}