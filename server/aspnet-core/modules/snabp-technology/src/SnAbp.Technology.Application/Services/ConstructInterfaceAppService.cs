using SnAbp.Technology.Dtos;
using SnAbp.Technology.Entities;
using SnAbp.Technology.IServices;
using System;
using SnAbp.Technology.enums;
using System.Collections.Generic;
using System.IO;
using SnAbp.Common;
using System.Text;
using Volo.Abp.Uow;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Volo.Abp.Data;
using NPOI.SS.UserModel;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Identity;
using SnAbp.Resource.Entities;

namespace SnAbp.Technology.Services
{
    public class ConstructInterfaceAppService : TechnologyAppService, IConstructInterfaceAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ConstructInterface, Guid> _constructInterfaceResposotory;
        private readonly IRepository<ConstructInterfaceInfoRltMarkFile, Guid> _constructInterfaceInfoRltMarkFileResposotory;
        readonly IFileImportHandler _fileImport;
        readonly IUnitOfWorkManager _unitOfWork;
        protected IDataDictionaryRepository _dataDictionaryRepository;
        private IOrganizationRepository _organizationRepository;
        private readonly IRepository<EquipmentGroup, Guid> _equipmentGroupRepository;
        private readonly IRepository<Equipment, Guid> _equipmentRepository;
        readonly IDataFilter _dataFilter;
        public ConstructInterfaceAppService(
            IGuidGenerator guidGenerator,
             IFileImportHandler fileImport,
            IDataFilter dataFilter,
            IRepository<EquipmentGroup, Guid> equipmentGroupRepository,
            IOrganizationRepository organizationRepository,
            IRepository<Equipment, Guid> equipmentRepository,
        IUnitOfWorkManager unitOfWork,
            IDataDictionaryRepository dataDictionaryRepository,
            IRepository<ConstructInterface, Guid> constructInterfaceResposotory,
            IRepository<ConstructInterfaceInfoRltMarkFile, Guid> constructInterfaceInfoRltMarkFileResposotory
            )
        {
            _equipmentRepository = equipmentRepository;
            _equipmentGroupRepository = equipmentGroupRepository;
            _organizationRepository = organizationRepository;
            _dataDictionaryRepository = dataDictionaryRepository;
            _dataFilter = dataFilter;
            _unitOfWork = unitOfWork;
            _fileImport = fileImport;
            _guidGenerator = guidGenerator;
            _constructInterfaceResposotory = constructInterfaceResposotory;
            _constructInterfaceInfoRltMarkFileResposotory = constructInterfaceInfoRltMarkFileResposotory;

        }
        public Task<ConstructInterfaceDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");
            var result = _constructInterfaceResposotory.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (result == null) throw new UserFriendlyException("当前接口不存在");
            var res = ObjectMapper.Map<ConstructInterface, ConstructInterfaceDto>(result);
            return Task.FromResult(res);
        }
        public async Task<ConstructInterfaceDto> Create(ConstructInterfaceCreateDto input)
        {
            var ConstructInterface = new ConstructInterface();
            ObjectMapper.Map(input, ConstructInterface);
            //1、保存基本信息
            ConstructInterface.SetId(_guidGenerator.Create());
            ConstructInterface.MarkType = MarkType.NoCheck;
            await CheckSameName(input.Name, input.Code);
            await _constructInterfaceResposotory.InsertAsync(ConstructInterface);
            var ConstructInterfaceDto = ObjectMapper.Map<ConstructInterface, ConstructInterfaceDto>(ConstructInterface);
            return ConstructInterfaceDto;
        }



        public Task<PagedResultDto<ConstructInterfaceDto>> GetList(ConstructInterfaceSearchDto input)
        {
            var result = new PagedResultDto<ConstructInterfaceDto>();
            var constructInterface = _constructInterfaceResposotory.WithDetails()
                //.WhereIf(input.ConstructionSectionId != null && input.ConstructionSectionId != Guid.Empty, x => x.ConstructionSectionId == input.ConstructionSectionId)
                .WhereIf(input.ProfessionId != null && input.ProfessionId != Guid.Empty, x => x.ProfessionId == input.ProfessionId)
                .WhereIf(input.BuilderId != null && input.BuilderId != Guid.Empty, x => x.BuilderId == input.BuilderId)
                .WhereIf(input.InterfaceManagementTypeId != null && input.InterfaceManagementTypeId != Guid.Empty, x => x.InterfaceManagementTypeId == input.InterfaceManagementTypeId)
                .WhereIf(input.MarkType.IsIn(MarkType.NoCheck, MarkType.NoQualified, MarkType.Qualified), x => x.MarkType == input.MarkType);

            var res = ObjectMapper.Map<List<ConstructInterface>, List<ConstructInterfaceDto>>(constructInterface.OrderBy(x => x.Name).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            result.Items = res;
            result.TotalCount = constructInterface.Count();
            return Task.FromResult(result);
        }

        public async Task<ConstructInterfaceDto> Update(ConstructInterfaceUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的数据");
            var constructInterface = await _constructInterfaceResposotory.GetAsync(input.Id);
            if (constructInterface == null) throw new UserFriendlyException("当前数据不存在");
            ObjectMapper.Map(input, constructInterface);
            await _constructInterfaceResposotory.UpdateAsync(constructInterface);
            return ObjectMapper.Map<ConstructInterface, ConstructInterfaceDto>(constructInterface);
        }

        public Task<Stream> Export(ConstructInterfaceExportDto input)
        {
            //1、获取需要导出的所有数据
            var datas = _constructInterfaceResposotory.WithDetails()
                 .WhereIf(input.Paramter.InterfaceManagementTypeId != null && input.Paramter.InterfaceManagementTypeId != Guid.Empty, x => x.InterfaceManagementTypeId == input.Paramter.InterfaceManagementTypeId)
                //.WhereIf(input.Paramter.ConstructionSectionId != null && input.Paramter.ConstructionSectionId != Guid.Empty, x => x.ConstructionSectionId == input.Paramter.ConstructionSectionId)
                .WhereIf(input.Paramter.ProfessionId != null && input.Paramter.ProfessionId != Guid.Empty, x => x.ProfessionId == input.Paramter.ProfessionId)
                .WhereIf(input.Paramter.BuilderId != null && input.Paramter.BuilderId != Guid.Empty, x => x.BuilderId == input.Paramter.BuilderId)
                .WhereIf(input.Paramter.MarkType.IsIn(MarkType.NoCheck, MarkType.NoQualified, MarkType.Qualified), x => x.MarkType == input.Paramter.MarkType).ToList();
            if (datas.Count == 0)
            {
                throw new UserFriendlyException("数据为空!!!");
            }
            List<ConstructInterfaceTemplateDto> constructInterfaceList = new List<ConstructInterfaceTemplateDto>();

            foreach (var item in datas)
            {
                ConstructInterfaceTemplateDto constructInterface = new ConstructInterfaceTemplateDto();
                constructInterface.Name = item.Name;
                //constructInterface.ConstructionSection = item.ConstructionSection != null ? item.ConstructionSection.Name : null;
                constructInterface.Code = item.Code;
                constructInterface.MarerialCount = item.MarerialCount;
                constructInterface.MarerialName = item.MarerialName;
                constructInterface.MaterialSpec = item.MaterialSpec;
                constructInterface.Profession = item.Profession != null ? item.Profession.Name : null;
                constructInterface.GisData = item.GisData;
                constructInterface.Position = item.Position;
                constructInterface.Builder = item.Builder != null ? item.Builder.Name : null;
                constructInterface.Equipment = item.Equipment != null ? item.Equipment.Name : null;
                constructInterface.EquipmentGroup = item.Equipment != null && item.Equipment.Group != null ? item.Equipment.Group.Name : null;
                //constructInterface.MarkType = transformMarkType(item.MarkType);
                constructInterface.InterfaceManagementType = item.InterfaceManagementType?.Name;
                constructInterfaceList.Add(constructInterface);
            }

            var stream = Utils.ExcelHelper.ExcelHelper.ExcelExportStream(constructInterfaceList, input.TemplateKey, input.RowIndex);
            return Task.FromResult(stream);
        }


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Volo.Abp.Uow.UnitOfWork]
        public async Task Upload([FromForm] ConstructInterfaceUploadDto input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            var rowIndex = 2; //有效数据额的起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<ConstructInterfaceTemplateDto> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<ConstructInterfaceTemplateDto>(rowIndex);
                datalist = sheet
                    .TryTransToList<ConstructInterfaceTemplateDto>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }
            // 定义错误信息列
            var wrongInfos = new List<WrongInfo>();
            ConstructInterface hasConstructInterfaceModel;
            var addConstructInterface = new List<ConstructInterface>();

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
                    if (item.Name.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("接口名称不能为空");
                    }
                    if (item.Code.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("接口编号不能为空");
                    }
                    //if (item.Position.IsNullOrEmpty())
                    //{
                    //    canInsert = false;
                    //    newInfo.AppendInfo("接口位置不能为空");
                    //}
                    if (item.InterfaceManagementType.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("接口管理类型不能为空");
                    }
                    if (item.EquipmentGroup.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("设备分组不能为空");

                    }
                    if (item.Equipment.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("设备名称不能为空");
                    }
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    //判断专业是否存在
                    var profession = _dataDictionaryRepository.Where(x => x.Name == item.Profession).FirstOrDefault();
                    if (!string.IsNullOrEmpty(item.Profession) && profession == null)
                    {
                        newInfo.AppendInfo($"{item.Profession}不存在");
                        wrongInfos.Add(newInfo);
                        continue;

                    }

                    //判断土建单位是否存在
                    var builder = _dataDictionaryRepository.WithDetails().Where(x => x.Name == item.Builder).FirstOrDefault();
                    if (!string.IsNullOrEmpty(item.Builder) && builder == null)
                    {
                        newInfo.AppendInfo($"{item.Builder}不存在");
                        wrongInfos.Add(newInfo);
                        continue;

                    }
                    //if (!string.IsNullOrEmpty(item.MarkType))
                    //{
                    //    //判断接口检查情况是否存在
                    //    if (!transformMarkType(item.MarkType).IsIn(MarkType.NoCheck, MarkType.NoQualified, MarkType.Qualified))
                    //    {

                    //        newInfo.AppendInfo($"{item.MarkType}不存在（检查情况是未检查，合格，不合格）");
                    //        wrongInfos.Add(newInfo);
                    //        continue;

                    //    }
                    //}
                    //判断接口类型是否存在
                    var interfaceManagementType = _dataDictionaryRepository.Where(x => x.Name == item.InterfaceManagementType).FirstOrDefault();
                    if (!string.IsNullOrEmpty(item.InterfaceManagementType) && interfaceManagementType == null)
                    {
                        newInfo.AppendInfo($"{item.InterfaceManagementType}不存在");
                        wrongInfos.Add(newInfo);
                        continue;

                    }
                    //判断设备分组是否存在
                    var equipmentGroup = _equipmentGroupRepository.WithDetails().Where(x => x.Name == item.EquipmentGroup).FirstOrDefault();
                    if (!string.IsNullOrEmpty(item.EquipmentGroup) && equipmentGroup == null)
                    {
                        newInfo.AppendInfo($"{item.EquipmentGroup}不存在");
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    //判断设备是否存在
                    var equipment = _equipmentRepository.WithDetails().Where(x => x.Name == item.Equipment && x.Group.Name == item.EquipmentGroup).FirstOrDefault();
                    if (!string.IsNullOrEmpty(item.Equipment) && !string.IsNullOrEmpty(item.EquipmentGroup) && equipment == null)
                    {
                        newInfo.AppendInfo($"{item.Equipment}不存在");
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    /*判断接口名称是否规范*/
                    if (!string.IsNullOrEmpty(item.Name) && equipment != null && !item.Name.StartsWith(equipment.Name + "_[JK]") && (equipment.Name + "_[JK]").Contains(item.Name))
                    {
                        newInfo.AppendInfo($"{item.Name}命名格式错误，格式：设备名称+__[JK]+接口名称");
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    using var uow = _unitOfWork.Begin(true);

                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        hasConstructInterfaceModel =
                            _constructInterfaceResposotory.FirstOrDefault(a => a.Name == item.Name && a.Code == item.Code);
                    }
                    if (hasConstructInterfaceModel != null)
                    {
                        newInfo.AppendInfo($"{item.Name}已存在，且已更新");
                        hasConstructInterfaceModel.Name = item.Name;
                        hasConstructInterfaceModel.Code = item.Code;
                        hasConstructInterfaceModel.MarerialCount = item.MarerialCount;
                        hasConstructInterfaceModel.MarerialName = item.MarerialName;
                        hasConstructInterfaceModel.MaterialSpec = item.MaterialSpec;
                        hasConstructInterfaceModel.Position = item.Position;
                        hasConstructInterfaceModel.GisData = item.GisData;
                        if (!string.IsNullOrEmpty(item.Profession) && profession != null)
                        {
                            hasConstructInterfaceModel.ProfessionId = profession.Id;
                        }
                        if (!string.IsNullOrEmpty(item.Builder) && builder != null)
                        {
                            hasConstructInterfaceModel.BuilderId = builder.Id;
                        }
                        if (!string.IsNullOrEmpty(item.InterfaceManagementType) && interfaceManagementType != null)
                        {
                            hasConstructInterfaceModel.InterfaceManagementTypeId = interfaceManagementType.Id;
                        }
                        //if (!string.IsNullOrEmpty(item.Builder) && builder != null)
                        //{
                        //    hasConstructInterfaceModel.BuilderId = builder.Id;
                        //}
                        if (!string.IsNullOrEmpty(item.Equipment) && equipment != null)
                        {
                            hasConstructInterfaceModel.EquipmentId = equipment.Id;
                        }

                        hasConstructInterfaceModel.MarkType = MarkType.NoCheck;
                        await _constructInterfaceResposotory.UpdateAsync(hasConstructInterfaceModel);
                        await uow.SaveChangesAsync();
                        addConstructInterface.Add(hasConstructInterfaceModel);
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        var constructInterfaceAdd = new ConstructInterface();
                        ConstructInterfaceDto constructionDto = new ConstructInterfaceDto();

                        hasConstructInterfaceModel = new ConstructInterface(_guidGenerator.Create());
                        hasConstructInterfaceModel.Name = item.Name;
                        hasConstructInterfaceModel.Code = item.Code;
                        hasConstructInterfaceModel.MarerialCount = item.MarerialCount;
                        hasConstructInterfaceModel.MarerialName = item.MarerialName;
                        hasConstructInterfaceModel.MaterialSpec = item.MaterialSpec;
                        hasConstructInterfaceModel.Position = item.Position;
                        hasConstructInterfaceModel.GisData = item.GisData;
                        if (!string.IsNullOrEmpty(item.Profession) && profession != null)
                        {
                            hasConstructInterfaceModel.ProfessionId = profession.Id;
                        }
                        //if (!string.IsNullOrEmpty(item.Builder) && builder != null)
                        //{
                        //    hasConstructInterfaceModel.BuilderId = builder.Id;
                        //}
                        if (!string.IsNullOrEmpty(item.Builder) && builder != null)
                        {
                            hasConstructInterfaceModel.BuilderId = builder.Id;
                        }
                        if (!string.IsNullOrEmpty(item.Equipment) && equipment != null)
                        {
                            hasConstructInterfaceModel.EquipmentId = equipment.Id;
                        }

                        hasConstructInterfaceModel.MarkType = MarkType.NoCheck;

                        if (!string.IsNullOrEmpty(item.InterfaceManagementType) && interfaceManagementType != null)
                        {
                            hasConstructInterfaceModel.InterfaceManagementTypeId = interfaceManagementType.Id;
                        }
                        await _constructInterfaceResposotory.InsertAsync(hasConstructInterfaceModel);
                        addConstructInterface.Add(hasConstructInterfaceModel);
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
        private string transformMarkType(MarkType Type)
        {
            string typeName = null;

            if (Type == MarkType.NoCheck)
            {
                typeName = "未检查";
            }
            if (Type == MarkType.Qualified)
            {
                typeName = "合格";
            }
            if (Type == MarkType.NoQualified)
            {
                typeName = "不合格";
            }
            return typeName;
        }
        private string transformConstructType(ConstructType Type)
        {
            string typeName = null;

            if (Type == ConstructType.Civil)
            {
                typeName = "土建工程";
            }
            if (Type == ConstructType.Electric)
            {
                typeName = "四电工程";
            }
            return typeName;
        }
        private MarkType transformMarkType(string Type)
        {
            MarkType markType;
            switch (Type)
            {
                case "未检查":
                    markType = MarkType.NoCheck;
                    break;
                case "合格":
                    markType = MarkType.Qualified;
                    break;
                case "不合格":
                    markType = MarkType.NoQualified;
                    break;
                default:
                    markType = MarkType.NoCheck;
                    break;

            }
            return markType;
        }
        private ConstructType transformConstructType(string Type)
        {
            ConstructType typeName = 0;
            switch (Type)
            {
                case "土建工程":
                    typeName = ConstructType.Civil;
                    break;
                case "四电工程":
                    typeName = ConstructType.Electric;
                    break;
            }
            return typeName;
        }
        #endregion


        #region 私有方法
        private async Task<bool> CheckSameName(string name, string code)
        {
            return await Task.Run(() =>
            {
                var sameNames = _constructInterfaceResposotory
                .FirstOrDefault(a => a.Name == name || a.Code == code);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("当前计划名称或者编码已存在！");
                }

                return true;
            });
        }
        #endregion
    }

}