using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.Identity;
using SnAbp.Quality.Dtos;
using SnAbp.Quality.Entities;
using SnAbp.Quality.IServices;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Quality.Services
{
    public class QualityProblemLibraryAppService : QualityAppService, IQualityProblemLibraryAppService
    {
        private readonly IRepository<QualityProblemLibrary, Guid> _libraryRepository;
        private readonly IRepository<QualityProblemLibraryRltScop, Guid> _libraryRltScopRepository;
        private readonly IFileImportHandler _fileImport;
        readonly IUnitOfWorkManager _unitOfWork;
        readonly IDataFilter _dataFilter;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;
        private readonly IGuidGenerator _generator;
        public QualityProblemLibraryAppService(
            IRepository<QualityProblemLibrary, Guid> libraryReposityory,
            IRepository<QualityProblemLibraryRltScop, Guid> libraryRltScopRepository,
            IRepository<DataDictionary, Guid> dataDictionaries,
            IGuidGenerator generator,
            IFileImportHandler fileImport,
            IDataFilter dataFilter,
            IUnitOfWorkManager unitOfWork
            )
        {
            _libraryRepository = libraryReposityory;
            _libraryRltScopRepository = libraryRltScopRepository;
            _dataFilter = dataFilter;
            _unitOfWork = unitOfWork;
            _fileImport = fileImport;
            _dataDictionaries = dataDictionaries;
            _generator = generator;
        }
        /// <summary>
        /// 详情获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<QualityProblemLibraryDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var qualityProblemLibrary = _libraryRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (qualityProblemLibrary == null)
            {
                throw new UserFriendlyException("该质量问题不存在");
            }

            return Task.FromResult(ObjectMapper.Map<QualityProblemLibrary, QualityProblemLibraryDto>(qualityProblemLibrary));
        }

        /// <summary>
        /// 列表获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<QualityProblemLibraryDto>> GetList(QualityProblemLibrarySearchDto input)
        {
            var qualityProblemLibraries = _libraryRepository.WithDetails()
                   .WhereIf(!string.IsNullOrEmpty(input.Keyword), x =>
                            x.Content.Contains(input.Keyword) ||
                            x.Measures.Contains(input.Keyword) ||
                            x.Title.Contains(input.Keyword))
                   .WhereIf(input.Type != null, x => x.Type == input.Type)
                   .WhereIf(input.Level != null, x => x.Level == input.Level);

            var result = new PagedResultDto<QualityProblemLibraryDto>()
            {
                TotalCount = qualityProblemLibraries.Count(),
                Items = input.IsAll ? ObjectMapper.Map<List<QualityProblemLibrary>, List<QualityProblemLibraryDto>>(qualityProblemLibraries.OrderBy(x => x.Type).ThenBy(y => y.Level).ToList()) :
                                      ObjectMapper.Map<List<QualityProblemLibrary>, List<QualityProblemLibraryDto>>(qualityProblemLibraries.OrderBy(x => x.Type).ThenBy(y => y.Level).Skip(input.SkipCount).Take(input.MaxResultCount).ToList())
            };

            return Task.FromResult(result);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<QualityProblemLibraryDto> Create(QualityProblemLibraryCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Title.Trim())) throw new Volo.Abp.UserFriendlyException("质量问题标题不能为空");
            if (string.IsNullOrEmpty(input.Content.Trim())) throw new Volo.Abp.UserFriendlyException("质量问题内容不能为空");
            if (_libraryRepository.FirstOrDefault(x => x.Title == input.Title) != null) throw new Volo.Abp.UserFriendlyException("该问题标题已存在");

            var qualityProblemLibrary = new QualityProblemLibrary(_generator.Create())
            {
                Title = input.Title,
                Measures = input.Measures,
                Content = input.Content,
                ProfessionId = input.ProfessionId,
                Type = input.Type,
                Level = input.Level,

            };
            qualityProblemLibrary.Scops = new List<QualityProblemLibraryRltScop>();
            // 重新保存影响范围信息
            foreach (var scop in input.Scops)
            {
                qualityProblemLibrary.Scops.Add(new QualityProblemLibraryRltScop(_generator.Create())
                {
                    QualityProblemLibraryId = qualityProblemLibrary.Id,
                    ScopId = scop.ScopId
                });
            }

            await _libraryRepository.InsertAsync(qualityProblemLibrary);

            return ObjectMapper.Map<QualityProblemLibrary, QualityProblemLibraryDto>(qualityProblemLibrary);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public Task<Stream> Export(QualityProblemLibraryExportDto input)
        {
            var qualityProblemLibraries = _libraryRepository.WithDetails()
                   .WhereIf(!string.IsNullOrEmpty(input.Paramter.Keyword), x =>
                            x.Content.Contains(input.Paramter.Keyword) ||
                            x.Measures.Contains(input.Paramter.Keyword) ||
                            x.Title.Contains(input.Paramter.Keyword))
                   .WhereIf(input.Paramter.Type != null, x => x.Type == input.Paramter.Type);

            if (qualityProblemLibraries.Count() == 0)
            {
                throw new UserFriendlyException("数据为空!!!");
            }
            List<QualityProblemLibraryUploadTemplate> qualityProblemLibrarList = new List<QualityProblemLibraryUploadTemplate>();

            foreach (var item in qualityProblemLibraries)
            {
                QualityProblemLibraryUploadTemplate qualityProblemLibrar = new QualityProblemLibraryUploadTemplate();
                qualityProblemLibrar.Title = item.Title;
                qualityProblemLibrar.Profession = item.Profession?.Name;
                qualityProblemLibrar.Type = GetProblemTypeName(item.Type);
                qualityProblemLibrar.Level = GetProblemLevelName(item.Level);
                qualityProblemLibrar.Measures = item.Measures;
                qualityProblemLibrar.Content = item.Content;
                var _scopName = "";
                foreach (var scop in item.Scops)
                {
                    _scopName += scop.Scop.Name + '、';
                }
                if (!string.IsNullOrEmpty(_scopName))
                {
                    _scopName = _scopName.Substring(0, _scopName.Length - 1);
                }
                qualityProblemLibrar.Scops = _scopName;
                qualityProblemLibrarList.Add(qualityProblemLibrar);
            }

            var stream = ExcelHelper.ExcelExportStream(qualityProblemLibrarList, input.TemplateKey, input.RowIndex);
            return Task.FromResult(stream);
        }

        /// <summary>
        /// 导入文件上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<bool> Upload([FromForm] QualityProblemFileUploadDto input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            var rowIndex = 2; //有效数据额的起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<QualityProblemLibraryUploadTemplate> datalist = null;

            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<QualityProblemLibraryUploadTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<QualityProblemLibraryUploadTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }
            // 定义错误信息列
            var wrongInfos = new List<WrongInfo>();
            QualityProblemLibrary hasQualityProblemLibraryModel;
            var addQualityProblemLibrary = new List<QualityProblemLibrary>();

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
                    if (item.Level.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"事件类型不能为空");
                    }
                    if (item.Content.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"质量问题不能为空");
                    }
                    if (item.Profession.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"所属专业不能为空");
                    }
                    string[] levlels = new string[] { "一般质量事故", "重大质量事故", "质量问题" };
                    if (levlels.FindIndex(x => x == item.Level) == -1)
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"{item.Level}不属于问题等级");
                    }
                    string[] types = new string[] { "A类", "B类", "C类" };
                    if (types.FindIndex(x => x == item.Type) == -1)
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"{item.Type}不属于问题等级");
                    }

                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    using var uow = _unitOfWork.Begin(true);

                    var _scopsList = new List<DataDictionary>();
                    if (item.Scops != null && !string.IsNullOrWhiteSpace(item.Scops))
                    {
                        var _scops = item.Scops.Split('、');
                        var isScopName = false;
                        foreach (var scopName in _scops)
                        {
                            var res = _dataDictionaries.WithDetails().Where(x => x.Name == scopName && x.Key.StartsWith("QualityManager.Scop")).FirstOrDefault();

                            if (!string.IsNullOrEmpty(scopName) && res == null)
                            {
                                isScopName = true;
                                newInfo.AppendInfo($"使用范围:{scopName}在数据字典中不存在,格式：名字之间以、分开");
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

                    //判断所属专业是否存在
                    var profession = _dataDictionaries.FirstOrDefault(x => x.Name == item.Profession && x.Key.StartsWith("Profession"));
                    if (profession == null)
                    {
                        newInfo.AppendInfo($"所属专业:{item.Profession}在数据字典中不存在");
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        hasQualityProblemLibraryModel =
                            _libraryRepository.FirstOrDefault(a => a.Title == item.Title);
                    }
                    if (hasQualityProblemLibraryModel != null)
                    {
                        await _libraryRltScopRepository.DeleteAsync(a => a.QualityProblemLibraryId == hasQualityProblemLibraryModel.Id);
                        newInfo.AppendInfo($"{item.Title}已存在，且已更新");
                        hasQualityProblemLibraryModel.Title = item.Title;
                        hasQualityProblemLibraryModel.ProfessionId = profession.Id;
                        hasQualityProblemLibraryModel.Level = GetProblemLevel(item.Level);
                        hasQualityProblemLibraryModel.Type = GetProblemType(item.Type);
                        hasQualityProblemLibraryModel.Measures = item.Measures;
                        hasQualityProblemLibraryModel.Content = item.Content;
                        hasQualityProblemLibraryModel.Scops = new List<QualityProblemLibraryRltScop>();

                        foreach (var scop in _scopsList)
                        {
                            hasQualityProblemLibraryModel.Scops.Add(new QualityProblemLibraryRltScop(_generator.Create())
                            {
                                QualityProblemLibraryId = hasQualityProblemLibraryModel.Id,
                                ScopId = scop.Id
                            });
                        }

                        await _libraryRepository.UpdateAsync(hasQualityProblemLibraryModel);
                        await uow.SaveChangesAsync();
                        addQualityProblemLibrary.Add(hasQualityProblemLibraryModel);
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        hasQualityProblemLibraryModel = new QualityProblemLibrary(_generator.Create());
                        hasQualityProblemLibraryModel.Title = item.Title;
                        hasQualityProblemLibraryModel.ProfessionId = profession.Id;
                        hasQualityProblemLibraryModel.Level = GetProblemLevel(item.Level);
                        hasQualityProblemLibraryModel.Type = GetProblemType(item.Type);
                        hasQualityProblemLibraryModel.Measures = item.Measures;
                        hasQualityProblemLibraryModel.Content = item.Content;
                        hasQualityProblemLibraryModel.Scops = new List<QualityProblemLibraryRltScop>();

                        foreach (var scop in _scopsList)
                        {
                            hasQualityProblemLibraryModel.Scops.Add(new QualityProblemLibraryRltScop(_generator.Create())
                            {
                                ScopId = scop.Id
                            });
                        }
                        await _libraryRepository.InsertAsync(hasQualityProblemLibraryModel);
                        addQualityProblemLibrary.Add(hasQualityProblemLibraryModel);
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
            return true;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<QualityProblemLibraryDto> Update(QualityProblemLibraryUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Title.Trim())) throw new Volo.Abp.UserFriendlyException("质量问题标题不能为空");
            if (string.IsNullOrEmpty(input.Content.Trim())) throw new Volo.Abp.UserFriendlyException("质量问题内容不能为空");
            if (_libraryRepository.FirstOrDefault(x => x.Title == input.Title && x.Id != input.Id) != null) throw new Volo.Abp.UserFriendlyException("该问题标题已存在");

            var qualityProblemLibrary = _libraryRepository.FirstOrDefault(x => x.Id == input.Id);
            if (qualityProblemLibrary == null) throw new Volo.Abp.UserFriendlyException("更新对象不存在");

            qualityProblemLibrary.Title = input.Title;
            qualityProblemLibrary.Measures = input.Measures;
            qualityProblemLibrary.Content = input.Content;
            qualityProblemLibrary.Type = input.Type;
            qualityProblemLibrary.Level = input.Level;
            qualityProblemLibrary.ProfessionId = input.ProfessionId;

            // 清楚之前关联使用范围信息
            await _libraryRltScopRepository.DeleteAsync(x => x.QualityProblemLibraryId == qualityProblemLibrary.Id);

            qualityProblemLibrary.Scops = new List<QualityProblemLibraryRltScop>();
            // 重新保存关联使用范围信息
            foreach (var scop in input.Scops)
            {
                qualityProblemLibrary.Scops.Add(new QualityProblemLibraryRltScop(_generator.Create())
                {
                    QualityProblemLibraryId = qualityProblemLibrary.Id,
                    ScopId = scop.ScopId
                });
            }

            await _libraryRepository.UpdateAsync(qualityProblemLibrary);

            return ObjectMapper.Map<QualityProblemLibrary, QualityProblemLibraryDto>(qualityProblemLibrary);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            await _libraryRepository.DeleteAsync(id);

            return true;
        }


        /// <summary>
        /// 创建适用范围数据字典内容
        /// </summary>
        /// <param name="qualityId"></param>
        /// <param name="scopStrings"></param>
        /// <returns></returns>
        private async Task CreateScops(Guid qualityId, string scopStrings)
        {
            if (scopStrings.IsNullOrEmpty()) return;
            var arrs = scopStrings.Split(',');
            foreach (var item in arrs)
            {
                await _libraryRltScopRepository
                    .InsertAsync(new QualityProblemLibraryRltScop(_generator.Create())
                    {
                        QualityProblemLibraryId = qualityId,
                        ScopId = await GetOrAddDataDictionary(item, "QualityManager.Scop")
                    });
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
                await _dataDictionaries.InsertAsync(new Identity.DataDictionary(cid)
                {
                    ParentId = value.Id,
                    Name = name,
                    Key = $"{value.Key}.custom",
                    IsStatic = true,
                });
                await UnitOfWorkManager.Current.SaveChangesAsync();
                return cid;
            }
            throw new UserFriendlyException("请检查数据字典是否配置正确");
        }

        #region 私有放法

        /// <summary>
        /// 获取问题级别内容
        /// </summary>
        /// <returns></returns>
        private string GetProblemLevelName(QualityProblemLevel Type)
        {
            string typeName = null;

            if (Type == QualityProblemLevel.Great)
            {
                typeName = "重大质量事故";
            }
            if (Type == QualityProblemLevel.General)
            {
                typeName = "一般质量事故";
            }
            if (Type == QualityProblemLevel.Minor)
            {
                typeName = "质量问题";
            }
            return typeName;
        }

        /// <summary>
        /// 获取问题类型内容
        /// </summary>
        /// <returns></returns>
        private string GetProblemTypeName(QualityProblemType Type)
        {
            string typeName = null;

            if (Type == QualityProblemType.A)
            {
                typeName = "A类";
            }
            if (Type == QualityProblemType.B)
            {
                typeName = "B类";
            }
            if (Type == QualityProblemType.C)
            {
                typeName = "C类";
            }
            return typeName;
        }

        /// <summary>
        /// 获取问题级别枚举类型
        /// </summary>
        /// <returns></returns>
        private QualityProblemLevel GetProblemLevel(string title)
        {
            QualityProblemLevel level = QualityProblemLevel.General;

            if (title == "重大质量事故")
            {
                level = QualityProblemLevel.Great;
            }
            if (title == "一般质量事故")
            {
                level = QualityProblemLevel.General;
            }
            if (title == "质量问题")
            {
                level = QualityProblemLevel.Minor;
            }
            return level;
        }

        /// <summary>
        /// 获取问题类型内容
        /// </summary>
        /// <returns></returns>
        private QualityProblemType GetProblemType(string title)
        {
            QualityProblemType type = QualityProblemType.C;

            if (title == "A类")
            {
                type = QualityProblemType.A;
            }
            if (title == "B类")
            {
                type = QualityProblemType.B;
            }
            if (title == "C类")
            {
                type = QualityProblemType.C;
            }
            return type;
        }

        #endregion

    }
}
