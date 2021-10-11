using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.Project.Dtos;
using SnAbp.Project.Entities;
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
    public class ProjectDossierAppService : ProjectAppService, IProjectDossierAppService
    {
        private readonly IRepository<Dossier, Guid> _dossierRepository;
        private readonly IRepository<DossierRltFile, Guid> _dossierRltFileRepository;
        private readonly IGuidGenerator _guidGenerator;
        readonly IFileImportHandler _fileImport;
        private readonly IRepository<FileCategory, Guid> _fileCategoryRepository;
        readonly IUnitOfWorkManager _unitOfWork;
        readonly IDataFilter _dataFilter;
        public ProjectDossierAppService(
               IFileImportHandler fileImport,
        IRepository<FileCategory, Guid> fileCategoryRepository,
         IUnitOfWorkManager unitOfWork,
        IDataFilter dataFilter,
        IGuidGenerator guidGenerator,
            IRepository<DossierRltFile, Guid> dossierRltFileRepository,
            IRepository<Dossier, Guid> dossierRepository
            )
        {
            _fileCategoryRepository = fileCategoryRepository;
            _unitOfWork = unitOfWork;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _dossierRltFileRepository = dossierRltFileRepository;
            _dossierRepository = dossierRepository;
            _guidGenerator = guidGenerator;
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DossierDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的文件");
            var dossierCategory = _dossierRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (dossierCategory == null) throw new UserFriendlyException("当前文件不存在");
            var result = ObjectMapper.Map<Dossier, DossierDto>(dossierCategory);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<DossierDto>> GetList(DossierSearchDto input)
        {
            var result = new PagedResultDto<DossierDto>();
            var dossier = _dossierRepository.WithDetails()
                .Where(x => x.ArchivesId == input.ParentId)
                .WhereIf(!String.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name));

            var res = ObjectMapper.Map<List<Dossier>, List<DossierDto>>(dossier.OrderBy(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());

            result.Items = res;
            result.TotalCount = dossier.Count();
            return Task.FromResult(result);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DossierDto> Create(DossierCreateDto input)
        {
            DossierDto dossierDto = new DossierDto();
            var dossier = new Dossier();
            CheckSameCode(input.Code, input.ArchivesId, null);
            ObjectMapper.Map(input, dossier);
            dossier.SetId(_guidGenerator.Create());
            ///保存附件
            dossier.DossierRltFiles = new List<DossierRltFile>();
            foreach (var dossierRltFiles in input.DossierRltFiles)
            {
                dossier.DossierRltFiles.Add(
                    new DossierRltFile(_guidGenerator.Create())
                    {
                        FileId = dossierRltFiles.FileId,
                    });
            }
            await _dossierRepository.InsertAsync(dossier);
            dossierDto = ObjectMapper.Map<Dossier, DossierDto>(dossier);
            return dossierDto;
        }
        #region  修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<DossierDto> Update(DossierUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的文件");
            var dossier = await _dossierRepository.GetAsync(input.Id);
            if (dossier == null) throw new UserFriendlyException("当前文件不存在");
            //清除保存的附件关联表信息
            await _dossierRltFileRepository.DeleteAsync(x => x.DossierId == input.Id);

            CheckSameCode(input.Code, input.ArchivesId, input.Id);
            ObjectMapper.Map(input, dossier);
            ///保存附件
            dossier.DossierRltFiles = new List<DossierRltFile>();
            foreach (var dossierRltFiles in input.DossierRltFiles)
            {
                dossier.DossierRltFiles.Add(
                    new DossierRltFile(_guidGenerator.Create())
                    {
                        FileId = dossierRltFiles.FileId,
                    });
            }
            await _dossierRepository.UpdateAsync(dossier);
            return ObjectMapper.Map<Dossier, DossierDto>(dossier);
        }
        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _dossierRepository.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此文件不存在");
            await _dossierRepository.DeleteAsync(id);
            return true;
        }
        #region  导入
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task Upload([FromForm] DossierUploadDto input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            var rowIndex = 2; //有效数据额的起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<DossierTemplateDto> datalist = null;

            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<DossierTemplateDto>(rowIndex);
                datalist = sheet
                    .TryTransToList<DossierTemplateDto>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            var wrongInfos = new List<WrongInfo>();

            var allDossier = _dossierRepository.Where(s => s.IsDeleted == false && s.ArchivesId == input.ParentId);
            Dossier hasDossierModel;
            var addDossier = new List<Dossier>();

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
                    if (item.页号.ToString().IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("页号不能为空");
                    }
                    if (item.责任人.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("责任人不能为空");
                    }
                    if (item.文件题名.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("文件题名不能为空");
                    }
                    if (item.文件编号.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("文件编号不能为空");
                    }
                    if (item.文件日期.ToString().IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("文件日期不能为空");
                    }
                    if (item.文件分类.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("案卷号不能为空");
                    }
                    if (item.备注.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("备注不能为空");
                    }


                    var dossier = _dossierRepository.WithDetails().Where(x => x.ArchivesId == input.ParentId);
                    if (item.文件编号 != null)
                    {
                        dossier = dossier.Where(x => x.Code == item.文件编号);
                        if (dossier.Count() > 0)
                        {
                            canInsert = false;
                            newInfo.AppendInfo("文件编号已存在");
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
                        hasDossierModel =
                            _dossierRepository.FirstOrDefault(a => a.ArchivesId == input.ParentId && a.Code == item.文件编号);
                    }
                    //判断分类是否存在，如果不存在，创建
                    var fileCategory = _fileCategoryRepository.WithDetails().Where(x => x.Name == item.文件分类).FirstOrDefault();
                    if (fileCategory == null)
                    {
                        fileCategory = new FileCategory(_guidGenerator.Create());

                        fileCategory.Name = item.文件分类;
                        await _fileCategoryRepository.InsertAsync(fileCategory);
                        await uow.SaveChangesAsync();
                    }
                    if (hasDossierModel != null)
                    {
                        newInfo.AppendInfo($"{item.文件编号}已存在，且已更新");
                        hasDossierModel.Code = item.文件编号;
                        hasDossierModel.Remark = item.备注;
                        hasDossierModel.Date = item.文件日期;
                        hasDossierModel.Page = item.页号;
                        hasDossierModel.PersonName = item.责任人;
                        hasDossierModel.Name = item.文件题名;
                        hasDossierModel.FileCategoryId = fileCategory.Id;
                        hasDossierModel.IsDeleted = false;
                        await _dossierRepository.UpdateAsync(hasDossierModel);
                        await uow.SaveChangesAsync();
                        addDossier.Add(hasDossierModel);
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        hasDossierModel = new Dossier(_guidGenerator.Create());
                        hasDossierModel.Code = item.文件编号;
                        hasDossierModel.Remark = item.备注;
                        hasDossierModel.Date = item.文件日期;
                        hasDossierModel.Page = item.页号;
                        hasDossierModel.PersonName = item.责任人;
                        hasDossierModel.Name = item.文件题名;
                        hasDossierModel.FileCategoryId = fileCategory.Id;
                        hasDossierModel.ArchivesId = input.ParentId;
                        await _dossierRepository.InsertAsync(hasDossierModel);
                        addDossier.Add(hasDossierModel);
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

        #endregion
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Produces("application/octet-stream")]
        [HttpGet]
        public async Task<Stream> Export(DossierExportDto input)
        {
            // 1、获取导出的数据
            var result = new List<DossierDto>();
           var dossier = _dossierRepository.WithDetails()
                .Where(x => x.ArchivesId == input.ParentId)
               .WhereIf(!String.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name)).ToList();
            if (dossier.Count == 0)
            {
                throw new UserFriendlyException("当前案卷下面不存在文件!!!");
            }
            string TableName = "天津地铁11号线建设";
            string WorkOrganization = "中交机电局";
            string FileName = "施工文件";
            int count = dossier.Count();
            return NpoiWordExportService.SaveDossierWordFile(dossier, count, FileName, TableName, WorkOrganization);
        }
        #region
        bool CheckSameCode(string code, Guid? parentId, Guid? id)
        {
            var dossier = _dossierRepository.WithDetails()
                .Where(x => x.ArchivesId == parentId)
                .Where(x => x.Code == code);
            if (id.HasValue)
            {
                dossier = dossier.Where(x => x.Id != id.Value);
            }
            if (dossier.Count() > 0)
            {
                throw new UserFriendlyException("当前分类中已存在相同的文件号!!!");
            }
            return true;
        }
        #endregion

    }
}
