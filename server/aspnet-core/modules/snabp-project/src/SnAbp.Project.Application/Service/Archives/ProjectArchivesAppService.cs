using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.Project.Dtos;
using SnAbp.Project.Entities;
using SnAbp.Project.enums;
using SnAbp.Project.IServices;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
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

namespace SnAbp.Project.Service
{
    public class ProjectArchivesAppService : ProjectAppService, IProjectArchivesAppService
    {
        private readonly IRepository<Archives, Guid> _archivesRepository;
        private readonly IGuidGenerator _guidGenerator;
        readonly IFileImportHandler _fileImport;
        private readonly IRepository<BooksClassification, Guid> _booksClassificationRepository;
        readonly IUnitOfWorkManager _unitOfWork;
        readonly IDataFilter _dataFilter;
        public ProjectArchivesAppService(
                 IGuidGenerator guidGenerator,
                 IRepository<Archives, Guid> archivesRepository,
                 IFileImportHandler fileImport,
                  IRepository<BooksClassification, Guid> booksClassificationRepository,
                  IUnitOfWorkManager unitOfWork,
            IDataFilter dataFilter
                 )
        {
            _booksClassificationRepository = booksClassificationRepository;
            _unitOfWork = unitOfWork;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _archivesRepository = archivesRepository;
            _guidGenerator = guidGenerator;
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ArchivesDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的档案");
            var archives = _archivesRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (archives == null) throw new UserFriendlyException("当前档案不存在");
            var result = ObjectMapper.Map<Archives, ArchivesDto>(archives);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<ArchivesDto>> GetList(ArchivesSearchDto input)
        {
            var result = new PagedResultDto<ArchivesDto>();
            var archives = _archivesRepository.WithDetails()
                .Where(x => x.ArchivesCategoryId == input.ParentId)
                .WhereIf(!String.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name));

            var res = ObjectMapper.Map<List<Archives>, List<ArchivesDto>>(archives.OrderBy(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());

            result.Items = res;
            result.TotalCount = archives.Count();
            return Task.FromResult(result);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ArchivesDto> Create(ArchivesCreateDto input)
        {
            ArchivesDto archivesDto = new ArchivesDto();
            var archives = new Archives();
            CheckSameCode(input.ArchivesFilesCode, input.ProjectCode, input.FileCode, input.ArchivesCategoryId, null);
            ObjectMapper.Map(input, archives);
            archives.SetId(_guidGenerator.Create());
            await _archivesRepository.InsertAsync(archives);
            archivesDto = ObjectMapper.Map<Archives, ArchivesDto>(archives);
            return archivesDto;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _archivesRepository.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此档案不存在");
            await _archivesRepository.DeleteAsync(id);
            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ArchivesDto> Update(ArchivesUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的档案");
            var archives = await _archivesRepository.GetAsync(input.Id);
            if (archives == null) throw new UserFriendlyException("当前档案不存在");
            CheckSameCode(input.ArchivesFilesCode, input.ProjectCode, input.FileCode, input.ArchivesCategoryId, input.Id);
            ObjectMapper.Map(input, archives);
            await _archivesRepository.UpdateAsync(archives);
            return ObjectMapper.Map<Archives, ArchivesDto>(archives);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Produces("application/octet-stream")]
        [HttpGet]
        public async Task<Stream> Export(ArchivesExportDto input)
        {
            //1、获取需要导出的所有数据
            var archives = _archivesRepository.WithDetails()
                .Where(x => x.ArchivesCategoryId == input.ParentId)
                .WhereIf(!String.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name)).ToList();
            if (archives.Count == 0)
            {
                throw new UserFriendlyException("当前档案下面不存在案卷!!!");
            }
            string TableName = "天津地铁11号线建设";
            string WorkOrganization = "中交机电局";
            string FileName = "施工档案案卷目录";
            int count = archives.Count();
            return NpoiWordExportService.SaveArchivesWordFile(archives, count,FileName, TableName,WorkOrganization);
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task Upload([FromForm] ArchivesUploadDto input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            var rowIndex = 2; //有效数据额的起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<ArchivesTemplateDto> datalist = null;
         
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<ArchivesTemplateDto>(rowIndex);
                datalist = sheet
                    .TryTransToList<ArchivesTemplateDto>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            var wrongInfos = new List<WrongInfo>();

            var allArchives = _archivesRepository.Where(s => s.IsDeleted == false && s.ArchivesCategoryId == input.ParentId);
            Archives hasArchivesModel;
            var addArchives = new List<Archives>();

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
                    if (item.份数.ToString().IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("份数不能为空");
                    }
                    if (item.宗号.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("宗号不能为空");
                    }
                    if (item.密级.ToString().IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("密级不能为空");
                    }
                    if (!item.密级.IsIn((int)Security.Common, (int)Security.Secret, (int)Security.Strice))
                    {
                        canInsert = false;
                        newInfo.AppendInfo("密级程度只能为1，2,3");
                    }
                    if (item.年度.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("年度不能为空");
                    }
                    if (item.案卷分类.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("案卷分类不能为空");
                    }
                    if (item.案卷号.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("案卷号不能为空");
                    }
                    if (item.案卷题名.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("案卷题名不能为空");
                    }
                    if (item.档号.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("档号不能为空");
                    }
                    if (item.编制单位.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("编制单位不能为空");
                    }
                    if (item.编制日期.ToString().IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("编制日期不能为空");
                    }
                    if (item.页数.ToString().IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("页数不能为空");
                    }

                    var archives = _archivesRepository.WithDetails().Where(x => x.ArchivesCategoryId == input.ParentId);
                    if (item.宗号 != null)
                    {
                        archives = archives.Where(x => x.ArchivesFilesCode == item.宗号);
                        if (archives.Count() > 0)
                        {
                            canInsert = false;
                            newInfo.AppendInfo("宗号已存在");
                        }
                    }
                    if (item.档号 != null)
                    {
                        archives = archives.Where(x => x.ProjectCode == item.档号);

                        if (archives.Count() > 0)
                        {
                            canInsert = false;
                            newInfo.AppendInfo("档号已存在");
                        }
                    }
                    if (item.案卷号 != null)
                    {
                        archives = archives.Where(x => x.FileCode == item.案卷号);
                        if (archives.Count() > 0)
                        {
                            canInsert = false;
                            newInfo.AppendInfo("案卷号已存在");
                        }
                    }
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    using var uow = _unitOfWork.Begin(true);
                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        hasArchivesModel =
                            _archivesRepository.FirstOrDefault(a => a.ArchivesCategoryId == input.ParentId && a.FileCode == item.宗号 && a.ProjectCode == item.档号 && a.ArchivesFilesCode == item.案卷号);
                    }
                    //判断分类是否存在，如果不存在，创建
                    var booksClassification = _booksClassificationRepository.WithDetails().Where(x => x.Name == item.案卷分类).FirstOrDefault();
                    if (booksClassification == null)
                    {
                        booksClassification = new BooksClassification(_guidGenerator.Create());

                        booksClassification.Name = item.案卷分类;
                        await _booksClassificationRepository.InsertAsync(booksClassification);
                        await uow.SaveChangesAsync();
                    }                
                    if (hasArchivesModel != null)
                    {
                        newInfo.AppendInfo($"{item.案卷题名}已存在，且已更新");
                        hasArchivesModel.FileCode = item.宗号;
                        hasArchivesModel.ProjectCode = item.档号;
                        hasArchivesModel.ArchivesFilesCode = item.案卷号;
                        hasArchivesModel.Name = item.案卷题名;
                        hasArchivesModel.Date = item.编制日期;
                        hasArchivesModel.Unit = item.编制单位;
                        hasArchivesModel.Year = item.年度;
                        hasArchivesModel.Page = item.页数;
                        hasArchivesModel.Copies = item.份数;
                        hasArchivesModel.Remark = item.备注;
                        hasArchivesModel.Security = (enums.Security)item.密级;
                        hasArchivesModel.BooksClassificationId = booksClassification.Id;
                        hasArchivesModel.IsDeleted = false;
                        await _archivesRepository.UpdateAsync(hasArchivesModel);
                        await uow.SaveChangesAsync();
                        addArchives.Add(hasArchivesModel);
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        var archivesAdd = new Archives();
                        ArchivesDto archivesDto = new ArchivesDto();

                        hasArchivesModel = new Archives(_guidGenerator.Create());
                        hasArchivesModel.FileCode = item.宗号;
                        hasArchivesModel.ProjectCode = item.档号;
                        hasArchivesModel.ArchivesFilesCode = item.案卷号;
                        hasArchivesModel.Name = item.案卷题名;
                        hasArchivesModel.Date = item.编制日期;
                        hasArchivesModel.Unit = item.编制单位;
                        hasArchivesModel.Year = item.年度;
                        hasArchivesModel.Page = item.页数;
                        hasArchivesModel.Remark = item.备注;
                        hasArchivesModel.Copies = item.份数;
                        hasArchivesModel.BooksClassificationId = booksClassification.Id;
                        hasArchivesModel.Security = (enums.Security)item.密级;
                        hasArchivesModel.ArchivesCategoryId = input.ParentId;
                       
                        await _archivesRepository.InsertAsync(hasArchivesModel);
                        addArchives.Add(hasArchivesModel);
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

        #region 私有方法
        List<ArchivesCategory> GetChildren(List<ArchivesCategory> data, Guid? id)
        {
            var archivesCategory = new List<ArchivesCategory>();
            var children = data.Where(x => x.ParentId == id);
            foreach (var item in children)
            {
                var node = new ArchivesCategory(item.Id);
                node.Name = item.Name;
                node.Order = item.Order;
                node.ParentId = item.ParentId;
                node.Remark = item.Remark;
                node.Children = GetChildren(data, item.Id);
                archivesCategory.Add(node);
            }
            return archivesCategory;
        }
        bool CheckSameCode(string ArchivesFilesCode, string ProjectCode, string FileCode, Guid? parentId, Guid? Id)
        {
            var archives = _archivesRepository.WithDetails().Where(x => x.ArchivesCategoryId == parentId);
            if (ArchivesFilesCode != null)
            {
                archives = archives.Where(x => x.ArchivesFilesCode == ArchivesFilesCode);
                if (Id.HasValue)
                {
                    archives = archives.Where(x => x.Id != Id.Value);
                }
                if (archives.Count() > 0)
                {
                    throw new UserFriendlyException("当前分类中已存在相同名字的案卷号!!!");
                }
            }
            if (ProjectCode != null)
            {
                archives = archives.Where(x => x.ProjectCode == ProjectCode);
                if (Id.HasValue)
                {
                    archives = archives.Where(x => x.Id != Id.Value);
                }
                if (archives.Count() > 0)
                {
                    throw new UserFriendlyException("当前分类中已存在相同名字的档号!!!");
                }
            }
            if (FileCode != null)
            {
                archives = archives.Where(x => x.FileCode == FileCode);
                if (Id.HasValue)
                {
                    archives = archives.Where(x => x.Id != Id.Value);
                }
                if (archives.Count() > 0)
                {
                    throw new UserFriendlyException("当前分类中已存在相同名字的宗号!!!");
                }
            }
            return true;
        }




        #endregion

    }
}
