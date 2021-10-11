using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Enums;
using SnAbp.StdBasic.IServices.Model.ModelMVD;
using SnAbp.StdBasic.TempleteModel;
using SnAbp.Utils;
using SnAbp.Utils.EnumHelper;
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
    public class StdBasicMVDPropertyAppService : StdBasicAppService, IStdBasicMVDPropertyAppService
    {
        private readonly IRepository<MVDProperty, Guid> _repository;
        private readonly IRepository<ModelRltMVDProperty, Guid> _repositoryRlt;
        private readonly IRepository<ComponentCategoryRltMVDProperty, Guid> _repositoryComponentCategoryRltMVDProperty;
        private readonly IRepository<ProductCategoryRltMVDProperty, Guid> _repositoryProductCategoryRltMVDProperty;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IUnitOfWorkManager _unitOfWork;
        private readonly IDataFilter _dataFilter;

        public StdBasicMVDPropertyAppService(
            IRepository<MVDProperty, Guid> repository,
            IGuidGenerator guidGenerator, IFileImportHandler fileImport,
            IUnitOfWorkManager unitOfWork,
            IDataFilter dataFilter,
            IRepository<ComponentCategoryRltMVDProperty, Guid> repositoryComponentCategoryRltMVDProperty,
            IRepository<ProductCategoryRltMVDProperty, Guid> repositoryProductCategoryRltMVDProperty,
            IRepository<ModelRltMVDProperty, Guid> repositoryRlt)
        {
            _repositoryComponentCategoryRltMVDProperty = repositoryComponentCategoryRltMVDProperty;
            _repositoryProductCategoryRltMVDProperty = repositoryProductCategoryRltMVDProperty;
            _repository = repository;
            _repositoryRlt = repositoryRlt;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _unitOfWork = unitOfWork;
            _dataFilter = dataFilter;
        }

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.MVDProperty.Detail)]
        public Task<MVDPropertyDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var entity = _repository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (entity == null) throw new UserFriendlyException("该实体不存在！");
            return Task.FromResult(ObjectMapper.Map<MVDProperty, MVDPropertyDto>(entity));
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<MVDPropertyDto>> GetList(MVDSearchDto input)
        {
            var res = new PagedResultDto<MVDPropertyDto>();
            if (input.Ids == null)
                input.Ids = new List<Guid>();
            if (input.Ids.Count > 0)
            {
                var entityList = _repository.WithDetails().Where(x => input.Ids.Contains(x.Id));
                res.TotalCount = entityList.Count();
                res.Items = ObjectMapper.Map<List<MVDProperty>, List<MVDPropertyDto>>(entityList.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(x => x.Order).ThenBy(x=>x.Name).ToList());
            }
            else
            {
                var entityList = _repository.WithDetails().Where(x => x.MVDCategoryId == input.mvdCategoryId).WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords));
                res.TotalCount = entityList.Count();
                res.Items = input.IsAll ? ObjectMapper.Map<List<MVDProperty>, List<MVDPropertyDto>>(entityList.OrderBy(x => x.Order).ThenBy(x => x.Name).ToList()) : ObjectMapper.Map<List<MVDProperty>, List<MVDPropertyDto>>(entityList.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(x => x.Order).ThenBy(x => x.Name).ToList());
            }
            return Task.FromResult(res);
        }

        /// <summary>
        /// 新建信息交换模板分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.MVDProperty.Create)]
        public async Task<MVDPropertyDto> Create(MVDPropertyCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("请输入名称");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("信息交换模板属性名称不能包含空格");
            }
            CheckSameName(input.Name, null, input.MVDCategoryId);
            //if (_repository.Any(x => x.Name == input.Name && !string.IsNullOrEmpty(input.Name))) throw new UserFriendlyException("此名称已存在!");
            var entity = new MVDProperty(_guidGenerator.Create())
            {
                MVDCategoryId = input.MVDCategoryId,
                Name = input.Name,
                //DataType = input.DataType,
                Value = input.Value,
                Order = input.Order,
                Unit = input.Unit,
                IsInstance = input.IsInstance,
                Remark = input.Remark
            };
            await _repository.InsertAsync(entity);
            return ObjectMapper.Map<MVDProperty, MVDPropertyDto>(entity);
        }

        /// <summary>
        /// 删除信息交换模板分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.MVDProperty.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("该信息交换模板属性不存在！");
            if (_repository.Any(x => x.Id == id))
            {

                var componentCategoryRltMVDProperty = _repositoryComponentCategoryRltMVDProperty.Where(x => x.MVDPropertyId == id);
                if (componentCategoryRltMVDProperty?.Count() > 0)
                {
                    throw new UserFriendlyException("该信息交换模板属性已关联构件，无法删除！");
                }
                var productCategoryRltMVDProperty = _repositoryProductCategoryRltMVDProperty.Where(x => x.MVDPropertyId == id);
                if (productCategoryRltMVDProperty?.Count() > 0)
                {
                    throw new UserFriendlyException("该信息交换模板属性已关联产品，无法删除！");
                }
                var list = _repositoryRlt.Where(x => x.MVDPropertyId == id);
                if (list?.Count() > 0)
                {
                    throw new UserFriendlyException("该信息交换模板属性已关联标准设备，无法删除！");
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
        [Authorize(StdBasicPermissions.MVDProperty.Update)]
        public async Task<MVDPropertyDto> Update(MVDPropertyDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("请输入名称");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("信息交换模板属性名称不能包含空格");
            }
            var entity = await _repository.GetAsync(input.Id);
            if (entity == null) throw new UserFriendlyException("当前信息交换模板属性不存在");
            CheckSameName(input.Name, input.Id, input.MVDCategoryId);
            entity.Name = input.Name;
            //entity.DataType = input.DataType;
            entity.Value = input.Value;
            entity.Order = input.Order;
            entity.Unit = input.Unit;
            entity.IsInstance = input.IsInstance;
            entity.Remark = input.Remark;
            await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<MVDProperty, MVDPropertyDto>(entity);
        }

        /// <summary>
        /// 导出信息交换模板分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.MVDProperty.Export)]
        public Task<Stream> Export(MVDData input)
        {
            var list = _repository.WithDetails().Where(x => x.MVDCategoryId == input.Paramter.mvdCategoryId).WhereIf(!string.IsNullOrEmpty(input.Paramter.Keywords), x => x.Name.Contains(input.Paramter.Keywords));
            var dtoList = ObjectMapper.Map<List<MVDProperty>, List<MVDPropertyTemplate>>(list.ToList());
            foreach (var item in dtoList)
            {
                item.IsInstance = item.IsInstance == "True" ? "是" : "否";
            }
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return Task.FromResult(stream);
        }

        /// <summary>
        /// 导入信息交换模板分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mvdCategoryId"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.MVDProperty.Import)]
        public async Task Import([FromForm] ImportData input, Guid mvdCategoryId)
        {

            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            // 获取excel表格，判断表格是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            IWorkbook workbook = null;
            ISheet sheet = null;
            List<MVDPropertyTemplate> datalist = null;

            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<MVDPropertyTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<MVDPropertyTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            MVDProperty hasMVDModel;
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
                    //if (item.DataType.IsNullOrEmpty())
                    //{
                    //    canInsert = false;
                    //    newInfo.AppendInfo("参数类型为空");
                    //}
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    //using var uow = _unitOfWork.Begin(true, false);
                    //using (_dataFilter.Disable<ISoftDelete>())
                    //{ }
                    //查询信息交换模板分类下的属性是否重复
                    hasMVDModel = _repository.Where(x => x.MVDCategoryId == mvdCategoryId).FirstOrDefault(a => a.Name == item.Name);
                    if (hasMVDModel != null)
                    {
                        newInfo.AppendInfo($"{item.Name}已存在，且已更新");
                        hasMVDModel.Name = item.Name;
                        //hasMVDModel.DataType = GetMVDPropertyDataType(item.DataType);
                        //hasMVDModel.Value = item.Value;
                        hasMVDModel.Order = item.Order;
                        hasMVDModel.Remark = item.Remark;
                        hasMVDModel.Unit = item.Unit;
                        hasMVDModel.IsInstance = item.Equals("是");
                        await _repository.UpdateAsync(hasMVDModel);
                        //await uow.SaveChangesAsync();
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        hasMVDModel = new MVDProperty(_guidGenerator.Create());
                        hasMVDModel.Name = item.Name;
                        //hasMVDModel.DataType = GetMVDPropertyDataType(item.DataType);
                        //hasMVDModel.Value = item.Value;
                        hasMVDModel.Order = item.Order;
                        hasMVDModel.Remark = item.Remark;
                        hasMVDModel.Unit = item.Unit;
                        hasMVDModel.IsInstance = item.Equals("是");
                        hasMVDModel.MVDCategoryId = mvdCategoryId;
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
        private bool CheckSameName(string Name, Guid? id, Guid? MVDCategoryId)
        {
            var entity = _repository.WithDetails()
                .Where(x => x.Name.ToUpper() == Name.ToUpper());
            if (MVDCategoryId != null && MVDCategoryId != Guid.Empty)
            {
                entity = entity.Where(x => x.MVDCategoryId == MVDCategoryId);
            }
            else
            {
                entity = entity.Where(x => x.MVDCategoryId == null || x.MVDCategoryId == Guid.Empty);
            }

            if (id.HasValue)
            {
                entity = entity.Where(x => x.Id != id.Value);
            }

            if (entity.Count() > 0)
            {
                throw new UserFriendlyException("当前分类中已存在相同名字的属性!!!");
            }

            return true;
        }
        private MVDPropertyDataType GetMVDPropertyDataType(string dataType)
        {

            if (dataType == MVDPropertyDataType.Digit.GetDescription())
            {
                return MVDPropertyDataType.Digit;
            }

            if (dataType == MVDPropertyDataType.String.GetDescription())
            {
                return MVDPropertyDataType.String;
            }

            if (dataType == MVDPropertyDataType.Length.GetDescription())
            {
                return MVDPropertyDataType.Length;
            }

            return MVDPropertyDataType.String;
        }
    }
}
