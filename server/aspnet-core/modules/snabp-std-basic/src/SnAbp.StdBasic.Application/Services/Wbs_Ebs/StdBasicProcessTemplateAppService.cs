using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnAbp.StdBasic.Enums;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicProcessTemplateAppService : StdBasicAppService, IStdBasicProcessTemplateAppService
    {
        private readonly IRepository<ProcessTemplate, Guid> _repositoryProcessTemplate;
        
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IDataFilter _dataFilter;
        private readonly IUnitOfWorkManager _unitOfWork;


        public StdBasicProcessTemplateAppService(IRepository<ProcessTemplate, Guid> repositoryProcessTemplate,
           IGuidGenerator guidGenerator,
           IFileImportHandler fileImport,
           IDataFilter dataFilter,
           IUnitOfWorkManager unitOfWork
       )
        {
            _repositoryProcessTemplate = repositoryProcessTemplate;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _unitOfWork = unitOfWork;
        }
        [Authorize(StdBasicPermissions.ProcessTemplate.Create)]
        public async Task<ProcessTemplateDto> Create(ProcessTemplateCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("工序模板名称不能为空");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("工序模板编码不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }
            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("单位不能为纯数字");
            }
            var processTemplate = new ProcessTemplate(_guidGenerator.Create());
            processTemplate.Name = input.Name;
            processTemplate.ParentId = input.ParentId;
            processTemplate.Code = input.Code;
            processTemplate.Content = input.Content;
            processTemplate.Duration = input.Duration;
            processTemplate.DurationUnit = input.DurationUnit;
            processTemplate.PrepositionId = input.PrepositionId;
            processTemplate.Type = input.Type;
            processTemplate.Unit = input.Unit;
            processTemplate.Remark = input.Remark;
            await _repositoryProcessTemplate.InsertAsync(processTemplate);
            return ObjectMapper.Map<ProcessTemplate, ProcessTemplateDto>(processTemplate);
        }

        [Authorize(StdBasicPermissions.ProcessTemplate.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var processTemplate = _repositoryProcessTemplate.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (string.IsNullOrEmpty(processTemplate.Name)) throw new UserFriendlyException("该工序模板不存在");
            //var children = _repositoryProcessTemplate.WithDetails().Where(x => x.ParentId != null && x.ParentId == id);
            //if (children != null) throw new UserFriendlyException("请先删除该工序模板的下级分类");
            if (processTemplate.Children.Count > 0)
                throw new UserFriendlyException("请先删除该工序模板的下级分类");
            var list = _repositoryProcessTemplate.WithDetails().Where(x => x.PrepositionId == id);
            if (list?.Count() > 0)
            {
                throw new UserFriendlyException("该工序模板为别的工序模板的前置任务，无法删除，请先修改数据");
            }
            await _repositoryProcessTemplate.DeleteAsync(id);

            return true;
        }
        [Authorize(StdBasicPermissions.ProcessTemplate.Export)]
        public async Task<Stream> Export(ProcessTemplateData input)
        {
            List<ProcessTemplate> list;
            if (input.Paramter.KeyWords != "" || input.Paramter.ParentId != null)
            {
                list = GetProcessTemplateData(input.Paramter).AsEnumerable().OrderBy(y => y.Code).ToList();
            }
            else
            {
                list = _repositoryProcessTemplate.Where(x => x.Id != Guid.Empty).OrderBy(y => y.Code).ToList();
            }
            var dtoList = ObjectMapper.Map<List<ProcessTemplate>, List<ProcessTemplateTemplate>>(list);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].PrepositionId.HasValue)
                {
                    var processTemplate = await _repositoryProcessTemplate.GetAsync(list[i].PrepositionId.Value);
                    dtoList[i].PrepositionCode = processTemplate.Code;
                }
                if (list[i].ParentId.HasValue)
                {
                    var parent = await _repositoryProcessTemplate.GetAsync(list[i].ParentId.Value);
                    dtoList[i].PrepositionCode = parent.Code;
                }
                if (list[i].Type == ProcessTypeEnum.ConstructionTask)
                    dtoList[i].Type = "施工任务";
                if (list[i].Type == ProcessTypeEnum.ManagemenetTask)
                    dtoList[i].Type = "管理任务";
                if (list[i].DurationUnit == ServiceLifeUnit.YEAR)
                    dtoList[i].DurationUnit = "年";
                if (list[i].DurationUnit == ServiceLifeUnit.MONTH)
                    dtoList[i].DurationUnit = "月";
                if (list[i].DurationUnit == ServiceLifeUnit.DAY)
                    dtoList[i].DurationUnit = "日";
            }
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }
        [Authorize(StdBasicPermissions.ProcessTemplate.Detail)]
        public async Task<ProcessTemplateDto> Get(Guid id)
        {
            ProcessTemplateDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryProcessTemplate.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此工序模板不存在");
                result = ObjectMapper.Map<ProcessTemplate, ProcessTemplateDto>(ent);
                if (result.PrepositionId.HasValue && result.PrepositionId != Guid.Empty)
                {
                    var preposition = _repositoryProcessTemplate.FirstOrDefault(x => x.Id == result.PrepositionId);
                    result.PrepositionCode = preposition.Name;
                }
            });
            return result;
        }

        public Task<PagedResultDto<ProcessTemplateDto>> GetList(ProcessTemplateGetListByIdsDto input)
        {
            var result = new PagedResultDto<ProcessTemplateDto>();

            var productCategories = new List<ProcessTemplate>();

            var dtos = new List<ProcessTemplateDto>();

            if (input.Ids != null && input.Ids.Count > 0)
            {

                foreach (var id in input.Ids)
                {
                    var componentCategory = _repositoryProcessTemplate.WithDetails().FirstOrDefault(x => x.Id == id);
                    if (componentCategory == null) continue;
                    productCategories.Add(componentCategory);
                    if (componentCategory.ParentId.HasValue && componentCategory.ParentId != Guid.Empty)
                    {
                        FindAllParents(componentCategory.ParentId.Value, ref productCategories);
                    }
                    var parentIds = productCategories.Select(x => x.Id).ToList();
                    var filterList = _repositoryProcessTemplate.WithDetails()
                        .Where(x => parentIds.Contains(x.ParentId.Value) || x.ParentId == null)
                        .ToList();
                    productCategories.AddRange(filterList);

                }

                // 数据去重并转成dto
                var listDtos = ObjectMapper.Map<List<ProcessTemplate>, List<ProcessTemplateDto>>(productCategories.Distinct().ToList());

                //如果子集为空设置children为null
                foreach (var item in listDtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<ProcessTemplateDto>();
                    }
                }

                dtos = GuidKeyTreeHelper<ProcessTemplateDto>.GetTree(listDtos);
            }
            else //按需加载
            {
                //根据关键字查询
                if (!string.IsNullOrEmpty(input.KeyWords))
                {
                    productCategories = _repositoryProcessTemplate.WithDetails()
                        .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                    dtos = ObjectMapper.Map<List<ProcessTemplate>, List<ProcessTemplateDto>>(productCategories);
                    foreach (var item in dtos)
                    {
                        item.Children = null;
                    }
                }

                else
                {
                    productCategories = _repositoryProcessTemplate.WithDetails()
                        .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                        .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
                        .ToList();
                    dtos = ObjectMapper.Map<List<ProcessTemplate>, List<ProcessTemplateDto>>(productCategories);
                    foreach (var item in dtos)
                    {
                        if (item.Children.Count == 0)
                        {
                            item.Children = null;
                        }
                        else
                        {
                            item.Children = new List<ProcessTemplateDto>();
                        }
                    }
                }


            }
            result.TotalCount = dtos.Count();
            result.Items = input.IsAll ? dtos.OrderBy(x => x.Code).ToList()
                : dtos.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return Task.FromResult(result);
        }

        public Task<ProcessTemplateDto> GetListCode(Guid? id)
        {
            var list = new List<ProcessTemplate>();
            if (id != null && id != Guid.Empty)
            {
                list = _repositoryProcessTemplate.Where(x => x.ParentId == id).OrderBy(x => x.Code).ToList();
            }
            else
            {
                list = _repositoryProcessTemplate.Where(x => x.ParentId == null).OrderBy(x => x.Code).ToList();
            }
            var dtos = Task.FromResult(ObjectMapper.Map<ProcessTemplate, ProcessTemplateDto>(list.Count > 0 ? list.Last() : null));
            return dtos;
        }
        [Authorize(StdBasicPermissions.ProcessTemplate.Update)]
        public async Task<ProcessTemplateDto> Update(ProcessTemplateUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入工序模板的id");
            var processTemplate = await _repositoryProcessTemplate.GetAsync(input.Id);
            if (processTemplate == null) throw new UserFriendlyException("该工序模板不存在");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("工序模板名不能为空");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("工序模板编码不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }
            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("单位不能为纯数字");
            }
            if (input.PrepositionId.HasValue && input.PrepositionId == input.Id)
            {
                throw new UserFriendlyException("不能选择自己为前置任务");
            }
            processTemplate.Name = input.Name;
            processTemplate.ParentId = input.ParentId;
            processTemplate.Code = input.Code;
            processTemplate.Content = input.Content;
            processTemplate.Duration = input.Duration;
            processTemplate.DurationUnit = input.DurationUnit;
            processTemplate.PrepositionId = input.PrepositionId;
            processTemplate.Type = input.Type;
            processTemplate.Unit = input.Unit;
            processTemplate.Remark = input.Remark;
            await _repositoryProcessTemplate.UpdateAsync(processTemplate);
            return ObjectMapper.Map<ProcessTemplate, ProcessTemplateDto>(processTemplate);
        }
        [Authorize(StdBasicPermissions.ProcessTemplate.Import)]
        public async Task Upload([FromForm] ImportData input)
        {


            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            //ProcessTemplateTemplate
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<ProcessTemplateTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<ProcessTemplateTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<ProcessTemplateTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            ProcessTemplate processTemplateModel;
            if (datalist.Any())
            {
                await _fileImport.ChangeTotalCount(input.ImportKey, datalist.Count());

                var updateIndex = 1;

                foreach (var item in datalist.ToList())
                {
                    await _fileImport.UpdateState(input.ImportKey, updateIndex);
                    updateIndex++;
                    var canInsert = true;
                    var newInfo = new WrongInfo(item.Index);
                    if (item.Code.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"编码为空");
                    }
                    if (item.Name.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"名称为空");
                    }
                    if (item.Type == "施工任务")
                        item.Type = "ConstructionTask";
                    if (item.Type == "管理任务")
                        item.Type = "ManagemenetTask";
                    if (item.DurationUnit == "年")
                        item.DurationUnit = "YEAR";
                    if (item.DurationUnit == "月")
                        item.DurationUnit = "MONTH";
                    if (item.DurationUnit == "日")
                        item.DurationUnit = "DAY";
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    using var uow = _unitOfWork.Begin(true);
                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        processTemplateModel = _repositoryProcessTemplate.FirstOrDefault(a => a.Code == item.Code && a.Name == item.Name);
                    }
                    if (processTemplateModel != null)
                    {
                        newInfo.AppendInfo($"{item.Code}已存在，且已更新");
                        //修改自己本身数据
                        processTemplateModel.Name = item.Name;
                        processTemplateModel.Remark = item.Remark;
                        processTemplateModel.Content = item.Content;
                        processTemplateModel.Duration = item.Duration;
                        processTemplateModel.DurationUnit = (ServiceLifeUnit)Enum.Parse(typeof(ServiceLifeUnit), item.DurationUnit, true);
                        processTemplateModel.Type = (ProcessTypeEnum)Enum.Parse(typeof(ProcessTypeEnum), item.Type, true);
                        processTemplateModel.Unit = item.Unit;
                        if (item.ParentCode != null && !string.IsNullOrEmpty(item.ParentCode) && processTemplateModel.ParentId.HasValue == false)
                        {
                            var parent = _repositoryProcessTemplate.FirstOrDefault(a => a.Code == item.ParentCode);
                            if (parent != null)
                            {
                                processTemplateModel.ParentId = parent.Id;
                            }
                            else
                            {
                                var parentModel = datalist.ToList().Find(x => x.Code == item.ParentCode);
                                if (parentModel != null)
                                {
                                    var parentEnt = new ProcessTemplate(_guidGenerator.Create());
                                    parentEnt.Name = parentModel.Name;
                                    parentEnt.Remark = parentModel.Remark;
                                    parentEnt.Content = parentModel.Content;
                                    parentEnt.Duration = parentModel.Duration;
                                    parentEnt.DurationUnit = (ServiceLifeUnit)Enum.Parse(typeof(ServiceLifeUnit), parentModel.DurationUnit, true);
                                    parentEnt.Type = (ProcessTypeEnum)Enum.Parse(typeof(ProcessTypeEnum), parentModel.Type, true);
                                    parentEnt.Unit = parentModel.Unit;
                                    await _repositoryProcessTemplate.InsertAsync(parentEnt);
                                    processTemplateModel.ParentId = parentEnt.Id;
                                    await uow.SaveChangesAsync();
                                }
                            }

                        }
                        if (item.PrepositionCode != null && !string.IsNullOrEmpty(item.PrepositionCode) && processTemplateModel.PrepositionId.HasValue == false)
                        {
                            var prepositiont = _repositoryProcessTemplate.FirstOrDefault(a => a.Code == item.PrepositionCode);
                            if (prepositiont != null)
                            {
                                processTemplateModel.PrepositionId = prepositiont.Id;
                            }
                            else
                            {
                                var prepositionModel = datalist.ToList().Find(x => x.Code == item.PrepositionCode);
                                if (prepositionModel != null)
                                {
                                    var prepositionEnt = new ProcessTemplate(_guidGenerator.Create());
                                    prepositionEnt.Name = prepositionModel.Name;
                                    prepositionEnt.Remark = prepositionModel.Remark;
                                    prepositionEnt.Content = prepositionModel.Content;
                                    prepositionEnt.Duration = prepositionModel.Duration;
                                    prepositionEnt.DurationUnit = (ServiceLifeUnit)Enum.Parse(typeof(ServiceLifeUnit), prepositionModel.DurationUnit, true);
                                    prepositionEnt.Type = (ProcessTypeEnum)Enum.Parse(typeof(ProcessTypeEnum), prepositionModel.Type, true);
                                    prepositionEnt.Unit = prepositionModel.Unit;
                                    await _repositoryProcessTemplate.InsertAsync(prepositionEnt);
                                    processTemplateModel.PrepositionId = prepositionEnt.Id;
                                    await uow.SaveChangesAsync();
                                }
                            }

                        }
                        await _repositoryProcessTemplate.UpdateAsync(processTemplateModel);
                        await uow.SaveChangesAsync(); 
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {

                        processTemplateModel = new ProcessTemplate(_guidGenerator.Create());
                        processTemplateModel.Name = item.Name;
                        processTemplateModel.Code = item.Code;
                        processTemplateModel.Remark = item.Remark;
                        processTemplateModel.Content = item.Content;
                        processTemplateModel.Duration = item.Duration;
                        processTemplateModel.DurationUnit = (ServiceLifeUnit)Enum.Parse(typeof(ServiceLifeUnit), item.DurationUnit, true);
                        processTemplateModel.Type = (ProcessTypeEnum)Enum.Parse(typeof(ProcessTypeEnum), item.Type, true);
                        processTemplateModel.Unit = item.Unit;
                        if (item.ParentCode != null && !string.IsNullOrEmpty(item.ParentCode))
                        {
                            var parent = _repositoryProcessTemplate.FirstOrDefault(a => a.Code == item.ParentCode);
                            if (parent == null)
                            {
                                var parentModel = datalist.ToList().Find(x => x.Code == item.ParentCode);
                                if (parentModel != null)
                                {
                                    var parentEnt = new ProcessTemplate(_guidGenerator.Create());
                                    parentEnt.Name = parentModel.Name;
                                    parentEnt.Code = parentModel.Code;
                                    parentEnt.Remark = parentModel.Remark;
                                    parentEnt.Content = parentModel.Content;
                                    parentEnt.Duration = parentModel.Duration;
                                    parentEnt.DurationUnit = (ServiceLifeUnit)Enum.Parse(typeof(ServiceLifeUnit), parentModel.DurationUnit, true);
                                    parentEnt.Type = (ProcessTypeEnum)Enum.Parse(typeof(ProcessTypeEnum), parentModel.Type, true);
                                    parentEnt.Unit = parentModel.Unit;
                                    await _repositoryProcessTemplate.InsertAsync(parentEnt);
                                    processTemplateModel.ParentId = parentEnt.Id;
                                    await uow.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                processTemplateModel.ParentId = parent.Id;
                            }
                        }
                        if (item.PrepositionCode != null && !string.IsNullOrEmpty(item.PrepositionCode) && processTemplateModel.PrepositionId.HasValue == false)
                        {
                            var prepositiont = _repositoryProcessTemplate.FirstOrDefault(a => a.Code == item.PrepositionCode);
                            if (prepositiont != null)
                            {
                                processTemplateModel.PrepositionId = prepositiont.Id;
                            }
                            else
                            {
                                var prepositionModel = datalist.ToList().Find(x => x.Code == item.PrepositionCode);
                                if (prepositionModel != null)
                                {
                                    var prepositionEnt = new ProcessTemplate(_guidGenerator.Create());
                                    prepositionEnt.Name = prepositionModel.Name;
                                    prepositionEnt.Code = prepositionModel.Code;
                                    prepositionEnt.Remark = prepositionModel.Remark;
                                    prepositionEnt.Content = prepositionModel.Content;
                                    prepositionEnt.Duration = prepositionModel.Duration;
                                    prepositionEnt.DurationUnit = (ServiceLifeUnit)Enum.Parse(typeof(ServiceLifeUnit), prepositionModel.DurationUnit, true);
                                    prepositionEnt.Type = (ProcessTypeEnum)Enum.Parse(typeof(ProcessTypeEnum), prepositionModel.Type, true);
                                    prepositionEnt.Unit = prepositionModel.Unit;
                                    await _repositoryProcessTemplate.InsertAsync(prepositionEnt);
                                    processTemplateModel.PrepositionId = prepositionEnt.Id;
                                    await uow.SaveChangesAsync();
                                }
                            }
                        }

                        await _repositoryProcessTemplate.InsertAsync(processTemplateModel);
                        await uow.SaveChangesAsync();
                    }
                }


                await _fileImport.Complete(input.ImportKey);
                if (wrongInfos.Any())
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
                }
            }
        }


        private List<ProcessTemplate> GetProcessTemplateData(ProcessTemplateGetListByIdsDto input)
        {
            var processTemplates = new List<ProcessTemplate>();
            var dtos = new List<ProcessTemplateDto>();

            //根据关键字查询
            if (!string.IsNullOrEmpty(input.KeyWords))
            {
                processTemplates = _repositoryProcessTemplate.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                dtos = ObjectMapper.Map<List<ProcessTemplate>, List<ProcessTemplateDto>>(processTemplates);
                foreach (var item in dtos)
                {
                    item.Children = null;
                }
            }
            else
            {
                processTemplates = _repositoryProcessTemplate.WithDetails()
                               .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                               .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
                               .ToList();
                dtos = ObjectMapper.Map<List<ProcessTemplate>, List<ProcessTemplateDto>>(processTemplates);
                foreach (var item in dtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<ProcessTemplateDto>();
                    }
                }
            }

            var resource = ObjectMapper.Map<List<ProcessTemplateDto>, List<ProcessTemplate>>(dtos);
            return resource;
        }


        private void FindAllParents(Guid parentid, ref List<ProcessTemplate> list)
        {
            var parentModel = _repositoryProcessTemplate.WithDetails().FirstOrDefault(a => a.Id == parentid);
            if (parentModel != null)
            {
                list.Add(parentModel);
                if (parentModel.ParentId != null && parentModel.ParentId != Guid.Empty)
                {
                    FindAllParents(parentModel.ParentId.Value, ref list);
                }
            }

        }
    }
}
