using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.Common.Entities;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Enums;
using SnAbp.StdBasic.IServices;
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
    /// <summary>
    /// 电算代号管理
    /// </summary>
    [Authorize]
    public class StdBasicComputerCodeAppService : StdBasicAppService, IStdBasicComputerCodeAppService
    {
        private readonly IRepository<ComputerCode, Guid> _repositoryComputerCode;
        private readonly IRepository<BasePrice, Guid> _repositoryBasePrice;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IUnitOfWorkManager _unitOfWork;
        private readonly IDataFilter _dataFilter;
        private readonly IRepository<Area> _repositoryArea;//区域

        public StdBasicComputerCodeAppService(
        IRepository<ComputerCode, Guid> repositoryComputerCode,
        IGuidGenerator guidGenerator,
        IFileImportHandler fileImport,
        IDataFilter dataFilter,
        IUnitOfWorkManager unitOfWork,
        IRepository<BasePrice, Guid> repositoryBasePrice,
        IRepository<Area> repositoryArea
            )
        {
            _repositoryBasePrice = repositoryBasePrice;
            _repositoryArea = repositoryArea;
            _repositoryComputerCode = repositoryComputerCode;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _unitOfWork = unitOfWork;
        }

        [Authorize(StdBasicPermissions.ComputerCode.Create)]
        public async Task<ComputerCodeDto> Create(ComputerCodeCreateDto input)
        {

            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("电算代号编号不能为空");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("电算代号名称不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("电算代号不合法");
            }
            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("单位不能为纯数字");
            }

            bool isSame = CheckSameCode(input.Code, input.Name, null);
            if (isSame)
            {
                throw new UserFriendlyException("电算代号或名称重复");
            }
            var computerCode = new ComputerCode(_guidGenerator.Create());
            computerCode.Code = input.Code;
            computerCode.Name = input.Name;
            computerCode.Type = input.Type;
            computerCode.Weight = Convert.ToDecimal(input.Weight>=0?input.Weight:0);
            computerCode.Unit = input.Unit;
            computerCode.Remark = input.Remark;
            await _repositoryComputerCode.InsertAsync(computerCode);
            return ObjectMapper.Map<ComputerCode, ComputerCodeDto>(computerCode); ;
        }

        [Authorize(StdBasicPermissions.ComputerCode.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryComputerCode.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此电算代号不存在");
            var list = _repositoryBasePrice.Where(x => x.ComputerCodeId == id);
            if (list?.Count() > 0) throw new UserFriendlyException("此电算代号存在基价数据，无法删除");
            await _repositoryComputerCode.DeleteAsync(id);
            return true;
        }

        [Authorize(StdBasicPermissions.ComputerCode.Export)]
        public async Task<Stream> Export(ComputerCodeData input)
        {
            var list = GetComputerCodeData(input.Paramter);
            List<ComputerCodeTemplate> dtoList = new List<ComputerCodeTemplate>();
            if (list!.Count() > 0)
            {
                list = list.OrderBy(x=>x.Type).ToList();
                foreach (var item in list)
                {
                    ComputerCodeTemplate dto = ObjectMapper.Map<ComputerCode, ComputerCodeTemplate>(item);

                    if (item.Type == ComputerCodeType.Artificial)
                    {
                        dto.Type = "人工";
                    }
                    else if (item.Type == ComputerCodeType.Material)
                    {
                        dto.Type = "材料";
                    }
                    else
                    {
                        dto.Type = "机械";
                    }
                    dtoList.Add(dto);
                }
            }
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }

        [Authorize(StdBasicPermissions.ComputerCode.Detail)]
        public async Task<ComputerCodeDto> Get(Guid id)
        {
            ComputerCodeDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryComputerCode.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此电算代号不存在");
                result = ObjectMapper.Map<ComputerCode, ComputerCodeDto>(ent);

            });
            return result;
        }


        public async Task<PagedResultDto<ComputerCodeDto>> GetList(ComputerCodeGetListDto input)
        {
            if (input == null) return null;

            PagedResultDto<ComputerCodeDto> res = new PagedResultDto<ComputerCodeDto>();
            await Task.Run(() =>
            {
                List<ComputerCodeDto> list = new List<ComputerCodeDto>();
                var allEnt = input.Ids?.Count > 0 ? _repositoryComputerCode.WithDetails()
                        .Where(x => input.Ids.Contains(x.Id)).ToList()
                        : (input.IsRltMaterial.HasValue && input.IsRltMaterial == true ? null : _repositoryComputerCode.WithDetails()
                        .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Code.Contains(input.Keyword) || x.Name.Contains(input.Keyword)).ToList());
                if (allEnt == null)
                {
                    allEnt = new List<ComputerCode>();
                }
                if (input.Type != null && allEnt.Count > 0)

                {
                    allEnt = allEnt.FindAll(x => x.Type == input.Type);
                }
                res.TotalCount = allEnt.Count();
                if (allEnt.Count() > 0)
                {
                    allEnt = allEnt?.OrderBy(x => x.Type).ToList();
                }
                if (input.IsAll == false)
                {

                    list = ObjectMapper.Map<List<ComputerCode>, List<ComputerCodeDto>>(allEnt.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                }
                else
                {
                    list = ObjectMapper.Map<List<ComputerCode>, List<ComputerCodeDto>>(allEnt);

                }
                if (list?.Count > 0)
                {
                    var ids = list.ConvertAll(x => x.Id);
                    var basePriceList = _repositoryBasePrice.WithDetails().Where(x => ids.Contains(x.ComputerCodeId)).ToList();
                    if (basePriceList?.Count > 0)
                    {
                        basePriceList.ForEach(m =>
                        {
                            BasePriceDto model = ObjectMapper.Map<BasePrice, BasePriceDto>(m);
                            if (m.Area != null)
                            {

                                List<string> nameList = new List<string>();
                                nameList.Add(m.Area.FullName);
                                if (m.Area.ParentId.HasValue && m.Area.ParentId.Value >= 0)
                                {
                                    FindAllAreaParents(m.Area.ParentId.Value, ref nameList);

                                }
                                if (nameList?.Count > 0)
                                {
                                    for (int i = nameList.Count - 1; i >= 0; i--)
                                    {
                                        if (i == nameList.Count - 1)
                                        {
                                            model.AreaName = nameList[i];
                                        }
                                        else
                                        {
                                            model.AreaName += "_" + nameList[i];
                                        }
                                    }


                                }

                            }
                            var computerCode = list.Find(x => x.Id == m.ComputerCodeId);
                            if (computerCode != null)
                            {
                                if (computerCode.BasePrices == null)
                                {
                                    computerCode.BasePrices = new List<BasePriceDto>();
                                }
                                computerCode.BasePrices.Add(model);
                            }
                        });
                    }
                }
                res.Items = list;
            });
            return res;
        }

        [Authorize(StdBasicPermissions.ComputerCode.Update)]
        public async Task<ComputerCodeDto> Update(ComputerCodeUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("电算代号编号不能为空");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("电算代号名称不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("电算代号不合法");
            }
            if (!string.IsNullOrEmpty(input.Unit) && StringUtil.CheckNumberValidity(input.Unit))
            {
                throw new UserFriendlyException("单位不能为纯数字");
            }
            bool isSame = CheckSameCode(input.Code, input.Name, input.Id);
            if (isSame)
            {
                throw new UserFriendlyException("电算代号或名称重复");
            }
            var ent = _repositoryComputerCode.FirstOrDefault(s => input.Id == s.Id);
            if (ent == null) throw new UserFriendlyException("此电算代号不存在");

            ent.Code = input.Code;
            ent.Name = input.Name;
            ent.Type = input.Type;
            ent.Weight = Convert.ToDecimal(input.Weight>=0?input.Weight:0);
            ent.Unit = input.Unit;
            ent.Remark = input.Remark;
            var resEnt = await _repositoryComputerCode.UpdateAsync(ent);
            return ObjectMapper.Map<ComputerCode, ComputerCodeDto>(resEnt);
        }

        [Authorize(StdBasicPermissions.ComputerCode.Import)]
        public async Task Upload([FromForm] ImportData input)
        {
            // 虚拟进度0 %
            await _fileImport.Start(input.ImportKey, 100);
            //ComputerCodeTemplate
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<ComputerCodeTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<ComputerCodeTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<ComputerCodeTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            ComputerCode computerCodeModel = null;
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
                    if (item.Code.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"电算代号编号为空");
                    }
                    if (item.Name.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"电算代号名称为空");
                    }
                    if (item.Type.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"电算代号类型为空");
                    }
                    if (item.Type == "人工")
                        item.Type = "Artificial";
                    if (item.Type == "材料")
                        item.Type = "Material";
                    if (item.Type == "机械")
                        item.Type = "Mechanics";

                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    using var uow = _unitOfWork.Begin(true, false);
                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        computerCodeModel = _repositoryComputerCode.FirstOrDefault(a => a.Code == item.Code && a.Name == item.Name);
                    }
                    if (computerCodeModel != null)
                    {
                        newInfo.AppendInfo($"{item.Name}已存在，且已更新");
                        //修改自己本身数据
                        computerCodeModel.Code = item.Code;
                        computerCodeModel.Name = item.Name;
                        computerCodeModel.Type = (ComputerCodeType)Enum.Parse(typeof(ComputerCodeType), item.Type, true);
                        computerCodeModel.Weight = Convert.ToDecimal(item.Weight);
                        computerCodeModel.Unit = item.Unit;
                        computerCodeModel.Remark = item.Remark;
                        await _repositoryComputerCode.UpdateAsync(computerCodeModel);
                        await uow.SaveChangesAsync();
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {

                        computerCodeModel = new ComputerCode(_guidGenerator.Create());
                        computerCodeModel.Code = item.Code;
                        computerCodeModel.Name = item.Name;
                        computerCodeModel.Type = (ComputerCodeType)Enum.Parse(typeof(ComputerCodeType), item.Type, true);
                        computerCodeModel.Weight = Convert.ToDecimal(item.Weight);
                        computerCodeModel.Unit = item.Unit;
                        computerCodeModel.Remark = item.Remark;
                        await _repositoryComputerCode.InsertAsync(computerCodeModel);
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
        private List<ComputerCode> GetComputerCodeData(ComputerCodeGetListDto input)
        {
            var query = _repositoryComputerCode
                .WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Keyword),
                    s => s.Name.Contains(input.Keyword) ||

                        s.Code.Contains(input.Keyword)

                 );

            return query.ToList();
        }


        bool CheckSameCode(string Code, string name, Guid? id)
        {
            bool isSame = false;
            var componentCode = _repositoryComputerCode.WithDetails()
                .Where(x => x.Code.ToUpper() == Code.ToUpper() || x.Name.ToUpper() == name.ToUpper());

            if (id.HasValue)
            {
                componentCode = componentCode.Where(x => x.Id != id.Value);
            }

            if (componentCode.Count() > 0)
            {
                isSame = true;
            }

            return isSame;
        }

        private void FindAllAreaParents(int parentid, ref List<string> list)
        {
            var parentModel = _repositoryArea.FirstOrDefault(a => a.Id == parentid);
            if (parentModel != null)
            {
                list.Add(parentModel.FullName);
                if (parentModel.ParentId != null && parentModel.ParentId >= 0)
                {
                    FindAllAreaParents(parentModel.ParentId.Value, ref list);
                }
            }

        }
    }
}
