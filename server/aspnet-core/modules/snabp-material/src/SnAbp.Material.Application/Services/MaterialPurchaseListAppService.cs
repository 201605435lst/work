using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.XWPF.UserModel;
using SnAbp.Bpm;
using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.IServices;
using SnAbp.Bpm.Services;
using SnAbp.Identity;
using SnAbp.Material.Dtos;
using SnAbp.Material.Entities;
using SnAbp.Material.Enums;
using SnAbp.Material.IServices;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.WordHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Material.Services
{
    public class MaterialPurchaseListAppService : MaterialAppService, IMaterialPurchaseListAppService
    {
        private readonly IRepository<PurchaseList, Guid> _purchaseListRepository;
        private readonly IRepository<PurchaseListRltMaterial, Guid> _purchaseRltMaterialsRepository;
        private readonly IRepository<PurchaseListRltFile, Guid> _purchaseListRltFileRepository;
        private readonly IRepository<PurchaseListRltFlow, Guid> _purchaseListRltFlowRepository;
        private readonly IRepository<PurchaseListRltPurchasePlan, Guid> _purchaseListRltPurchasePlanRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Organization, Guid> _orgRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        readonly SingleFlowProcessService _singleFlowProcess;
        readonly ISingleFlowProcessService _singleFlowProcessService;
        readonly AppUserAppService _appUserAppService;
        readonly BpmManager _bpmManager;
        readonly IdentityUserManager _identityUser;
        protected IIdentityUserRepository _userRepository { get; }
        private readonly IRepository<FlowTemplateNode, Guid> _flowTemplateNodeRepository;
        private readonly IRepository<FlowTemplateStep, Guid> _flowTemplateStepRepository;
        private readonly IRepository<WorkflowTemplate, Guid> _workflowTemplateRepository;
        private readonly IRepository<Workflow, Guid> _workflowsRepository;
        private readonly IRepository<WorkflowData, Guid> _workflowDataRepository;
        private readonly IRepository<WorkflowStateRltMember, Guid> _workflowStateRltMemberRepository;



        public MaterialPurchaseListAppService(
            IRepository<Organization, Guid> orgRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<PurchaseListRltPurchasePlan, Guid> purchaseListRltPurchasePlanRepository,
            ISingleFlowProcessService singleFlowProcessService,
            IRepository<WorkflowTemplate, Guid> workflowTemplateRepository,
            IIdentityUserRepository userRepository,
            IRepository<PurchaseListRltFlow, Guid> purchaseListRltFlowRepository,
            IRepository<PurchaseList, Guid> purchaseListRepository,
            IRepository<PurchaseListRltMaterial, Guid> purchaseRltMaterialsRepository,
            IRepository<PurchaseListRltFile, Guid> purchaseListRltFileRepository,
            IGuidGenerator guidGenerator,
            IUnitOfWorkManager unitOfWorkManager,
            SingleFlowProcessService singleFlowProcess,
            AppUserAppService appUserAppService,
            BpmManager bpmManager,
            IdentityUserManager identityUser,
            IRepository<FlowTemplateNode, Guid> flowTemplateNodeRepository,
            IRepository<FlowTemplateStep, Guid> flowTemplateStepRepository,
            IRepository<WorkflowStateRltMember, Guid> workflowStateRltMemberRepository,
            IRepository<Workflow, Guid> workflowsRepository,
            IRepository<WorkflowData, Guid> workflowDataRepository
            )
        {
            _orgRepository = orgRepository;
            _httpContextAccessor = httpContextAccessor;
            _purchaseListRltPurchasePlanRepository = purchaseListRltPurchasePlanRepository;
            _singleFlowProcessService = singleFlowProcessService;
            _workflowTemplateRepository = workflowTemplateRepository;
            _userRepository = userRepository;
            _purchaseListRltFlowRepository = purchaseListRltFlowRepository;
            _purchaseListRepository = purchaseListRepository;
            _purchaseRltMaterialsRepository = purchaseRltMaterialsRepository;
            _purchaseListRltFileRepository = purchaseListRltFileRepository;
            _guidGenerator = guidGenerator;
            _unitOfWorkManager = unitOfWorkManager;
            _singleFlowProcess = singleFlowProcess;
            _appUserAppService = appUserAppService;
            _bpmManager = bpmManager;
            _identityUser = identityUser;
            _flowTemplateNodeRepository = flowTemplateNodeRepository;
            _flowTemplateStepRepository = flowTemplateStepRepository;
            _workflowStateRltMemberRepository = workflowStateRltMemberRepository;
            _workflowsRepository = workflowsRepository;
            _workflowDataRepository = workflowDataRepository;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PurchaseListDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null) throw new UserFriendlyException("数据错误，请刷新页面重新尝试");
            var purchaseList = _purchaseListRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (purchaseList == null) throw new UserFriendlyException("数据错误，请刷新页面重新尝试");
            return Task.FromResult(ObjectMapper.Map<PurchaseList, PurchaseListDto>(purchaseList));
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PurchaseListDto>> GetList(PurchaseListSearchDto input)
        {
            var purchaseLists = _purchaseListRepository.WithDetails()
                 .WhereIf(input.StartTime != null, x => x.PlanTime >= input.StartTime)
                  .WhereIf(input.EndTime != null, x => x.PlanTime <= input.EndTime)
                  .WhereIf(input.State != null, x => x.State == input.State)
                  .WhereIf(input.Type.IsIn(PurchaseListType.CentralPurchasing, PurchaseListType.MinorPurchasing, PurchaseListType.MonthPurchasing), x => x.Type == input.Type)
                  .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Name.Contains(input.Keyword));

            var list = new List<PurchaseList>();
            var plans = new PagedResultDto<PurchaseListDto>();
            if (input.Approval)
            {
                if (input.Waiting)
                {
                    // 获取待我审批的数据
                    foreach (var item in purchaseLists)
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
                        list = purchaseLists.Where(a => workflowIds.Contains(a.WorkflowId))
                      .ToList();
                    }
                }
            }
            else
            {

                list = purchaseLists.ToList();

            }
            plans.TotalCount = list.Count();
            list = CalculateMaterialPlanState(list.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            plans.Items = ObjectMapper.Map<List<PurchaseList>, List<PurchaseListDto>>(list.ToList());
            return plans;
        }
        #region 丢弃process
        //public async Task<bool> Process(PurchaseListProcessDto input)
        //{
        //    var purchaseLists = await _purchaseListRepository.GetAsync(input.PlanId);
        //    var workflowId = purchaseLists.WorkflowId.GetValueOrDefault();
        //    Bpm.Dtos.WorkflowDetailDto dto = null;
        //    if (input.State == PurchaseState.Pass)
        //    {
        //        dto = await _singleFlowProcessService.Approved(workflowId, input.Content,CurrentUser.Id);
        //    }
        //    else if (input.State == PurchaseState.UnPass)
        //    {
        //        dto = await _singleFlowProcessService.Rejected(workflowId, input.Content,CurrentUser.Id);
        //    }
        //    else
        //    {
        //        throw new UserFriendlyException("流程处理异常");
        //    }

        //    var planInfo = new PurchaseListRltFlow(GuidGenerator.Create());
        //    planInfo.Content = input.Content;
        //    planInfo.PurchaseListId = purchaseLists.Id;
        //    planInfo.WorkFlowId = workflowId;
        //    planInfo.State = dto.State;
        //    await _purchaseListRltFlowRepository.InsertAsync(planInfo);
        //    return true;
        //}
        #endregion
        public async Task<bool> Process(PurchaseListProcessDto input)
        {
            var purchaseLists = await _purchaseListRepository.GetAsync(input.PlanId);
            var workflowId = purchaseLists.WorkflowId.GetValueOrDefault();
            Bpm.Dtos.WorkflowDetailDto dto = null;
            using (var uow = _unitOfWorkManager.Begin(true, false))
            {
                if (input.State == PurchaseState.Pass)
                {
                    dto = await _singleFlowProcessService.Approved(workflowId, input.Content, CurrentUser.Id);
                }
                else if (input.State == PurchaseState.UnPass)
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
                    purchaseLists.State = PurchaseState.OnReview;
                    break;
                case Bpm.WorkflowState.Finished:
                    purchaseLists.State = PurchaseState.Pass;
                    break;
                case Bpm.WorkflowState.Rejected:
                    purchaseLists.State = PurchaseState.UnPass;
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
                    purchaseLists.State = PurchaseState.UnPass;
                }
            }
            await _purchaseListRepository.UpdateAsync(purchaseLists);
            var planInfo = new PurchaseListRltFlow(GuidGenerator.Create());
            planInfo.Content = input.Content;
            planInfo.PurchaseListId = purchaseLists.Id;
            planInfo.WorkFlowId = workflowId;
            //planInfo.State = input.State == PurchaseState.UnPass ? WorkflowState.Rejected : WorkflowState.Finished;
            planInfo.State = workflow.State;
            await _purchaseListRltFlowRepository.InsertAsync(planInfo);
            return true;
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PurchaseListDto> Create(PurchaseListCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new Volo.Abp.UserFriendlyException("采购单名称不能为空");
            if (input.PurchaseListRltMaterials.Count == 0) throw new Volo.Abp.UserFriendlyException("采购材料列表不能为空");

            var purchaseList = new PurchaseList();
            ObjectMapper.Map(input, purchaseList);
            await CheckSameName(input.Name, null);
            //1、保存基本信息
            purchaseList.SetId(_guidGenerator.Create());
            var code = "CGD_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace("-", "");
            purchaseList.Code = code;
            purchaseList.PurchaseListRltFiles = new List<PurchaseListRltFile>();
            #region 保存关联文件
            // 保存关联文件信息
            foreach (var item in input.PurchaseListRltFiles)
            {
                purchaseList.PurchaseListRltFiles.Add(new PurchaseListRltFile(_guidGenerator.Create())
                {
                    FileId = item.FileId,
                });
            }
            purchaseList.PurchaseListRltPurchasePlan = new List<PurchaseListRltPurchasePlan>();

            // 保存关联计划信息
            foreach (var item in input.PurchaseListRltPurchasePlan)
            {
                purchaseList.PurchaseListRltPurchasePlan.Add(new PurchaseListRltPurchasePlan(_guidGenerator.Create())
                {
                    PurchasePlanId = item.PurchasePlanId,
                });
            }
            purchaseList.PurchaseListRltMaterials = new List<PurchaseListRltMaterial>();

            // 保存关联物资信息
            foreach (var material in input.PurchaseListRltMaterials)
            {
                purchaseList.PurchaseListRltMaterials.Add(new PurchaseListRltMaterial(_guidGenerator.Create())
                {
                    MaterialId = material.Id,
                    Number = material.Number,
                    Price = material.Price,
                });
            }
            #endregion


            if (input.Submit)
            {
                if (input.WorkflowTemplateId == null || input.WorkflowTemplateId == Guid.Empty) throw new Volo.Abp.UserFriendlyException("计划流程不能为空");
                var workflow = await _singleFlowProcessService.CreateSingleWorkFlow((Guid)input.WorkflowTemplateId);
                purchaseList.WorkflowId = workflow.Id;
                purchaseList.State = PurchaseState.OnReview;// 审核中
            }
            await _purchaseListRepository.InsertAsync(purchaseList);
            return ObjectMapper.Map<PurchaseList, PurchaseListDto>(purchaseList);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PurchaseListDto> Update(PurchaseListUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new Volo.Abp.UserFriendlyException("采购计划名称不能为空");
            if (input.PurchaseListRltMaterials.Count == 0) throw new Volo.Abp.UserFriendlyException("采购材料列表不能为空");
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的数据");
            var purchaseList = await _purchaseListRepository.GetAsync(input.Id);
            if (purchaseList == null) throw new UserFriendlyException("当前数据不存在");
            purchaseList.Name = input.Name;
            purchaseList.PlanTime = input.PlanTime;
            purchaseList.Content = input.Content;
            purchaseList.Type = input.Type;
            purchaseList.State = input.State;
            purchaseList.Submit = input.Submit;
            purchaseList.WorkflowTemplateId = input.WorkflowTemplateId;

            await CheckSameName(input.Name, input.Id);
            #region 关联文件
            // 清除之前关联文件信息
            await _purchaseListRltFileRepository.DeleteAsync(x => x.PurchaseListId == purchaseList.Id);
            purchaseList.PurchaseListRltFiles = new List<PurchaseListRltFile>();

            // 保存关联文件信息
            foreach (var item in input.PurchaseListRltFiles)
            {
                purchaseList.PurchaseListRltFiles.Add(new PurchaseListRltFile(_guidGenerator.Create())
                {
                    FileId = item.FileId,
                });
            }
            // 清除之前关联信息
            await _purchaseListRltPurchasePlanRepository.DeleteAsync(x => x.PurchaseListId == purchaseList.Id);
            purchaseList.PurchaseListRltPurchasePlan = new List<PurchaseListRltPurchasePlan>();

            // 保存关联计划信息
            foreach (var item in input.PurchaseListRltPurchasePlan)
            {
                purchaseList.PurchaseListRltPurchasePlan.Add(new PurchaseListRltPurchasePlan(_guidGenerator.Create())
                {
                    PurchasePlanId = item.PurchasePlanId,
                });
            }
            //清除之前关联物资信息
            await _purchaseRltMaterialsRepository.DeleteAsync(x => x.PurchaseListId == purchaseList.Id);
            purchaseList.PurchaseListRltMaterials = new List<PurchaseListRltMaterial>();
            // 保存关联物资信息
            foreach (var material in input.PurchaseListRltMaterials)
            {
                purchaseList.PurchaseListRltMaterials.Add(new PurchaseListRltMaterial(_guidGenerator.Create())
                {
                    MaterialId = material.Id,
                    Number = material.Number,
                    Price = material.Price,
                });
            }
            #endregion
            if (input.Submit)
            {
                if (input.WorkflowTemplateId == null || input.WorkflowTemplateId == Guid.Empty) throw new Volo.Abp.UserFriendlyException("计划流程不能为空");
                var workflow = await _singleFlowProcessService.CreateSingleWorkFlow((Guid)input.WorkflowTemplateId);
                purchaseList.WorkflowId = workflow.Id;
                purchaseList.State = PurchaseState.OnReview;// 审核中
            }
            await _purchaseListRepository.UpdateAsync(purchaseList);
            return ObjectMapper.Map<PurchaseList, PurchaseListDto>(purchaseList);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            await _purchaseListRepository.DeleteAsync(id);

            return true;
        }
        /// <summary>
        /// 计算用料计划的流程状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<PurchaseList> CalculateMaterialPlanState(List<PurchaseList> list)
        {
            foreach (var item in list)
            {
                if (item.WorkflowId != null)
                {
                    var workflow = _singleFlowProcessService.GetWorkflowById(item.WorkflowId.Value).Result;
                    switch (workflow.State)
                    {
                        case Bpm.WorkflowState.Waiting:
                            item.State = PurchaseState.OnReview;
                            break;
                        case Bpm.WorkflowState.Finished:
                            item.State = PurchaseState.Pass;
                            break;
                        case Bpm.WorkflowState.Rejected:
                            item.State = PurchaseState.UnPass;
                            break;
                        default:
                            item.State = PurchaseState.ToSubmit;
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
                            item.State = PurchaseState.UnPass;
                        }
                    }
                }
            }
            return list;
        }
        #region
        //public async Task<PurchaseDto> Create(PurchaseCreateDto input)
        //{
        //    if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("计划名称不能为空");
        //    if (input.PlanTime == null) throw new UserFriendlyException("用料计划时间不能为空");

        //    await CheckSameName(input.Name);

        //    var purchase = new PurchaseList(_guidGenerator.Create())
        //    {
        //        Code = await GetCode(),
        //        Name = input.Name,
        //        PlanTime = input.PlanTime,
        //        State = input.State,
        //    };

        //    purchase.PurchaseRltMaterials = new List<PurchaseListRltMaterial>();
        //    foreach (var prm in input.PurchaseRltMaterials) //保存关联材料信息
        //    {
        //        purchase.PurchaseRltMaterials.Add(new PurchaseListRltMaterial(_guidGenerator.Create())
        //        {
        //            PurchaseId = purchase.Id,
        //            MaterialId = prm.MaterialId,
        //            Number = prm.Number,
        //            Price = prm.Price,
        //        });
        //    }
        //    //保存功能不会与流程关联
        //    await _purchaseRepository.InsertAsync(purchase);
        //    return await Task.FromResult(ObjectMapper.Map<PurchaseList, PurchaseDto>(purchase));
        //}

        //public Task<PurchaseDto> Get(Guid id)
        //{
        //    if (id == null || Guid.Empty == id) throw new UserFriendlyException("id不能为空");
        //    var purchase = _purchaseRepository.WithDetails().FirstOrDefault(s => s.Id == id);
        //    if (purchase == null) throw new UserFriendlyException("当前采购计划不存在");

        //    var result = ObjectMapper.Map<PurchaseList, PurchaseDto>(purchase);

        //    return Task.FromResult(result);
        //}

        //public async Task<PagedResultDto<PurchaseDto>> GetList(PurchaseSearchDto input)
        //{
        //    var canUpdate = false;
        //    var purchase = _purchaseRepository.WithDetails()
        //        .WhereIf(input.IsInitiate, x => x.CreatorId == CurrentUser.Id)
        //        .WhereIf(input.StartTime != null && input.EndTime != null, x => x.PlanTime >= input.StartTime && x.PlanTime <= input.EndTime);

        //    var purchaseDtos = new List<PurchaseDto>();
        //    var unow = _unitOfWorkManager.Begin(true, false);
        //    foreach (var pur in purchase)
        //    {
        //        var purchaseDto = new PurchaseDto();
        //        purchaseDto = ObjectMapper.Map<PurchaseList, PurchaseDto>(pur);
        //        var workflowStateRltMember = _workflowStateRltMemberRepository.Where(x => x.WorkflowId == pur.WorkflowId && x.MemberId == CurrentUser.Id);
        //        foreach (var item in workflowStateRltMember)
        //        {
        //            if (item.Group == Bpm.UserWorkflowGroup.Waiting)
        //            {
        //                purchaseDto.CanApproval = true;  //判断登录人是否可以审批 状态为审批中的 申请
        //                break;
        //            }
        //        }
        //        var userDto = await _appUserAppService.GetAsync((Guid)pur.CreatorId);
        //        purchaseDto.UserName = userDto.UserName;
        //        purchaseDto.WorkflowId = pur.WorkflowId.GetValueOrDefault();
        //        canUpdate = CanUpdate(purchaseDto.WorkflowId);
        //        if (canUpdate)
        //        {
        //            pur.State = PurchaseState.Pass;
        //            await _purchaseRepository.UpdateAsync(pur);
        //            await unow.SaveChangesAsync();
        //        }
        //        purchaseDtos.Add(purchaseDto);
        //    }

        //    var result = new PagedResultDto<PurchaseDto>
        //    {

        //        TotalCount = purchase.Count(),
        //        Items = purchaseDtos.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList()
        //    };

        //    return await Task.FromResult(result);
        //}

        //public async Task<PurchaseDto> Update(PurchaseUpdateDto input)
        //{
        //    var oldPurchase = _purchaseRepository.FirstOrDefault(s => s.Id == input.Id);
        //    if (oldPurchase == null) throw new UserFriendlyException("当前更新审批不存在");

        //    oldPurchase.Name = input.Name;
        //    oldPurchase.PlanTime = input.PlanTime;

        //    //清除关联物资
        //    await _purchaseRltMaterialsRepository.DeleteAsync(x => x.PurchaseId == oldPurchase.Id);
        //    oldPurchase.PurchaseRltMaterials = new List<PurchaseListRltMaterial>();
        //    foreach (var prm in input.PurchaseRltMaterials) //保存关联材料信息
        //    {
        //        oldPurchase.PurchaseRltMaterials.Add(new PurchaseListRltMaterial(_guidGenerator.Create())
        //        {
        //            PurchaseId = oldPurchase.Id,
        //            MaterialId = prm.MaterialId,
        //            Number = prm.Number,
        //            Price = prm.Price,
        //        });
        //    }

        //    if (input.UpdateState) //编辑时若为提交，则更新状态，且与流程关联
        //    {
        //        oldPurchase.State = input.State;

        //        var purchaseflow = _purchaseFlowTemplateRepository.FirstOrDefault();
        //        if (purchaseflow != null)
        //        {
        //            oldPurchase.WorkflowTemplateId = purchaseflow.WorkflowTemplateId;
        //            // 先创建一个并启动一个计划
        //            var workflow = await _singleFlowProcess.CreateSingleWorkFlow((Guid)oldPurchase.WorkflowTemplateId);
        //            oldPurchase.WorkflowId = workflow.Id;
        //        }
        //        else
        //        {
        //            throw new UserFriendlyException("请先配置流程！");
        //        }
        //    }

        //    await _purchaseRepository.UpdateAsync(oldPurchase);
        //    return await Task.FromResult(ObjectMapper.Map<PurchaseList, PurchaseDto>(oldPurchase));
        //}

        //public async Task<bool> UpdateState(Guid id)
        //{
        //    var oldPurchase = _purchaseRepository.FirstOrDefault(s => s.Id == id);
        //    if (oldPurchase == null) throw new UserFriendlyException("当前采购计划不存在");

        //    var unow = _unitOfWorkManager.Begin(true, false);
        //    var workFlow = _workflowsRepository.FirstOrDefault(x => x.Id == oldPurchase.WorkflowId);
        //    if (workFlow != null)
        //    {
        //        if (workFlow.State == Bpm.WorkflowState.Finished)
        //        {
        //            oldPurchase.State = PurchaseState.Pass;
        //            await _purchaseRepository.UpdateAsync(oldPurchase);
        //            await unow.SaveChangesAsync();
        //        }
        //        else if(workFlow.State == Bpm.WorkflowState.Stopped)
        //        {
        //            oldPurchase.State = PurchaseState.UnPass;
        //            await _purchaseRepository.UpdateAsync(oldPurchase);
        //            await unow.SaveChangesAsync();
        //        }
        //    }
        //    else
        //    {
        //        throw new UserFriendlyException("当前审批流程不存在");
        //    }
        //    return true;

        //}


        //public async Task<bool> Delete(Guid id)
        //{
        //    if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");

        //    await _purchaseRltMaterialsRepository.DeleteAsync(x => x.PurchaseId == id);

        //    await _purchaseRepository.DeleteAsync(id);

        //    return true;
        //}

        //public Task<Stream> Export(EducePurchaseDto input)
        //{
        //    var purchases = _purchaseRepository.WithDetails().Where(x => input.PurchaseIds.Contains(x.Id)).ToList();
        //    var list = purchases.OrderBy(x => x.CreationTime).ToList();
        //    Stream stream = null;
        //    byte[] sbuf;
        //    var dt = (DataTable)null;
        //    var dataColumn = (DataColumn)null;
        //    var dataRow = (DataRow)null;
        //    dt = new DataTable();
        //    //添加表头
        //    var enumValues = Enum.GetValues(typeof(ExportPurchases));
        //    if (enumValues.Length > 0)
        //    {
        //        foreach (int item in enumValues)
        //        {
        //            dataColumn = new DataColumn(Enum.GetName(typeof(ExportPurchases), item));
        //            dt.Columns.Add(dataColumn);
        //        }
        //    }
        //    //添加内容
        //    var count = 1;
        //    foreach (var row in list)
        //    {
        //        foreach (var material in row.PurchaseRltMaterials)
        //        {
        //            dataRow = dt.NewRow();
        //            dataRow[ExportPurchases.序号.ToString()] = count;
        //            dataRow[ExportPurchases.专业.ToString()] = material.Material.Profession.Name;
        //            dataRow[ExportPurchases.名称.ToString()] = material.Material.Name;
        //            dataRow[ExportPurchases.类别.ToString()] = material.Material.Type.Name;
        //            dataRow[ExportPurchases.型号.ToString()] = material.Material.Model;
        //            dataRow[ExportPurchases.规格.ToString()] = material.Material.Spec;
        //            dataRow[ExportPurchases.单位.ToString()] = material.Material.Unit;
        //            dataRow[ExportPurchases.数量.ToString()] = material.Number;
        //            dataRow[ExportPurchases.单价.ToString()] = "￥ " + material.Price;
        //            dataRow[ExportPurchases.合计.ToString()] = "￥ " + (material.Number * material.Price);
        //            dt.Rows.Add(dataRow);
        //            count++;
        //        }
        //    }
        //    sbuf = ExcelHelper.DataTableToExcel(dt, "采购计划表.xlsx", null, "物资采购计划清单", false);
        //    stream = new MemoryStream(sbuf);
        //    return Task.FromResult(stream);
        //    throw new NotImplementedException();
        //}

        //public async Task<List<SingleFlowNodeDto>> GetFlowInfo(Guid workflowTemplateId)
        //{
        //    var workflowTemplate = await _bpmManager.GetWorkflowTemplate(workflowTemplateId);
        //    if (workflowTemplate == null)
        //    {
        //        throw new UserFriendlyException("工作流不存在");
        //    }
        //    var formTemplate = workflowTemplate.FormTemplates.FirstOrDefault();
        //    var flowTemplate = formTemplate?.FlowTemplates.FirstOrDefault(); //获得工作流

        //    var list = new List<SingleFlowNodeDto>();
        //    var guidList = new List<Guid>();
        //    guidList.Add((Guid)workflowTemplate.CreatorId);
        //    flowTemplate.Nodes?.ForEach(a =>
        //    {
        //        var backNode = GetWorkflowNodes(flowTemplate.Id, a.Id).Result;
        //        var dto = new SingleFlowNodeDto();
        //        dto.Id = a.Id;
        //        dto.ParentId = backNode.FirstOrDefault()?.Id;
        //        dto.Name = a.Label;
        //        dto.Type = a.Type;
        //        dto.Active = a.Active;
        //        dto.Approvers = a.Type == "bpmStart" ? GetUsers(guidList).Result : GetUsers(a.Members.Select(b => b.MemberId).ToList()).Result;
        //        list.Add(dto);
        //    });

        //    foreach (var node in list)
        //    {
        //        node.Comments ??= new List<CommentDto>();
        //        node.Approvers?.ForEach(a =>
        //        {
        //            var comment = new CommentDto()
        //            {
        //                User = a,
        //                ApproveTime = flowTemplate?.CreationTime ?? default
        //            };
        //            node.Comments.Add(comment);
        //        });
        //    }

        //    return await _singleFlowProcess.GetNodeTree(list);
        //}


        //public async Task<List<SingleFlowNodeDto>> GetRunFlowInfo(Guid workFlowId, Guid purchaseId)
        //{
        //    return await GetSingleFlowNodes(workFlowId, purchaseId);
        //}


        //public Task<PagedResultDto<GapAnalysisSearchDto>> GetGapAnalysis(PurchaseSearchDto input)
        //{
        //    //查询采购了哪些材料
        //    var purchases = _purchaseRepository.WithDetails()
        //        .WhereIf(input.StartTime != null && input.EndTime != null, x => x.PlanTime >= input.StartTime && x.PlanTime <= input.EndTime);

        //    //查询用了哪些材料
        //    var materials = _usePlanRepository.WithDetails().Where(x => x.Id != null);

        //    var GgapAnalysisDtos = new List<GapAnalysisSearchDto>();

        //    foreach (var purchase in purchases)
        //    {
        //        foreach(var material in purchase.PurchaseRltMaterials)
        //        {
        //            var GgapAnalysisDto = new GapAnalysisSearchDto
        //            {
        //                Type = material.Material.Type.Name,
        //                Name = material.Material.Name,
        //                Model = material.Material.Model,
        //                Spec = material.Material.Spec,
        //                Unit = material.Material.Unit,
        //                PurchaseNum = material.Number
        //            };
        //        }

        //    }

        //    throw new NotImplementedException();
        //}
        #endregion
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Produces("application/octet-stream")]
        [HttpGet]
        public async Task<Stream> Export(Guid id)
        {
            //1、获取需要导出的所有数据
            var purchaseList = _purchaseListRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (purchaseList == null) throw new UserFriendlyException("请刷新页面重新尝试");
            var purchaseListDto = ObjectMapper.Map<PurchaseList, PurchaseListExportsDto>(purchaseList);
            purchaseListDto.Nodes = await GetSingleFlowNodes(purchaseListDto.WorkflowId, purchaseListDto.Id);

            //获取当前登录用户的组织机构
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? _orgRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var workOrganization = organization != null ? organization.Name : null;
            return SavepurchaseListWordFile(purchaseListDto, workOrganization);
        }
        #region   生成导出word文档
        public static Stream SavepurchaseListWordFile(PurchaseListExportsDto Data, string workOrganization)
        {
            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();
            //向文档流中写入内容，生成word
            MemoryStream stream = new MemoryStream();
            string currentDate = DateTime.Now.ToString("yyyy年MM月dd日");
            string checkTime = Data.PlanTime?.ToString("D");//检查时间
            string workFileName = Data.Name;
            string fileName = string.Format("{0}.docx", workFileName, System.Text.Encoding.UTF8);
            var tableWidth = 5000;

            //文本标题
            document.SetDocumentParagraph(0, workFileName, true, 16, "宋体", ParagraphAlignment.CENTER, position: 40);
            //TODO:这里一行需要显示两个文本
            var docString1 = $"编号：{Data.Code}";
            var docString2 = $"                                           计划时间：{checkTime}";
            document.SetDocumentParagraph(1, docString1, true, 10, "宋体", ParagraphAlignment.LEFT, true, $"{docString2}");

            // 创建一个 3行 4列的表格
            document
                .CreateTableAndSetColumnWidth(3, 1000, 1500, 1000, 1500)
                .SetTableWidth(tableWidth)
                .MergeRowCells(0, 1, 3)
                .SetTableParagraph(0, 0, "采购单名称", ParagraphAlignment.CENTER, 35, true)
                .SetTableParagraph(0, 1, $"{Data.Name}", ParagraphAlignment.CENTER, 35, true)
                .SetTableParagraph(1, 0, "提交人", ParagraphAlignment.CENTER, 35, true)
                .SetTableParagraph(1, 1, $"{Data.Creator.Name}", ParagraphAlignment.CENTER, 35, false)
                .SetTableParagraph(1, 2, "计划时间", ParagraphAlignment.CENTER, 50, true)
                .SetTableParagraph(1, 3, $"{ Data.PlanTime?.ToString("D") }", ParagraphAlignment.CENTER, 35, false)
                .MergeRowCells(2, 0, 3)// 合并单元格
                .SetTableParagraph(2, 0, "材料清单", ParagraphAlignment.CENTER, 35, true, 13);
            var rowCount1 = Data.PurchaseListRltMaterials.Count + 2;
            // 创建材料清单的表格
            var headTitles = new string[] { "序号", "材料类别", "材料名称", "规格型号", "计量单位", "需求数量" };
            var twoTable = document
                 .CreateTableAndSetColumnWidth(rowCount1, 250, 800, 1600, 1000, 700, 700)
                .SetTableHeadTitle(ParagraphAlignment.CENTER, 24, true, titles: headTitles)
                .MergeRowCells(rowCount1 - 1, 0, 5)
                .SetTableParagraph(rowCount1 - 1, 0, "审批结果", ParagraphAlignment.CENTER, 50, true, 13);
            var j = 1;
            foreach (var item in Data.PurchaseListRltMaterials)
            {
                twoTable.SetTableParagraph(j, 0, $"{j}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 1, $"{item.Material.Type.Name}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 2, $"{item.Material.Name}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 3, $"{item.Material.Spec}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 4, $"{item.Material.Unit}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 5, $"{item.Number}", ParagraphAlignment.CENTER, 24, false);
                j++;
            }
            var comments = new List<CommentDto>();
            foreach (var com in Data.Nodes)
            {
                if (com.Comments != null && com.Comments.Count > 0)
                {
                    comments.AddRange(com.Comments);
                }
            }
            // 创建第三个表格
            var rowCount2 = comments.Count > 0 ? comments.Count + 1 : comments.Count + 2;
            var threeTableHeadTitle = new string[] { "序号", "审批意见", "审批状态", "审批人", "审批时间" };
            var threeTable = document.CreateTableAndSetColumnWidth(rowCount2, 250, 2450, 1000, 900, 1000)
                .SetTableWidth(tableWidth)
                .SetTableHeadTitle(ParagraphAlignment.CENTER, 24, true, titles: threeTableHeadTitle);
            var i = 1;
            foreach (var item in comments)
            {
                threeTable
                    .SetTableParagraph(i, 0, $"{i}", ParagraphAlignment.CENTER, 80, false)
                    .SetTableParagraph(i, 1, $"{item.Content}", ParagraphAlignment.CENTER, 80, false)
                    .SetTableParagraph(i, 2, item.State == WorkflowState.Stopped ? "审核通过" : item.State == WorkflowState.Rejected ? "审核未通过" : "", ParagraphAlignment.CENTER, 80, false)
                    .SetTableParagraph(i, 3, $"{item.User.Name}", ParagraphAlignment.CENTER, 80, false)
                    .SetTableParagraph(i, 4, $"{item.ApproveTime.ToString("F")}", ParagraphAlignment.CENTER, 80, false);
                i++;
            }
            document.
                SetDocumentMargin("1000", "1500", "1000", "1000")
                .Write(stream);
            var buf = stream.ToArray();
            stream = new MemoryStream(buf);
            return stream;
        }

        public async Task<List<SingleFlowNodeDto>> GetSingleFlowNodes(Guid? workflowId, Guid purchasePlanId)
        {
            var infos = _purchaseListRltFlowRepository.Where(a => a.PurchaseListId == purchasePlanId && a.WorkFlowId == workflowId).ToList();
            var nodes = (await _singleFlowProcess.GetWorkFlowNodes((Guid)workflowId)).Where(x => x.Type == "bpmApprove").ToList();
            foreach (var node in nodes)
            {
                node.Comments ??= new List<CommentDto>();
                node.Approvers?.ForEach(a =>
                {
                    var info = infos.FirstOrDefault(b => b.CreatorId == a.Id);
                    if (info != null)
                    {
                        var comment = new CommentDto()
                        {
                            User = a,
                            Content = info.Content,
                            ApproveTime = info?.CreationTime ?? default,
                            State = info.State
                        };
                        node.Comments.Add(comment);
                    }
                });
            }
            return nodes;
        }

        #endregion
        #region 私有方法

        private bool CanUpdate(Guid id)
        {
            var unow = _unitOfWorkManager.Begin(true, false);
            var workFlow = _workflowsRepository.FirstOrDefault(x => x.Id == id);
            if (workFlow != null)
            {
                if (workFlow.State == Bpm.WorkflowState.Finished)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        #region
        //public async Task<List<SingleFlowNodeDto>> GetSingleFlowNodes(Guid workflowId, Guid purchaseId)
        //{
        //    SingleFlowNodeDto endNode;
        //    DateTime? endTime = null;
        //    var nodes = await _singleFlowProcess.GetWorkFlowNodes(workflowId);
        //    var purchase = _purchaseListRepository.FirstOrDefault(x => x.Id == purchaseId);
        //    //获取workflowData中的节点审批状态及审批意见
        //    var infos = _workflowDataRepository.Where(x => x.WorkflowId == workflowId).ToList();

        //    WorkflowData info;
        //    foreach (var node in nodes)
        //    {
        //        if (node.Type == "bpmStart")
        //        {
        //            info = infos.FirstOrDefault(b => b.TargetNodeId == null || b.TargetNodeId == Guid.Empty);
        //        }
        //        else
        //        {
        //            info = infos.FirstOrDefault(b => b.TargetNodeId == node.Id); //获取 除开始节点外，其余节点对应的信息
        //        }
        //        //获取结束时间：
        //        if (node.Type == "bpmEnd" && purchase.State == PurchaseState.Pass)
        //        {
        //            endNode = nodes.FirstOrDefault(x => x.Type == "bpmEnd");
        //            var data = infos.FirstOrDefault(x => x.TargetNodeId == endNode.ParentId);  //前提是结束节点前只能有一个抄送节点指向他，否则，结束时间不准确
        //            if (data != null)
        //            {
        //                node.Comments ??= new List<CommentDto>();
        //                endTime = data.CreationTime;
        //                var comment = new CommentDto()
        //                {
        //                    ApproveTime = (DateTime)(endTime),
        //                };
        //                node.Comments.Add(comment);
        //            }
        //        }
        //        node.Comments ??= new List<CommentDto>();
        //        node.Approvers?.ForEach(a =>
        //        {
        //            var comment = new CommentDto()
        //            {
        //                User = a,

        //                ApproveTime = (DateTime)(endTime != null ? endTime : info?.CreationTime ?? default),
        //            };
        //            node.Comments.Add(comment);
        //        });
        //    }
        //    return await _singleFlowProcess.GetNodeTree(nodes);
        //}
        #endregion
        private async Task<List<FlowTemplateNode>> GetWorkflowNodes(Guid flowTemplateId, Guid nodeId, bool backNode = true)
        {
            var nodes = _flowTemplateNodeRepository.Where(x => x.FlowTemplateId == flowTemplateId).ToList();
            var steps = _flowTemplateStepRepository.Where(x => x.FlowTemplateId == flowTemplateId).ToList();
            var target = nodes.SingleOrDefault(a => a.Id == nodeId);
            var pnode = backNode ? _bpmManager.GetFlowBackNodes(nodes, steps, target) : _bpmManager.GetFlowNextNodes(nodes, steps, target);
            return pnode;
        }

        private async Task<List<IdentityUserDto>> GetUsers(IReadOnlyList<Guid> ids)
        {
            var user = await _identityUser.GetUserListAsync(a => ids.Contains(a.Id));
            return ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(user);
        }

        private async Task<bool> CheckSameName(string name, Guid? id)
        {
            return await Task.Run(() =>
            {
                var sameNames = _purchaseListRepository.WhereIf(id != Guid.Empty && id != null, x => x.Id != id)
                .FirstOrDefault(a => a.Name == name);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("当前已存在该名称的采购计划名称！");
                }

                return true;
            });
        }

        public async Task<string> GetCode()
        {
            var code = "";
            var nowDate = DateTime.Now.ToString("yyyy-MM-dd");
            var purchase = _purchaseListRepository.Where(x => x.Code.Substring(3, 8) == nowDate.Replace("-", ""));
            if (purchase.Count() == 0)
            {
                code = "CG-" + nowDate.Replace("-", "") + "001";
            }
            else
            {
                purchase = purchase.OrderByDescending(x => x.CreationTime); //按照时间降序后的第一个
                var maxCode = purchase.FirstOrDefault().Code;
                var number = Convert.ToInt32(maxCode.Substring(12, maxCode.Length - 12)) + 1;
                if (number < 100)
                {
                    string trueNum = string.Format("{0:d3}", number);
                    code = "CG-" + nowDate.Replace("-", "") + trueNum;
                }
                else
                {
                    code = "CG-" + nowDate.Replace("-", "") + number;
                }
            }
            return code;
        }



        #endregion
    }
}
