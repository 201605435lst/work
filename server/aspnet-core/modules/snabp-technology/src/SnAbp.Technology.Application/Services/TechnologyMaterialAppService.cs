/**********************************************************************
*******命名空间： SnAbp.Technology.Services
*******类 名 称： TechnologyMaterialAppService
*******类 说 明： 材料管理相关服务，包括材料管理和用料计划管理等。
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 1:59:00 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NPOI.HSSF.Util;
using NPOI.XWPF.UserModel;
using SnAbp.Bpm;
using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.IServices;
using SnAbp.Common.Dtos.Task;
using SnAbp.Common.IServices;
using SnAbp.Identity;
using SnAbp.Technology.Dtos;
using SnAbp.Technology.Dtos.Material;
using SnAbp.Technology.Entities;
using SnAbp.Technology.enums;
using SnAbp.Technology.IServices;
using SnAbp.Utils;
using SnAbp.Utils.DataExport;
using SnAbp.Utils.WordHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace SnAbp.Technology.Services
{
    /// <summary>
    /// 材料管理相关服务 
    /// </summary>
    public class TechnologyMaterialAppService : TechnologyAppService, IMaterialAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Organization, Guid> _orgRepository;
        readonly IRepository<Material, Guid> _materialRepository;
        readonly IRepository<MaterialPlan, Guid> _materialPlanRepository;
        readonly IRepository<MaterialPlanRltMaterial, Guid> materialPlanRltMaterials;
        readonly IRepository<MaterialPlanFlowInfo, Guid> materialPlanFlowInfos;
        readonly IRepository<StdBasic.Entities.ProductCategory, Guid> _productCategoryRepository;
        readonly IRepository<Identity.DataDictionary, Guid> _dataDictionary;
        readonly ISingleFlowProcessService singleFlowProcessService;
        readonly IUnitOfWorkManager _unitOfWork;
        private readonly ICommonBackgroundTaskAppService _commonBackgroundTaskAppService;
        public TechnologyMaterialAppService(
             IRepository<Organization, Guid> orgRepository,
               IHttpContextAccessor httpContextAccessor,
                IRepository<Material, Guid> materialRepository,
                IRepository<MaterialPlan, Guid> materialPlanRepository,
                IRepository<StdBasic.Entities.ProductCategory, Guid> productCategoryRepository,
                  IUnitOfWorkManager unitOfWork,
                IRepository<Identity.DataDictionary, Guid> dataDictionary
, IRepository<MaterialPlanRltMaterial, Guid> materialPlanRltMaterials = null
, ISingleFlowProcessService singleFlowProcessService = null, IRepository<MaterialPlanFlowInfo, Guid> materialPlanFlowInfos = null, ICommonBackgroundTaskAppService commonBackgroundTaskAppService = null)
        {
            _orgRepository = orgRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            this._materialPlanRepository = materialPlanRepository;
            this._materialRepository = materialRepository;
            this._productCategoryRepository = productCategoryRepository;
            this._dataDictionary = dataDictionary;
            this.materialPlanRltMaterials = materialPlanRltMaterials;
            this.singleFlowProcessService = singleFlowProcessService;
            this.materialPlanFlowInfos = materialPlanFlowInfos;
            _commonBackgroundTaskAppService = commonBackgroundTaskAppService;
        }
        #region 材料维护相关接口
        public async Task<bool> Create(MaterialCreateDto input)
        {
            // 创建材料信息
            var model = ObjectMapper.Map<MaterialCreateDto, Material>(input);
            model.SetId(GuidGenerator.Create());

            // 判断是否有重名的情况
            if (_materialRepository.Any(a => a.Name == input.Name)){
                throw new UserFriendlyException($"名称为：{input.Name} 的材料已存在，请重新输入！");
            }

            // 处理数据字典
            await _materialRepository.InsertAsync(model);
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            await _materialRepository.DeleteAsync(id);
            return true;
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRange(List<Guid> ids)
        {
            await _materialRepository.DeleteAsync(a => ids.Contains(a.Id));
            return true;
        }

        /// <summary>
        /// 根据材料id获取材料信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MaterialDto> Get(Guid id) => ObjectMapper.Map<Material, MaterialDto>(await _materialRepository.GetAsync(id));
        public Task<List<MaterialDto>> GetAllList(Guid typeId)
        {
            var list = _materialRepository.Where(a => a.TypeId == typeId).ToList();
            return Task.FromResult(ObjectMapper.Map<List<Material>, List<MaterialDto>>(list));
        }

        public Task<PagedResultDto<MaterialDto>> GetList(MaterialSearchDto input)
        {
            var result = new PagedResultDto<MaterialDto>();
            var query = _materialRepository
                .WithDetails(a => a.Type)
                .WhereIf(!input.KeyWords.IsNullOrEmpty(), a => a.Name.Contains(input.KeyWords))
                .WhereIf(input.TypeId != null, a => a.TypeId == input.TypeId);
            result.TotalCount = query.Count();
            var list = query.PageBy(input.SkipCount, input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<Material>, List<MaterialDto>>(list);
            return Task.FromResult(result);
        }
        public async Task<bool> Update(MaterialCreateDto input)
        {
            var entity = await _materialRepository.GetAsync(input.Id);
            entity.Name = input.Name;
            entity.Spec = input.Spec;
            entity.Unit = input.Unit;
            entity.Price = input.Price;
            entity.Remark = input.Remark;
            entity.TypeId = input.TypeId;

            await _materialRepository.UpdateAsync(entity);
            return true;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Produces("application/octet-stream")]
        [HttpGet]
        public async Task<Stream> ExportPlan(Guid id)
        {
            //1、获取需要导出的所有数据
            var materialPlan = _materialPlanRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (materialPlan == null) throw new UserFriendlyException("请刷新页面重新尝试");
            var materialPlanDto = ObjectMapper.Map<MaterialPlan, MaterialPlanExportDto>(materialPlan);
            materialPlanDto.Nodes = await GetSingleFlowNodes(materialPlanDto.WorkflowId, materialPlanDto.Id);

            //获取当前登录用户的组织机构
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? _orgRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var workOrganization = organization != null ? organization.Name : null;
            return SaveMaterialPlanWordFile(materialPlanDto, workOrganization);
        }
        #region   生成导出word文档
        public static Stream SaveMaterialPlanWordFile(MaterialPlanExportDto Data, string workOrganization)
        {
            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();
            //向文档流中写入内容，生成word
            MemoryStream stream = new MemoryStream();
            string currentDate = DateTime.Now.ToString("yyyy年MM月dd日");
            string checkTime = Data.PlanTime.ToString("D");//检查时间
            string workFileName = Data.PlanName;
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
                .SetTableParagraph(0, 0, "计划名称", ParagraphAlignment.CENTER, 35, true)
                .SetTableParagraph(0, 1, $"{Data.PlanName}", ParagraphAlignment.CENTER, 35, true)
                .SetTableParagraph(1, 0, "提交人", ParagraphAlignment.CENTER, 35, true)
                .SetTableParagraph(1, 1, $"{Data.Creator.Name}", ParagraphAlignment.CENTER, 35, false)
                .SetTableParagraph(1, 2, "计划时间", ParagraphAlignment.CENTER, 50, true)
                .SetTableParagraph(1, 3, $"{ Data.PlanTime.ToString("D") }", ParagraphAlignment.CENTER, 35, false)
                .MergeRowCells(2, 0, 3)// 合并单元格
                .SetTableParagraph(2, 0, "材料清单", ParagraphAlignment.CENTER, 35, true, 13);
            var rowCount1 = Data.Materials.Count + 2;
            // 创建材料清单的表格
            var headTitles = new string[] { "序号", "材料类别", "材料名称", "规格型号", "计量单位", "需求数量" };
            var twoTable = document
                .CreateTableAndSetColumnWidth(rowCount1, 250, 800, 1600, 1000, 700, 700)
                .SetTableHeadTitle(ParagraphAlignment.CENTER, 24, true, titles: headTitles)
                .MergeRowCells(rowCount1 - 1, 0, 5)
                .SetTableParagraph(rowCount1 - 1, 0, "审批结果", ParagraphAlignment.CENTER, 50, true, 13);
            var j = 1;
            foreach (var item in Data.Materials)
            {
                twoTable.SetTableParagraph(j, 0, $"{j}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 1, $"{item.Material.Type.Name}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 2, $"{item.Material.Name}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 3, $"{item.Material.Spec}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 4, $"{item.Material.Unit}", ParagraphAlignment.CENTER, 24, false);
                twoTable.SetTableParagraph(j, 5, $"{item.Count}", ParagraphAlignment.CENTER, 24, false);
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
                var rowCount2 = comments.Count > 0? comments.Count + 1: comments.Count +2;
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
                        .SetTableParagraph(i, 2, item.State == WorkflowState.Finished ? "审核通过" : item.State == WorkflowState.Stopped ? "审核未通过" : "", ParagraphAlignment.CENTER, 80, false)
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

        public async Task<List<SingleFlowNodeDto>> GetSingleFlowNodes(Guid? workflowId, Guid materialPlanId)
        {
            var infos = materialPlanFlowInfos.Where(a => a.MaterialPlanId == materialPlanId && a.WorkFlowId == workflowId).ToList();
            var nodes = (await singleFlowProcessService.GetWorkFlowNodes((Guid)workflowId)).Where(x => x.Type == "bpmApprove").ToList();
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
        public async Task<Stream> Export(List<Guid>? ids)
        {
            List<Material> list = null;
            if (ids.Any())
            {
                list = _materialRepository
                    .WithDetails(a => a.Type)
                    .Where(a => ids.Contains(a.Id))
                    .ToList();
            }
            else
            {
                // 导出全部
                list = _materialRepository.WithDetails(a => a.Type)
                    .ToList();
            }
            return await CreateExcel(list);
        }
        private Task<Stream> CreateExcel(List<Material> list)
        {
            var handler = DataExportHandler
                   .CreateExcelFile(Utils.ExcelHelper.ExcelFileType.Xlsx);
            handler.CreateSheet("物资材料信息");
            var rowIndex = 0;
            var headRow = handler.CreateRow(rowIndex);
            var cellStyle = handler.CreateCellStyle
                (CellBorder.CreateBorder(NPOI.SS.UserModel.BorderStyle.Thin, lineColor: HSSFColor.Black.Index));
            var title = new string[]
                      {
                    "序号",
                    "材料名称",
                    "规格型号",
                    "类别",
                    "价格",
                    "备注"
                      };
            for (int i = 0; i < title.Length; i++)
            {
                headRow.CreateCell(i)
                    .SetCellStyle(cellStyle)
                    .SetCellValue(title[i]);
            }
            list?.ForEach(a =>
                {
                    var row = handler.CreateRow(++rowIndex);
                    row.CreateCell(0).SetCellStyle(cellStyle).SetCellValue(rowIndex);
                    row.CreateCell(1).SetCellStyle(cellStyle).SetCellValue(a.Name);
                    row.CreateCell(2).SetCellStyle(cellStyle).SetCellValue(a.Spec);
                    row.CreateCell(3).SetCellStyle(cellStyle).SetCellValue(a.Type.Name);
                    row.CreateCell(4).SetCellStyle(cellStyle).SetCellValue(a.Price.ToString());
                    row.CreateCell(5).SetCellStyle(cellStyle).SetCellValue(a.Remark);

                });
            Stream stream = handler.GetExcelDataBuffer().BytesToStream();
            return Task.FromResult(stream);
        }


        /// <summary>
        ///  同步产品分类信息为物资材料信息  
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public async Task Synchronize(string taskKey)
        {
            var task = await _commonBackgroundTaskAppService.Get(taskKey);
            if (task == null)
            {
                task = await _commonBackgroundTaskAppService.Create(new BackgroundTaskDto() { Key = taskKey });
                var index = 0;
                var allList = await _productCategoryRepository.GetListAsync(true);
                var dicType = await _dataDictionary.GetAsync(a => a.Key == "MaterialType.Component");
                if (allList.Any())
                {
                    foreach (var a in allList)
                    {
                        index++;
                        if (a.Parent != null)
                        {
                            var uow = _unitOfWork.Begin(true, false);
                            var material = new Material(GuidGenerator.Create());
                            material.Name = a.Parent.Name;
                            material.Spec = a.Name;
                            material.Unit = a.Unit;
                            material.TypeId = dicType.Id;
                            material.ProductCategoryId = a.Id;
                            // 判断材料和型号是否已经存在材料表中。
                            if (!this.ProductExist(material.Name, material.Spec, material.TypeId))
                            {
                                await _materialRepository.InsertAsync(material, true);
                                await uow.SaveChangesAsync();
                            }
                        }
                        await _commonBackgroundTaskAppService.Update(new BackgroundTaskDto
                        {
                            Key = taskKey,
                            Count = allList.Count(),
                            Index = index,
                            Message = string.Empty
                        });
                    }
                }
                await _commonBackgroundTaskAppService.Done(taskKey);
            }
        }


        private bool ProductExist(string name, string spec, Guid? typeId)
        {
            return _materialRepository.Any(a => a.Name == name && a.Spec == spec && a.TypeId == typeId);
        }
        #endregion


        #region 用料计划相关接口
        public async Task<PagedResultDto<MaterialPlanDto>> GetPlanList(MaterialPlanSearchDto input)
        {
            var result = new PagedResultDto<MaterialPlanDto>();
            var query = _materialPlanRepository
                .WithDetails()
                .WhereIf(!input.KeyWords.IsNullOrEmpty(), a => a.PlanName.Contains(input.KeyWords))
                .WhereIf(input.Submit != null && input.Submit == "true", a => a.Submit)
                .WhereIf(input.Submit != null && input.Submit == "false", a => !a.Submit)
                .WhereIf(input.StartTime != null, a => a.PlanTime >= input.StartTime)
                .WhereIf(input.EndTime != null, a => a.PlanTime <= input.EndTime)
                .WhereIf(input.MaterialUse, a => a.Status == ApprovalStatus.Pass)
                .OrderByDescending(a => a.CreationTime);
            // .WhereIf(input.State != ApprovalStatus.All, a => a.Status == input.State);
            var list = new List<MaterialPlan>();

            if (input.Approval)
            {
                if (input.Waiting)
                {
                    // 获取待我审批的数据
                    foreach (var item in query)
                    {
                        if (item.WorkflowId == null) continue;
                        if (await singleFlowProcessService.IsWaitingMyApproval(item.WorkflowId.GetValueOrDefault()))
                            list.Add(item);
                    }
                }
                else
                {
                    // 获取我已审批的数据
                    var workflowIds = await singleFlowProcessService.GetMyApprovaledWorkflow();
                    if (workflowIds.Any())
                    {
                        list = query.Where(a => workflowIds.Contains(a.WorkflowId)).ToList();
                      
                    }
                }
            }

            else
            {
                list = query.ToList();
            }
            if (input.IsSelect)
            {
                result.Items = CalculateMaterialPlanState(list).Where(x => x.Status == ApprovalStatus.Pass).ToList();
                result.TotalCount = list.Count();
                list = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            }
            else
            {
                result.TotalCount = list.Count();
                result.Items = CalculateMaterialPlanState(list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            //result.Items = CalculateMaterialPlanState(list);
            //result.Items = CalculateMaterialPlanState(list);
            return result;
        }

        /// <summary>
        /// 计算用料计划的流程状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<MaterialPlanDto> CalculateMaterialPlanState(List<MaterialPlan> list)
        {
            var result = new List<MaterialPlanDto>();
            foreach (var item in list)
            {
                var model = ObjectMapper.Map<MaterialPlan, MaterialPlanDto>(item);
                if (item.WorkflowId != null)
                {
                    var workflow = singleFlowProcessService.GetWorkflowById(item.WorkflowId.Value).Result;
                    switch (workflow.State)
                    {
                        case Bpm.WorkflowState.Waiting:
                            model.Status = ApprovalStatus.OnReview;
                            break;
                        case Bpm.WorkflowState.Finished:
                            model.Status = ApprovalStatus.Pass;
                            break;
                        case Bpm.WorkflowState.Rejected:
                            model.Status = ApprovalStatus.UnPass;
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
                            model.Status = ApprovalStatus.UnPass;
                        }
                    }
                }
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 根据计划id获取计划关联的材料信息
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public Task<List<MaterialPlanRltMaterialDto>> GetPlanMaterials(Guid planId)
        {
            var list = materialPlanRltMaterials
                .WithDetails(a => a.Material, a => a.Material.Type)
                .Where(a => a.MaterialPlanId == planId)
                .ToList();
            return Task.FromResult(ObjectMapper.Map<List<MaterialPlanRltMaterial>, List<MaterialPlanRltMaterialDto>>(list));
        }

        [UnitOfWork]
        public async Task<bool> PlanCreate(MaterialPlanCreateDto input)
        {
            // 判断用料计划名称是否为空
            if (_materialPlanRepository.Any(a => a.PlanName == input.PlanName))
            {
                throw new UserFriendlyException("计划名称已经存在，请重新添加");
            }

            // 创建材料信息
            var id = GuidGenerator.Create();
            var model = new MaterialPlan(id);
            model.Materials = new List<MaterialPlanRltMaterial>();
            model.PlanName = input.PlanName;
            model.PlanTime = input.PlanDate;
            model.Status = ApprovalStatus.ToSubmit;// 待提交
            await CheckSameName(input.PlanName, null);
            var code = "CG_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace("-", "");
            model.Code = code;
            // 处理数据字典
            await _materialPlanRepository.InsertAsync(model, true);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            foreach (var item in input.Materials)
            {
                var rlt = new MaterialPlanRltMaterial(id, item.Id)
                {
                    Count = item.Count
                };
                await materialPlanRltMaterials.InsertAsync(rlt, true);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            return true;
        }
        [HttpDelete]
        public async Task<bool> PlanDelete(Guid id)
        {
            await _materialPlanRepository.DeleteAsync(id);
            return true;
        }
        /// <summary>
        /// 用料计划清单中删除材料信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> PlanMaterialDelete(Guid id)
        {
            await materialPlanRltMaterials.DeleteAsync(a => a.MaterialId == id);
            return true;
        }
        /// <summary>
        /// 更新用料计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [UnitOfWork]
        public async Task<bool> PlanUpdate(MaterialPlanCreateDto input)
        {
            var entity = await _materialPlanRepository.GetAsync(input.Id);
            entity.PlanName = input.PlanName;
            entity.PlanTime = input.PlanDate;
            await _materialPlanRepository.UpdateAsync(entity, true);
            await CheckSameName(input.PlanName, input.Id);
            if (input.Materials.Any())
            {
                foreach (var material in input.Materials)
                {
                    var data = materialPlanRltMaterials.FirstOrDefault(a => a.MaterialId == material.Id && a.MaterialPlanId == entity.Id);
                    if (data != null)
                    {
                        data.Count = material.Count;
                        await materialPlanRltMaterials.UpdateAsync(data, true);
                    }
                    else
                    {
                        var rlt = new MaterialPlanRltMaterial(entity.Id, material.Id)
                        {
                            Count = material.Count
                        };
                        await materialPlanRltMaterials.InsertAsync(rlt, true);
                    }

                }
            }
            return true;
        }
        [HttpPut]
        /// <summary>
        /// 更新用料计划的提交状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> PlanSubmitStateUpdate(Guid id)
        {
            var entity = await _materialPlanRepository.GetAsync(id);
            entity.Submit = true;
            await _materialPlanRepository.UpdateAsync(entity, true);
            return true;
        }

        //public async Task<bool> PlanExport(Guid id)
        //{
        //    SnAbp.Utils.WordHelper.WordHelper.c
        //}

        #endregion


        #region 用料计划审批
        /// <summary>
        /// 创建工作流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CreateWorkFlow(Guid planId, Guid templateId)
        {
            var workflow = await singleFlowProcessService.CreateSingleWorkFlow(templateId);
            var materialPlan = await _materialPlanRepository.GetAsync(planId);
            materialPlan.WorkflowTemplateId = templateId;
            materialPlan.WorkflowId = workflow.Id;
            materialPlan.Status = ApprovalStatus.OnReview;// 审核中
            await _materialPlanRepository.UpdateAsync(materialPlan);
            return true;
        }
        [UnitOfWork]
        public async Task<bool> Process(MaterialPlanProcessDto input)
        {
            var materialPlan = await _materialPlanRepository.GetAsync(input.PlanId);
            var workflowId = materialPlan.WorkflowId.GetValueOrDefault();
            Bpm.Dtos.WorkflowDetailDto dto = null;
            using (var uow = _unitOfWork.Begin(true, false))
            {
                if (input.Status == ApprovalStatus.Pass)
                {
                    dto = await singleFlowProcessService.Approved(workflowId, input.Content, CurrentUser.Id);
                }
                else if (input.Status == ApprovalStatus.UnPass)
                {
                    dto = await singleFlowProcessService.Stopped(workflowId, input.Content, CurrentUser.Id);
                }
                else
                {
                    throw new UserFriendlyException("流程处理异常");
                }
                await uow.CompleteAsync();
            }
            // 更新当前 流程的状态
            var workflow = singleFlowProcessService.GetWorkflowById(workflowId).Result;
            switch (workflow.State)
            {
                case Bpm.WorkflowState.Waiting:
                    materialPlan.Status = ApprovalStatus.OnReview;
                    break;
                case Bpm.WorkflowState.Finished:
                    materialPlan.Status = ApprovalStatus.Pass;
                    break;
                case Bpm.WorkflowState.Rejected:
                    materialPlan.Status = ApprovalStatus.UnPass;
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
                    materialPlan.Status = ApprovalStatus.UnPass;
                }
            }
            await _materialPlanRepository.UpdateAsync(materialPlan);
            var planInfo = new MaterialPlanFlowInfo(GuidGenerator.Create());
            planInfo.Content = input.Content;
            planInfo.MaterialPlanId = materialPlan.Id;
            planInfo.WorkFlowId = workflowId;
            planInfo.State = workflow.State;
            await materialPlanFlowInfos.InsertAsync(planInfo);
            return true;
        }

        #endregion


        #region 私有方法
        private async Task<bool> CheckSameName(string name, Guid? id)
        {
            return await Task.Run(() =>
            {
                var sameNames = _materialPlanRepository.WhereIf(id != Guid.Empty && id != null, x => x.Id != id)
                .FirstOrDefault(a => a.PlanName == name);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("当前计划名称已存在！");
                }

                return true;
            });
        }
        #endregion
    }
}
