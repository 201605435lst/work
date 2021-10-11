using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices.Model.ModelMVD;
using SnAbp.StdBasic.TempleteModel;
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

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    /// <summary>
    /// 信息交换模板分类
    /// </summary>
    public class StdBasicMVDCategoryAppService : StdBasicAppService, IStdBasicMVDCategoryAppService
    {
        private readonly IRepository<MVDCategory, Guid> _repository;
        private readonly IRepository<MVDProperty, Guid> _repositoryMVDProperty;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IUnitOfWorkManager _unitOfWork;
        private readonly IDataFilter _dataFilter;

        public StdBasicMVDCategoryAppService(IRepository<MVDCategory, Guid> repository, IRepository<MVDProperty, Guid> repositoryMVDProperty, IGuidGenerator guidGenerator, IFileImportHandler fileImport, IUnitOfWorkManager unitOfWork, IDataFilter dataFilter)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _unitOfWork = unitOfWork;
            _dataFilter = dataFilter;
            _repositoryMVDProperty = repositoryMVDProperty;
        }

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.MVDCategory.Detail)]
        public Task<MVDCategoryDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var entity = _repository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (entity == null) throw new UserFriendlyException("该实体不存在！");
            return Task.FromResult(ObjectMapper.Map<MVDCategory, MVDCategoryDto>(entity));
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<MVDCategoryDto>> GetList(MVDSearchDto input)
        {
            var entityList = _repository.WithDetails().WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords) || x.Code.Contains(input.Keywords));
            var res = new PagedResultDto<MVDCategoryDto>()
            {
                TotalCount = entityList.Count(),
                Items = input.IsAll ?
                    ObjectMapper.Map<List<MVDCategory>, List<MVDCategoryDto>>(entityList
                    .OrderBy(x => x.Order)
                    .ThenBy(x => x.Name)
                    .ToList()
                    ) : ObjectMapper.Map<List<MVDCategory>, List<MVDCategoryDto>>(entityList.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(x => x.Order).ThenBy(x => x.Name).ToList()),
            };
            return Task.FromResult(res);
        }

        [HttpPost]
        public Task<PagedResultDto<MVDCategoryDto>> GetListTree(MVDSearchDto input)
        {
            var entityList = _repository.WithDetails().WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords) || x.Code.Contains(input.Keywords));
            var res = new PagedResultDto<MVDCategoryDto>()
            {
                TotalCount = entityList.Count(),
                Items = input.IsAll ? ObjectMapper.Map<List<MVDCategory>, List<MVDCategoryDto>>(entityList.ToList().OrderBy(x => x.Order).ThenBy(x => x.Name).ToList()) : ObjectMapper.Map<List<MVDCategory>, List<MVDCategoryDto>>(entityList.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(x => x.Order).ThenBy(x => x.Name).ToList()),
            };
            foreach (var item in res.Items)
            {
                var list = _repositoryMVDProperty.WithDetails().Where(x => x.MVDCategoryId == item.Id).OrderBy(x => x.Order).ThenBy(x => x.Name).ToList();
                if (list.Count == 0)
                {
                    item.Children = null;
                    item.IsDisabled = true;
                }
                else
                {
                    item.Children = ObjectMapper.Map<List<MVDProperty>, List<MVDPropertyDto>>(list);
                    foreach (var mvdProperty in item.Children)
                    {
                        //mvdProperty.Children = null;
                    }
                }
            }
            return Task.FromResult(res);
        }

        /// <summary>
        /// 新建信息交换模板分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.MVDCategory.Create)]
        public async Task<MVDCategoryDto> Create(MVDCategoryCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("请输入名称");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("信息交换模板分类名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("信息交换模板分类代号不合法");
            }
            if (_repository.Any(x => x.Code == input.Code && !string.IsNullOrEmpty(input.Code))) throw new UserFriendlyException("此代号已存在!");
            if (_repository.Any(x => x.Name == input.Name && !string.IsNullOrEmpty(input.Name))) throw new UserFriendlyException("此名称已存在!");
            var entity = new MVDCategory(_guidGenerator.Create())
            {
                Name = input.Name,
                Code = input.Code,
                Order = input.Order >= 0 ? input.Order : 0,
                Remark = input.Remark
            };
            await _repository.InsertAsync(entity);
            return ObjectMapper.Map<MVDCategory, MVDCategoryDto>(entity);
        }

        /// <summary>
        /// 删除信息交换模板分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.MVDCategory.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("该信息交换模板分类不存在！");
            if (_repository.Any(x => x.Id == id))
            {
                var list = _repositoryMVDProperty.Where(m => m.MVDCategoryId == id);
                if (list?.Count() > 0)
                {
                    throw new UserFriendlyException("该信息交换模板分类已关联属性不能删除！");
                }
                await _repository.DeleteAsync(id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改信息交换模板分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.MVDCategory.Update)]
        public async Task<MVDCategoryDto> Update(MVDCategoryDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("请输入名称");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("信息交换模板分类名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("信息交换模板分类代号不合法");
            }
            var entity = await _repository.GetAsync(input.Id);
            if (entity == null) throw new UserFriendlyException("当前信息交换模板分类不存在");
            entity.Name = input.Name;
            var bf = _repository.FirstOrDefault(x => x.Name == input.Name && x.Id != input.Id);
            if (bf != null)
            {
                throw new UserFriendlyException("信息交换模板分类已存在！");
            }
            bf = _repository.FirstOrDefault(x => !string.IsNullOrEmpty(input.Code) && x.Code == input.Code && x.Id != input.Id);
            if (bf != null)
            {
                throw new UserFriendlyException("此代号已存在！");
            }
            entity.Code = input.Code;
            entity.Order = input.Order >= 0 ? input.Order : 0;
            entity.Remark = input.Remark;
            await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<MVDCategory, MVDCategoryDto>(entity);
        }

        [Authorize(StdBasicPermissions.MVDCategory.Export)]
        /// <summary>
        /// 导出信息交换模板分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<Stream> Export(MVDData input)
        {
            var list = _repository.WithDetails().WhereIf(!string.IsNullOrEmpty(input.Paramter.Keywords), x => x.Name.Contains(input.Paramter.Keywords) || x.Code.Contains(input.Paramter.Keywords));
            var dtoList = ObjectMapper.Map<List<MVDCategory>, List<MVDCategoryTemplate>>(list.ToList());
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return Task.FromResult(stream);
        }

        [Authorize(StdBasicPermissions.MVDCategory.Import)]
        /// <summary>
        /// 导入信息交换模板分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Import([FromForm] ImportData input)
        {

            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            // 获取excel表格，判断表格是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            IWorkbook workbook = null;
            ISheet sheet = null;
            List<MVDCategoryTemplate> datalist = null;

            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<MVDCategoryTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<MVDCategoryTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            MVDCategory hasMVDModel;
            if (datalist.Any())
            {
                await _fileImport.ChangeTotalCount(input.ImportKey, datalist.Count());
                var updateIndex = 1;
                foreach (var item in datalist)
                {
                    await _fileImport.UpdateState(input.ImportKey, updateIndex);
                    updateIndex++;
                    var canInsert = true;
                    var newInfo = new WrongInfo(item.Index);
                    if (item.Name.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("名称为空");
                    }
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    //using var uow = _unitOfWork.Begin(true, false);
                    //using (_dataFilter.Disable<ISoftDelete>())
                    //{ }
                    hasMVDModel = _repository.FirstOrDefault(a => a.Name == item.Name);
                    if (hasMVDModel != null)
                    {
                        newInfo.AppendInfo($"{item.Name}已存在，且已更新");
                        hasMVDModel.Name = item.Name;
                        hasMVDModel.Code = item.Code;
                        hasMVDModel.Order = item.Order >= 0 ? item.Order : 0;
                        hasMVDModel.Remark = item.Remark;
                        await _repository.UpdateAsync(hasMVDModel);
                        //await uow.SaveChangesAsync();
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        hasMVDModel = new MVDCategory(_guidGenerator.Create());
                        hasMVDModel.Name = item.Name;
                        hasMVDModel.Code = item.Code;
                        hasMVDModel.Order = item.Order >= 0 ? item.Order : 0;
                        hasMVDModel.Remark = item.Remark;
                        await _repository.InsertAsync(hasMVDModel);
                        //await uow.SaveChangesAsync();
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
    }
}
