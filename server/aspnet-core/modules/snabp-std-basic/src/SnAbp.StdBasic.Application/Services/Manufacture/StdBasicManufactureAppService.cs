using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using SnAbp.StdBasic.IServices;
using SnAbp.StdBasic.Enums;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Dtos;
using NPOI.SS.UserModel;
using SnAbp.Utils.Validation;
using SnAbp.Utils.ExcelHelper;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.Common;
using Volo.Abp.Data;
using SnAbp.Utils;
using Volo.Abp.Uow;
using SnAbp.StdBasic.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using SnAbp.Utils.DataImport;
using SnAbp.StdBasic.Dtos.Export;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicManufactureAppService : StdBasicAppService, IStdBasicManufactureAppService
    {
        private readonly IRepository<Manufacturer, Guid> _manufacturerRepository;
        private readonly IRepository<EquipmentControlType, Guid> _equipmentTypeRepository;
        private readonly IRepository<Model, Guid> _modelRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IDataFilter _dataFilter;

        private readonly IUnitOfWorkManager _unitOfWork;
        public StdBasicManufactureAppService(
            IRepository<Manufacturer, Guid> manufacturerRepository,
            IRepository<EquipmentControlType, Guid> equipmentTypeRepository,
            IRepository<Model, Guid> modelRepository,
            IGuidGenerator guidGenerator,
            IDataFilter dataFilter,
            IFileImportHandler fileImport,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _manufacturerRepository = manufacturerRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            _modelRepository = modelRepository;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _unitOfWork = unitOfWorkManager;
        }

        public async Task<ManufacturerDto> Get(Guid id)
        {
            ManufacturerDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _manufacturerRepository.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此厂家不存在");
                result = ObjectMapper.Map<Manufacturer, ManufacturerDto>(ent);
            });
            return result;
        }

        public Task<PagedResultDto<ManufacturerSimpleDto>> GetList(ManufacturerGetListDto input)
        {
            var result = new PagedResultDto<ManufacturerSimpleDto>();
            var query = _manufacturerRepository
                .WithDetails()
                .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                .WhereIf(!string.IsNullOrEmpty(input.Keyword),
                    s => s.Name.Contains(input.Keyword) ||
                        s.ShortName.Contains(input.Keyword) ||
                        s.Address.Contains(input.Keyword) ||
                        s.Code.Contains(input.Keyword) ||
                        s.CSRGCode.Contains(input.Keyword)
                 );

            var list = input.IsAll ? query.ToList() : query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var dtos = ObjectMapper.Map<List<Manufacturer>, List<ManufacturerSimpleDto>>(list);
            foreach (var item in dtos)
            {
                item.Children = item.Children.Count == 0 || !string.IsNullOrEmpty(input.Keyword) ? null : new List<ManufacturerSimpleDto>();
            }

            result.TotalCount = input.IsAll ? dtos.Count() : query.Count();
            result.Items = dtos;
            return Task.FromResult(result);
        }

        /// <summary>
        /// 根据产品主键获得已关联的厂家列表
        /// </summary>
        public async Task<List<ManufacturerDto>> GetListByProductId(Guid productId)
        {
            var result = new List<ManufacturerDto>();
            await Task.Run(() =>
            {
                var q = from a in _modelRepository
                        join b in _manufacturerRepository
                        on a.ManufacturerId equals b.Id
                        where a.ProductCategoryId == productId
                        select b;
                result = ObjectMapper.Map<List<Manufacturer>, List<ManufacturerDto>>(q.ToList());
            });
            return result;
        }

        [Authorize(StdBasicPermissions.Manufacture.Create)]
        public async Task<ManufacturerDto> Create(ManufacturerCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("请输入名称");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("厂家名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("厂家编码不合法");
            }
            if (!string.IsNullOrEmpty(input.CSRGCode) && !StringUtil.CheckCodeValidity(input.CSRGCode))
            {
                throw new UserFriendlyException("厂家CSRG编码不合法");
            }
            var allEnts = _manufacturerRepository.WithDetails();
            var sameNameEnt = allEnts.Where(s => s.Name == input.Name);
            if (sameNameEnt.Count() > 0) throw new UserFriendlyException("此名称厂家已存在");
            var sameCodeEnt = allEnts.Where(s => s.Code == input.Code && !string.IsNullOrEmpty(input.Code));
            if (sameCodeEnt.Count() > 0) throw new UserFriendlyException("此编码已存在");

            var ent = new Manufacturer(_guidGenerator.Create())
            {
                Address = input.Address,
                Code = input.Code,
                Introduction = input.Introduction,
                IsDeleted = false,
                Name = input.Name,
                ParentId = input.ParentId,
                Principal = input.Principal,
                Remark = input.Remark,
                ShortName = input.ShortName,
                Telephone = input.Telephone,
                Type = input.Type,
                CSRGCode = input.CSRGCode,
            };

            try
            {
                ValidationMaxlength.Validate(ent);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("创建失败，" + e.Message);
            }
            foreach (var item in input.EquipmentTypes)
            {
                var equipmentTypes = new EquipmentControlType(_guidGenerator.Create())
                {
                    ManufacturerId = ent.Id,
                    Name = item.Name,
                    Remark = item.Remark,
                    TypeGroup = item.TypeGroup,
                };

                ent.EquipmentControlTypes.Add(equipmentTypes);
            }
            await _manufacturerRepository.InsertAsync(ent);
            return ObjectMapper.Map<Manufacturer, ManufacturerDto>(ent);
        }
        [Authorize(StdBasicPermissions.Manufacture.Import)]
        public async Task Upload([FromForm] ImportData input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<ManufactureTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<ManufactureTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<ManufactureTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            Manufacturer manufacturerModel;
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

                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    using var uow = _unitOfWork.Begin(true);
                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        manufacturerModel = _manufacturerRepository.FirstOrDefault(a => a.Code == item.Code && a.Name == item.Name);
                    }
                    if (manufacturerModel != null)
                    {
                        newInfo.AppendInfo($"{item.Code}已存在，且已更新");
                        //修改自己本身数据
                        manufacturerModel.Name = item.Name;
                        manufacturerModel.Address = item.Address;
                        manufacturerModel.CSRGCode = item.CSRGCode;
                        manufacturerModel.DeletionTime = DateTime.Now;
                        manufacturerModel.Introduction = item.Introduction;
                        manufacturerModel.LastModificationTime = DateTime.Now;
                        manufacturerModel.Principal = item.Principal;
                        manufacturerModel.ShortName = item.ShortName;
                        manufacturerModel.Telephone = item.Telephone; 
                        manufacturerModel.Code = item.Code;
                        manufacturerModel.IsDeleted = false;

                        if (item.ParentName != null && !string.IsNullOrEmpty(item.ParentName))
                        {
                            var parent = _manufacturerRepository.FirstOrDefault(a => a.Name == item.ParentName);
                            if (parent != null)
                            {
                                manufacturerModel.ParentId = parent.Id;
                            }
                            else
                            {
                                var parentModel = datalist.ToList().Find(x => x.Name == item.ParentName);
                                if (parentModel != null)
                                {
                                    var parentEnt = new Manufacturer(_guidGenerator.Create());
                                    parentEnt.Name = parentModel.Name;
                                    parentEnt.Address = parentModel.Address;
                                    parentEnt.CSRGCode = parentModel.CSRGCode;
                                    parentEnt.DeletionTime = DateTime.Now;
                                    parentEnt.Introduction = parentModel.Introduction;
                                    parentEnt.LastModificationTime = DateTime.Now;
                                    parentEnt.Principal = parentModel.Principal;
                                    parentEnt.ShortName = parentModel.ShortName;
                                    parentEnt.Telephone = parentModel.Telephone;
                                    parentEnt.Code = parentModel.Code;
                                    await _manufacturerRepository.InsertAsync(parentEnt);
                                    manufacturerModel.ParentId = parentEnt.Id;
                                    await uow.SaveChangesAsync();
                                }
                            }

                        }
                        manufacturerModel.IsDeleted = false;
                        await _manufacturerRepository.UpdateAsync(manufacturerModel);
                        await uow.SaveChangesAsync();
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {

                        manufacturerModel = new Manufacturer(_guidGenerator.Create());
                        manufacturerModel.Name = item.Name;
                        manufacturerModel.Name = item.Name;
                        manufacturerModel.Address = item.Address;
                        manufacturerModel.CSRGCode = item.CSRGCode;
                        manufacturerModel.DeletionTime = DateTime.Now;
                        manufacturerModel.Introduction = item.Introduction;
                        manufacturerModel.LastModificationTime = DateTime.Now;
                        manufacturerModel.Principal = item.Principal;
                        manufacturerModel.ShortName = item.ShortName;
                        manufacturerModel.Telephone = item.Telephone;
                        manufacturerModel.Code = item.Code;
                        if (item.ParentName != null && !string.IsNullOrEmpty(item.ParentName))
                        {
                            var parent = _manufacturerRepository.FirstOrDefault(a => a.Name == item.ParentName);
                            if (parent != null)
                            {
                                manufacturerModel.ParentId = parent.Id;
                            }
                            else
                            {
                                var parentModel = datalist.ToList().Find(x => x.Name == item.ParentName);
                                if (parentModel != null)
                                {
                                    var parentEnt = new Manufacturer(_guidGenerator.Create());
                                    parentEnt.Name = parentModel.Name;
                                    parentEnt.Address = parentModel.Address;
                                    parentEnt.CSRGCode = parentModel.CSRGCode;
                                    parentEnt.DeletionTime = DateTime.Now;
                                    parentEnt.Introduction = parentModel.Introduction;
                                    parentEnt.LastModificationTime = DateTime.Now;
                                    parentEnt.Principal = parentModel.Principal;
                                    parentEnt.ShortName = parentModel.ShortName;
                                    parentEnt.Telephone = parentModel.Telephone;
                                    parentEnt.Code = parentModel.Code;
                                    await _manufacturerRepository.InsertAsync(parentEnt);
                                    manufacturerModel.ParentId = parentEnt.Id;
                                    await uow.SaveChangesAsync();
                                }
                            }

                        }
                        await _manufacturerRepository.InsertAsync(manufacturerModel);
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
        //using (_dataFilter.Disable<ISoftDelete>())
        //{
        //    await _fileImport.Start(input.ImportKey, 100);
        //    StringBuilder failMsg = new StringBuilder();
        //    int addCount = 0;
        //    int updateCount = 0;
        //    int deleteCount = 0;
        //    DataTable dt = null;
        //    IWorkbook workbook = null;
        //    ISheet sheet = null;
        //    try
        //    {
        //        workbook = input.File.ConvertToWorkbook();
        //        sheet = workbook.GetSheetAt(0);
        //        //Todo: 李攀 2020年8月30日,未处理错误
        //        dt = ExcelHelper.ImportBaseDataToDataTable(input.File.File.OpenReadStream(), input.File.File.FileName, out var workbook1);
        //    }
        //    catch (Exception)
        //    {
        //        await _fileImport.Cancel(input.ImportKey);
        //        throw new UserFriendlyException("所选文件有错误，请重新选择");
        //    }
        //    if (dt == null)
        //    {
        //        await _fileImport.Cancel(input.ImportKey);
        //        throw new UserFriendlyException("未找到任何数据,请检查文件格式");
        //    }
        //    await _fileImport.ChangeTotalCount(input.ImportKey, dt.Rows.Count);
        //    #region 验证列是否存在
        //    //验证列是否存在
        //    if (!dt.Columns.Contains(ManufactureInportCol.SeenSun.ToString()))
        //    {
        //        await _fileImport.Cancel(input.ImportKey);
        //        throw new UserFriendlyException("列" + ManufactureInportCol.SeenSun.ToString() + "不存在");
        //    }
        //    if (!dt.Columns.Contains(ManufactureInportCol.Name.ToString()))
        //    {
        //        await _fileImport.Cancel(input.ImportKey);
        //        throw new UserFriendlyException("列" + ManufactureInportCol.Name.ToString() + "不存在");
        //    }
        //    if (!dt.Columns.Contains(ManufactureInportCol.CSRGCode.ToString()))
        //    {
        //        await _fileImport.Cancel(input.ImportKey);
        //        throw new UserFriendlyException("列" + ManufactureInportCol.CSRGCode.ToString() + "不存在");
        //    }
        //    if (!dt.Columns.Contains(ManufactureInportCol.ShortName.ToString()))
        //    {
        //        await _fileImport.Cancel(input.ImportKey);
        //        throw new UserFriendlyException("列" + ManufactureInportCol.ShortName.ToString() + "不存在");
        //    }
        //    if (!dt.Columns.Contains(ManufactureInportCol.Address.ToString()))
        //    {
        //        await _fileImport.Cancel(input.ImportKey);
        //        throw new UserFriendlyException("列" + ManufactureInportCol.Address.ToString() + "不存在");
        //    }
        //    if (!dt.Columns.Contains(ManufactureInportCol.Telephone.ToString()))
        //    {
        //        await _fileImport.Cancel(input.ImportKey);
        //        throw new UserFriendlyException("列" + ManufactureInportCol.Telephone.ToString() + "不存在");
        //    }
        //    #endregion

        //    //Todo: 李攀 2020年8月30日,未处理错误
        //    try
        //    {
        //        int index = 5;
        //        //var allManufact = _manufacturerRepository.Where(s => s.IsDeleted == false);
        //        List<string> manuCodes = new List<string>();
        //        List<WrongInfo> wrongInfos = new List<WrongInfo>();
        //        using var unitWork = _unitOfWorkManager.Begin(true, false);
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            WrongInfo wrong = new WrongInfo(index);
        //            index++;
        //            var CSRGCode = Convert.ToString(row[ManufactureInportCol.CSRGCode.ToString()]);
        //            var name = Convert.ToString(row[ManufactureInportCol.Name.ToString()]);
        //            var shortName = Convert.ToString(row[ManufactureInportCol.ShortName.ToString()]);
        //            var address = Convert.ToString(row[ManufactureInportCol.Address.ToString()]);
        //            var tele = Convert.ToString(row[ManufactureInportCol.Telephone.ToString()]);
        //            if (string.IsNullOrEmpty(CSRGCode.Trim()))
        //            {
        //                failMsg.Append("第" + index + "行数据无编码，导入失败。\r\n");
        //                wrong.WrongMsg = "数据无编码";
        //                wrongInfos.Add(wrong);
        //            }
        //            else
        //            {
        //                if (manuCodes.FirstOrDefault(s => s == CSRGCode) != null)
        //                {
        //                    failMsg.Append("第" + index + "行数据编码重复，导入失败。\r\n");
        //                    wrong.WrongMsg = "编码重复";
        //                    wrongInfos.Add(wrong);
        //                    continue;
        //                }
        //                manuCodes.Add(CSRGCode);
        //                try
        //                {

        //                    var sameCodeEnt = _manufacturerRepository.FirstOrDefault(s => s.CSRGCode == CSRGCode);
        //                    if (sameCodeEnt != null)
        //                    {
        //                        sameCodeEnt.Name = name;
        //                        sameCodeEnt.ShortName = shortName;
        //                        sameCodeEnt.Address = address;
        //                        sameCodeEnt.Telephone = tele;
        //                        sameCodeEnt.IsDeleted = false;
        //                        ValidationMaxlength.Validate(sameCodeEnt);
        //                        await _manufacturerRepository.UpdateAsync(sameCodeEnt);
        //                        updateCount++;
        //                    }
        //                    else
        //                    {
        //                        Manufacturer manu = new Manufacturer(_guidGenerator.Create())
        //                        {
        //                            CSRGCode = CSRGCode,
        //                            Name = name,
        //                            ShortName = shortName,
        //                            Address = address,
        //                            Telephone = tele,
        //                        };

        //                        ValidationMaxlength.Validate(manu);
        //                        await _manufacturerRepository.InsertAsync(manu);
        //                        addCount++;
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    failMsg.AppendFormat("{0}({1})导入失败，原因：{2}\r\n", name, CSRGCode, e.Message);
        //                    wrong.WrongMsg = e.Message;
        //                    wrongInfos.Add(wrong);
        //                }
        //            }
        //            await _fileImport.UpdateState(input.ImportKey, dt.Rows.IndexOf(row));
        //        }
        //        //软删除excel中不存在的数据
        //        var softDeleteManu = _manufacturerRepository.Where(s => !manuCodes.Contains(s.CSRGCode)).ToList();
        //        foreach (var item in softDeleteManu)
        //        {
        //            item.IsDeleted = true;
        //            await _manufacturerRepository.UpdateAsync(item);
        //            deleteCount++;
        //        }

        //        await _fileImport.UpdateState(input.ImportKey, dt.Rows.Count - 1);
        //        await unitWork.SaveChangesAsync();
        //        await _fileImport.Complete(input.ImportKey);

        //        //修改源excel 添加错误信息
        //        var resMsg = "成功添加" + addCount + "条数据，更新" + updateCount + "条数据，删除" + deleteCount + "条数据。\r\n" + failMsg;

        //        // 处理错误信息
        //        if (wrongInfos.Any())
        //        {
        //            sheet.CreateInfoColumn(wrongInfos);
        //            await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        await _fileImport.Cancel(input.ImportKey);
        //    }
        //}
        //}

        public async Task<Stream> Export(ManufacturerData input)
        {
            var list = GetManufacturersData(input.Paramter);
            //var list = await _manufacturerRepository.GetListAsync();
            var dtoList = ObjectMapper.Map<List<Manufacturer>, List<ManufactureTemplate>>(list);
            var parentList = list.FindAll(x => x.ParentId.HasValue);
            if (parentList?.Count > 0)
            {
                parentList.ForEach(m =>
                {
                    var dto = dtoList.Find(x => x.Name == m.Name);
                    if (m.Parent != null)
                    {
                        dto.ParentName = m.Parent.Name;
                    }
                    else
                    {
                        var parent = _manufacturerRepository.FirstOrDefault(x => x.Id == m.ParentId);
                        dto.ParentName = parent.Name;
                    }
                });
            }

            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }

        [Authorize(StdBasicPermissions.Manufacture.Update)]
        public async Task<ManufacturerDto> Update(ManufacturerUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入Id");

            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("请输入名称");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("厂家名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("厂家编码不合法");
            }
            if (!string.IsNullOrEmpty(input.CSRGCode) && !StringUtil.CheckCodeValidity(input.CSRGCode))
            {
                throw new UserFriendlyException("厂家CSRG编码不合法");
            }
            var manu = _manufacturerRepository.FirstOrDefault(x => x.Id == input.Id);

            if (manu == null) throw new UserFriendlyException("当前编辑的厂家实体不存在");

            var allEnts = _manufacturerRepository.WithDetails();
            var sameNameEnt = allEnts.Where(s => s.Id != input.Id && s.Name == input.Name);
            if (sameNameEnt.Count() > 0) throw new UserFriendlyException("此名称厂家已存在");
            var sameCodeEnt = allEnts.Where(s => s.Id != input.Id && s.Code == input.Code && !string.IsNullOrEmpty(input.Code));
            if (sameCodeEnt.Count() > 0) throw new UserFriendlyException("此编码已存在");


            if (input.Id == input.ParentId) throw new UserFriendlyException("父级不能为自身");

            //if (manu.Children.Count > 0 && isChildren(input.ParentId, manu.Children)) throw new UserFriendlyException("父级不能为自身的下级");

            manu.Address = input.Address;
            manu.Code = input.Code;
            manu.CSRGCode = input.CSRGCode;
            manu.Introduction = input.Introduction;
            manu.IsDeleted = false;
            manu.Name = input.Name;
            manu.ParentId = input.ParentId;
            manu.Principal = input.Principal;
            manu.Remark = input.Remark;
            manu.ShortName = input.ShortName;
            manu.Telephone = input.Telephone;
            manu.Type = input.Type;

            try
            {
                ValidationMaxlength.Validate(manu);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("编辑失败，" + e.Message);
            }
            await _equipmentTypeRepository.DeleteAsync(s => s.ManufacturerId == input.Id);
            foreach (var item in input.EquipmentTypes)
            {
                var type = new EquipmentControlType(_guidGenerator.Create())
                {
                    ManufacturerId = manu.Id,
                    Name = item.Name,
                    Remark = item.Remark,
                    TypeGroup = input.Remark,
                };

                await _equipmentTypeRepository.InsertAsync(type);
            }
            await _manufacturerRepository.UpdateAsync(manu);
            return ObjectMapper.Map<Manufacturer, ManufacturerDto>(manu);
        }

        [Authorize(StdBasicPermissions.Manufacture.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _manufacturerRepository.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此厂家不存在");
            if (ent.Children != null && ent.Children.Count > 0) throw new UserFriendlyException("请先删除下级厂家！！");
            await _equipmentTypeRepository.DeleteAsync(s => s.ManufacturerId == id);
            await _manufacturerRepository.DeleteAsync(id);
            return true;
        }


        #region 私有方法
        private bool isChildren(Guid? parentId, List<Manufacturer> children)
        {
            if (children.Count > 0)
            {
                bool result = false;
                foreach (var manu in children)
                {
                    if (manu.Id == parentId) result = true;
                    else
                    {
                        var manuChildren = _manufacturerRepository.FirstOrDefault(x => x.Id == manu.Id);
                        if (manuChildren.Children.Count > 0) { isChildren(parentId, manuChildren.Children); }
                    }
                }
                return result;
            }
            else return false;

        }

        private List<Manufacturer> GetManufacturersData(ManufacturerGetListDto input)
        {
            var query = _manufacturerRepository
                .WithDetails()
                .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                .WhereIf(!string.IsNullOrEmpty(input.Keyword),
                    s => s.Name.Contains(input.Keyword) ||
                        s.ShortName.Contains(input.Keyword)
                 );

            return query.ToList();
        }

        #endregion
    }
}
