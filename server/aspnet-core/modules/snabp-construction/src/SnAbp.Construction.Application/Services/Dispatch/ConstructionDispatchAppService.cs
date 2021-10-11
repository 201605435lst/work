using NPOI.XWPF.UserModel;
using SnAbp.Bpm;
using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.IServices;
using SnAbp.Bpm.Services;
using SnAbp.Construction.Dtos;
using SnAbp.Construction.Entities;
using SnAbp.Construction.Enums;
using SnAbp.Construction.IServices;
using SnAbp.Technology;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Construction.Services
{
    public class ConstructionDispatchAppService : ConstructionAppService, IConstructionDispatchAppService
    {
        private readonly IRepository<Dispatch, Guid> _dispatchRepository;
        private readonly IRepository<Daily, Guid> _dailyRepository;
        private readonly IRepository<DispatchRltFile, Guid> _dispatchRltFileRepository;
        private readonly IRepository<DispatchRltMaterial, Guid> _dispatchRltMaterialRepository;
        private readonly IRepository<DispatchRltPlanContent, Guid> _dispatchRltPlanContentRepository;
        private readonly IRepository<DispatchRltSection, Guid> _dispatchRltSectionRepository;
        private readonly IRepository<DispatchRltWorker, Guid> _dispatchRltWorkerRepository;
        private readonly IRepository<DispatchRltStandard, Guid> _dispatchRltStandardRepository;
        private readonly IRepository<DispatchRltWorkFlow, Guid> _dispatchRltWorkFlowRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ISingleFlowProcessService _singleFlowProcessService;
        readonly IUnitOfWorkManager _unitOfWork;
        readonly SingleFlowProcessService _singleFlowProcess;
        public ConstructionDispatchAppService(
            IRepository<Dispatch, Guid> dispatchRepository,
            IRepository<Daily, Guid> dailyRepository,
            IRepository<DispatchRltFile, Guid> dispatchRltFileRepository,
            IRepository<DispatchRltMaterial, Guid> dispatchRltMaterialRepository,
            IRepository<DispatchRltPlanContent, Guid> dispatchRltPlanContentRepository,
            IRepository<DispatchRltSection, Guid> dispatchRltSectionRepository,
            IRepository<DispatchRltWorker, Guid> dispatchRltWorkerRepository,
            IRepository<DispatchRltStandard, Guid> dispatchRltStandardRepository,
            IRepository<DispatchRltWorkFlow, Guid> dispatchRltWorkFlowRepository,
            IGuidGenerator guidGenerator,
            SingleFlowProcessService singleFlowProcess,
            IUnitOfWorkManager unitOfWork,
            ISingleFlowProcessService singleFlowProcessService
            )
        {
            _dispatchRepository = dispatchRepository;
            _dailyRepository = dailyRepository;
            _dispatchRltFileRepository = dispatchRltFileRepository;
            _dispatchRltPlanContentRepository = dispatchRltPlanContentRepository;
            _dispatchRltMaterialRepository = dispatchRltMaterialRepository;
            _dispatchRltSectionRepository = dispatchRltSectionRepository;
            _dispatchRltWorkerRepository = dispatchRltWorkerRepository;
            _dispatchRltStandardRepository = dispatchRltStandardRepository;
            _dispatchRltWorkFlowRepository = dispatchRltWorkFlowRepository;
            _guidGenerator = guidGenerator;
            _singleFlowProcess = singleFlowProcess;
            _unitOfWork = unitOfWork;
            _singleFlowProcessService = singleFlowProcessService;
        }

        /// <summary>
        /// 详情获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DispatchDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var dispatch = _dispatchRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (dispatch == null)
            {
                throw new UserFriendlyException("该派工不存在");
            }
            var dispatchRltPlanContents = _dispatchRltPlanContentRepository.WithDetails().Where(x => x.DispatchId == dispatch.Id).ToList();
            if (dispatchRltPlanContents.Count > 0)
            {
                dispatch.DispatchRltPlanContents = dispatchRltPlanContents;

            }
            var materials = _dispatchRltMaterialRepository.WithDetails().Where(x => x.DispatchId == dispatch.Id).ToList();
            if (materials.Count > 0)
            {
                dispatch.DispatchRltMaterials = materials;

            }
            var workers = _dispatchRltWorkerRepository.WithDetails().Where(x => x.DispatchId == dispatch.Id).ToList();
            if (workers.Count > 0)
            {
                dispatch.DispatchRltWorkers = workers;

            }
            var sections = _dispatchRltSectionRepository.WithDetails().Where(x => x.DispatchId == dispatch.Id).ToList();
            if (sections.Count > 0)
            {
                dispatch.DispatchRltSections = sections;

            }
            var standards = _dispatchRltStandardRepository.WithDetails().Where(x => x.DispatchId == dispatch.Id).ToList();
            if (standards.Count > 0)
            {
                dispatch.DispatchRltStandards = standards;

            }
            var dispatchDto = ObjectMapper.Map<Dispatch, DispatchDto>(dispatch);
            if (dispatch.State != DispatchState.UnSubmit)
            {
                dispatchDto.WorkFlowNodes = await GetSingleFlowNodes(dispatch.WorkflowId, dispatch.Id);
            }
            return dispatchDto;
        }

        /// <summary>
        /// 数据列表获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DispatchDto>> GetList(DispatchSearchDto input)
        {
            PagedResultDto<DispatchDto> result = new PagedResultDto<DispatchDto>();
            IQueryable<Dispatch> query = _dispatchRepository.WithDetails(x => x.Contractor, x => x.Creator)
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Code.Contains(input.Keyword) || x.Profession.Contains(input.Keyword)) // 模糊查询
                .WhereIf(input.StartTime != null && input.EndTime != null, x => x.Time >= input.StartTime && x.Time <= input.EndTime);

            var usedDispatchIds = _dailyRepository.Where(x => x.DispatchId != null).Select(y => y.DispatchId).ToList();

            var list = new List<Dispatch>();

            if (input.Approval) // 如果是审批模式的话
            {
                if (input.Waiting) //待我审批的数据 
                {
                    foreach (var item in query)
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
                        list = query.Where(a => workflowIds.Contains(a.WorkflowId))
                            .ToList();
                    }
                }
            }
            else
            {
                // 根据 onlyPass 查询全部或者 只有审批通过的数据 
                list = input.Passed
                    ? input.IsForDaily
                    ? query.Where(x => x.Workflow.State == WorkflowState.Finished && !usedDispatchIds.Contains(x.Id)).OrderBy(x => x.State).ToList()
                    : query.Where(x => x.Workflow.State == WorkflowState.Finished).OrderBy(x => x.State).ToList()
                    : query.OrderBy(x => x.State).ToList();
            }
            result.TotalCount = list.Count();
            result.Items = input.IsAll ?
                CalculateMasterPlanState(list.OrderByDescending(x => x.Time).ToList()) :
                CalculateMasterPlanState(list.OrderByDescending(x => x.Time).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return result;
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(DispatchCreateDto input)
        {
            if (input.DispatchRltPlanContents.Count == 0) throw new UserFriendlyException("施工内容不能为空");
            if (input.DispatchRltWorkers.Count == 0) throw new UserFriendlyException("施工员不能为空");
            if (input.Time == null) throw new UserFriendlyException("派工日期不能为空");
            var code = "PG-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace("-", "");
            var dispatch = new Dispatch(_guidGenerator.Create())
            {
                DispatchTemplateId = input.DispatchTemplateId,
                Code = code,
                Name = input.Name,
                Time = input.Time,
                Profession = input.Profession,
                ContractorId = input.ContractorId,
                Number = input.Number,
                Team = input.Team,
                ExtraDescription = input.ExtraDescription,
                IsNeedLargeEquipment = input.IsNeedLargeEquipment,
                LargeEquipment = input.LargeEquipment,
                IsDismantle = input.IsDismantle,
                IsHighWork = input.IsHighWork,
                Process = input.Process,
                RiskSources = input.RiskSources,
                RecoveryTime = input.RecoveryTime,
                SafetyMeasure = input.SafetyMeasure,
                ControlType = input.ControlType,
                WorkflowId = input.WorkflowId,
                WorkflowTemplateId = input.WorkflowTemplateId,
                Remark = input.Remark,
                State = DispatchState.UnSubmit,
            };

            dispatch.DispatchRltFiles = new List<DispatchRltFile>();

            // 重新保存关联文件信息
            foreach (var file in input.DispatchRltFiles)
            {
                dispatch.DispatchRltFiles.Add(new DispatchRltFile(_guidGenerator.Create())
                {
                    FileId = file.FileId,
                    DispatchId = dispatch.Id,
                });
            }

            dispatch.DispatchRltMaterials = new List<DispatchRltMaterial>();

            // 重新保存关联材料信息
            foreach (var material in input.DispatchRltMaterials)
            {
                dispatch.DispatchRltMaterials.Add(new DispatchRltMaterial(_guidGenerator.Create())
                {
                    MaterialId = material.MaterialId,
                    DispatchId = dispatch.Id,
                    Count = material.Count,
                });
            }

            dispatch.DispatchRltPlanContents = new List<DispatchRltPlanContent>();

            // 重新保存关联计划内容信息
            foreach (var planContent in input.DispatchRltPlanContents)
            {
                dispatch.DispatchRltPlanContents.Add(new DispatchRltPlanContent(_guidGenerator.Create())
                {
                    PlanContentId = planContent.PlanContentId,
                    DispatchId = dispatch.Id,
                });
            }

            dispatch.DispatchRltSections = new List<DispatchRltSection>();

            // 重新保存关联计划内容信息
            foreach (var section in input.DispatchRltSections)
            {
                dispatch.DispatchRltSections.Add(new DispatchRltSection(_guidGenerator.Create())
                {
                    SectionId = section.SectionId,
                    DispatchId = dispatch.Id,
                });
            }

            dispatch.DispatchRltStandards = new List<DispatchRltStandard>();

            // 重新保存关联计划内容信息
            foreach (var standard in input.DispatchRltStandards)
            {
                dispatch.DispatchRltStandards.Add(new DispatchRltStandard(_guidGenerator.Create())
                {
                    StandardId = standard.StandardId,
                    DispatchId = dispatch.Id,
                });
            }

            dispatch.DispatchRltWorkers = new List<DispatchRltWorker>();

            // 重新保存关联计划内容信息
            foreach (var Worker in input.DispatchRltWorkers)
            {
                dispatch.DispatchRltWorkers.Add(new DispatchRltWorker(_guidGenerator.Create())
                {
                    WorkerId = Worker.WorkerId,
                    DispatchId = dispatch.Id,
                });
            }

            await _dispatchRepository.InsertAsync(dispatch);


            return default;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Update(DispatchUpdateDto input)
        {
            var dispatch = _dispatchRepository.FirstOrDefault(x => x.Id == input.Id);
            if (dispatch == null) throw new UserFriendlyException("当前更新派工单不存在");
            if (input.DispatchRltPlanContents.Count == 0) throw new UserFriendlyException("施工内容不能为空");
            if (input.DispatchRltWorkers.Count == 0) throw new UserFriendlyException("施工员不能为空");
            if (input.Time == null) throw new UserFriendlyException("派工日期不能为空");

            dispatch.DispatchTemplateId = input.DispatchTemplateId;
            dispatch.Name = input.Name;
            dispatch.Time = input.Time;
            dispatch.Profession = input.Profession;
            dispatch.ContractorId = input.ContractorId;
            dispatch.Number = input.Number;
            dispatch.Team = input.Team;
            dispatch.ExtraDescription = input.ExtraDescription;
            dispatch.IsNeedLargeEquipment = input.IsNeedLargeEquipment;
            dispatch.LargeEquipment = input.LargeEquipment;
            dispatch.IsDismantle = input.IsDismantle;
            dispatch.IsHighWork = input.IsHighWork;
            dispatch.Process = input.Process;
            dispatch.RiskSources = input.RiskSources;
            dispatch.RecoveryTime = input.RecoveryTime;
            dispatch.SafetyMeasure = input.SafetyMeasure;
            dispatch.ControlType = input.ControlType;
            dispatch.WorkflowId = input.WorkflowId;
            dispatch.WorkflowTemplateId = input.WorkflowTemplateId;
            dispatch.State = DispatchState.UnSubmit;
            dispatch.Remark = input.Remark;

            // 清楚之前关联信息
            await _dispatchRltFileRepository.DeleteAsync(x => x.DispatchId == dispatch.Id);
            dispatch.DispatchRltFiles = new List<DispatchRltFile>();

            // 重新保存关联文件信息
            foreach (var file in input.DispatchRltFiles)
            {
                dispatch.DispatchRltFiles.Add(new DispatchRltFile(_guidGenerator.Create())
                {
                    FileId = file.FileId,
                    DispatchId = dispatch.Id,
                });
            }

            // 清楚之前关联信息
            await _dispatchRltMaterialRepository.DeleteAsync(x => x.DispatchId == dispatch.Id);
            dispatch.DispatchRltMaterials = new List<DispatchRltMaterial>();

            // 重新保存关联材料信息
            foreach (var material in input.DispatchRltMaterials)
            {
                dispatch.DispatchRltMaterials.Add(new DispatchRltMaterial(_guidGenerator.Create())
                {
                    MaterialId = material.MaterialId,
                    DispatchId = dispatch.Id,
                    Count = material.Count,
                });
            }

            // 清楚之前关联信息
            await _dispatchRltPlanContentRepository.DeleteAsync(x => x.DispatchId == dispatch.Id);
            dispatch.DispatchRltPlanContents = new List<DispatchRltPlanContent>();

            // 重新保存关联计划内容信息
            foreach (var planContent in input.DispatchRltPlanContents)
            {
                dispatch.DispatchRltPlanContents.Add(new DispatchRltPlanContent(_guidGenerator.Create())
                {
                    PlanContentId = planContent.PlanContentId,
                    DispatchId = dispatch.Id,
                });
            }

            // 清楚之前关联信息
            await _dispatchRltSectionRepository.DeleteAsync(x => x.DispatchId == dispatch.Id);
            dispatch.DispatchRltSections = new List<DispatchRltSection>();

            // 重新保存关联计划内容信息
            foreach (var section in input.DispatchRltSections)
            {
                dispatch.DispatchRltSections.Add(new DispatchRltSection(_guidGenerator.Create())
                {
                    SectionId = section.SectionId,
                    DispatchId = dispatch.Id,
                });
            }

            // 清楚之前关联信息
            await _dispatchRltStandardRepository.DeleteAsync(x => x.DispatchId == dispatch.Id);
            dispatch.DispatchRltStandards = new List<DispatchRltStandard>();

            // 重新保存关联计划内容信息
            foreach (var standard in input.DispatchRltStandards)
            {
                dispatch.DispatchRltStandards.Add(new DispatchRltStandard(_guidGenerator.Create())
                {
                    StandardId = standard.StandardId,
                    DispatchId = dispatch.Id,
                });
            }

            // 清楚之前关联信息
            await _dispatchRltWorkerRepository.DeleteAsync(x => x.DispatchId == dispatch.Id);
            dispatch.DispatchRltWorkers = new List<DispatchRltWorker>();

            // 重新保存关联计划内容信息
            foreach (var Worker in input.DispatchRltWorkers)
            {
                dispatch.DispatchRltWorkers.Add(new DispatchRltWorker(_guidGenerator.Create())
                {
                    WorkerId = Worker.WorkerId,
                    DispatchId = dispatch.Id,
                });
            }

            await _dispatchRepository.UpdateAsync(dispatch);


            return default;
        }

        /// <summary>
		/// 提交审批
		/// </summary>
		/// <param name="id">派工单id</param>
		/// <param name="workFlowId">审批流程id </param>
		/// <returns></returns>
		public async Task<bool> ForSubmit(Guid id, Guid workFlowId)
        {
            Workflow workflow = await _singleFlowProcessService.CreateSingleWorkFlow(workFlowId);
            Dispatch dispatch = await _dispatchRepository.GetAsync(id);
            dispatch.WorkflowTemplateId = workFlowId;
            dispatch.WorkflowId = workflow.Id;
            dispatch.State = DispatchState.OnReview;// 审核中
            await _dispatchRepository.UpdateAsync(dispatch);
            return true;
        }

        /// <summary>
		/// 审批流程操作
		/// </summary>
		/// <returns></returns>
		public async Task<bool> Process(DispatchProcessDto input)
        {
            var dispatch = await _dispatchRepository.GetAsync(input.DispatchId);
            var workflowId = dispatch.WorkflowId.GetValueOrDefault();
            Bpm.Dtos.WorkflowDetailDto dto;
            using (var uow = _unitOfWork.Begin(true, false))
            {
                if (input.State == DispatchState.Pass)
                {
                    dto = await _singleFlowProcessService.Approved(workflowId, input.Content, CurrentUser.Id);
                }
                else if (input.State == DispatchState.UnPass)
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
                case WorkflowState.Waiting:
                    dispatch.State = DispatchState.OnReview;
                    break;
                case WorkflowState.Finished:
                    dispatch.State = DispatchState.Pass;
                    break;
                case WorkflowState.Rejected:
                    dispatch.State = DispatchState.UnPass;
                    break;
                default:
                    break;
            }
            if (workflow.WorkflowDatas.Any())
            {
                var data = workflow.WorkflowDatas.Where(a => a.StepState != null)
                    .OrderByDescending(a => a.CreationTime)
                    .FirstOrDefault();
                if (data != null && (data.StepState == WorkflowStepState.Stopped || data.StepState == WorkflowStepState.Rejected))
                {
                    dispatch.State = DispatchState.UnPass;
                }
            }
            await _dispatchRepository.UpdateAsync(dispatch);

            DispatchRltWorkFlow dispatchRltWorkFlow = new DispatchRltWorkFlow(GuidGenerator.Create())
            {
                Content = input.Content,
                DispatchId = dispatch.Id,
                WorkFlowId = workflowId,
                State = workflow.State
            };
            await _dispatchRltWorkFlowRepository.InsertAsync(dispatchRltWorkFlow);
            return true;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stream> Export(Guid id)
        {
            var dispatch = await Get(id);

            var WorkFlowNodes = new List<CommentDto>();

            string fileName = dispatch.DispatchTemplate?.Name;

            //施工区段名称
            var sectionName = "";
            if (dispatch.DispatchRltSections.Count > 0)
            {
                foreach (var item in dispatch.DispatchRltSections)
                {
                    if (string.IsNullOrEmpty(sectionName))
                    {
                        sectionName = item.Section?.Name;
                    }
                    else
                    {
                        sectionName = sectionName + "、" + item.Section?.Name;
                    }
                }
            }

            //施工员名称
            var workerName = "";
            if (dispatch.DispatchRltWorkers.Count > 0)
            {
                foreach (var item in dispatch.DispatchRltWorkers)
                {
                    if (string.IsNullOrEmpty(workerName))
                    {
                        workerName = item.Worker?.Name;
                    }
                    else
                    {
                        workerName = workerName + "、" + item.Worker?.Name;
                    }
                }

            }

            //工序指引名称
            var standardName = "";
            if (dispatch.DispatchRltStandards.Count > 0)
            {
                foreach (var item in dispatch.DispatchRltStandards)
                {
                    if (string.IsNullOrEmpty(standardName))
                    {
                        standardName = item.Standard?.Name;
                    }
                    else
                    {
                        standardName = standardName + "、" + item.Standard?.Name;
                    }
                }
            }

            //安全防护措施
            var safetyMeasureName = "";
            if (!string.IsNullOrEmpty(dispatch.SafetyMeasure))
            {
                var array = dispatch.SafetyMeasure.Split(",");
                if (array.Length > 0)
                {
                    foreach (var item in array)
                    {
                        if (string.IsNullOrEmpty(safetyMeasureName))
                        {
                            safetyMeasureName = GetSafetyMeasure(item);
                        }
                        else
                        {
                            safetyMeasureName = safetyMeasureName + "、" + GetSafetyMeasure(item);
                        }
                    }
                }

            }

            //工序控制类型
            var controlTypeName = "";
            if (!string.IsNullOrEmpty(dispatch.ControlType))
            {
                var array = dispatch.ControlType.Split(",");
                if (array.Length > 0)
                {
                    foreach (var item in array)
                    {
                        if (string.IsNullOrEmpty(controlTypeName))
                        {
                            controlTypeName = GetControlType(item);
                        }
                        else
                        {
                            controlTypeName = controlTypeName + "、" + GetControlType(item);
                        }
                    }
                }

            }

            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, fileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", false, 11, "宋体", ParagraphAlignment.CENTER), 1);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"派单日期：{dispatch.Time}", false, 11, "宋体", ParagraphAlignment.RIGHT), 2);

            #region 文档第一个表格对象实例(基础信息)
            //创建文档中的表格对象实例
            XWPFTable basicInfo = document.CreateTable(7, 4);//显示的行列数rows:3行,cols:4列
            basicInfo.Width = 5600;//总宽度
            basicInfo.SetColumnWidth(0, 1000); /* 设置列宽 */
            basicInfo.SetColumnWidth(1, 1800); /* 设置列宽 */
            basicInfo.SetColumnWidth(2, 1000); /* 设置列宽 */
            basicInfo.SetColumnWidth(3, 1800); /* 设置列宽 */

            //Table 表格第一行展示...后面的都是一样，只改变GetRow中的行数
            basicInfo.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "派工单编号", ParagraphAlignment.CENTER, 24, true, 11));
            basicInfo.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{dispatch.Code}", ParagraphAlignment.CENTER, 24, false, 11));
            basicInfo.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "施工专业", ParagraphAlignment.CENTER, 24, true, 11));
            basicInfo.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{dispatch.Profession}", ParagraphAlignment.CENTER, 24, false, 11));

            //Table 表格第二行
            basicInfo.GetRow(1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "施工班组", ParagraphAlignment.CENTER, 24, true, 11));
            basicInfo.GetRow(1).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{dispatch.Team}", ParagraphAlignment.CENTER, 24, false, 11));
            basicInfo.GetRow(1).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "承包商", ParagraphAlignment.CENTER, 24, true, 11));
            basicInfo.GetRow(1).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{dispatch.Contractor?.Name}", ParagraphAlignment.CENTER, 24, false, 11));


            //Table 表格第三行
            basicInfo.GetRow(2).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "施工员", ParagraphAlignment.CENTER, 24, true, 11));
            basicInfo.GetRow(2).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{workerName}", ParagraphAlignment.CENTER, 24, false, 11));
            basicInfo.GetRow(2).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "施工人数", ParagraphAlignment.CENTER, 24, true, 11));
            basicInfo.GetRow(2).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{dispatch.Number}", ParagraphAlignment.CENTER, 24, false, 11));

            basicInfo.GetRow(3).MergeCells(1, 3);//合并2列
            basicInfo.GetRow(3).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "施工区段", ParagraphAlignment.CENTER, 24, true, 11));
            basicInfo.GetRow(3).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{sectionName}", ParagraphAlignment.CENTER, 24, false, 11));

            basicInfo.GetRow(4).MergeCells(1, 3);//合并2列
            basicInfo.GetRow(4).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "工序指引", ParagraphAlignment.CENTER, 10, true, 11));
            basicInfo.GetRow(4).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{standardName}", ParagraphAlignment.CENTER, 10, false, 11));

            basicInfo.GetRow(5).MergeCells(0, 3);//合并3列
            basicInfo.GetRow(5).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "", ParagraphAlignment.CENTER, 10, true, 11));


            basicInfo.GetRow(6).MergeCells(0, 3);//合并3列
            basicInfo.GetRow(6).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "施工任务", ParagraphAlignment.CENTER, 10, true, 11));
            #endregion

            #region 文档第二个表格对象实例（施工任务）
            //创建文档中的表格对象实例
            XWPFTable planContents = document.CreateTable(dispatch.DispatchRltPlanContents.Count + 3, 3);//显示的行列数rows:8行,cols:7列
            planContents.Width = 5600;//总宽度
            planContents.SetColumnWidth(0, 1000); /* 设置列宽 */
            planContents.SetColumnWidth(1, 2000); /* 设置列宽 */
            planContents.SetColumnWidth(2, 2600); /* 设置列宽 */

            //遍历表格标题
            planContents.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, planContents, "序号", ParagraphAlignment.CENTER, 22, true, 11));
            planContents.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, planContents, "任务名称", ParagraphAlignment.CENTER, 22, true, 11));
            planContents.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, planContents, "工作内容", ParagraphAlignment.CENTER, 22, true, 11));
            var i = 1;
            //遍历数据
            foreach (var item in dispatch.DispatchRltPlanContents)
            {
                planContents.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, planContents, $"{i}", ParagraphAlignment.CENTER, 22, false, 11));
                planContents.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, planContents, $"{item.PlanContent?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                planContents.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, planContents, $"{item.PlanContent?.Content}", ParagraphAlignment.CENTER, 22, false, 11));
                i++;
            }

            planContents.GetRow(dispatch.DispatchRltPlanContents.Count + 1).MergeCells(0, 2);//合并3列
            planContents.GetRow(dispatch.DispatchRltPlanContents.Count + 1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, planContents, "", ParagraphAlignment.CENTER, 10, true, 11));

            planContents.GetRow(dispatch.DispatchRltPlanContents.Count + 2).MergeCells(0, 2);//合并3列
            planContents.GetRow(dispatch.DispatchRltPlanContents.Count + 2).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, planContents, "待安装设备", ParagraphAlignment.CENTER, 10, true, 11));

            #endregion

            var equipments = new List<DispatchRltEquipmentDto>();
            if (dispatch.DispatchRltPlanContents.Count > 0)
            {
                foreach (var item in dispatch?.DispatchRltPlanContents)
                {
                    if (item.PlanContent != null && item.PlanContent.PlanMaterials != null && item.PlanContent.PlanMaterials.Count > 0)
                    {
                        foreach (var _item in item.PlanContent?.PlanMaterials)
                        {
                            if (_item.PlanMaterialRltEquipments.Count > 0)
                            {
                                var equipmentDto = new DispatchRltEquipmentDto()
                                {
                                    Id = _item.Id,
                                    ComponentCategoryId = _item.PlanMaterialRltEquipments[0]?.Equipment?.ComponentCategoryId,
                                    Name = _item.PlanMaterialRltEquipments[0]?.Equipment?.Name,
                                    Spec = _item.Spec,
                                    Unit = _item.Unit,
                                    Quantity = (decimal)_item.Quantity,
                                };
                                equipments.Add(equipmentDto);
                            }
                        }
                    }
                }
            }

            #region 文档第三个表格对象实例（待安装设备）
            //创建文档中的表格对象实例
            XWPFTable equipmentsTable = document.CreateTable(equipments.Count + 3, 5);//显示的行列数rows:8行,cols:7列
            equipmentsTable.Width = 5600;//总宽度
            equipmentsTable.SetColumnWidth(0, 600); /* 设置列宽 */
            equipmentsTable.SetColumnWidth(1, 1500); /* 设置列宽 */
            equipmentsTable.SetColumnWidth(2, 1500); /* 设置列宽 */
            equipmentsTable.SetColumnWidth(3, 1000); /* 设置列宽 */
            equipmentsTable.SetColumnWidth(4, 1000); /* 设置列宽 */

            //遍历表格标题
            equipmentsTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, "序号", ParagraphAlignment.CENTER, 22, true, 11));
            equipmentsTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, "设备名称", ParagraphAlignment.CENTER, 22, true, 11));
            equipmentsTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, "规格型号", ParagraphAlignment.CENTER, 22, true, 11));
            equipmentsTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, "计量单位", ParagraphAlignment.CENTER, 22, true, 11));
            equipmentsTable.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, "工程数量", ParagraphAlignment.CENTER, 22, true, 11));

            var j = 1;
            foreach (var item in equipments)
            {
                equipmentsTable.GetRow(j).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, $"{j}", ParagraphAlignment.CENTER, 22, false, 11));
                equipmentsTable.GetRow(j).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, $"{item.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                equipmentsTable.GetRow(j).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, $"{item.Spec}", ParagraphAlignment.CENTER, 22, false, 11));
                equipmentsTable.GetRow(j).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, $"{item.Unit}", ParagraphAlignment.CENTER, 22, false, 11));
                equipmentsTable.GetRow(j).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, $"{item.Quantity}", ParagraphAlignment.CENTER, 22, false, 11));
                j++;
            }

            equipmentsTable.GetRow(equipments.Count + 1).MergeCells(0, 4);//合并4列
            equipmentsTable.GetRow(equipments.Count + 1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, "", ParagraphAlignment.CENTER, 10, true, 11));


            equipmentsTable.GetRow(equipments.Count + 2).MergeCells(0, 4);//合并4列
            equipmentsTable.GetRow(equipments.Count + 2).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, equipmentsTable, "待施工材料", ParagraphAlignment.CENTER, 10, true, 11));

            #endregion

            #region 文档第四个表格对象实例（待施工材料）
            //创建文档中的表格对象实例
            XWPFTable materials = document.CreateTable(dispatch.DispatchRltMaterials.Count + 2, 5);//显示的行列数rows:8行,cols:7列
            materials.Width = 5600;//总宽度
            materials.SetColumnWidth(0, 600); /* 设置列宽 */
            materials.SetColumnWidth(1, 1500); /* 设置列宽 */
            materials.SetColumnWidth(2, 1500); /* 设置列宽 */
            materials.SetColumnWidth(3, 1000); /* 设置列宽 */
            materials.SetColumnWidth(4, 1000); /* 设置列宽 */

            //遍历表格标题
            materials.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, "序号", ParagraphAlignment.CENTER, 22, true, 11));
            materials.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, "材料名称", ParagraphAlignment.CENTER, 22, true, 11));
            materials.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, "规格型号", ParagraphAlignment.CENTER, 22, true, 11));
            materials.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, "计量单位", ParagraphAlignment.CENTER, 22, true, 11));
            materials.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, "工程数量", ParagraphAlignment.CENTER, 22, true, 11));

            var k = 1;
            //遍历数据
            foreach (var item in dispatch.DispatchRltMaterials)
            {
                materials.GetRow(k).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, $"{k}", ParagraphAlignment.CENTER, 22, false, 11));
                materials.GetRow(k).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, $"{item.Material?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                materials.GetRow(k).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, $"{item.Material?.Spec}", ParagraphAlignment.CENTER, 22, false, 11));
                materials.GetRow(k).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, $"{item.Material.Unit}", ParagraphAlignment.CENTER, 22, false, 11));
                materials.GetRow(k).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, $"{item.Count}", ParagraphAlignment.CENTER, 22, false, 11));
                k++;
            }

            materials.GetRow(dispatch.DispatchRltMaterials.Count + 1).MergeCells(0, 4);//合并4列
            materials.GetRow(dispatch.DispatchRltMaterials.Count + 1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, materials, "", ParagraphAlignment.CENTER, 10, true, 11));

            #endregion

            #region 文档第五个表格对象实例(其他信息)
            //创建文档中的表格对象实例
            XWPFTable otherInfo = document.CreateTable(8, 4);//显示的行列数rows:3行,cols:4列
            otherInfo.Width = 5600;//总宽度
            otherInfo.SetColumnWidth(0, 1000); /* 设置列宽 */
            otherInfo.SetColumnWidth(1, 1800); /* 设置列宽 */
            otherInfo.SetColumnWidth(2, 1000); /* 设置列宽 */
            otherInfo.SetColumnWidth(3, 1800); /* 设置列宽 */

            //第一行
            otherInfo.GetRow(0).MergeCells(1, 3);//合并2列
            otherInfo.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "补充说明", ParagraphAlignment.CENTER, 10, true, 11));
            otherInfo.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{dispatch.ExtraDescription}", ParagraphAlignment.CENTER, 10, false, 11));

            //第二行
            otherInfo.GetRow(1).MergeCells(0, 3);//合并3列
            otherInfo.GetRow(1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "其他内容", ParagraphAlignment.CENTER, 10, true, 11));

            //第三行
            otherInfo.GetRow(2).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "是否需要大型吊装", ParagraphAlignment.CENTER, 24, true, 11));
            otherInfo.GetRow(2).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{ (dispatch.IsNeedLargeEquipment ? "是" : "否")}", ParagraphAlignment.CENTER, 24, false, 11));
            otherInfo.GetRow(2).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "大型吊装机具", ParagraphAlignment.CENTER, 24, true, 11));
            otherInfo.GetRow(2).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{dispatch.LargeEquipment}", ParagraphAlignment.CENTER, 24, false, 11));

            //第四行
            otherInfo.GetRow(3).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "是否涉及围蔽拆除", ParagraphAlignment.CENTER, 24, true, 11));
            otherInfo.GetRow(3).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{ (dispatch.IsDismantle ? "是" : "否")}", ParagraphAlignment.CENTER, 24, false, 11));
            otherInfo.GetRow(3).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "是否高空作业", ParagraphAlignment.CENTER, 24, true, 11));
            otherInfo.GetRow(3).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{ (dispatch.IsHighWork ? "是" : "否")}", ParagraphAlignment.CENTER, 24, false, 11));

            //第五行
            otherInfo.GetRow(4).MergeCells(1, 3);//合并2列
            otherInfo.GetRow(4).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "处理方式", ParagraphAlignment.CENTER, 10, true, 11));
            otherInfo.GetRow(4).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{dispatch.Process}", ParagraphAlignment.CENTER, 10, false, 11));

            //第六行
            otherInfo.GetRow(5).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "安全风险源", ParagraphAlignment.CENTER, 24, true, 11));
            otherInfo.GetRow(5).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{ dispatch.RiskSources}", ParagraphAlignment.CENTER, 24, false, 11));
            otherInfo.GetRow(5).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "计划恢复时间", ParagraphAlignment.CENTER, 24, true, 11));
            otherInfo.GetRow(5).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{ dispatch.RecoveryTime}", ParagraphAlignment.CENTER, 24, false, 11));

            //第七行
            otherInfo.GetRow(6).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "安全防护措施", ParagraphAlignment.CENTER, 24, true, 11));
            otherInfo.GetRow(6).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{ safetyMeasureName}", ParagraphAlignment.CENTER, 24, false, 11));
            otherInfo.GetRow(6).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "工序控制类型", ParagraphAlignment.CENTER, 24, true, 11));
            otherInfo.GetRow(6).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{ controlTypeName}", ParagraphAlignment.CENTER, 24, false, 11));

            //第八行
            otherInfo.GetRow(7).MergeCells(1, 2);//合并2列
            otherInfo.GetRow(7).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, "其他事宜", ParagraphAlignment.CENTER, 10, true, 11));
            otherInfo.GetRow(7).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, otherInfo, $"{dispatch.Remark}", ParagraphAlignment.CENTER, 10, false, 11));
            #endregion

            // 流程审批表格
            if (dispatch.State != DispatchState.UnSubmit)
            {
                WorkFlowNodes = await GetSingleFlowNodes(dispatch.WorkflowId, dispatch.Id);
                XWPFTable WorkFlowTable = document.CreateTable(WorkFlowNodes.Count + 2, 5);//显示的行列数rows:8行,cols:7列
                WorkFlowTable.Width = 5600;//总宽度
                WorkFlowTable.SetColumnWidth(0, 600); /* 设置列宽 */
                WorkFlowTable.SetColumnWidth(1, 1500); /* 设置列宽 */
                WorkFlowTable.SetColumnWidth(2, 1500); /* 设置列宽 */
                WorkFlowTable.SetColumnWidth(3, 1000); /* 设置列宽 */
                WorkFlowTable.SetColumnWidth(4, 1000); /* 设置列宽 */

                //遍历表格标题

                //第一行
                WorkFlowTable.GetRow(0).MergeCells(0, 4);//合并4列
                WorkFlowTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, "审批结果", ParagraphAlignment.CENTER, 10, true, 11));

                WorkFlowTable.GetRow(1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, "序号", ParagraphAlignment.CENTER, 22, true, 11));
                WorkFlowTable.GetRow(1).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, "审批意见", ParagraphAlignment.CENTER, 22, true, 11));
                WorkFlowTable.GetRow(1).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, "审批状态", ParagraphAlignment.CENTER, 22, true, 11));
                WorkFlowTable.GetRow(1).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, "审批人", ParagraphAlignment.CENTER, 22, true, 11));
                WorkFlowTable.GetRow(1).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, "审批时间", ParagraphAlignment.CENTER, 22, true, 11));
                var m = 2;
                //遍历数据
                foreach (var item in WorkFlowNodes)
                {
                    WorkFlowTable.GetRow(m).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, $"{m}", ParagraphAlignment.CENTER, 22, false, 11));
                    WorkFlowTable.GetRow(m).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, $"{item.Content}", ParagraphAlignment.CENTER, 22, false, 11));
                    WorkFlowTable.GetRow(m).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, item.State == WorkflowState.Finished ? "审核通过" : item.State == WorkflowState.Stopped ? "审核未通过" : "", ParagraphAlignment.CENTER, 22, false, 11));
                    WorkFlowTable.GetRow(m).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, $"{item.User?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                    WorkFlowTable.GetRow(m).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, WorkFlowTable, $"{item.ApproveTime}", ParagraphAlignment.CENTER, 22, false, 11));
                    m++;
                }
            }


            //向文档流中写入内容，生成word
            MemoryStream stream = new MemoryStream();
            document.Write(stream);
            var buf = stream.ToArray();
            stream = new MemoryStream(buf);
            return stream;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> Delete(List<Guid> ids)
        {
            await _dispatchRepository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }

        #region  私有方法
        /// <summary>
        /// 计算总体计划的流程状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<DispatchDto> CalculateMasterPlanState(List<Dispatch> list)
        {
            var result = new List<DispatchDto>();
            foreach (var item in list)
            {
                var model = ObjectMapper.Map<Dispatch, DispatchDto>(item);
                if (item.WorkflowId != null)
                {
                    var workflow = _singleFlowProcessService.GetWorkflowById(item.WorkflowId.Value).Result;
                    switch (workflow.State)
                    {
                        case Bpm.WorkflowState.Waiting:
                            model.State = DispatchState.OnReview;
                            break;
                        case Bpm.WorkflowState.Finished:
                            model.State = DispatchState.Pass;
                            break;
                        case Bpm.WorkflowState.Rejected:
                            model.State = DispatchState.UnPass;
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
                        if (data != null && (data.StepState == WorkflowStepState.Stopped || data.StepState == WorkflowStepState.Rejected))
                        {
                            model.State = DispatchState.UnPass;
                        }
                    }
                }
                result.Add(model);
            }
            return result;
        }

        public async Task<List<CommentDto>> GetSingleFlowNodes(Guid? workflowId, Guid dispatchId)
        {
            var infos = _dispatchRltWorkFlowRepository.Where(a => a.DispatchId == dispatchId && a.WorkFlowId == workflowId).ToList();
            var nodes = (await _singleFlowProcess.GetWorkFlowNodes((Guid)workflowId)).Where(x => x.Type == "bpmApprove").ToList();
            var comments = new List<CommentDto>();
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
                comments.AddRange(node.Comments);
            }
            return comments;
        }

        /// <summary>
        /// 安全防护措施
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetSafetyMeasure(string type)
        {
            if (type == "1")
            {
                return "内部培训";
            }
            else if (type == "2")
            {
                return "自身装备";
            }
            else if (type == "3")
            {
                return "现场环境无安全隐患";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 转化工序控制类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetControlType(string type)
        {
            if (type == "1")
            {
                return "关键工序";
            }
            else if (type == "2")
            {
                return "一般工序";
            }
            else if (type == "3")
            {
                return "隐蔽";
            }
            else if (type == "4")
            {
                return "旁站";
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}
