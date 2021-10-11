using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.Bpm;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.IServices;
using SnAbp.Construction.Dtos.MasterPlan;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlan;
using SnAbp.Construction.Enums;
using SnAbp.Construction.IServices;
using SnAbp.Construction.IServices.MasterPlan;
using SnAbp.Construction.MasterPlans.Entities;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Services.MaterPlan
{

	/// <summary>
	/// 施工计划 Service 
	/// </summary>
	// [Authorize]
	public class MasterPlanAppService : CrudAppService<
			MasterPlan, MasterPlanDto, Guid, MasterPlanSearchDto, MasterPlanCreateDto,
			MasterPlanUpdateDto>,
		IMasterPlanAppService
	{
		private readonly IRepository<MasterPlanContent, Guid> _masterPlanContentRepository;
		private readonly IRepository<MasterPlanRltWorkflowInfo, Guid> _masterPlanRltWorkflowInfoRepository;
		private readonly ISingleFlowProcessService _singleFlowProcessService;

		public MasterPlanAppService(
			IRepository<MasterPlan, Guid> repository,
			IRepository<MasterPlanContent, Guid> masterPlanContentRepository,
			IRepository<MasterPlanRltWorkflowInfo, Guid> masterPlanRltWorkflowInfoRepository,
			ISingleFlowProcessService singleFlowProcessService
		) : base(repository)
		{
			_masterPlanContentRepository = masterPlanContentRepository;
			_masterPlanRltWorkflowInfoRepository = masterPlanRltWorkflowInfoRepository;
			_singleFlowProcessService = singleFlowProcessService;
			repository.GetListAsync();
		}

		/// <summary>
		/// 更新前的数据验证
		/// </summary>
		protected override void MapToEntity(MasterPlanUpdateDto updateInput, MasterPlan entity)
		{
			Console.WriteLine("更新前验证数据");
			if (updateInput.Name.Trim().Length>20) throw new UserFriendlyException("名称不能超过20位");
			base.MapToEntity(updateInput, entity);
		}

		/// <summary>
		/// 创建前的数据验证
		/// </summary>
		protected override MasterPlan MapToEntity(MasterPlanCreateDto updateInput)
		{
			if (updateInput.Name.Trim().Length>20) throw new UserFriendlyException("名称不能超过20位");
			if (Repository.Any(x=>x.Name==updateInput.Name.Trim())) throw new UserFriendlyException($"{updateInput.Name} 已存在!");
			Console.WriteLine("创建前验证数据");
			return base.MapToEntity(updateInput);
		}


		/// <summary>
		/// 在 getList 方法 前 构造 query.where 重写
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		protected override IQueryable<MasterPlan> CreateFilteredQuery(MasterPlanSearchDto input)
		{
			IQueryable<MasterPlan> query = Repository.WithDetails();
			// 这里手动 自己写,实在是拼不动了…… 
			query = query
				.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) || x.Content.Contains(input.SearchKey)); ; // 根据xx类型查询 

			return query;
		}


		public override async Task<PagedResultDto<MasterPlanDto>> GetListAsync(MasterPlanSearchDto input)
		{
			PagedResultDto<MasterPlanDto> result = new PagedResultDto<MasterPlanDto> ();
			IQueryable<MasterPlan> query = Repository.WithDetails()
				.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) || x.Content.Contains(input.SearchKey));
            var list = new List<MasterPlan>();
            if (input.Approval) // 如果是审批模式的话
            {
	            if (input.Waiting) //待我审批的数据 
	            {
                    foreach (MasterPlan item in query)
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
            result.Items = CalculateMasterPlanState(list.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return result;
		}
        /// <summary>
        /// 计算总体计划的流程状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<MasterPlanDto> CalculateMasterPlanState(List<MasterPlan> list)
        {
            var result = new List<MasterPlanDto>();
            foreach (var item in list)
            {
                var model = ObjectMapper.Map<MasterPlan, MasterPlanDto>(item);
                if (item.WorkflowId != null)
                {
                    var workflow = _singleFlowProcessService.GetWorkflowById(item.WorkflowId.Value).Result;
                    switch (workflow.State)
                    {
                        case Bpm.WorkflowState.Waiting:
                            model.State = MasterPlanState.OnReview;
                            break;
                        case Bpm.WorkflowState.Finished:
                            model.State = MasterPlanState.Pass;
                            break;
                        case Bpm.WorkflowState.Rejected:
                            model.State = MasterPlanState.UnPass;
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
                        if (data != null&& (data.StepState == Bpm.WorkflowStepState.Stopped|| data.StepState == Bpm.WorkflowStepState.Rejected))
                        {
                            model.State = MasterPlanState.UnPass;
                        }
                    }
                }
                result.Add(model);
            }
            return result;
        }
		
		

		/// <summary>
		/// 删除多个
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		public async Task<bool> DeleteRange(List<Guid> ids)
		{
			
			if (_masterPlanContentRepository.Any(x=>ids.Contains(x.MasterPlanId.Value)))
			{
				throw new UserFriendlyException("删除的计划里面有关联的计划详情进度,不能删除!");
			}
			await Repository.DeleteAsync(x => ids.Contains(x.Id));

			return true;
		}

		/// <summary>
		/// 是否有编制(有masterContent 就是有,否则就没有)
		/// </summary>
		/// <returns></returns>
		public bool HasContent(Guid id)
		{
			return _masterPlanContentRepository.Any(x => x.MasterPlanId == id);
		}
		/// <summary>
		/// 审批流程 
		/// </summary>
		/// <returns></returns>
		public async  Task<bool> Process(MasterPlanProcessDto input)
		{
            var materialPlan = await Repository.GetAsync(input.PlanId);
            var workflowId = materialPlan.WorkflowId.GetValueOrDefault();
            Bpm.Dtos.WorkflowDetailDto dto = null;
            if (input.Status == MasterPlanState.Pass)
            {
                dto = await _singleFlowProcessService.Approved(workflowId, input.Content,CurrentUser.Id);
            }
            else if(input.Status==MasterPlanState.UnPass)
            {
                dto = await _singleFlowProcessService.Stopped(workflowId, input.Content,CurrentUser.Id);
            }
            else
            {
                throw new UserFriendlyException("流程处理异常");
            }

            MasterPlanRltWorkflowInfo planInfo = new MasterPlanRltWorkflowInfo(GuidGenerator.Create())
            {
	            Content = input.Content,
	            MasterPlanId = materialPlan.Id,
	            WorkflowId = workflowId,
	            WorkflowState = dto.State
            };
            await _masterPlanRltWorkflowInfoRepository.InsertAsync(planInfo);
            return true;
		}
        /// <summary>
        /// 根据id 修改 施工计划时间(开始/结束时间)
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ChangeDateById(Guid id,DateTime[] times)
        {
	        MasterPlan plan = await Repository.GetAsync(id);
	        plan.PlanStartTime = times[0];
	        plan.PlanEndTime = times[1];
	        int duration = (plan.PlanEndTime.Date - plan.PlanStartTime .Date).Days;
	        plan.Period = duration + 1;
	        await Repository.UpdateAsync(plan);
	        return true;
        }

		/// <summary>
		/// 创建 审批 工作流
		/// </summary>
		/// <param name="id">任务计划id</param>
		/// <param name="workFlowId">审批id </param>
		/// <returns></returns>
		public async Task<bool> CreateWorkFlow(Guid id,Guid workFlowId)
        {
            Workflow workflow = await _singleFlowProcessService.CreateSingleWorkFlow(workFlowId);
            MasterPlan masterPlan = await Repository.GetAsync(id);
            masterPlan.WorkflowTemplateId = workFlowId;
            masterPlan.WorkflowId = workflow.Id;
            masterPlan.State = MasterPlanState.OnReview;// 审核中
            await Repository.UpdateAsync(masterPlan);
            return true;
        }
	}
}
