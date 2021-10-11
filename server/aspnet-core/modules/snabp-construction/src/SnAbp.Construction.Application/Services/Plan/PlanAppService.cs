using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.Bpm;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.IServices;
using SnAbp.Construction.Dtos.Plan;
using SnAbp.Construction.Dtos.Plan.Plan;
using SnAbp.Construction.Enums;
using SnAbp.Construction.IServices;
using SnAbp.Construction.IServices.Plan;
using SnAbp.Construction.Plans;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Services.Plan
{

    /// <summary>
    /// 施工计划 Service 
    /// </summary>
    // [Authorize]
    public class PlanAppService : CrudAppService<
            Plans.Plan, PlanDto, Guid, PlanSearchDto, PlanCreateDto,
            PlanUpdateDto>,
        IPlanAppService
    {
        private readonly IRepository<PlanContent, Guid> _planContentRepository;
        private readonly IRepository<PlanRltWorkflowInfo, Guid> _planRltWorkflowInfoRepository;
        private readonly ISingleFlowProcessService _singleFlowProcessService;

        public PlanAppService(
        IRepository<Plans.Plan, Guid> repository,
            IRepository<PlanContent, Guid> planContentRepository,
            IRepository<PlanRltWorkflowInfo, Guid> planRltWorkflowInfoRepository,
            ISingleFlowProcessService singleFlowProcessService
        ) : base(repository)
        {
            _planContentRepository = planContentRepository;
            _planRltWorkflowInfoRepository = planRltWorkflowInfoRepository;
            _singleFlowProcessService = singleFlowProcessService;
            repository.GetListAsync();
        }

        /// <summary>
        /// 更新前的数据验证
        /// </summary>
        protected override void MapToEntity(PlanUpdateDto updateInput, Plans.Plan entity)
        {
            if (updateInput.Name.Trim().Length > 20) throw new UserFriendlyException("名称不能超过20位");
            Console.WriteLine("更新前验证数据");
            base.MapToEntity(updateInput, entity);
        }
        /// <summary>
        /// 创建前的数据验证
        /// </summary>
        protected override Plans.Plan MapToEntity(PlanCreateDto updateInput)
        {
            if (updateInput.Name.Trim().Length > 20) throw new UserFriendlyException("名称不能超过20位");
            if (Repository.Any(x => x.Name == updateInput.Name.Trim())) throw new UserFriendlyException($"{updateInput.Name} 已存在!");
            Console.WriteLine("创建前验证数据");
            return base.MapToEntity(updateInput);
        }


        /// <summary>
        /// 在 getList 方法 前 构造 query.where 重写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<Plans.Plan> CreateFilteredQuery(PlanSearchDto input)
        {
            IQueryable<Plans.Plan> query = Repository.WithDetails(); // include 关联查询,在 对应的 EntityFrameworkCoreModule.cs 文件里面 设置 include 

            // 这里自己手动 写吧,实在是拼不动了…… 

            query = query
                .WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) || x.Content.Contains(input.SearchKey));

            return query;
        }

        public override async Task<PagedResultDto<PlanDto>> GetListAsync(PlanSearchDto input)
        {
            PagedResultDto<PlanDto> result = new PagedResultDto<PlanDto>();
            IQueryable<Plans.Plan> query = Repository.WithDetails()
                .WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) || x.Content.Contains(input.SearchKey));
            var list = new List<Plans.Plan>();
            if (input.Approval) // 如果是审批模式的话
            {
                if (input.Waiting) //待我审批的数据 
                {
                    foreach (Plans.Plan item in query)
                    {
                        if (item.WorkflowId == null) continue;
                        if (await _singleFlowProcessService.IsWaitingMyApproval(item.WorkflowId.GetValueOrDefault()))
                        {
                            list.Add(item);
                        }
                    }
                }
                else // 我已审批的数据  
                {
                    // 获取我已审批的数据
                    var workflowIds = await _singleFlowProcessService.GetMyApprovaledWorkflow();
                    if (workflowIds.Any())
                    {
                        list = query.Where(a => workflowIds.Contains(a.WorkflowId)).ToList();
                    }
                }
            }
            else
            {
                // 根据 onlyPass 查询全部或者 只有审批通过的数据 
                list = input.OnlyPass
                    ? query.Where(x => x.Workflow.State == WorkflowState.Finished).ToList()
                    : query.ToList();
            }
            result.TotalCount = list.Count();
            result.Items = CalculatePlanState(list.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return result;
        }

        /// <summary>
        /// 计算总体计划的流程状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<PlanDto> CalculatePlanState(List<Plans.Plan> list)
        {
            var result = new List<PlanDto>();
            foreach (var item in list)
            {
                var model = ObjectMapper.Map<Plans.Plan, PlanDto>(item);
                if (item.WorkflowId != null)
                {
                    var workflow = _singleFlowProcessService.GetWorkflowById(item.WorkflowId.Value).Result;
                    switch (workflow.State)
                    {
                        case WorkflowState.Waiting:
                            model.State = ConstructionPlanState.OnReview;
                            break;
                        case WorkflowState.Finished:
                            model.State = ConstructionPlanState.Pass;
                            break;
                        case WorkflowState.Rejected:
                            model.State = ConstructionPlanState.UnPass;
                            break;
                        default:
                            break;
                    }
                    //根据审批数据进行判断当前的状态
                    if (workflow.WorkflowDatas.Any())
                    {
                        var data = workflow.WorkflowDatas.Where(a => a.StepState != null)
                            .OrderByDescending(a => a.CreationTime)
                            .FirstOrDefault();
                        if (data != null && (data.StepState == Bpm.WorkflowStepState.Stopped || data.StepState == Bpm.WorkflowStepState.Rejected))
                        {
                            model.State = ConstructionPlanState.UnPass;
                        }
                    }
                }
                result.Add(model);
            }
            return result;
        }
        /// <summary>
        /// 根据id 修改 施工计划时间(开始/结束时间)
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ChangeDateById(Guid id, DateTime[] times)
        {
            Plans.Plan plan = await Repository.GetAsync(id);
            plan.PlanStartTime = times[0];
            plan.PlanEndTime = times[1];
            int duration = (plan.PlanEndTime.Date - plan.PlanStartTime.Date).Days;
            plan.Period = duration + 1;
            await Repository.UpdateAsync(plan);
            return true;
        }

        /// <summary>
        /// 删除多个 施工计划
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRange(List<Guid> ids)
        {
            if (_planContentRepository.Any(x => ids.Contains(x.PlanId.Value)))
            {
                throw new UserFriendlyException("删除的计划里面有关联的计划详情进度,不能删除!");
            }
            await Repository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }


        /// <summary>
        /// 是否有编制(有Content 就是有,否则就没有)
        /// </summary>
        /// <returns></returns>
        public bool HasContent(Guid id)
        {
            return _planContentRepository.Any(x => x.PlanId == id);
        }
        /// <summary>
        /// 审批流程 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Process(PlanProcessDto input)
        {
            var materialPlan = await Repository.GetAsync(input.PlanId);
            var workflowId = materialPlan.WorkflowId.GetValueOrDefault();
            Bpm.Dtos.WorkflowDetailDto dto = null;
            if (input.Status == ConstructionPlanState.Pass)
            {
                dto = await _singleFlowProcessService.Approved(workflowId, input.Content, CurrentUser.Id);
            }
            else if (input.Status == ConstructionPlanState.UnPass)
            {
                dto = await _singleFlowProcessService.Stopped(workflowId, input.Content, CurrentUser.Id);
            }
            else
            {
                throw new UserFriendlyException("流程处理异常");
            }

            PlanRltWorkflowInfo planInfo = new PlanRltWorkflowInfo(GuidGenerator.Create())
            {
                Content = input.Content,
                PlanId = materialPlan.Id,
                WorkFlowId = workflowId,
                State = dto.State
            };
            await _planRltWorkflowInfoRepository.InsertAsync(planInfo);
            return true;
        }
        /// <summary>
        /// 创建 审批 工作流
        /// </summary>
        /// <param name="id">任务计划id</param>
        /// <param name="workFlowId">审批id </param>
        /// <returns></returns>
        public async Task<bool> CreateWorkFlow(Guid id, Guid workFlowId)
        {
            Workflow workflow = await _singleFlowProcessService.CreateSingleWorkFlow(workFlowId);
            Plans.Plan masterPlan = await Repository.GetAsync(id);
            masterPlan.WorkflowTemplateId = workFlowId;
            masterPlan.WorkflowId = workflow.Id;
            masterPlan.State = MasterPlanState.OnReview;// 审核中
            await Repository.UpdateAsync(masterPlan);
            return true;
        }
    }
}
