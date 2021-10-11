using Microsoft.AspNetCore.Http;
using SnAbp.Bpm.IServices;
using SnAbp.Construction.Dtos;
using SnAbp.Construction.Entities;
using SnAbp.Construction.Enums;
using SnAbp.Construction.IServices.Daily;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

/************************************************************************************
*命名空间：SnAbp.Construction.Services.Daily
*文件名：ConstructionDailyAppService
*创建人： liushengtao
*创建时间：2021/7/21 14:18:43
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Services
{
    public class ConstructionDailyAppService : ConstructionAppService, IConstructionDailyAppService
    {
        readonly ISingleFlowProcessService _singleFlowProcessService;
        readonly IUnitOfWorkManager _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Organization, Guid> _orgRepository;
        private readonly IRepository<Daily, Guid> _dailyRepository;
        private readonly IRepository<DailyRltFile, Guid> _dailyRltFileRepository;
        private readonly IRepository<DailyRltQuality, Guid> _dailyRltQualityRepository;
        private readonly IRepository<DailyRltSafe, Guid> _dailyRltSafeRepository;
        private readonly IRepository<DailyTemplate, Guid> _dailyTemplateRepository;
        private readonly IRepository<UnplannedTask, Guid> _unplannedTaskRepository;
        private readonly IRepository<DailyFlowInfo, Guid> _dailyFlowInfoRepository;
        private readonly IRepository<DailyRltPlanMaterial, Guid> _dailyRltPlanMaterialRepository;
        private readonly IGuidGenerator _guidGenerator;
        public ConstructionDailyAppService(
            IRepository<DailyTemplate, Guid> dailyTemplateRepository,
            IRepository<DailyFlowInfo, Guid> dailyFlowInfoRepository,
                      IRepository<Organization, Guid> orgRepository,
               IHttpContextAccessor httpContextAccessor,
                   IUnitOfWorkManager unitOfWork,
                   ISingleFlowProcessService singleFlowProcessService,
                  IRepository<Daily, Guid> dailyRepository,
                  IRepository<DailyRltFile, Guid> dailyRltFileRepository,
                  IRepository<DailyRltQuality, Guid> dailyRltQualityRepository,
                  IRepository<DailyRltSafe, Guid> dailyRltSafeRepository,
                  IRepository<UnplannedTask, Guid> unplannedTaskRepository,
                  IRepository<DailyRltPlanMaterial, Guid> dailyRltPlanMaterialRepository,
                  IGuidGenerator guidGenerator

            )
        {
            _dailyFlowInfoRepository = dailyFlowInfoRepository;
            _orgRepository = orgRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _singleFlowProcessService = singleFlowProcessService;
            _dailyRepository = dailyRepository;
            _dailyRltFileRepository = dailyRltFileRepository;
            _dailyRltQualityRepository = dailyRltQualityRepository;
            _dailyRltSafeRepository = dailyRltSafeRepository;
            _dailyTemplateRepository = dailyTemplateRepository;
            _unplannedTaskRepository = unplannedTaskRepository;
            _guidGenerator = guidGenerator;
            _dailyRltPlanMaterialRepository = dailyRltPlanMaterialRepository;
        }
        /// <summary>
        /// 获取单个施工日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DailyDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");

            var daily = _dailyRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (daily == null) throw new UserFriendlyException("当前数据不存在");

            return Task.FromResult(ObjectMapper.Map<Daily, DailyDto>(daily));
        }
        /// <summary>
        /// 获取施工日志列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DailyDto>> GetList(DailySearchDto input)
        {
            var result = new PagedResultDto<DailyDto>();
            var dailys = _dailyRepository.WithDetails(x=>x.Informant)
                .Where(x => x.CreatorId == CurrentUser.Id)
                   .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Informant.Name.Contains(input.KeyWords))
                    .WhereIf(input.StartTime != null, x => x.Date >= input.StartTime)
                   .WhereIf(input.EndTime != null, x => x.Date <= input.EndTime);
            var list = new List<Daily>();
            if (input.Approval)
            {
                if (input.Waiting)
                {
                    // 获取待我审批的数据
                    foreach (var item in dailys)
                    {
                        if (item.WorkflowId == null) continue;
                        if (await _singleFlowProcessService.IsWaitingMyApproval(item.WorkflowId.GetValueOrDefault()))
                            list.Add(item);
                    }
                }
                else
                {
                    // 获取我已审批的数据
                    var workflowIds = await _singleFlowProcessService.GetMyApprovaledWorkflow();
                    if (workflowIds.Any())
                    {
                        list = dailys.Where(a => workflowIds.Contains(a.WorkflowId)).ToList();

                    }
                }
            }

            else
            {
                list = dailys.ToList();
            }
            if (input.IsAll)
            {
                result.Items = CalculateDailyState(list).Where(x => x.Status == DailyStatus.Pass).ToList();
                result.TotalCount = list.Count();
                list = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            }
            else
            {
                result.TotalCount = list.Count();
                result.Items = CalculateDailyState(list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            return result;
        }
        /// <summary>
        /// 获取已完成的施工任务量
        /// </summary>
        /// <param name="id">工程量id</param>
        /// <returns></returns>
        public Task<int> GetDailyRltPlanMaterial(Guid id)
        {
            var dailys = _dailyRltPlanMaterialRepository.WithDetails()
                   .Where(x => x.PlanMaterialId == id);
            var count = 0;
            foreach (var item in dailys)
            {
                count += item.Count;
            }
            return Task.FromResult(count);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(DailyCreateDto input)
        {
            var daily = new Daily();
            ObjectMapper.Map(input, daily);
            daily.Status = DailyStatus.ToSubmit;// 待提交
            //1、保存基本信息
            daily.SetId(_guidGenerator.Create());

            daily.DailyRltFiles = new List<DailyRltFile>();
            // 保存附件信息
            foreach (var file in input.DailyRltFiles)
            {
                daily.DailyRltFiles.Add(new DailyRltFile(_guidGenerator.Create())
                {
                    FileId = file.FileId
                });
            }
            daily.UnplannedTask = new List<UnplannedTask>();
            // 保存临时任务信息
            foreach (var task in input.UnplannedTask)
            {
                daily.UnplannedTask.Add(new UnplannedTask(_guidGenerator.Create())
                {
                    Content = task.Content,
                    TaskType = task.TaskType,
                });
            }
            daily.DailyRltSafe = new List<DailyRltSafe>();
            // 保存安全问题信息
            foreach (var safe in input.DailyRltSafe)
            {
                daily.DailyRltSafe.Add(new DailyRltSafe(_guidGenerator.Create())
                {
                    SafeProblemId = safe.SafeProblemId,
                });
            }
            daily.DailyRltQuality = new List<DailyRltQuality>();
            // 保存质量问题信息
            foreach (var safe in input.DailyRltQuality)
            {
                daily.DailyRltQuality.Add(new DailyRltQuality(_guidGenerator.Create())
                {
                    QualityProblemId = safe.QualityProblemId,
                });
            }
            daily.DailyRltPlan = new List<DailyRltPlanMaterial>();
            // 保存施工任务信息
            foreach (var plan in input.DailyRltPlan)
            {
                daily.DailyRltPlan.Add(new DailyRltPlanMaterial(_guidGenerator.Create())
                {
                    PlanMaterialId = plan.PlanMaterialId,
                    Count = plan.Count,
                });
            }
            var dailyTemplate = _dailyTemplateRepository.FirstOrDefault(x => x.IsDefault);
            if (dailyTemplate != null)
            {
                daily.DailyTemplateId = dailyTemplate.Id;
            }
            await _dailyRepository.InsertAsync(daily);
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            await _dailyRepository.DeleteAsync(id);

            return true;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DailyDto> Update(DailyUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的数据");
            var daily = await _dailyRepository.GetAsync(input.Id);
            if (daily == null) throw new UserFriendlyException("当前数据不存在");
            daily.Code = input.Code;
            daily.DispatchId = input.DispatchId;
            daily.Date = input.Date;
            daily.InformantId = input.InformantId;
            daily.Weathers = input.Weathers;
            daily.Temperature = input.Temperature;
            daily.WindDirection = input.WindDirection;
            daily.AirQuality = input.AirQuality;
            daily.Team = input.Team;
            daily.BuilderCount = input.BuilderCount;
            daily.Location = input.Location;
            daily.Summary = input.Summary;
            daily.Remark = input.Remark;
            // 清除之前关联信息
            await _dailyRltFileRepository.DeleteAsync(x => x.DailyId == daily.Id);
            daily.DailyRltFiles = new List<DailyRltFile>();
            // 保存附件信息
            foreach (var file in input.DailyRltFiles)
            {
                daily.DailyRltFiles.Add(new DailyRltFile(_guidGenerator.Create())
                {
                    FileId = file.FileId,
                    DailyId = daily.Id
                }); ;
            }
            // 清除之前关联信息
            await _unplannedTaskRepository.DeleteAsync(x => x.DailyId == daily.Id);
            daily.UnplannedTask = new List<UnplannedTask>();
            // 保存临时任务信息
            foreach (var task in input.UnplannedTask)
            {
                daily.UnplannedTask.Add(new UnplannedTask(_guidGenerator.Create())
                {
                    Content = task.Content,
                    TaskType = task.TaskType,
                    DailyId = daily.Id
                });
            }
            // 清除之前安全关联信息
            await _dailyRltSafeRepository.DeleteAsync(x => x.DailyId == daily.Id);
            daily.DailyRltSafe = new List<DailyRltSafe>();
            // 保存安全问题信息
            foreach (var safe in input.DailyRltSafe)
            {
                daily.DailyRltSafe.Add(new DailyRltSafe(_guidGenerator.Create())
                {
                    SafeProblemId = safe.SafeProblemId,
                    DailyId = daily.Id
                });
            }

            // 清除之前质量关联信息
            await _dailyRltQualityRepository.DeleteAsync(x => x.DailyId == daily.Id);
            daily.DailyRltQuality = new List<DailyRltQuality>();
            // 保存质量问题信息
            foreach (var safe in input.DailyRltQuality)
            {
                daily.DailyRltQuality.Add(new DailyRltQuality(_guidGenerator.Create())
                {
                    QualityProblemId = safe.QualityProblemId,
                });
            }
            // 清除之前任务关联信息
            await _dailyRltPlanMaterialRepository.DeleteAsync(x => x.DailyId == daily.Id);
            daily.DailyRltPlan = new List<DailyRltPlanMaterial>();
            // 保存施工任务信息
            foreach (var plan in input.DailyRltPlan)
            {
                daily.DailyRltPlan.Add(new DailyRltPlanMaterial(_guidGenerator.Create())
                {
                    PlanMaterialId = plan.PlanMaterialId,
                    Count = plan.Count,
                });
            }
            var dailyTemplate = _dailyTemplateRepository.FirstOrDefault(x => x.IsDefault);
            if (dailyTemplate != null)
            {
                daily.DailyTemplateId = dailyTemplate.Id;
            }
            await _dailyRepository.UpdateAsync(daily);
            return ObjectMapper.Map<Daily, DailyDto>(daily);
        }


        #region 日志审批
        /// <summary>
        /// 创建工作流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CreateWorkFlow(Guid planId, Guid templateId)
        {
            var workflow = await _singleFlowProcessService.CreateSingleWorkFlow(templateId);
            var daily = await _dailyRepository.GetAsync(planId);
            daily.WorkflowTemplateId = templateId;
            daily.WorkflowId = workflow.Id;
            daily.Status = DailyStatus.OnReview;// 审核中
            await _dailyRepository.UpdateAsync(daily);
            return true;
        }
        [UnitOfWork]
        public async Task<bool> Process(DailyProcessDto input)
        {
            var daily = await _dailyRepository.GetAsync(input.PlanId);
            var workflowId = daily.WorkflowId.GetValueOrDefault();
            Bpm.Dtos.WorkflowDetailDto dto = null;
            using (var uow = _unitOfWork.Begin(true, false))
            {
                if (input.Status == DailyStatus.Pass)
                {
                    dto = await _singleFlowProcessService.Approved(workflowId, input.Content, CurrentUser.Id);
                }
                else if (input.Status == DailyStatus.UnPass)
                {
                    dto = await _singleFlowProcessService.Stopped(workflowId, input.Content, CurrentUser.Id);
                }
                else
                {
                    throw new UserFriendlyException("流程处理异常");
                }
                await uow.CompleteAsync();
            }
            // 更新当前 流程的状态
            var workflow = _singleFlowProcessService.GetWorkflowById(workflowId).Result;
            switch (workflow.State)
            {
                case Bpm.WorkflowState.Waiting:
                    daily.Status = DailyStatus.OnReview;
                    break;
                case Bpm.WorkflowState.Finished:
                    daily.Status = DailyStatus.Pass;
                    break;
                case Bpm.WorkflowState.Rejected:
                    daily.Status = DailyStatus.UnPass;
                    break;
                default:
                    break;
            }
            if (workflow.WorkflowDatas.Any())
            {
                var data = workflow.WorkflowDatas.Where(a => a.StepState != null)
                    .OrderByDescending(a => a.CreationTime)
                    .FirstOrDefault();
                if (data != null && (data.StepState == Bpm.WorkflowStepState.Stopped || data.StepState == Bpm.WorkflowStepState.Rejected))
                {
                    daily.Status = DailyStatus.UnPass;
                }
            }
            await _dailyRepository.UpdateAsync(daily);
            var planInfo = new DailyFlowInfo(GuidGenerator.Create());
            planInfo.Content = input.Content;
            planInfo.DailyId = daily.Id;
            planInfo.WorkFlowId = workflowId;
            planInfo.State = workflow.State;
            await _dailyFlowInfoRepository.InsertAsync(planInfo);
            return true;
        }
        /// <summary>
        /// 计算用料计划的流程状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<DailyDto> CalculateDailyState(List<Daily> list)
        {
            var result = new List<DailyDto>();
            foreach (var item in list)
            {
                var model = ObjectMapper.Map<Daily, DailyDto>(item);
                if (item.WorkflowId != null)
                {
                    var workflow = _singleFlowProcessService.GetWorkflowById(item.WorkflowId.Value).Result;
                    switch (workflow.State)
                    {
                        case Bpm.WorkflowState.Waiting:
                            model.Status = DailyStatus.OnReview;
                            break;
                        case Bpm.WorkflowState.Finished:
                            model.Status = DailyStatus.Pass;
                            break;
                        case Bpm.WorkflowState.Rejected:
                            model.Status = DailyStatus.UnPass;
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
                            model.Status = DailyStatus.UnPass;
                        }
                    }
                }
                result.Add(model);
            }
            return result;
        }

        #endregion


    }
}
