using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.Identity;
using SnAbp.Resource.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Dtos.Export;
using SnAbp.Resource.Entities;
using SnAbp.Resource.IServices.EquipmentGroup;
using SnAbp.Resource.TempleteModel;
using SnAbp.StdBasic.Dtos;
using SnAbp.Utils;
using SnAbp.Utils.DataImport;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Resource.Services
{
    [Authorize(ResourcePermissions.EquipmentGroup.Default)]
    public class ResourceEquipmentGroupAppService : ResourceAppService, IResourceEquipmentGroupAppService
    {
        private readonly IRepository<Equipment, Guid> _repositoryEquipment;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IRepository<EquipmentGroup, Guid> _equipmentGroupRepository;
        private readonly IRepository<Organization, Guid> _organizationsRepository;
        readonly IUnitOfWorkManager _unitOfWork;

        public ResourceEquipmentGroupAppService(
            IRepository<Equipment, Guid> repositoryEquipment,
            IGuidGenerator guidGenerator,
            IFileImportHandler fileImport,
        IRepository<EquipmentGroup, Guid> equipmentGroupRepository,
        IRepository<Organization, Guid> organizationsRepository,
        IUnitOfWorkManager unitOfWork
            )
        {
            _repositoryEquipment = repositoryEquipment;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _equipmentGroupRepository = equipmentGroupRepository;
            _organizationsRepository = organizationsRepository;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 获取设备分组信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<EquipmentGroupDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var equipmentGroup = _equipmentGroupRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (equipmentGroup == null) throw new UserFriendlyException("该设备分组不存在");
            return Task.FromResult(ObjectMapper.Map<EquipmentGroup, EquipmentGroupDto>(equipmentGroup));
        }

        // 按需加载接口
        /// <summary>
        /// 根据条件获取设备分组
        /// </summary>
        /// <param name="input">产品Id</param>
        /// <returns></returns>
        public Task<PagedResultDto<EquipmentGroupDto>> GetList(EquipmentGroupGetListDto input)
        {
            var result = new PagedResultDto<EquipmentGroupDto>();
            var equipmentGroup = new List<EquipmentGroup>();
            var dto = new List<EquipmentGroupDto>();

            if (input.Ids != null && input.Ids.Count > 0)
            {
                foreach (var id in input.Ids)
                {
                    var equipmentGroups = _equipmentGroupRepository.WithDetails().FirstOrDefault(x => x.Id == id);
                    if (equipmentGroups == null) continue;
                    var filterList = _equipmentGroupRepository
                        .WithDetails()
                        .Where(x => x.ParentId == equipmentGroups.ParentId || x.ParentId == null)
                        .ToList();
                    equipmentGroup.AddRange(filterList);
                }

                // 数据去重并转成dto
                var listDtos = ObjectMapper.Map<List<EquipmentGroup>, List<EquipmentGroupDto>>(equipmentGroup.Distinct().ToList());

                //如果子集为空设置children为null
                foreach (var item in listDtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<EquipmentGroupDto>();
                    }
                }

                dto = GuidKeyTreeHelper<EquipmentGroupDto>.GetTree(listDtos);
            }
            else
            {
                if (!string.IsNullOrEmpty(input.Name))
                {
                    equipmentGroup = _equipmentGroupRepository.WithDetails()
                                             .WhereIf(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name)).Take(200).ToList();
                    dto = ObjectMapper.Map<List<EquipmentGroup>, List<EquipmentGroupDto>>(equipmentGroup);
                    foreach (var item in dto)
                    {
                        item.Children = null;
                    }
                }
                else
                {
                    equipmentGroup = _equipmentGroupRepository.WithDetails()
               .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
               .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
               .ToList();
                    dto = ObjectMapper.Map<List<EquipmentGroup>, List<EquipmentGroupDto>>(equipmentGroup);
                    foreach (var item in dto)
                    {
                        if (item.Children.Count == 0)
                        {
                            item.Children = null;
                        }
                        else
                        {
                            item.Children = new List<EquipmentGroupDto>();
                        }
                    }
                }

            }


            result.TotalCount = dto.Count();
            result.Items = input.IsAll ? dto : dto.OrderBy(x => x.Order).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return Task.FromResult(result);
        }

        /// <summary>
        /// 添加设备分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.EquipmentGroup.Create)]
        public async Task<EquipmentGroupDto> Create(EquipmentGroupCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("设备分组名字不能为空");
            if (input.OrganizationId == null || input.OrganizationId == Guid.Empty) throw new UserFriendlyException("组织机构名字不能为空");
            CheckSameName(input.Name, null);
            var equipmentGroup = new EquipmentGroup(_guidGenerator.Create());
            equipmentGroup.Name = input.Name;
            equipmentGroup.Order = input.Order;
            equipmentGroup.ParentId = input.ParentId;
            equipmentGroup.OrganizationId = input.OrganizationId;
            await _equipmentGroupRepository.InsertAsync(equipmentGroup);
            return ObjectMapper.Map<EquipmentGroup, EquipmentGroupDto>(equipmentGroup);

        }

        /// <summary>
        /// 修改设备分组信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.EquipmentGroup.Update)]
        public async Task<EquipmentGroupDto> Update(EquipmentGroupUpdateDto input)
        {
            if (input.Id == null && input.Id == Guid.Empty) throw new UserFriendlyException("请输入设备分组的id");
            var equipmentGroup = await _equipmentGroupRepository.GetAsync(input.Id);
            if (equipmentGroup == null) throw new UserFriendlyException("当前设备分组不存在");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("设备分组名字不能为空");
            if (input.OrganizationId == null || input.OrganizationId == Guid.Empty) throw new UserFriendlyException("组织机构名字不能为空");
            CheckSameName(input.Name, input.Id);
            equipmentGroup.Name = input.Name;
            equipmentGroup.Order = input.Order;
            equipmentGroup.ParentId = input.ParentId;
            equipmentGroup.OrganizationId = input.OrganizationId;
            await _equipmentGroupRepository.UpdateAsync(equipmentGroup);
            return ObjectMapper.Map<EquipmentGroup, EquipmentGroupDto>(equipmentGroup);
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.EquipmentGroup.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var equipmentGroup = _equipmentGroupRepository.WithDetails(x => x.Children).FirstOrDefault(x => x.Id == id);
            if (string.IsNullOrEmpty(equipmentGroup.Name)) throw new UserFriendlyException("该设备分组不存在");
            if (equipmentGroup.Children != null && equipmentGroup.Children.Count > 0) throw new UserFriendlyException("请先删除该设备分组的下级分类");
            var equipments = _repositoryEquipment.WithDetails(x=>x.Group).Where(s => s.GroupId==id).ToList();
            if (equipments.Count > 0) throw new UserFriendlyException("当前设备分组下存在设备，不能被删除");
            await _equipmentGroupRepository.DeleteAsync(id);

            return true;
        }

        /// <summary>
        /// 导入设备分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task upload([FromForm] DataImportDto input)
        {
            await _fileImport.Start(input.ImportKey, 100);
            DataTable dt = null;
            IWorkbook workbook = null;
            ISheet sheet = null;

            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0);
                dt = ExcelHelper.ImportBaseDataToDataTable(input.File.File.OpenReadStream(), input.File.File.FileName, out var workbook1);
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }
            if (dt == null)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("未找到任何数据,请检查文件格式");
            }

            #region 验证列是否存在
            if (!dt.Columns.Contains("SeenSun"))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format("导入表格式列序号不存在"));
            }
            if (!dt.Columns.Contains("Name"))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format("导入表格式列名称不存在"));
            }
            if (!dt.Columns.Contains("Order"))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format("导入表格式列序号不存在"));
            }
            if (!dt.Columns.Contains("Organization"))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format("导入表格式列所属机构不存在"));
            }
            #endregion

            var dataList = new List<GroupCol>();
            Organization hasOrganization;
            IEnumerable<EquipmentGroup> hasEquipmentGroup;


            #region 验证并获取数据
            var dataModel = (GroupCol)null;
            var colVal = (string)null;
            var rowNmb = (int)2;

            foreach (DataRow item in dt.Rows)
            {
                rowNmb++;
                //序号
                colVal = Convert.ToString(item["SeenSun"]);
                dataModel = new GroupCol()
                {
                    SeenSun = colVal,
                };

                //设备分类
                colVal = Convert.ToString(item["Name"]);
                if (string.IsNullOrEmpty(colVal))
                {
                    WrongInfo wrong = new WrongInfo(rowNmb - 1, "编码为空");
                    wrongInfos.Add(wrong);
                    continue;
                }
                dataModel.Name = colVal;

                //序号
                colVal = Convert.ToString(item["Order"]);
                if (string.IsNullOrEmpty(colVal))
                {
                    dataModel.Order = "0";
                }
                else
                {
                    dataModel.Order = colVal;
                }

                //名称
                dataModel.Items = new List<PartItem>();
                var names = dataModel.Name.Split("-");
                var childItem = (PartItem)null;
                for (var i = 0; i < names.Length; i++)
                {
                    childItem = new PartItem();
                    childItem.Name = names[i];
                    dataModel.Items.Add(childItem);
                }

                //所属机构
                colVal = Convert.ToString(item["Organization"]);
                childItem = (PartItem)null;
                if (string.IsNullOrEmpty(colVal))
                {
                    WrongInfo wrong = new WrongInfo(rowNmb - 1, "编码为空");
                    wrongInfos.Add(wrong);
                    continue;
                }
                dataModel.Organization = colVal;

                //所属机构编码
                colVal = Convert.ToString(item["OrganizationCode"]);
                childItem = (PartItem)null;
                if (string.IsNullOrEmpty(colVal))
                {
                    WrongInfo wrong = new WrongInfo(rowNmb - 1, "机构编码为空");
                    wrongInfos.Add(wrong);
                    continue;
                }
                dataModel.OrganizationCode = colVal;

                dataList.Add(dataModel);
            }

            #endregion

            #region 处理数据
            var allEquipmentGroup = _equipmentGroupRepository.Where(x => x.Id != Guid.Empty).ToList();
            var allOrganization = _organizationsRepository.Where(x => x.Id != Guid.Empty).ToList();
            var groupList = dataList.GroupBy(a => a.Organization).ToList();

            _allGroupCol = _equipmentGroupRepository.Where(x => x.Id != Guid.Empty).ToList();

            await _fileImport.ChangeTotalCount(input.ImportKey, dataList.Count);
            var updateIndex = 1;
            using var unow = _unitOfWork.Begin(true, false);
            foreach (var group in groupList)
            {
                foreach (var item in group.ToList())
                {
                    await _fileImport.UpdateState(input.ImportKey, updateIndex);
                    updateIndex++;

                    hasOrganization = allOrganization.FirstOrDefault(x => x.Name == item.Organization && x.CSRGCode == item.OrganizationCode);
                    if (hasOrganization != null) //所属机构已导入时，再进行。
                    {
                        //递归判断与添加名称与组织机构
                        DgInsertToEquipmentGroup(item, item.Items, item.Items.First(), null, hasOrganization.Id, 0);
                        await unow.SaveChangesAsync();
                    }
                    else
                    {
                        WrongInfo wrong = new WrongInfo(int.Parse(item.SeenSun) + 1, "找不到所属机构");
                        wrongInfos.Add(wrong);
                        continue;
                    }
                }
            }
            await _fileImport.Complete(input.ImportKey);

            if (wrongInfos.Count > 0)
            {
                sheet.CreateInfoColumn(wrongInfos);
                await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
            }
            #endregion
        }

        public async Task<Stream> Export(EquipmentGroupData input)
        {
            List<IGrouping<Guid?,EquipmentGroup>> list;
            if (input.paramter.Name != "" || input.paramter.ParentId != null)
            {
                list = GetGroupData(input.paramter).AsEnumerable().GroupBy(y => y.ParentId).ToList();
            }
            else
            {
                list = _equipmentGroupRepository.Where(x => x.Id != Guid.Empty).AsEnumerable().GroupBy(y => y.ParentId).ToList();
            }
            var groupList = await _equipmentGroupRepository.GetListAsync();
            var orgList = await _organizationsRepository.GetListAsync();

            var groupCols = new List<EquipmentGroupModel>();
            var groupCol = new EquipmentGroupModel();
            foreach (var item in list)
            {
                foreach (var ite in item)
                {
                    groupCol = new EquipmentGroupModel();
                    var org = orgList.FirstOrDefault(x => x.Id == ite.OrganizationId);
                    groupCol.Organization = org.Name;
                    groupCol.OrganizationCode = org.CSRGCode;
                    if (ite.ParentId != null && ite.ParentId != Guid.Empty)
                    {
                        groupCol.Name = ite.Name;
                        DgGetToEquipmentGroupName(groupList, groupCol, ite); //递归获取拼接后的名称
                    }
                    else
                    {
                        groupCol.Name = ite.Name;
                    }
                    groupCol.Order = ite.Order.ToString();

                    groupCols.Add(groupCol);
                }
            }
            var stream = ExcelHelper.ExcelExportStream(groupCols, input.TemplateKey, input.RowIndex);
            return stream;
        }


        #region 私有方法
        
        private List<EquipmentGroup> GetGroupData(EquipmentGroupGetListDto input)
        {
            var equipmentGroup = new List<EquipmentGroup>();
            var dto = new List<EquipmentGroupDto>();

            if (input.Ids != null && input.Ids.Count > 0)
            {
                foreach (var id in input.Ids)
                {
                    var equipmentGroups = _equipmentGroupRepository.WithDetails().FirstOrDefault(x => x.Id == id);
                    if (equipmentGroups == null) continue;
                    var filterList = _equipmentGroupRepository
                        .WithDetails()
                        .Where(x => x.ParentId == equipmentGroups.ParentId || x.ParentId == null)
                        .ToList();
                    equipmentGroup.AddRange(filterList);
                }

                // 数据去重并转成dto
                var listDtos = ObjectMapper.Map<List<EquipmentGroup>, List<EquipmentGroupDto>>(equipmentGroup.Distinct().ToList());

                //如果子集为空设置children为null
                foreach (var item in listDtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<EquipmentGroupDto>();
                    }
                }

                dto = GuidKeyTreeHelper<EquipmentGroupDto>.GetTree(listDtos);
            }
            else
            {
                if (!string.IsNullOrEmpty(input.Name))
                {
                    equipmentGroup = _equipmentGroupRepository.WithDetails()
                                             .WhereIf(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name)).Take(200).ToList();
                    dto = ObjectMapper.Map<List<EquipmentGroup>, List<EquipmentGroupDto>>(equipmentGroup);
                    foreach (var item in dto)
                    {
                        item.Children = null;
                    }
                }
                else
                {
                    equipmentGroup = _equipmentGroupRepository.WithDetails()
               .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
               .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
               .ToList();
                    dto = ObjectMapper.Map<List<EquipmentGroup>, List<EquipmentGroupDto>>(equipmentGroup);
                    foreach (var item in dto)
                    {
                        if (item.Children.Count == 0)
                        {
                            item.Children = null;
                        }
                        else
                        {
                            item.Children = new List<EquipmentGroupDto>();
                        }
                    }
                }
            }
            var resource = ObjectMapper.Map<List<EquipmentGroupDto>, List<EquipmentGroup>>(dto);
            return resource;
        }
        
        private bool CheckSameName(string Name)
        {
            var sameEquipmentGroup = _equipmentGroupRepository.Where(x => x.Name.ToUpper() == Name.ToUpper());
            if (sameEquipmentGroup.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("分类中已经存在相同设备分组的名字!!!");
            }
            return true;
        }
        private bool CheckSameName(string Name, Guid? id)
        {
            var sameStoreHouse = _equipmentGroupRepository.Where(x => x.Name.ToUpper() == Name.ToUpper());
            if (id.HasValue)
            {
                sameStoreHouse = sameStoreHouse.Where(x => x.Id != id.Value);
            }
            if (sameStoreHouse.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("已存在相同名字的设备分组！！！");
            }
            return true;
        }

        private List<EquipmentGroup> _allGroupCol;
        private List<WrongInfo> wrongInfos = new List<WrongInfo>();
        private async void DgInsertToEquipmentGroup(GroupCol groupCol, List<PartItem> partItems, PartItem items, Guid? parentId, Guid? orgId, int count)
        {
            EquipmentGroup group;
            if (count < partItems.Count)
            {
                group = _allGroupCol.FirstOrDefault(x => x.Name == items.Name && x.ParentId == parentId && x.OrganizationId == orgId);
                if (group == null)
                {
                    var group1 = _allGroupCol.FirstOrDefault(x => x.Name == items.Name && x.ParentId != parentId);
                    if (group1 != null)
                    {
                        WrongInfo wrong = new WrongInfo(int.Parse(groupCol.SeenSun) + 1, $"设备分组名称＜{items.Name}＞重复");
                        wrongInfos.Add(wrong);
                    }
                    else
                    {
                        group = new EquipmentGroup(_guidGenerator.Create());
                        group.Name = items.Name;
                        group.ParentId = parentId;
                        group.Order = int.Parse(groupCol.Order);
                        group.OrganizationId = orgId;
                        _allGroupCol.Add(group);
                        await _equipmentGroupRepository.InsertAsync(group);

                        count++;
                        if (count < partItems.Count)
                        {
                            DgInsertToEquipmentGroup(groupCol, partItems, partItems[count], group.Id, orgId, count);
                        }
                    }
                }
                else
                {
                    count++;
                    if (count < partItems.Count)
                    {
                        DgInsertToEquipmentGroup(groupCol, partItems, partItems[count], group.Id, orgId, count);
                    }
                }
            }
        }

        private void DgGetToEquipmentGroupName(List<EquipmentGroup> groupList,EquipmentGroupModel groupCol, EquipmentGroup ite)
        {
            var group = groupList.FirstOrDefault(x => x.Id == ite.ParentId);

            groupCol.Name = group.Name + "-" + groupCol.Name;

            if (group.ParentId != null && group.ParentId != Guid.Empty)
            {
                DgGetToEquipmentGroupName(groupList, groupCol, group);
            }
            else
            {
                groupCol.Name = groupCol.Name.Trim('-');
            }
        }

        #endregion

    }
}
