/**********************************************************************
*******命名空间： SnAbp.Safe.Services
*******类 名 称： SafeProblemLibraryAppService
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/7 15:33:55
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

using SnAbp.Common;
using SnAbp.Identity;
using SnAbp.Safe.Dtos;
using SnAbp.Safe.Entities;
using SnAbp.Safe.IServices;
using SnAbp.Utils;
using SnAbp.Utils.DataExport;
using SnAbp.Utils.DataImport;
using SnAbp.Utils.ExcelHelper;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Safe.Services
{

    /// <summary>
    /// 问题库管理服务
    /// </summary>
    public class SafeProblemLibraryAppService : SafeAppService, ISafeProblemLibraryAppService
    {
        readonly IRepository<SafeProblemLibrary, Guid> _safePoblemLibraries;
        readonly IRepository<SafeProblemLibraryRltScop, Guid> _safeProblemLibraryLibraryRlts;
        protected IDataDictionaryRepository _dataDictionaryRepository;
        readonly IFileImportHandler _fileImport;
        readonly IUnitOfWorkManager _unitOfWork;
        readonly IDataFilter _dataFilter;
        readonly IRepository<DataDictionary, Guid> _dataDictionaries;
        private readonly IGuidGenerator _guidGenerator;
        public SafeProblemLibraryAppService(
            IDataDictionaryRepository dataDictionaryRepository,
            IGuidGenerator guidGenerator,
            IRepository<SafeProblemLibrary, Guid> problemLibraries,
            IRepository<SafeProblemLibraryRltScop, Guid> safeProblemLibraryLibraryRlts,
            IRepository<DataDictionary, Guid> dataDictionaries,
            IFileImportHandler fileImport,
            IDataFilter dataFilter,
            IUnitOfWorkManager unitOfWork
            )
        {
            _dataDictionaryRepository = dataDictionaryRepository;
            _dataFilter = dataFilter;
            _unitOfWork = unitOfWork;
            _fileImport = fileImport;
            _guidGenerator = guidGenerator;
            _safePoblemLibraries = problemLibraries;
            _safeProblemLibraryLibraryRlts = safeProblemLibraryLibraryRlts;
            _fileImport = fileImport;
            _dataDictionaries = dataDictionaries;
        }
        /// <summary>
        /// 下载指定的文件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<Stream> Download(List<Guid> ids)
        {
            var list = _safePoblemLibraries.WithDetails(a => a.EventType, a => a.Scops)
                .Where(a => ids.Contains(a.Id))
                .ToList();
            // 组装文件 下载
            var headTitles = new string[]
            {
                "序号",
                "工作内容",
                "事件类型",
                "风险因素",
                "整改措施",
                "使用范围",
            };
            var workbook = DataExportHandler.CreateExcelFile(Utils.ExcelHelper.ExcelFileType.Xlsx);
            workbook.CreateSheet("安全问题库");
            var rowIndex = 0;
            var headRow = workbook.CreateRow(rowIndex);
            var cellStyle = workbook.CreateCellStyle(
                    CellBorder.CreateBorder(NPOI.SS.UserModel.BorderStyle.Thin, lineColor: HSSFColor.Black.Index));
            for (int i = 0; i < headTitles.Length; i++)
            {
                headRow.CreateCell(i)
                    .SetCellStyle(cellStyle)
                    .SetCellValue(headTitles[i]);
            }
            list?.ForEach(a =>
            {
                var row = workbook.CreateRow(++rowIndex);
                row.CreateCell(0).SetCellStyle(cellStyle).SetCellValue(list.IndexOf(a) + 1);
                row.CreateCell(1).SetCellStyle(cellStyle).SetCellValue(a.Title);
                row.CreateCell(2).SetCellStyle(cellStyle).SetCellValue(a.EventType.Name);
                row.CreateCell(3).SetCellStyle(cellStyle).SetCellValue(a.Content);
                row.CreateCell(4).SetCellStyle(cellStyle).SetCellValue(a.Measures);

                if (a.Scops != null && a.Scops.Any())
                {
                    var scops = a.Scops.Select(b => b.Scop.Name).JoinAsString(",");
                    row.CreateCell(5).SetCellStyle(cellStyle).SetCellValue(scops);
                }
            });
            Stream stream = workbook.GetExcelDataBuffer().BytesToStream();
            return Task.FromResult(stream);
        }



        public Task<SafeProblemLibraryDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");

            var safeProblemLibrary = _safePoblemLibraries.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (safeProblemLibrary == null) throw new UserFriendlyException("当前数据不存在");

            return Task.FromResult(ObjectMapper.Map<SafeProblemLibrary, SafeProblemLibraryDto>(safeProblemLibrary));
        }
        /// <summary>
        /// 新增一条问题库记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<SafeProblemLibraryDto> Create(SafeProblemLibraryCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Title.Trim())) throw new Volo.Abp.UserFriendlyException("工作内容不能为空");
            if (input.ProfessionId == null || input.ProfessionId == Guid.Empty) throw new Volo.Abp.UserFriendlyException("安全问题所属专业不能为空");
            if (input.EventTypeId == null || input.EventTypeId == Guid.Empty) throw new Volo.Abp.UserFriendlyException("安全问题事件类型不能为空");
            if (string.IsNullOrEmpty(input.Content.Trim())) throw new Volo.Abp.UserFriendlyException("安全问题风险因素不能为空");
            if (_safePoblemLibraries.FirstOrDefault(x => x.Title == input.Title && x.Id != input.Id) != null) throw new Volo.Abp.UserFriendlyException("该工作内容的问题已存在");

            var safeProblemLibrary = ObjectMapper.Map<SafeProblemLibraryCreateDto, SafeProblemLibrary>(input);
            safeProblemLibrary.SetId(_guidGenerator.Create());
            safeProblemLibrary.Scops = new List<SafeProblemLibraryRltScop>();

            foreach (var scop in input.Scops)
            {
                safeProblemLibrary.Scops.Add(new SafeProblemLibraryRltScop(_guidGenerator.Create())
                {
                    SafeProblemLibraryId = safeProblemLibrary.Id,
                    ScopId = scop.ScopId
                });
            }

            await _safePoblemLibraries.InsertAsync(safeProblemLibrary);

            return ObjectMapper.Map<SafeProblemLibrary, SafeProblemLibraryDto>(safeProblemLibrary);
        }

        public Task<PagedResultDto<SafeProblemLibraryDto>> GetList(SafeProblemLibrarySearchDto input)
        {
            var safeProblemLibraries = _safePoblemLibraries.WithDetails()
                 .WhereIf(!string.IsNullOrEmpty(input.Keyword), x =>
                          x.Content.Contains(input.Keyword) ||
                          x.Measures.Contains(input.Keyword) ||
                          x.Title.Contains(input.Keyword))
                 .WhereIf(input.RiskLevel != null, x => x.RiskLevel == input.RiskLevel)
                 .WhereIf(input.EventTypeId != null && input.EventTypeId != Guid.Empty, x => x.EventTypeId == input.EventTypeId);

            var result = new PagedResultDto<SafeProblemLibraryDto>()
            {
                TotalCount = safeProblemLibraries.Count(),
                Items = input.IsAll ? ObjectMapper.Map<List<SafeProblemLibrary>, List<SafeProblemLibraryDto>>(safeProblemLibraries.OrderBy(x => x.RiskLevel).ThenBy(y => y.EventType.Name).ToList()) :
                                      ObjectMapper.Map<List<SafeProblemLibrary>, List<SafeProblemLibraryDto>>(safeProblemLibraries.OrderBy(x => x.RiskLevel).ThenBy(y => y.EventType.Name).Skip(input.SkipCount).Take(input.MaxResultCount).ToList())
            };

            return Task.FromResult(result);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SafeProblemLibraryDto> Update(SafeProblemLibraryUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Title.Trim())) throw new Volo.Abp.UserFriendlyException("安全问题标题不能为空");
            if (input.EventTypeId == null || input.EventTypeId == Guid.Empty) throw new Volo.Abp.UserFriendlyException("安全问题类型不能为空");
            if (string.IsNullOrEmpty(input.Content.Trim())) throw new Volo.Abp.UserFriendlyException("安全问题内容不能为空");
            if (_safePoblemLibraries.FirstOrDefault(x => x.Title == input.Title && x.Id != input.Id) != null) throw new Volo.Abp.UserFriendlyException("该问题标题已存在");

            var safeProblemLibrary = _safePoblemLibraries.FirstOrDefault(x => x.Id == input.Id);
            if (safeProblemLibrary == null) throw new Volo.Abp.UserFriendlyException("更新对象不存在");
            ObjectMapper.Map(input, safeProblemLibrary);

            // 清楚之前关联使用范围信息
            await _safeProblemLibraryLibraryRlts.DeleteAsync(x => x.SafeProblemLibraryId == safeProblemLibrary.Id);

            safeProblemLibrary.Scops = new List<SafeProblemLibraryRltScop>();
            // 重新保存关联使用范围信息
            foreach (var scop in input.Scops)
            {
                safeProblemLibrary.Scops.Add(new SafeProblemLibraryRltScop(_guidGenerator.Create())
                {
                    SafeProblemLibraryId = safeProblemLibrary.Id,
                    ScopId = scop.ScopId
                });
            }

            await _safePoblemLibraries.UpdateAsync(safeProblemLibrary);

            return ObjectMapper.Map<SafeProblemLibrary, SafeProblemLibraryDto>(safeProblemLibrary);
        }

        /// <summary>
        /// 导入文件上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task Upload([FromForm] SafeProblemFileUploadDto input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            var rowIndex = 2; //有效数据额的起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<SafeProblemLibraryUploadTemplate> datalist = null;

            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<SafeProblemLibraryUploadTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<SafeProblemLibraryUploadTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }
            // 定义错误信息列
            var wrongInfos = new List<WrongInfo>();
            SafeProblemLibrary hasSafeProblemLibraryModel;
            var addSafeProblemLibrary = new List<SafeProblemLibrary>();

            if (datalist.Any())
            {
                await _fileImport.ChangeTotalCount(input.ImportKey, count: datalist.Count());
                var updateIndex = 1;

                foreach (var item in datalist)
                {
                    await _fileImport.UpdateState(input.ImportKey, updateIndex);
                    updateIndex++;
                    var canInsert = true;
                    var newInfo = new WrongInfo(item.Index);
                    if (item.Title.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"工作内容不能为空");
                    }
                    if (item.EventType.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"事件类型不能为空");
                    }
                    if (item.Content.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"风险因素不能为空");
                    }
                    if (item.Profession.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"所属专业不能为空");
                    }
                    if (item.RiskLevel.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"风险等级不能为空");
                    }
                    string[] levlels = new string[] { "特别重大事故", "重大事故", "较大事故", "一般事故" };
                    if (levlels.FindIndex(x => x == item.RiskLevel) == -1)
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"{item.RiskLevel}不属于问题等级");
                    }
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    using var uow = _unitOfWork.Begin(true);
                    //判断事件类型是否存在
                    var enentType = _dataDictionaryRepository.WithDetails().Where(x => x.Name == item.EventType && x.Key.StartsWith("SafeManager.EventType")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(item.EventType) && enentType == null)
                    {
                        newInfo.AppendInfo($"事件类型:{item.EventType}不存在");
                        wrongInfos.Add(newInfo);
                        continue;

                    }

                    //判断所属专业是否存在
                    var profession = _dataDictionaries.FirstOrDefault(x => x.Name == item.Profession && x.Key.StartsWith("Profession"));
                    if (profession == null)
                    {
                        newInfo.AppendInfo($"所属专业:{item.Profession}在数据字典中不存在");
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    var _scopsList = new List<DataDictionary>();
                    if (item.Scops != null && !string.IsNullOrEmpty(item.Scops))
                    {
                        var _scops = item.Scops.Split('、');
                        var isScopName = false;
                        foreach (var scopName in _scops)
                        {
                            var res = _dataDictionaryRepository.WithDetails().Where(x => x.Name == scopName && x.Key.StartsWith("SafeManager.Scop")).FirstOrDefault();

                            if (!string.IsNullOrEmpty(scopName) && res == null)
                            {
                                isScopName = true;
                                newInfo.AppendInfo($"使用范围:{scopName}不存在,格式：名字之间以、分开");
                                wrongInfos.Add(newInfo);
                                continue;

                            }
                            else
                            {
                                _scopsList.Add(res);
                            }
                        }
                        if (isScopName == true)
                        {
                            continue;
                        }
                    }

                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        hasSafeProblemLibraryModel =
                            _safePoblemLibraries.FirstOrDefault(a => a.Title.Contains(item.Title));
                    }
                    if (hasSafeProblemLibraryModel != null)
                    {
                        await _safeProblemLibraryLibraryRlts.DeleteAsync(a => a.SafeProblemLibraryId == hasSafeProblemLibraryModel.Id);
                        newInfo.AppendInfo($"{item.Title}已存在，且已更新");
                        hasSafeProblemLibraryModel.Title = item.Title;
                        hasSafeProblemLibraryModel.EventTypeId = enentType.Id;
                        hasSafeProblemLibraryModel.ProfessionId = profession.Id;
                        hasSafeProblemLibraryModel.RiskLevel = GetProblemRiskLevel(item.RiskLevel);
                        hasSafeProblemLibraryModel.Measures = item.Measures;
                        hasSafeProblemLibraryModel.Content = item.Content;
                        hasSafeProblemLibraryModel.Scops = new List<SafeProblemLibraryRltScop>();

                        if (_scopsList.Count > 0)
                        {
                            foreach (var scop in _scopsList)
                            {
                                hasSafeProblemLibraryModel.Scops.Add(new SafeProblemLibraryRltScop(_guidGenerator.Create())
                                {
                                    SafeProblemLibraryId = hasSafeProblemLibraryModel.Id,
                                    ScopId = scop.Id
                                });
                            }

                        }


                        await _safePoblemLibraries.UpdateAsync(hasSafeProblemLibraryModel);
                        await uow.SaveChangesAsync();
                        addSafeProblemLibrary.Add(hasSafeProblemLibraryModel);
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        hasSafeProblemLibraryModel = new SafeProblemLibrary(_guidGenerator.Create());
                        hasSafeProblemLibraryModel.Title = item.Title;
                        hasSafeProblemLibraryModel.EventTypeId = enentType.Id;
                        hasSafeProblemLibraryModel.ProfessionId = profession.Id;
                        hasSafeProblemLibraryModel.RiskLevel = GetProblemRiskLevel(item.RiskLevel);
                        hasSafeProblemLibraryModel.Measures = item.Measures;
                        hasSafeProblemLibraryModel.Content = item.Content;
                        hasSafeProblemLibraryModel.Scops = new List<SafeProblemLibraryRltScop>();

                        foreach (var scop in _scopsList)
                        {
                            hasSafeProblemLibraryModel.Scops.Add(new SafeProblemLibraryRltScop(_guidGenerator.Create())
                            {
                                ScopId = scop.Id
                            });
                        }
                        await _safePoblemLibraries.InsertAsync(hasSafeProblemLibraryModel);
                        addSafeProblemLibrary.Add(hasSafeProblemLibraryModel);
                        await uow.SaveChangesAsync();
                    }
                }
                await _fileImport.Complete(input.ImportKey);
                if (wrongInfos.Any())
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(useId: CurrentUser.Id.GetValueOrDefault(), input.ImportKey,
                        fileBytes: workbook.ConvertToBytes());
                }
            }
        }



        /// <summary>
        /// 创建适用范围数据字典内容
        /// </summary>
        /// <param name="safeId"></param>
        /// <param name="scopStrings"></param>
        /// <returns></returns>

        private async Task CreateScops(Guid safeId, string scopStrings)
        {
            if (scopStrings.IsNullOrEmpty()) return;
            var arrs = scopStrings.Split(',');
            foreach (var item in arrs)
            {
                await _safeProblemLibraryLibraryRlts
                    .InsertAsync(new SafeProblemLibraryRltScop(safeId, await GetOrAddDataDictionary(item, "SafeManager.Scop")));
            }
            return;
        }
        /// <summary>
        /// 获取数据字典id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<Guid> GetOrAddDataDictionary(string name, string key)
        {
            var value = _dataDictionaries.FirstOrDefault(a => a.Key == key);
            if (value != null)
            {
                var datas = _dataDictionaries.Where(a => a.ParentId == value.Id).ToList();
                var checkKey = datas.FirstOrDefault(a => a.Name == name);
                if (checkKey != null) return checkKey.Id;
                // 没有名称，则进行新增
                var cid = GuidGenerator.Create();
                var num = "";
                var date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                num = date.Replace("-", "");
                await _dataDictionaries.InsertAsync(new Identity.DataDictionary(cid)
                {
                    ParentId = value.Id,
                    Name = name,
                    Key = $"{value.Key}.${num}",
                    IsStatic = true,
                });
                await UnitOfWorkManager.Current.SaveChangesAsync();
                return cid;
            }
            throw new UserFriendlyException("请检查数据字典是否配置正确");
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        public Task<Stream> DownloadUploadTemplate()
        {
            var headTitles = new string[]
           {
                "序号",
                "工作内容",
                "事件类型",
                "问题内容",
                "整改措施",
                "使用范围",
           };
            var workbook = DataExportHandler.CreateExcelFile(Utils.ExcelHelper.ExcelFileType.Xlsx);
            workbook.CreateSheet("安全问题库导入模板");
            var rowIndex = 0;
            var headRow = workbook.CreateRow(rowIndex);
            var cellStyle = workbook.CreateCellStyle(
                    CellBorder.CreateBorder(NPOI.SS.UserModel.BorderStyle.Thin, lineColor: HSSFColor.Black.Index));
            for (int i = 0; i < headTitles.Length; i++)
            {
                headRow.CreateCell(i)
                    .SetCellStyle(cellStyle)
                    .SetCellValue(headTitles[i]);
            }
            var row = workbook.CreateRow(++rowIndex);
            row.CreateCell(0).SetCellStyle(cellStyle).SetCellValue(1);
            row.CreateCell(1).SetCellStyle(cellStyle).SetCellValue("示例：工作内容xx");
            row.CreateCell(2).SetCellStyle(cellStyle).SetCellValue("示例：事件类型xx");
            row.CreateCell(3).SetCellStyle(cellStyle).SetCellValue("示例：问题内容xx");
            row.CreateCell(4).SetCellStyle(cellStyle).SetCellValue("示例：整改措施xx");
            row.CreateCell(5).SetCellStyle(cellStyle).SetCellValue("示例：使用范围xx，多个用英文逗号隔开");
            Stream stream = workbook.GetExcelDataBuffer().BytesToStream();
            return Task.FromResult(stream);
        }
        public Task<Stream> Export(SafeProblemLibraryExportDto input)
        {
            var safeProblemLibraries = _safePoblemLibraries.WithDetails()
                   .WhereIf(!string.IsNullOrEmpty(input.Paramter.Keyword), x =>
                            x.Content.Contains(input.Paramter.Keyword) ||
                            x.Measures.Contains(input.Paramter.Keyword) ||
                            x.Title.Contains(input.Paramter.Keyword))
                   .WhereIf(input.Paramter.EventTypeId != null && input.Paramter.EventTypeId != Guid.Empty, x => x.EventTypeId == input.Paramter.EventTypeId);
            if (safeProblemLibraries.Count() == 0)
            {
                throw new UserFriendlyException("数据为空!!!");
            }
            List<SafeProblemLibraryUploadTemplate> safeProblemLibrarList = new List<SafeProblemLibraryUploadTemplate>();

            foreach (var item in safeProblemLibraries)
            {
                SafeProblemLibraryUploadTemplate safeProblemLibrar = new SafeProblemLibraryUploadTemplate();
                safeProblemLibrar.Title = item.Title;
                safeProblemLibrar.RiskLevel = GetProblemRiskLevelName(item.RiskLevel);
                safeProblemLibrar.EventType = item.EventType != null ? item.EventType.Name : "";
                safeProblemLibrar.Profession = item.Profession != null ? item.Profession.Name : "";
                safeProblemLibrar.Measures = item.Measures;
                safeProblemLibrar.Content = item.Content;
                var _scopName = "";
                foreach (var scop in item.Scops)
                {
                    _scopName += scop.Scop.Name + '、';
                }
                if (!string.IsNullOrEmpty(_scopName))
                {
                    _scopName = _scopName.Substring(0, _scopName.Length - 1);
                }
                safeProblemLibrar.Scops = _scopName;
                safeProblemLibrarList.Add(safeProblemLibrar);
            }

            var stream = ExcelHelper.ExcelExportStream(safeProblemLibrarList, input.TemplateKey, input.RowIndex);
            return Task.FromResult(stream);
        }
        /// <summary>
        /// 删除指定数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            await _safePoblemLibraries.DeleteAsync(id);
            return true;
        }

        #region
        /// <summary>
        /// 获取问题风险等级内容
        /// </summary>
        /// <returns></returns>
        private string GetProblemRiskLevelName(SafetyRiskLevel level)
        {
            string Name = null;

            if (level == SafetyRiskLevel.Especially)
            {
                Name = "特别重大事故";
            }
            if (level == SafetyRiskLevel.Great)
            {
                Name = "重大事故";
            }
            if (level == SafetyRiskLevel.Larger)
            {
                Name = "较大事故";
            }
            if (level == SafetyRiskLevel.General)
            {
                Name = "一般事故";
            }
            return Name;
        }

        /// <summary>
        /// 获取问题风险等级
        /// </summary>
        /// <returns></returns>
        private SafetyRiskLevel GetProblemRiskLevel(string name)
        {
            SafetyRiskLevel level = SafetyRiskLevel.General;

            if (name == "特别重大事故")
            {
                level = SafetyRiskLevel.Especially;
            }
            if (name == "重大事故")
            {
                level = SafetyRiskLevel.Great;
            }
            if (name == "较大事故")
            {
                level = SafetyRiskLevel.Larger;
            }
            if (name == "一般事故")
            {
                level = SafetyRiskLevel.General;
            }
            return level;
        }
        #endregion
    }
}
