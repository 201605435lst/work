using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Basic.Entities;
using SnAbp.Common;
using SnAbp.Identity;
using SnAbp.Resource.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using SnAbp.Resource.IServices.Equipment;
using SnAbp.StdBasic.Entities;
using SnAbp.Utils.DataImport;
using SnAbp.Utils.EnumHelper;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using SnAbp.StdBasic.Dtos;
using SnAbp.Utils;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Uow;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.Basic.Enums;
using Terminal = SnAbp.Resource.Entities.Terminal;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Data;
using System.IO;
using SnAbp.Resource.TempleteModel;
using SnAbp.Resource.Dtos.Export;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using SnAbp.Common.IServices;
using SnAbp.Common.Dtos.Task;
using System.Threading;
using Newtonsoft.Json;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceEquipmentAppService : ResourceAppService, IResourceEquipmentAppService
    {
        private readonly IRepository<Equipment, Guid> _repositoryEquipment;
        private readonly IRepository<EquipmentGroup, Guid> _repositoryEquipmentGroup;
        private readonly IRepository<ComponentCategory, Guid> _componentCategoryRepository;
        private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<EquipmentRltOrganization, Guid> equipmentOrgRepository;
        private readonly IRepository<EquipmentRltFile, Guid> _equipmentRltFile;
        private readonly IRepository<EquipmentProperty, Guid> _equipmentPropertyRepository;
        private readonly IRepository<EquipmentServiceRecord, Guid> _equipmentServiceProperty;
        private readonly IRepository<StoreEquipment, Guid> _storeEquipmentRepository;
        private readonly IRepository<InstallationSite, Guid> installationSitesRepository;
        private readonly IRepository<Organization, Guid> orgRepository;
        private readonly IRepository<ComponentCategory, Guid> crmRepository;//标准库构件
        private readonly IRepository<ProductCategory, Guid> productRepository;//标准库产品
        private readonly IRepository<Model, Guid> modelRepository;//标准库标准设备
        private readonly IRepository<Manufacturer, Guid> manufacturerRepository;//标准库厂家
        private readonly IRepository<CableCore, Guid> _cableCoreRepository;//电缆芯
        private readonly IRepository<CableExtend, Guid> _cableExtendRepository;
        private readonly IRepository<Terminal, Guid> _terminalRepository;// 端子表
        private readonly IRepository<TerminalLink, Guid> _terminalLinkRepository;//端子关联表
        private readonly IRepository<EquipmentGroup, Guid> _equipmentGroupsRepository;//设备分类
        private readonly IFileImportHandler _fileImport;
        private readonly IEquipmentRepository EquipmentManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Station, Guid> _stations;
        private readonly IRepository<Railway, Guid> _railways;
        readonly IUnitOfWorkManager _unitOfWork;
        private readonly IRepository<ProjectItemRltComponentCategory, Guid> _repositoryProjectItemRltComponentCategory;
        private readonly IRepository<ProjectItemRltProductCategory, Guid> _repositoryProjectItemRltProductCategory;
        private readonly IDistributedCache<List<EquipmentDto>> _cacheManagerScopeEquipmentDtos;
        private readonly IDistributedCache<List<ComponentCategoryDto>> _cacheManagerComponentCategories;
        private readonly ICommonBackgroundTaskAppService _commonBackgroundTaskAppService;
        private readonly IdentityUserManager usersRepository;
        private readonly OrganizationManager orgManager;
        private readonly IRepository<MVDProperty> _repositoryMVDProperty;

        protected IDataFilter DataFilter { get; }


        public ResourceEquipmentAppService(
            IRepository<Equipment, Guid> repositoryEquipment,
            IRepository<EquipmentGroup, Guid> repositoryEquipmentGroup,
            IRepository<ComponentCategory, Guid> componentCategoryRepository,
            IRepository<ProductCategory, Guid> productCategoryRepository,
            IRepository<EquipmentRltOrganization, Guid> equipmentOrgRep,
            IRepository<EquipmentRltFile, Guid> equipmentRltFile,
            IRepository<StoreEquipment, Guid> storeEquipment,
            IRepository<InstallationSite, Guid> installationSitesRep,
            IRepository<Organization, Guid> orgRep, IRepository<ComponentCategory, Guid> crmRep,
            IRepository<ProductCategory, Guid> productRep,
            IRepository<Model, Guid> modelRep,
            IRepository<Manufacturer, Guid> manufacturerRep,
            IUnitOfWorkManager unitOfWork,
            IGuidGenerator guidGenerator, IFileImportHandler fileImport,
            IRepository<StoreEquipment, Guid> storeEquipmentRepository,
            IRepository<EquipmentServiceRecord, Guid> equipmentServiceProperty,
            IEquipmentRepository equipmentManager,
            IRepository<CableCore, Guid> cableCoreRepository,
            IRepository<CableExtend, Guid> cableExtendRepository,
            IRepository<Terminal, Guid> terminalRepository,
            IRepository<TerminalLink, Guid> terminalLinkRepository,
            IRepository<EquipmentGroup, Guid> equipmentGroupsRepository,
            IHttpContextAccessor httpContextAccessor,
            IDataFilter dataFilter,
            IRepository<EquipmentProperty, Guid> equipmentProperty,
            IRepository<Station, Guid> stations, //站点
            IRepository<Railway, Guid> railways, //线路
            IRepository<ProjectItemRltComponentCategory, Guid> repositoryProjectItemRltComponentCategory,
            IRepository<ProjectItemRltProductCategory, Guid> repositoryProjectItemRltProductCategory,
            IDistributedCache<List<EquipmentDto>> cacheManagerScopeEquipmentDtos,
            IDistributedCache<List<ComponentCategoryDto>> cacheManagerComponentCategories,
            ICommonBackgroundTaskAppService commonBackgroundTaskAppService,
            IdentityUserManager usersRepository,
            OrganizationManager orgManager,
            IRepository<MVDProperty> repositoryMVDProperty
            )
        {
            _repositoryEquipment = repositoryEquipment;
            _repositoryEquipmentGroup = repositoryEquipmentGroup;
            _componentCategoryRepository = componentCategoryRepository;
            _productCategoryRepository = productCategoryRepository;
            equipmentOrgRepository = equipmentOrgRep;
            _equipmentRltFile = equipmentRltFile;
            _storeEquipmente = storeEquipment;
            installationSitesRepository = installationSitesRep;
            orgRepository = orgRep;
            crmRepository = crmRep;
            productRepository = productRep;
            modelRepository = modelRep;
            manufacturerRepository = manufacturerRep;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _unitOfWork = unitOfWork;
            _storeEquipmentRepository = storeEquipmentRepository;
            _equipmentServiceProperty = equipmentServiceProperty;
            EquipmentManager = equipmentManager;
            _cableCoreRepository = cableCoreRepository;
            _cableExtendRepository = cableExtendRepository;
            _terminalRepository = terminalRepository;
            _terminalLinkRepository = terminalLinkRepository;
            _equipmentGroupsRepository = equipmentGroupsRepository;
            _httpContextAccessor = httpContextAccessor;
            DataFilter = dataFilter;
            _equipmentPropertyRepository = equipmentProperty;
            _stations = stations;
            _railways = railways;
            _repositoryProjectItemRltComponentCategory = repositoryProjectItemRltComponentCategory;
            _repositoryProjectItemRltProductCategory = repositoryProjectItemRltProductCategory;
            _cacheManagerScopeEquipmentDtos = cacheManagerScopeEquipmentDtos;
            _cacheManagerComponentCategories = cacheManagerComponentCategories;
            _commonBackgroundTaskAppService = commonBackgroundTaskAppService;
            this.usersRepository = usersRepository;
            this.orgManager = orgManager;
            _repositoryMVDProperty = repositoryMVDProperty;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<EquipmentDetailDto> Get(Guid id)
        {
            EquipmentDetailDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");

            var equipment = _repositoryEquipment.WithDetails().FirstOrDefault(s => s.Id == id);
            if (equipment == null) throw new UserFriendlyException("该设备不存在");
            result = ObjectMapper.Map<Equipment, EquipmentDetailDto>(equipment);
            if (crmRepository.FirstOrDefault(x => x.Id == equipment.ComponentCategoryId) == null) result.ComponentCategoryId = null;

            if (productRepository.FirstOrDefault(x => x.Id == equipment.ProductCategoryId) == null)
            {
                result.ProductCategoryId = null;
                result.ManufacturerId = null;
            }

            if (manufacturerRepository.FirstOrDefault(x => x.Id == equipment.ManufacturerId) == null) result.ManufacturerId = null;

            if (installationSitesRepository.FirstOrDefault(x => x.Id == equipment.InstallationSiteId) == null) result.InstallationSiteId = null;

            return Task.FromResult(result);
        }


        public Task<string> GetGisData(GetGisDataInputDto input)
        {
            Equipment equipment = null;

            if (input.Id != Guid.Empty && input.Id != null)
            {
                equipment = _repositoryEquipment.WithDetails().FirstOrDefault(x => x.Id == input.Id);
            }
            else if (!string.IsNullOrEmpty(input.GroupName) && !string.IsNullOrEmpty(input.Name))
            {
                equipment = _repositoryEquipment.WithDetails().FirstOrDefault(x => x.Name == input.Name && x.Group.Name == input.GroupName);
            }


            return Task.FromResult(equipment != null ? equipment.GisData : null);
        }


        public Task<PagedResultDto<EquipmentMiniDto>> GetByIds(List<Guid> ids)
        {
            var equipments = _repositoryEquipment.WithDetails().WhereIf(ids.Any(), x => ids.Contains((Guid)x.Id));
            var result = new PagedResultDto<EquipmentMiniDto>();
            result.TotalCount = equipments.Count();
            result.Items = ObjectMapper.Map<List<Equipment>, List<EquipmentMiniDto>>(equipments.ToList());
            return Task.FromResult(result);
        }


        /// <summary>
        /// 获得实体对象
        /// </summary>
        public Task<EquipmentDetailDto> GetByGroupNameAndName(string groupName, string name)
        {
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(name))
            {
                throw new UserFriendlyException("groupName，name 不能为空");
            }

            var equipment = _repositoryEquipment.WithDetails(x => x.Group).FirstOrDefault(x => x.Name == name && x.Group.Name == groupName);
            if (equipment == null)
            {
                return Task.FromResult(new EquipmentDetailDto());
            }

            return Task.FromResult(ObjectMapper.Map<Equipment, EquipmentDetailDto>(equipment));
        }

        /// <summary>
        /// 获得设备关联文件列表
        /// </summary>
        public Task<List<EquipmentRltFileDto>> GetFileList(Guid equipmentId)
        {
            if (equipmentId == null || equipmentId == Guid.Empty) return null;
            var fileList = _equipmentRltFile.WithDetails().Where(x => x.EquipmentId == equipmentId).ToList();

            if (fileList == null)
            {
                return Task.FromResult(new List<EquipmentRltFileDto>());
            }

            return Task.FromResult(ObjectMapper.Map<List<EquipmentRltFile>, List<EquipmentRltFileDto>>(fileList));
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EquipmentGetListDto> GetList(EquipmentSearchDto input)
        {
            using (DataFilter.Disable<ISoftDelete>())
            {

                //获取当前登录用户的组织机构
                var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
                var organization = !string.IsNullOrEmpty(organizationIdString) ? orgRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
                var organizationCode = organization != null ? organization.Code : null;

                var allOrgs = new List<Guid>();
                if (!string.IsNullOrEmpty(organizationCode))
                {
                    allOrgs = orgRepository.Where(x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();
                }
                var componentCategoryIds = new List<Guid>();
                if (input.ComponentCategoryId != null || input.ComponentCategoryId != Guid.Empty)
                {
                    var componentCategory = crmRepository.FirstOrDefault(x => x.Id == input.ComponentCategoryId);
                    if (componentCategory != null)
                        componentCategoryIds = crmRepository.Where(x => x.Code.StartsWith(componentCategory.Code)).Select(s => s.Id).ToList();
                }

                var result = new EquipmentGetListDto();

                var query = _repositoryEquipment.WithDetails()
                .WhereIf(organization != null && !string.IsNullOrEmpty(organizationCode), x => x.EquipmentRltOrganizations.Any(s => allOrgs.Contains(s.OrganizationId)) || x.EquipmentRltOrganizations.Count == 0)
                .WhereIf(string.IsNullOrEmpty(input.Keyword) &&
                        (input.InstallationSiteId == null || input.InstallationSiteId == Guid.Empty) &&
                        (input.OrganizationIds == null || input.OrganizationIds.Count == 0) &&
                        (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty), x => x.ParentId == input.ParentId)
               .WhereIf(input.InstallationSiteId != null && input.InstallationSiteId != Guid.Empty, x => x.InstallationSiteId == input.InstallationSiteId)
               .WhereIf(input.OrganizationIds != null && input.OrganizationIds.Any(), x => x.EquipmentRltOrganizations.Any(y => input.OrganizationIds.Contains(y.OrganizationId)))
               .WhereIf(componentCategoryIds.Count > 0, x => componentCategoryIds.Contains(x.ComponentCategory.Id))
               .WhereIf(!string.IsNullOrEmpty(input.Keyword), x =>
                    x.Name.Contains(input.Keyword) ||
                    x.Manufacturer.Name.Contains(input.Keyword) ||
                    x.ProductCategory.Name.Contains(input.Keyword) ||
                    x.Code.Contains(input.Keyword));

                #region 新增待入库设备统计
                var waitingStorages = query.Where(x => x.CheckState == EquipmentCheckState.UnCheck).ToList();
                var waitingStorageList = waitingStorages.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                var waitingStoragesDto = ObjectMapper.Map<List<Equipment>, List<EquipmentDto>>(waitingStorageList);

                foreach (var item in waitingStoragesDto)
                {
                    var createUserInfo = await usersRepository.GetByIdAsync(item.CreatorId);
                    if (createUserInfo != null)
                    {
                        var orgId = createUserInfo.Organizations.FirstOrDefault()?.OrganizationId;
                        var orgName = orgRepository.WhereIf(orgId != null, x => x.Id == orgId).FirstOrDefault()?.Name;
                        if (orgName == null || string.IsNullOrWhiteSpace(orgName))
                        {
                            orgName = "暂无组织机构";
                        }
                        item.CreatorName = createUserInfo.Name + "(" + orgName + ")";
                    }
                }
                result.WaitingStorageCount = waitingStorages.Count();
                result.WaitingStorages = waitingStoragesDto;
                #endregion

                var inStorages = query.Where(x => x.CheckState == EquipmentCheckState.Checked);
                var list = input.IsAll ? inStorages.ToList() : inStorages.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                var dtos = ObjectMapper.Map<List<Equipment>, List<EquipmentDto>>(list);
                foreach (var item in dtos)
                {
                    if (item.EquipmentRltOrganizations.Count > 0)
                    {
                        foreach (var _item in item.EquipmentRltOrganizations)
                        {
                            if (_item.Organization != null && _item.Organization.IsDeleted)
                            {
                                _item.Organization = null;
                                _item.OrganizationId = null;
                            }
                        }
                        if (item.ComponentCategory != null && item.ComponentCategory.IsDeleted)
                        {
                            item.ComponentCategory = null;
                            item.ComponentCategoryId = null;
                        }
                        if (item.ProductCategory != null && item.ProductCategory.IsDeleted)
                        {
                            item.ProductCategory = null;
                            item.ProductCategoryId = null;
                            item.Manufacturer = null;
                            item.ManufacturerId = null;
                        }
                        if (item.Manufacturer != null && item.Manufacturer.IsDeleted)
                        {
                            item.Manufacturer = null;
                            item.ManufacturerId = null;
                        }
                    }
                    item.Children = item.Children.Count == 0 || (!string.IsNullOrEmpty(input.Keyword) &&
                                   (input.OrganizationIds == null || input.OrganizationIds.Count == 0) &&
                                   (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty) &&
                                   (input.InstallationSiteId != null || input.InstallationSiteId != Guid.Empty)) ? null : new List<EquipmentDto>();
                }

                result.TotalCount = input.IsAll ? dtos.Count() : inStorages.Count();
                result.Items = dtos;
                return result;
            }
        }


        /// <summary>
        /// 查询待选计划关联设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<EquipmentMiniDto>> GetSimpleList(EquipmentSimpleSearchDto input)
        {
            if (input.OrgId == null || input.OrgId == Guid.Empty) throw new UserFriendlyException("组织机构未选择");

            if (input.IFDCodes.Count == 0) throw new UserFriendlyException("未关联设备");

            var organizationCode = orgRepository.FirstOrDefault(x => x.Id == input.OrgId)?.Code;
            var equipments = _repositoryEquipment.WithDetails()
              .Where(x => x.EquipmentRltOrganizations.Select(s => s.Organization).Any(m => m.Code.StartsWith(organizationCode)))
              .Where(x => input.IFDCodes.Contains(x.ComponentCategory.Code))
             //机房查询有变更
             .WhereIf(input.InstallationSiteIds != null && input.InstallationSiteIds.Count > 0, x => input.InstallationSiteIds.Contains(x.InstallationSiteId.Value))
              .WhereIf(!input.Keyword.IsNullOrEmpty(), x =>
               x.Name.Contains(input.Keyword) ||
               x.Code.Contains(input.Keyword))
              .OrderBy(x => x.Name);

            var result = new PagedResultDto<EquipmentMiniDto>();
            result.TotalCount = equipments.Count();
            result.Items = ObjectMapper.Map<List<Equipment>, List<EquipmentMiniDto>>(equipments.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return Task.FromResult(result);
        }


        [UnitOfWork(IsDisabled = true)]
        /// <summary>
        /// 获取安装位置下的设备构件分类，按需逐级加载
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<GetListByScopeOutput>> SearchListByScopeCode(SearchListByScopeInput input)
        {
            //查询范围 1@Name @OrganizationId.2@Name @RailwayId.3@Name @StationId.4@Name @InstallationId
            var scope = new Scope(input.ScopeCode);

            // 1.查询当前范围内的所有设备，为了进一步查找该范围内的设备分类
            var equipmentsScopeDtoList = await GetEquipmentsByScope(scope);
            //var equipmentsScopeDtoList = await _cacheManagerScopeEquipmentDtos.GetOrAddAsync(
            //    scope.Id.ToString(),
            //    async () =>
            //    {
            //        return await Task.Run(() =>
            //        {
            //            // 1.查询当前范围内的所有设备，为了进一步查找该范围内的设备分类
            //            var equipments = equipmentRepository
            //                .WithDetails(
            //                    x => x.ComponentCategory,
            //                    x => x.InstallationSite.Organization,
            //                    x => x.Children,
            //                    x => x.Group)
            //                .WhereIf(scope.Type == ScopeType.Organization, x => x.InstallationSite.OrganizationId == scope.Id)
            //                .WhereIf(scope.Type == ScopeType.Railway, x => x.InstallationSite.RailwayId == scope.Id)
            //                .WhereIf(scope.Type == ScopeType.Station, x => x.InstallationSite.StationId == scope.Id)
            //                .WhereIf(scope.Type == ScopeType.InstallationSite, x => x.InstallationSiteId == scope.Id)
            //                .OrderBy(x => x.Name)
            //                .ToList();

            //            equipments.ForEach(item =>
            //            {
            //                item.CreatorId = null;
            //                item.InstallationSite = null;
            //                item.InstallationSiteId = null;
            //                item.StoreEquipmentId = null;
            //            });

            //            var dtos = ObjectMapper.Map<List<Equipment>, List<EquipmentDto>>(equipments);
            //            return dtos;
            //        });
            //    },
            //    () => new DistributedCacheEntryOptions
            //    {
            //        AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
            //    }
            //);


            // 2.统计构件编码
            var equipmentScopeCagetoryCodes = equipmentsScopeDtoList
                .Where(x => x.ComponentCategoryId != null)
                .Select(x => x.ComponentCategory.Code)
                .Distinct().ToList();

            // 3.查找当前范围内的所有设备构件分类
            var categoriesAllDtos = await GetAllComponentCategories();
            var categoriesScopeDtoList = categoriesAllDtos
                .Where(x => equipmentScopeCagetoryCodes.Exists(code => code.StartsWith(x.Code)))
                .ToList();

            // 4.把实体构件成 dto，在dto上操作，防止改变数据库
            var dtos = new List<GetListByScopeOutput>();


            // 5.如果选中了一组，或多组设备（也是同一范围内的多组设备），根据设备名称反向查找这些设备的父级及父级的构件分类
            if (input.InitialGroupAndNames != null && input.InitialGroupAndNames.Count > 0 && input.ParentId == null)
            {
                var equipments = new List<EquipmentDto>();
                var categories = new List<ComponentCategoryDto>();

                // 查找设备的父级及兄弟节点
                foreach (var item in input.InitialGroupAndNames)
                {
                    var split = item.Split('@');
                    var group = split.First();
                    var name = split.Last();
                    var target = equipmentsScopeDtoList.Find(x => x.Name == name && x.Group.Name == group);
                    if (target != null)
                    {
                        equipments.AddRange(GuidKeyTreeHelper<EquipmentDto>.GetParents(equipmentsScopeDtoList, target));
                        // 添加与目标设备相同构件分类的设备
                        equipments.AddRange(equipmentsScopeDtoList.Where(x => x.ComponentCategoryId == target.ComponentCategoryId).ToList());
                    }
                }
                equipments = equipments.Distinct().ToList();

                // 父级转树状结构，然后找到根级父级
                var rootEquipments = GuidKeyTreeHelper<EquipmentDto>.GetTree(equipments);

                // 根据根级父级找出构件分类
                foreach (var equipment in rootEquipments)
                {
                    var target = categoriesScopeDtoList.Find(x => x.Id == equipment.ComponentCategoryId);
                    categories.AddRange(GuidKeyTreeHelper<ComponentCategoryDto>.GetParents(categoriesScopeDtoList, target));
                }
                categories = categories.Distinct().ToList();

                // 加入结果
                foreach (var e in equipments)
                {
                    var dto = new GetListByScopeOutput()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Type = GetListByScopeType.Equipment,
                        ComponentCategoryId = e.ComponentCategoryId,
                        ParentId = rootEquipments.IndexOf(e) > -1 ? e.ComponentCategoryId : e.ParentId,
                        Children = equipmentsScopeDtoList.Find(x => x.ParentId == e.Id) != null ? new List<GetListByScopeOutput>() : null,
                        EquipmentGroupName = e.Group.Name
                    };
                    dtos.Add(dto);
                }

                foreach (var c in categories)
                {
                    var dto = new GetListByScopeOutput()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = GetListByScopeType.ComponentCategory,
                        ParentId = c.ParentId,
                    };
                    dtos.Add(dto);
                }
                // 添加跟级分类编码
                var rootCategoryDtos = GuidKeyTreeHelper<ComponentCategoryDto>.GetTree(categoriesScopeDtoList).Where(x => x.Parent == null).ToList();
                foreach (var c in rootCategoryDtos)
                {
                    if (dtos.Find(x => x.Id == c.Id) == null)
                    {
                        dtos.Add(new GetListByScopeOutput()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Type = GetListByScopeType.ComponentCategory,
                            ParentId = c.ParentId,
                        });
                    }
                }

                dtos = GuidKeyTreeHelper<GetListByScopeOutput>.GetTree(dtos);
            }

            // 没选设备
            else
            {
                var equipments = new List<EquipmentDto>();
                var categories = new List<ComponentCategoryDto>();

                // 类型为：构件分类
                if (input.Type == GetListByScopeType.ComponentCategory)
                {
                    // 异步加载
                    if (input.ParentId != null)
                    {
                        // 查询当前节点的父节点分类
                        categories = categoriesScopeDtoList.Where(x => x.ParentId == input.ParentId).ToList();

                        // 查询设备
                        if (input.ParentId != null)
                        {
                            equipments = equipmentsScopeDtoList
                               .Where(x => x.ComponentCategoryId == input.ParentId && x.ParentId == null)
                               .ToList();

                            foreach (var e in equipments)
                            {
                                var dto = new GetListByScopeOutput()
                                {
                                    Id = e.Id,
                                    Name = e.Name,
                                    Type = GetListByScopeType.Equipment,
                                    ComponentCategoryId = e.ComponentCategoryId,
                                    ParentId = input.ParentId, // 此时为构件分类ID
                                    EquipmentGroupName = e.Group.Name,
                                    Children = e.Children != null && e.Children.Count > 0 ? new List<GetListByScopeOutput>() : null
                                };
                                dtos.Add(dto);
                            }
                        }
                    }

                    // 顶级加载
                    else
                    {
                        var parents = GuidKeyTreeHelper<ComponentCategoryDto>.GetTree(categoriesScopeDtoList);
                        categories = categoriesAllDtos.Where(x => parents.Exists(y => y.Code.StartsWith(x.Code))).ToList();
                        categories = categories.Where(x => x.Parent == null).ToList();
                    }

                    // 加入结果
                    foreach (var c in categories)
                    {
                        var dto = new GetListByScopeOutput()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Type = GetListByScopeType.ComponentCategory,
                            ParentId = c.ParentId,
                            Children = new List<GetListByScopeOutput>() // 分类永远都有子集（至少有设备）
                        };
                        dtos.Add(dto);
                    }

                }
                // 设备
                else if (input.Type == GetListByScopeType.Equipment)
                {
                    equipments = equipmentsScopeDtoList
                        .Where(x => x.ParentId == input.ParentId)
                        .ToList();

                    foreach (var e in equipments)
                    {
                        var dto = new GetListByScopeOutput()
                        {
                            Id = e.Id,
                            Name = e.Name,
                            Type = GetListByScopeType.Equipment,
                            ComponentCategoryId = e.ComponentCategoryId,
                            ParentId = input.ParentId,// 此时为构件分类ID
                            EquipmentGroupName = e.Group.Name,
                            Children = e.Children != null && e.Children.Count > 0 ? new List<GetListByScopeOutput>() : null
                        };
                        dtos.Add(dto);
                    }
                }
            }

            // 排序
            dtos = dtos.OrderBy(x => x.Type).ThenBy(x => x.Name).ToList();
            return dtos;
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task<List<EquipmentDto>> GetEquipmentsByScope(Scope scope)
        {
            return await _cacheManagerScopeEquipmentDtos.GetOrAddAsync(
                scope.Id.ToString(),
               async () =>
                {
                    return await Task.Run(() =>
                    {
                        // 1.查询当前范围内的所有设备，为了进一步查找该范围内的设备分类
                        var equipments = _repositoryEquipment
                            .WithDetails(
                                x => x.ComponentCategory,
                                x => x.InstallationSite.Organization,
                                x => x.Children,
                                x => x.Group)
                            .WhereIf(scope.Type == ScopeType.Organization, x => x.InstallationSite.OrganizationId == scope.Id)
                            .WhereIf(scope.Type == ScopeType.Railway, x => x.InstallationSite.RailwayId == scope.Id)
                            .WhereIf(scope.Type == ScopeType.Station, x => x.InstallationSite.StationId == scope.Id)
                            .WhereIf(scope.Type == ScopeType.InstallationSite, x => x.InstallationSiteId == scope.Id)
                            .OrderBy(x => x.Name)
                            .ToList();

                        equipments.ForEach(item =>
                        {
                            item.CreatorId = null;
                            item.InstallationSite = null;
                            item.InstallationSiteId = null;
                            item.StoreEquipmentId = null;
                        });

                        var dtos = ObjectMapper.Map<List<Equipment>, List<EquipmentDto>>(equipments);
                        return dtos;
                    });
                },
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
                }
            );
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task<List<ComponentCategoryDto>> GetAllComponentCategories()
        {
            return await _cacheManagerComponentCategories.GetOrAddAsync(
                "all-component-catogries",
                async () =>
                {
                    return await Task.Run(() =>
                    {
                        var categories = _componentCategoryRepository.OrderBy(x => x.Code).ToList();
                        var dtos = ObjectMapper.Map<List<ComponentCategory>, List<ComponentCategoryDto>>(categories);
                        return dtos;
                    });
                },
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
                }
            );
        }


        /// <summary>
        /// 获取一个范围的设备分组列表
        /// </summary>
        /// <param name="scopeCode"></param>
        /// <returns></returns>
        public Task<List<string>> GetEquipmentGroupsByScopeCode(string scopeCode)
        {
            var scope = new Scope(scopeCode);

            // 当前范围内的所有设备
            var groups = _repositoryEquipment
                    .WithDetails(
                        x => x.Group)
                    .WhereIf(scope.Type == ScopeType.Organization, x => x.InstallationSite.OrganizationId == scope.Id)
                    .WhereIf(scope.Type == ScopeType.Railway, x => x.InstallationSite.RailwayId == scope.Id)
                    .WhereIf(scope.Type == ScopeType.Station, x => x.InstallationSite.StationId == scope.Id)
                    .WhereIf(scope.Type == ScopeType.InstallationSite, x => x.InstallationSiteId == scope.Id)
                    .Select(x => x.Group.Name)
                    .Distinct()
                    .ToList();
            return Task.FromResult(groups);
        }


        /// <summary>
        /// 获取一组设备所属范围
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<List<GetScopesByGroupAndNameOut>> GetScopesByGroupAndName(GetScopesByGroupAndNameInput input)
        {
            var rst = new List<GetScopesByGroupAndNameOut>();

            if (input.EquipmentGroupAndNames != null)
            {

                var groups = input.EquipmentGroupAndNames.Select(x => x.Split('@').First());
                var names = input.EquipmentGroupAndNames.Select(x => x.Split('@').Last());

                var equipments = _repositoryEquipment
                    .WithDetails(
                        x => x.Organization,
                        x => x.InstallationSite.Railway,
                        x => x.InstallationSite.Station,
                        x => x.Group
                    )
                    .Where(x => groups.Contains(x.Group.Name))
                    .Where(x => names.Contains(x.Name))
                    .ToList();


                foreach (var item in equipments)
                {
                    var dto = new GetScopesByGroupAndNameOut
                    {
                        EquipmentId = item.Id,
                        EquipmentGroupName = item.Group.Name,
                        EquipmentName = item.Name,
                    };

                    if (item.Organization != null)
                    {
                        dto.ScopeCode += $"{(int)ScopeType.Organization}@{item.Organization.Name}@{item.Organization.Id}";

                        if (item.InstallationSite != null)
                        {
                            if (item.InstallationSite.Railway != null)
                            {
                                dto.ScopeCode += $".{(int)ScopeType.Railway}@{item.InstallationSite.Railway.Name}@{item.InstallationSite.Railway.Id}";

                                if (item.InstallationSite.Station != null)
                                {
                                    dto.ScopeCode += $".{(int)ScopeType.Station}@{item.InstallationSite.Station.Name}@{item.InstallationSite.Station.Id}";

                                    if (item.InstallationSite != null)
                                    {
                                        dto.ScopeCode += $".{(int)ScopeType.InstallationSite}@{item.InstallationSite.Name}@{item.InstallationSite.Id}";
                                    }
                                }
                            }
                            else
                            {
                                dto.ScopeCode += $".{(int)ScopeType.InstallationSite}@{item.InstallationSite.Name}@{item.InstallationSite.Id}";
                            }
                        }
                    }

                    rst.Add(dto);
                }
            }

            return Task.FromResult(rst.Where(x => !string.IsNullOrEmpty(x.ScopeCode)).ToList());
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.Equipment.Create)]
        public async Task<bool> Create(EquipmentCreateDto input)
        {
            if (input.EquipmentRltOrganizations == null || input.EquipmentRltOrganizations.Count == 0) throw new UserFriendlyException("维护班组不能为空");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("设备编号不能为空");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("设备名称不能为空");
            if (input.InstallationSiteId == null || input.InstallationSiteId == Guid.Empty) throw new UserFriendlyException("设备安装地点不能为空");
            if (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty) throw new UserFriendlyException("设备分类不能为空");
            if (input.ProductCategoryId == null || input.ProductCategoryId == Guid.Empty) throw new UserFriendlyException("设备型号不能为空");
            if (input.ManufacturerId == null || input.ManufacturerId == Guid.Empty) throw new UserFriendlyException("设备厂家不能为空");
            await CheckSameName(input.ParentId, null, input.Name);
            await CheckSameCode(null, input.Code);

            var ent = new Equipment(_guidGenerator.Create())
            {
                ParentId = input.ParentId,
                ComponentCategoryId = input.ComponentCategoryId,
                Code = input.Code,
                Name = input.Name,
                State = input.State,
                ManufacturerId = input.ManufacturerId,
                ProductCategoryId = input.ProductCategoryId,
                InstallationSiteId = input.InstallationSiteId,
                CheckState = EquipmentCheckState.UnCheck
            };
            ent.EquipmentRltOrganizations = new List<EquipmentRltOrganization>();
            // 重新保存维护单位信息
            foreach (var org in input.EquipmentRltOrganizations)
            {
                ent.EquipmentRltOrganizations.Add(new EquipmentRltOrganization(_guidGenerator.Create())
                {
                    EquipmentId = ent.Id,
                    OrganizationId = org.OrganizationId,
                });
            }

            await _repositoryEquipment.InsertAsync(ent);


            return true;
        }


        /// <summary>
        /// 添加关联文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CreateFile(EquipmentRltFileCreateDto input)
        {
            var equipment = _repositoryEquipment.FirstOrDefault(x => x.Id == input.EquipmentId);

            if (equipment == null) throw new UserFriendlyException("该设备实体不存在");

            if (input.FileIds.Count > 0)
            {
                foreach (var fileId in input.FileIds)
                {
                    var existFile = _equipmentRltFile.FirstOrDefault(x => x.EquipmentId == input.EquipmentId && x.FileId == fileId);
                    if (existFile != null) continue;
                    var equipmentRltFile = new EquipmentRltFile(_guidGenerator.Create())
                    {
                        EquipmentId = input.EquipmentId,
                        FileId = fileId,
                    };
                    await _equipmentRltFile.InsertAsync(equipmentRltFile);
                }
            }
            //var existFile = _equipmentRltFile.FirstOrDefault(x => x.EquipmentId == input.EquipmentId && x.FileId == input.FileId);

            //if (existFile != null) throw new UserFriendlyException("该设备已关联此文件");

            //var equipmentRltFile = new EquipmentRltFile(_guidGenerator.Create())
            //{
            //    EquipmentId = input.EquipmentId,
            //    FileId = input.FileId,
            //};
            //await _equipmentRltFile.InsertAsync(equipmentRltFile);
            return true;
        }

        [UnitOfWork]
        /// <summary>
        /// 生成工程量
        /// 
        /// 计算方式：根据改设备的产品分类单位（unit）及改设备的扩展属性进行计算，
        /// 如果单位为“米”、“m”，将属性名称为“长度”、“length”的属性值设为该值。
        /// 
        /// 如：["米","m","M"]->["长度","length","Length"]
        /// 如：["立方米","m3","M3"]->["体积","volume","Volume"]
        /// 如：["平方米","m2","M2"]->["体积","area","Area"]
        /// 
        /// </summary>
        public async Task GenerateQuantity(string taskKey)
        {
            var task = await _commonBackgroundTaskAppService.Get(taskKey);
            if (task == null)
            {
                task = await _commonBackgroundTaskAppService.Create(new BackgroundTaskDto() { Key = taskKey });

                var index = 0;
                var equipmentIds = _repositoryEquipment.Select(x => x.Id).ToList();
                foreach (var id in equipmentIds)
                {
                    using var uow = _unitOfWork.Begin(true, false);
                    index++;
                    var e = _repositoryEquipment
                         .WithDetails(x => x.ProductCategory, x => x.ComponentCategory, x => x.EquipmentProperties)
                         .Where(x => x.Id == id)
                         .FirstOrDefault();

                    var properties = e.EquipmentProperties;
                    var unit = e.ProductCategory?.Unit;
                    decimal quantity = 1;

                    var pairs = new List<List<List<string>>>()
                            {
                                 new List<List<string>>{ new List<string>() { "米", "m", "M" }, new List<string>() { "长度", "length", "Length" }},
                                 new List<List<string>>{ new List<string>() { "平方米", "m2", "M2" }, new List<string>() { "面积", "area", "Area" }},
                                 new List<List<string>>{ new List<string>() { "立方米", "m3", "M3" }, new List<string>() { "体积", "volume", "Volume" }},
                            };

                    var pair = pairs.Where(x => x.FirstOrDefault().Contains(unit)).FirstOrDefault();
                    if (pair != null)
                    {
                        var names = pair.LastOrDefault();
                        foreach (var name in names)
                        {

                            var property = properties.Find(x => x.Name == name);
                            if (property != null && decimal.TryParse(property.Value, out quantity))
                            {
                                break;
                            }
                        }
                    }

                    e.Quantity = quantity;
                    await _repositoryEquipment.UpdateAsync(e);
                    await uow.SaveChangesAsync();

                    if (index == equipmentIds.Count())
                    {
                        await _commonBackgroundTaskAppService.Done(taskKey);
                    }

                    await _commonBackgroundTaskAppService.Update(new BackgroundTaskDto
                    {
                        Key = taskKey,
                        Count = equipmentIds.Count(),
                        Index = index,
                        Message = $"正在生成：{e.Name}"
                    });
                }

                await _commonBackgroundTaskAppService.Done(taskKey);
            }
        }


        /// <summary>
        /// 04-02 更改，将标准库接口移至此
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<EquipmentSimpleDto>> GetEquipment(List<Guid> ids)
        {
            var componentCategoryIds = _repositoryProjectItemRltComponentCategory.Where(x => ids.Contains(x.ProjectItemId)).Select(y => y.ComponentCategoryId).ToList();
            var productCategoryIds = _repositoryProjectItemRltProductCategory.Where(x => ids.Contains(x.ProjectItemId)).Select(y => y.ProductCategoryId).ToList();

            var noProductCategoryId = _repositoryEquipment.Where(x => x.ProductCategoryId == null || x.ProductCategoryId == Guid.Empty); //先找到产品Id为空的
            List<Equipment> noProductCategories = new List<Equipment>();
            if (noProductCategoryId != null)
            {
                noProductCategories = noProductCategoryId.Where(x => componentCategoryIds.Contains((Guid)x.ComponentCategoryId)).ToList();
            }
            var ProductCategories = _repositoryEquipment.Where(x => productCategoryIds.Contains((Guid)x.ProductCategoryId)).ToList();

            return ObjectMapper.Map<List<Equipment>, List<EquipmentSimpleDto>>(ProductCategories.Concat(noProductCategories).ToList());
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.Equipment.Update)]
        public async Task<bool> Update(EquipmentUpdateDto input)
        {
            if (input.EquipmentRltOrganizations == null || input.EquipmentRltOrganizations.Count == 0) throw new UserFriendlyException("维护班组不能为空");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("设备编号不能为空");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("设备名称不能为空");
            if (input.InstallationSiteId == null || input.InstallationSiteId == Guid.Empty) throw new UserFriendlyException("安装地点不能为空");
            if (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty) throw new UserFriendlyException("设备分类不能为空");
            if (input.ProductCategoryId == null || input.ProductCategoryId == Guid.Empty) throw new UserFriendlyException("设备型号不能为空");
            if (input.ManufacturerId == null || input.ManufacturerId == Guid.Empty) throw new UserFriendlyException("设备型号不能为空");

            var oldEnt = _repositoryEquipment.FirstOrDefault(s => s.Id == input.Id);
            if (oldEnt == null) throw new UserFriendlyException("当前更新设备实体不存在");

            await CheckSameName(input.ParentId, input.Id, input.Name);
            await CheckSameCode(input.Id, input.Code);

            // 清楚之前关联组织机构信息
            await equipmentOrgRepository.DeleteAsync(x => x.EquipmentId == oldEnt.Id);

            // 重新保存关联组织机构信息信息
            oldEnt.EquipmentRltOrganizations = new List<EquipmentRltOrganization>();
            foreach (var org in input.EquipmentRltOrganizations)
            {
                oldEnt.EquipmentRltOrganizations.Add(new EquipmentRltOrganization(_guidGenerator.Create())
                {
                    EquipmentId = oldEnt.Id,
                    OrganizationId = org.OrganizationId
                });
            }

            oldEnt.ParentId = input.ParentId;
            oldEnt.ComponentCategoryId = input.ComponentCategoryId;
            oldEnt.Code = input.Code;
            oldEnt.Name = input.Name;
            oldEnt.State = input.State;
            oldEnt.ManufacturerId = input.ManufacturerId;
            oldEnt.ProductCategoryId = input.ProductCategoryId;
            oldEnt.InstallationSiteId = input.InstallationSiteId;

            await _repositoryEquipment.UpdateAsync(oldEnt);

            return true;
        }


        /// <summary>
        /// 更新 Gis 数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateGisData(UpdateGisDataInput input)
        {
            var equipment = _repositoryEquipment.FirstOrDefault(s => s.Id == input.Id);
            if (equipment == null)
            {
                throw new UserFriendlyException("设备不存在");
            }

            equipment.GisData = input.GisData;
            await CurrentUnitOfWork.SaveChangesAsync();

            return true;
        }


        /// <summary>
        /// 导入
        /// </summary>
        [Authorize(ResourcePermissions.Equipment.Import)]
        [UnitOfWork]
        public async Task<string> Upload([FromForm] ImportData input)
        {
            //虚拟进度
            await _fileImport.Start(input.ImportKey, 100);
            await _fileImport.UpdateState(input.ImportKey, 1);  //将进度调为1
            var failMsg = new StringBuilder();
            IWorkbook workbook = null;
            ISheet sheet = null;
            DataSet ds = null;

            var fileName = input.File.File.FileName;
            if (!(fileName.Contains('[') && fileName.Contains(']')))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("文件名称中需包含'[]'");
            }

            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0);
                ds = ExcelHelper.ImportBaseDataToDataSet(input.File.File.OpenReadStream(), input.File.File.FileName);
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            if (ds == null || ds.Tables.Count == 0)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("未找到任何数据,请检查文件格式");
            }

            var treeList = new List<TreeNode>();
            var dataList = new List<PartImport>();
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            await Task.Run(async () =>
            {
                #region 读取并验证数据
                foreach (DataTable dt in ds.Tables)
                {
                    #region 验证列是否存在
                    //验证列是否存在
                    if (!dt.Columns.Contains(PartImportCol.SeenSun.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.SeenSun.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.SeenSun.ToString()));
                    }
                    if (!dt.Columns.Contains(PartImportCol.CSRGCode.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.CSRGCode.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.CSRGCode.ToString()));
                    }
                    if (!dt.Columns.Contains(PartImportCol.Organization.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.Organization.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.Organization.ToString()));
                    }
                    if (!dt.Columns.Contains(PartImportCol.EquipmentSubclass.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.EquipmentSubclass.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.EquipmentSubclass.ToString()));
                    }
                    if (!dt.Columns.Contains(PartImportCol.ElementType.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.ElementType.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.ElementType.ToString()));
                    }
                    if (!dt.Columns.Contains(PartImportCol.SectionIFD.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.SectionIFD.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.SectionIFD.ToString()));
                    }
                    if (!dt.Columns.Contains(PartImportCol.Name.ToString()))
                    {
                        failMsg.AppendLine($"sheet={dt.TableName}:列{PartImportCol.Name}不存在");
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.Name.ToString()));
                    }
                    //if (!dt.Columns.Contains(PartImportCol.SystemName.ToString())) throw new Exception("列" + PartImportCol.SystemName.ToString() + "不存在");
                    /*if (!dt.Columns.Contains(PartImportCol.InstallationSiteCode.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.InstallationSiteCode.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                    }*/
                    if (!dt.Columns.Contains(PartImportCol.MaintenanceOrganizationCode.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.MaintenanceOrganizationCode.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.MaintenanceOrganizationCode.ToString()));
                    }
                    if (!dt.Columns.Contains(PartImportCol.Manufacturer.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.Manufacturer.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.Manufacturer.ToString()));
                    }
                    /*if (!dt.Columns.Contains(PartImportCol.ProductCategory.ToString()))  设备型号列不存在也能导。
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.ProductCategory.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                    }*/
                    if (!dt.Columns.Contains(PartImportCol.UseDate.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.UseDate.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.UseDate.ToString()));
                    }
                    if (!dt.Columns.Contains(PartImportCol.State.ToString()))
                    {
                        failMsg.AppendLine(string.Format("sheet={0}:列{1}不存在", dt.TableName, PartImportCol.State.ToString()));
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException(string.Format("列{0}不存在", PartImportCol.State.ToString()));
                    }
                    #endregion

                    #region 验证并获取数据
                    var dataModel = (PartImport)null;
                    var childItem = (PartItem)null;
                    var colVal = (string)null;
                    var rowNmb = (int)5;
                    foreach (DataRow item in dt.Rows)
                    {
                        rowNmb++;
                        //序号
                        colVal = Convert.ToString(item[PartImportCol.SeenSun.ToString()]);
                        dataModel = new PartImport()
                        {
                            SeenSun = colVal,
                            SheetName = dt.TableName,
                        };

                        //设备编码
                        if (dt.Columns.Contains(PartImportCol.CSRGCode.ToString()))
                        {
                            colVal = Convert.ToString(item[PartImportCol.CSRGCode.ToString()]);
                            if (string.IsNullOrEmpty(colVal))
                            {
                                failMsg.AppendLine(FormartErrMsg(dataModel.SheetName, dataModel.SeenSun, colVal, "编码为空"));
                                WrongInfo wrong = new WrongInfo(rowNmb - 1, "编码为空");
                                wrongInfos.Add(wrong);
                                continue;
                            }
                            dataModel.CSRGCode = colVal;
                        }


                        //系统名称
                        if (dt.Columns.Contains(PartImportCol.SystemName.ToString()))
                        {
                            dataModel.SystemName = Convert.ToString(item[PartImportCol.SystemName.ToString()]);
                        }

                        //班组
                        colVal = Convert.ToString(item[PartImportCol.Organization.ToString()]);
                        if (string.IsNullOrEmpty(colVal))
                        {
                            failMsg.AppendLine(FormartErrMsg(dataModel.SheetName, dataModel.SeenSun, dataModel.CSRGCode, "班组为空"));
                            WrongInfo wrong = new WrongInfo(rowNmb - 1, "班组为空");
                            wrongInfos.Add(wrong);
                            continue;
                        }
                        dataModel.Organization = colVal;

                        //设备子类
                        colVal = Convert.ToString(item[PartImportCol.EquipmentSubclass.ToString()]);
                        dataModel.EquipmentSubclass = colVal;

                        //单元类型
                        colVal = Convert.ToString(item[PartImportCol.ElementType.ToString()]);
                        dataModel.ElementType = colVal;

                        //组合分类
                        colVal = Convert.ToString(item[PartImportCol.Group.ToString()]);
                        if (string.IsNullOrEmpty(colVal))
                        {
                            failMsg.AppendLine(FormartErrMsg(dataModel.SheetName, dataModel.SeenSun, dataModel.CSRGCode, "组合分类为空"));
                            WrongInfo wrong = new WrongInfo(rowNmb - 1, "组合分类为空");
                            wrongInfos.Add(wrong);
                            continue;
                        }
                        dataModel.Group = colVal;

                        //节
                        colVal = Convert.ToString(item[PartImportCol.SectionIFD.ToString()]);
                        if (string.IsNullOrEmpty(colVal))
                        {
                            failMsg.AppendLine(FormartErrMsg(dataModel.SheetName, dataModel.SeenSun, dataModel.CSRGCode, "节为空"));
                            WrongInfo wrong = new WrongInfo(rowNmb - 1, "节为空");
                            wrongInfos.Add(wrong);
                            continue;
                        }
                        dataModel.SectionIFD = colVal;

                        //设备名称
                        colVal = Convert.ToString(item[PartImportCol.Name.ToString()]);
                        if (string.IsNullOrEmpty(colVal))
                        {
                            failMsg.AppendLine(FormartErrMsg(dataModel.SheetName, dataModel.SeenSun, dataModel.CSRGCode, "设备名称为空"));
                            WrongInfo wrong = new WrongInfo(rowNmb - 1, "设备名称为空");
                            wrongInfos.Add(wrong);
                            continue;
                        }
                        dataModel.Name = colVal;

                        //车站名称
                        if (dt.Columns.Contains(PartImportCol.StaStationId.ToString()))
                        {
                            dataModel.StaStationId = Convert.ToString(item[PartImportCol.StaStationId.ToString()]);
                        }
                        //安装地点(车站）
                        if (dt.Columns.Contains(PartImportCol.StaMachineRoomId.ToString()))
                        {
                            dataModel.StaMachineRoomId = Convert.ToString(item[PartImportCol.StaMachineRoomId.ToString()]);
                        }
                        //所属线路(车站）
                        if (dt.Columns.Contains(PartImportCol.StaRailwayId.ToString()))
                        {
                            dataModel.StaRailwayId = Convert.ToString(item[PartImportCol.StaRailwayId.ToString()]);
                        }

                        //所属线路(区间）
                        if (dt.Columns.Contains(PartImportCol.ZoneRailwayId.ToString()))
                        {
                            dataModel.ZoneRailwayId = Convert.ToString(item[PartImportCol.ZoneRailwayId.ToString()]);
                        }
                        //公里标
                        if (dt.Columns.Contains(PartImportCol.ZoneKilometerMark.ToString()))
                        {
                            dataModel.ZoneKilometerMark = Convert.ToString(item[PartImportCol.ZoneKilometerMark.ToString()]);
                        }
                        //安装地点(区间)
                        if (dt.Columns.Contains(PartImportCol.ZoneMachineRoomId.ToString()))
                        {
                            dataModel.ZoneMachineRoomId = Convert.ToString(item[PartImportCol.ZoneMachineRoomId.ToString()]);
                        }

                        //所属线路(其他）
                        if (dt.Columns.Contains(PartImportCol.OthRailwayId.ToString()))
                        {
                            dataModel.OthRailwayId = Convert.ToString(item[PartImportCol.OthRailwayId.ToString()]);
                        }
                        //安装地点(其他)
                        if (dt.Columns.Contains(PartImportCol.OthMachineRoomId.ToString()))
                        {
                            dataModel.OthMachineRoomId = Convert.ToString(item[PartImportCol.OthMachineRoomId.ToString()]);
                        }
                        //单位
                        if (dt.Columns.Contains(PartImportCol.Unit.ToString()))
                        {
                            dataModel.Unit = Convert.ToString(item[PartImportCol.Unit.ToString()]);
                        }

                        //机房编码(起点)(需求变更2020.11.23)
                        if (dt.Columns.Contains(PartImportCol.InstallationSiteCode.ToString()))
                        {
                            dataModel.InstallationSiteCode =
                                Convert.ToString(item[PartImportCol.InstallationSiteCode.ToString()]);
                        }
                        //判断是否需要导入设备安装止点
                        if (dt.Columns.Contains(PartImportCol.EndInstallationSiteCode.ToString()))
                        {
                            //机房编码(止点)
                            dataModel.EndInstallationSiteCode =
                                Convert.ToString(item[PartImportCol.EndInstallationSiteCode.ToString()]);
                        }

                        //维护单位编码
                        colVal = Convert.ToString(item[PartImportCol.MaintenanceOrganizationCode.ToString()]);
                        if (string.IsNullOrEmpty(colVal))
                        {
                            failMsg.AppendLine(FormartErrMsg(dataModel.SheetName, dataModel.SeenSun, dataModel.CSRGCode, "维护单位编码为空"));
                            WrongInfo wrong = new WrongInfo(rowNmb - 1, "维护单位编码为空");
                            wrongInfos.Add(wrong);
                            continue;
                        }
                        dataModel.EquipmentRltOrganizations = colVal;

                        //设备厂家
                        colVal = Convert.ToString(item[PartImportCol.Manufacturer.ToString()]);
                        if (string.IsNullOrEmpty(colVal))
                        {
                            failMsg.AppendLine(FormartErrMsg(dataModel.SheetName, dataModel.SeenSun, dataModel.CSRGCode, "设备厂家为空"));
                            WrongInfo wrong = new WrongInfo(rowNmb - 1, "设备厂家为空");
                            wrongInfos.Add(wrong);
                            continue;
                        }
                        dataModel.Manufacturer = colVal;

                        //设备型号,为空时也能添加
                        if (dt.Columns.Contains(PartImportCol.ProductCategory.ToString()))
                        {
                            colVal = Convert.ToString(item[PartImportCol.ProductCategory.ToString()]);
                            dataModel.StandardEquipment = colVal;
                        }

                        /*if (string.IsNullOrEmpty(colVal))
                        {
                            failMsg.AppendLine(FormartErrMsg(dataModel.SheetName, dataModel.SeenSun, dataModel.CSRGCode, "设备型号为空"));
                            WrongInfo wrong = new WrongInfo(rowNmb - 1, "设备型号为空");
                            wrongInfos.Add(wrong);
                            continue;
                        }*/


                        //使用日期
                        colVal = Convert.ToString(item[PartImportCol.UseDate.ToString()]);
                        if (string.IsNullOrEmpty(colVal))
                        {
                            WrongInfo wrong = new WrongInfo(rowNmb - 1, "使用日期为空");
                            wrongInfos.Add(wrong);
                            continue;
                        }
                        DateTime useDate;
                        if (DateTime.TryParse(colVal, out useDate))
                        {
                            dataModel.UseDate = useDate;
                        }

                        //设备运行状态
                        //var runningStateEum = SnAbp.Resource.Enums.EquipmentState.OnService;
                        colVal = Convert.ToString(item[PartImportCol.State.ToString()]);
                        //if (!Enum.TryParse(colVal, out runningStateEum))
                        if (!SetRunningStates(dataModel, colVal))
                        {
                            failMsg.AppendLine(FormartErrMsg(dataModel.SheetName, dataModel.SeenSun, dataModel.CSRGCode, "设备运行状态不存在"));
                            WrongInfo wrong = new WrongInfo(rowNmb - 1, "设备运行状态不存在");
                            wrongInfos.Add(wrong);
                            continue;
                        }
                        //dataModel.RunningState = runningStateEum;

                        dataModel.Items = new List<PartItem>();
                        var names = dataModel.Group.Split('_');
                        for (var i = 0; i < names.Length; i++)
                        {
                            childItem = new PartItem();
                            childItem.Name = names[i];
                            childItem.ParentName = "";
                            foreach (var ele in dataModel.Items)
                            {
                                childItem.ParentName = childItem.ParentName + (ele.Name + "✪");
                            }
                            childItem.ParentName = childItem.ParentName.TrimEnd('✪');
                            dataModel.Items.Add(childItem);
                        }

                        //产品名称
                        colVal = dataModel.SectionIFD;
                        childItem = new PartItem()
                        {
                            Name = colVal,
                            ParentName = "",
                        };
                        foreach (var ele in dataModel.Items)
                        {
                            childItem.ParentName = childItem.ParentName + (ele.Name + "✪");
                        }
                        childItem.ParentName = childItem.ParentName.TrimEnd('✪');
                        dataModel.Items.Add(childItem);
                        dataList.Add(dataModel);
                    }
                    #endregion
                }
                #endregion

                #region 构件构件数据结构树
                var thisNode = (TreeNode)null;
                var parentNode = (TreeNode)null;
                foreach (var item in dataList)
                {
                    var index = (int)0;
                    foreach (var ele in item.Items)
                    {
                        index++;
                        parentNode = GetNodesParentNode(treeList, ele);
                        if (parentNode != null && string.IsNullOrEmpty(ele.ParentName)) continue;
                        if (parentNode == null)
                        {
                            thisNode = new TreeNode()
                            {
                                Id = Guid.NewGuid(),
                                ParentId = Guid.Empty,
                                Name = ele.Name,
                                ParentName = ele.ParentName,
                                Level = 1,
                                Child = new List<TreeNode>(),
                            };

                            treeList.Add(thisNode);
                        }
                        else
                        {
                            if (parentNode.Child.All(z => z.Name != ele.Name))
                            {
                                thisNode = new TreeNode()
                                {
                                    Id = Guid.NewGuid(),
                                    ParentId = parentNode.Id,
                                    Name = ele.Name,
                                    ParentName = ele.ParentName,
                                    Level = parentNode.Level + 1,
                                    Child = new List<TreeNode>(),
                                };

                                parentNode.Child.Add(thisNode);
                            }
                        }
                        if (index == item.Items.Count)
                        {
                            item.CrmId = parentNode.Child.LastOrDefault().Id;
                        }
                    }
                }

                //一级
                var newTree = new List<TreeNode>();
                var parentId = Guid.NewGuid();
                thisNode = new TreeNode();
                thisNode.Id = parentId;
                thisNode.ParentId = Guid.Empty;
                thisNode.Name = "通信专业";
                thisNode.Level = 1;
                thisNode.Child = new List<TreeNode>();
                newTree.Add(thisNode);
                //二级为文件名
                fileName = fileName.Replace(']', '[').Split('[')[1];
                thisNode = new TreeNode();
                thisNode.Id = Guid.NewGuid();
                thisNode.ParentId = parentId;
                thisNode.Name = fileName;
                thisNode.ParentName = "通信专业";
                thisNode.Level = 2;
                thisNode.Child = new List<TreeNode>();
                newTree[0].Child.Add(thisNode);

                foreach (var item in treeList)
                {
                    item.ParentId = thisNode.Id;
                    item.ParentName = thisNode.ParentName + "✪" + thisNode.Name;
                }
                newTree[0].Child[0].Child = treeList;
                #endregion


                await _fileImport.ChangeTotalCount(input.ImportKey, dataList.Count + 1);
                await _fileImport.UpdateState(input.ImportKey, ((dataList.Count + 1) / 99));  //将进度调为1

                #region 开始往标准库写入构件数据
                using var unoww = _unitOfWork.Begin(true, false);
                var crmCodeList = InsertToStandComponentCategory(newTree);//写入标准库后返回的ID和标准库code
                await unoww.SaveChangesAsync();
                #endregion

                #region 开始往业务库写入设备数据
                var crmId = (Guid?)null;//构件编码
                var equi = (Equipment)null;
                var equiOrg = (EquipmentRltOrganization)null;
                var org = (OrganizationDto)null;
                var endInstallationSite = (InstallationSite)null;
                var allOrg = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(orgRepository.Where(z => z.Id != Guid.Empty).ToList());//获得所有组织机构列表
                var allInstallationSite = installationSitesRepository.Where(z => z.Id != Guid.Empty).ToList();//获得所有机房列表
                var allEquipment = _repositoryEquipment.Where(z => z.Id != Guid.Empty).ToList();//获得所有设备
                var allEquipmentOrg = equipmentOrgRepository.Where(z => z.Id != Guid.Empty).ToList();
                var isUpdate = true;//执行新增或更新标志
                var codeList = new List<string>();

                foreach (var item in dataList)
                {
                    if (codeList.Any(z => z == item.CSRGCode))
                    {
                        WrongInfo wrong = new WrongInfo(int.Parse(item.SeenSun) + 4, "设备信息已存在");
                        wrongInfos.Add(wrong);
                        continue;
                    }
                    codeList.Add(item.CSRGCode);
                    isUpdate = true;
                    equi = allEquipment.FirstOrDefault(z => z.CSRGCode == item.CSRGCode);
                    var standardEquipmentId =
                    await GetStandardEquipmentIdAsync(item.Manufacturer, item.StandardEquipment, item.SectionIFD, item.ElementType, item.EquipmentSubclass, input.ImportKey); //根据厂家、设备型号、单元类型、设备子类找到ProductCategory

                    //块执行

                    //设备类型(标准库的构件) 
                    crmId = crmCodeList[item.CrmId];//构件编码

                    //设备新增审查状态（新旧设备都将审查状态修改为未审查）
                    equi.CheckState = EquipmentCheckState.UnCheck;

                    if (!crmId.HasValue)
                    {
                        failMsg.AppendLine(FormartErrMsg(item.SheetName, item.SeenSun, item.CSRGCode, "构件编码不存在"));
                        WrongInfo wrong = new WrongInfo(int.Parse(item.SeenSun) + 4, "构件编码不存在");
                        wrongInfos.Add(wrong);
                        continue;
                    }
                    using var unow = _unitOfWork.Begin(true, false);
                    var storeEquipment = new StoreEquipment(_guidGenerator.Create());

                    if (equi == null)
                    {
                        isUpdate = false;

                        //库存id
                        equi = new Equipment(_guidGenerator.Create());
                        if (standardEquipmentId != null)
                        {
                            //新增库存
                            storeEquipment.Code = item.CSRGCode;
                            storeEquipment.Name = item.Name;
                            storeEquipment.ComponentCategoryId = crmId.GetValueOrDefault();
                            storeEquipment.ManufacturerId = await GetManufacturerIdAsync(item.Manufacturer, input.ImportKey);
                            storeEquipment.State = (StoreEquipmentState)item.State;
                            storeEquipment.ProductCategoryId = standardEquipmentId.GetValueOrDefault();
                            await _storeEquipmente.InsertAsync(storeEquipment);
                            await unow.SaveChangesAsync();

                            equi.StoreEquipmentId = storeEquipment.Id;
                        }
                    }

                    //设备名称
                    equi.Name = item.Name;
                    //设备编码
                    equi.CSRGCode = item.CSRGCode;
                    equi.Code = item.CSRGCode;
                    //设备型号(标准库的产品)
                    equi.ProductCategoryId = await GetStandardEquipmentIdAsync(item.Manufacturer, item.StandardEquipment, item.SectionIFD, item.ElementType, item.EquipmentSubclass, input.ImportKey);//查找设备关联标准库产品编码

                    //安装地点：机房(起点)
                    InstallationSite installationSite = null;
                    if (!string.IsNullOrEmpty(item.InstallationSiteCode) && null != item.InstallationSiteCode)
                    {
                        installationSite = installationSitesRepository.WithDetails(x => x.Railway).FirstOrDefault(z => z.CSRGCode == item.InstallationSiteCode); //查询机房是否存在(编码查询)
                    }
                    else//根据名称查询
                    {
                        installationSite = !string.IsNullOrEmpty(item.ZoneMachineRoomId) ? installationSitesRepository.FirstOrDefault(z => z.Name == item.ZoneMachineRoomId && z.Railway.Name == item.ZoneRailwayId) : installationSitesRepository.WithDetails(x => x.Railway).FirstOrDefault(z => z.Name == item.ZoneRailwayId + item.ZoneKilometerMark && z.Railway.Name == item.ZoneRailwayId);
                        if (!string.IsNullOrEmpty(item.StaMachineRoomId)) //站点
                        {
                            installationSite = installationSitesRepository.FirstOrDefault(z => z.Name == item.StaMachineRoomId && z.Railway.Name == item.StaRailwayId);
                        }
                        else if (!string.IsNullOrEmpty(item.OthMachineRoomId))
                        {
                            installationSite = installationSitesRepository.FirstOrDefault(z => z.Name == item.OthRailwayId + item.OthMachineRoomId && z.Railway.Name == item.OthRailwayId);
                        }
                    }
                    if (installationSite != null)
                    {
                        equi.InstallationSiteId = installationSite.Id;
                    }
                    else
                    {
                        Guid? stationId = null; //站点
                        Guid? railwayId = null; //线路
                        var installationSiteName = "";//安装位置
                        var locationType = InstallationSiteLocationType.StationInner; //位置类型
                        var mark = 0;//公里标
                        if (!string.IsNullOrEmpty(item.StaStationId) && null != item.StaStationId) //站点
                        {
                            var railway = _railways.FirstOrDefault(x => x.Name == item.StaRailwayId);//待优化，总结为私有方法
                            if (railway != null)
                            {
                                railwayId = railway.Id;
                            }
                            else
                            {
                                railwayId = null;
                            }
                            var station = _stations.FirstOrDefault(x => x.Name == item.StaStationId);
                            if (station != null)
                            {
                                stationId = station.Id;
                            }
                            else
                            {
                                stationId = null;
                            }
                            installationSiteName = item.StaMachineRoomId;
                        }
                        else if (!string.IsNullOrEmpty(item.ZoneKilometerMark) && null != item.ZoneKilometerMark) //区间
                        {
                            var railway = _railways.FirstOrDefault(x => x.Name == item.ZoneRailwayId);
                            if (railway != null)
                            {
                                locationType = InstallationSiteLocationType.SectionInner;
                                railwayId = railway.Id;
                            }
                            else
                            {
                                railwayId = null;
                            }
                            if (string.IsNullOrEmpty(item.ZoneMachineRoomId))
                            {
                                installationSiteName = item.ZoneRailwayId + item.ZoneKilometerMark;
                            }
                            else
                            {
                                installationSiteName = item.ZoneMachineRoomId;
                            }
                            mark = int.Parse(item.ZoneKilometerMark);
                        }
                        else if (!string.IsNullOrEmpty(item.OthRailwayId) && null != item.OthRailwayId)//其他
                        {
                            var railway = _railways.FirstOrDefault(x => x.Name == item.OthRailwayId);
                            if (railway != null)
                            {
                                locationType = InstallationSiteLocationType.Other;
                                railwayId = railway.Id;
                            }
                            else
                            {
                                railwayId = null;
                            }
                            installationSiteName = item.OthRailwayId + item.OthMachineRoomId;
                        }

                        var newInstallationSite = new InstallationSite(_guidGenerator.Create())
                        {
                            State = InstallationSiteState.Using,//默认状态为在用
                            Name = installationSiteName,
                            RailwayId = railwayId,
                            KMMark = mark,
                            LocationType = locationType,
                        };

                        if (!string.IsNullOrEmpty(item.InstallationSiteCode))
                        {
                            newInstallationSite.CSRGCode = item.InstallationSiteCode;
                        }
                        else
                        {
                            newInstallationSite.CSRGCode = "SC000" + _guidGenerator.Create();
                        }
                        if (Guid.Empty != stationId)
                        {
                            newInstallationSite.StationId = stationId;

                        }
                        await installationSitesRepository.InsertAsync(newInstallationSite);
                        await unow.SaveChangesAsync();

                        equi.InstallationSiteId = newInstallationSite.Id;
                    }




                    //安装地点（机房）(止点）
                    if (!string.IsNullOrEmpty(item.EndInstallationSiteCode))
                    {
                        endInstallationSite = allInstallationSite.FirstOrDefault(x => x.CSRGCode == item.EndInstallationSiteCode);
                        if (endInstallationSite == null)
                        {
                            failMsg.AppendLine(FormartErrMsg(item.SheetName, item.SeenSun, item.CSRGCode, "止点机房编码不存在,但已导入"));
                            WrongInfo wrong = new WrongInfo(int.Parse(item.SeenSun) + 4, "止点机房编码不存在,但已导入");
                            wrongInfos.Add(wrong);
                        }
                        equi.EndInstallationSiteId = endInstallationSite?.Id;
                    }


                    org = allOrg.FirstOrDefault(z => z.CSRGCode == item.EquipmentRltOrganizations);
                    if (org != null)
                    {
                        equi.OrganizationId = org.Id;
                    }
                    //}
                    //标准库构件分类
                    equi.ComponentCategoryId = crmId;
                    //设备厂家
                    equi.ManufacturerId = await GetManufacturerIdAsync(item.Manufacturer, input.ImportKey);//根据厂家名称获取厂家编码

                    //使用日期
                    var useDate = item.UseDate.ToString();
                    if (!string.IsNullOrEmpty(useDate))
                    {
                        if (!useDate.Contains('-'))
                        {
                            useDate = string.Join("-", useDate.Split(useDate.Substring(4, 1)));
                        }
                        equi.UseDate = Convert.ToDateTime(useDate);
                    }

                    //运行状态
                    equi.State = item.State;



                    if (isUpdate)//执行更新
                    {
                        await _repositoryEquipment.UpdateAsync(equi);
                        await unow.SaveChangesAsync();
                        //更新维护单位
                        var equiOrgList = allEquipmentOrg.Where(z => z.EquipmentId == equi.Id).ToList();
                        if (equiOrgList.Count() > 0)
                        {
                            equiOrg = equiOrgList.FirstOrDefault();
                            if (org != null)
                            {
                                if (equiOrg.OrganizationId != org.Id)
                                {
                                    equiOrg.OrganizationId = org.Id;
                                    await equipmentOrgRepository.UpdateAsync(equiOrg);
                                    await unow.SaveChangesAsync();
                                }
                                foreach (var ele in equiOrgList)
                                {
                                    if (ele.Id == equiOrg.Id) continue;
                                    await equipmentOrgRepository.DeleteAsync(z => z.Id == ele.Id);
                                    await unow.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            if (org != null)
                            {
                                //保存班组
                                equiOrg = new EquipmentRltOrganization(_guidGenerator.Create());
                                equiOrg.EquipmentId = equi.Id;
                                equiOrg.OrganizationId = org.Id;
                                await equipmentOrgRepository.InsertAsync(equiOrg);//保存维护单位
                                await unow.SaveChangesAsync();
                            }
                        }
                    }
                    else//更新新增
                    {
                        await _repositoryEquipment.InsertAsync(equi);
                        await unow.SaveChangesAsync();
                        if (org != null)
                        {
                            //保存班组
                            equiOrg = new EquipmentRltOrganization(_guidGenerator.Create())
                            {
                                EquipmentId = equi.Id,
                                OrganizationId = org.Id,
                            };
                            await equipmentOrgRepository.InsertAsync(equiOrg);//保存维护单位
                            await unow.SaveChangesAsync();
                        }
                    }

                    //添加设备服务表
                    if (!isUpdate && standardEquipmentId != null)
                    {
                        var serviceRecord = new EquipmentServiceRecord(_guidGenerator.Create())
                        {
                            StoreEquipmentId = storeEquipment.Id,
                            EquipmentId = equi.Id,
                        };
                        await _equipmentServiceProperty.InsertAsync(serviceRecord);
                        await unow.SaveChangesAsync();
                    }
                    if (dataList.IndexOf(item) >= dataList.Count * 0.02)  //当进度开始进行并为2时，再刷新进度
                    {
                        await _fileImport.UpdateState(input.ImportKey, dataList.IndexOf(item) + 1);
                    }
                }
                await _fileImport.Complete(input.ImportKey);
                if (wrongInfos.Count > 0)
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
                }
                #endregion
            });
            return failMsg.ToString();
        }


        /// <summary>
        /// 工程数据导入
        /// </summary>
        public async Task EngineeringDataImport([FromForm] DataImportDto input)
        {
            input.CheckNull();
            switch (input.Type)
            {
                case DataImportType.EngineEquipment:
                    await EngineeringEquipmentImport(input.File, input.ImportKey);
                    break;
                case DataImportType.EngineCable:
                    await EngineCableImport(input.File, input.ImportKey);
                    break;
                case DataImportType.CableWiring:
                    await CableWiringImport(input.File, input.ImportKey);
                    break;
                case DataImportType.CabinetWiring:
                    await CabinetWiringImport(input.File, input.ImportKey);
                    break;
                default:
                    throw new UserFriendlyException("源数据有误");
            }
        }


        /// <summary>
        /// 设备导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Stream> Export(EquipmentData input)
        {
            Stream stream = null;
            /*if (false)
            {
                switch (input.TemplateKey)
                {
                    case "engineeringEquipments": //工程设备
                        await EngineeringEquipmentExport(input);
                        break;
                    case "engineeringCable": //工程电缆
                        await EngineCableExport(input);
                        break;
                    case "cableWiring": //线缆配线
                        await CableWiringExport(input);
                        break;
                    case "cabinetWiring": //机柜配线
                        await CabinetWiringExport(input);
                        break;
                    default:
                        throw new UserFriendlyException("找不到导出对应模板");
                }
            }
            else
            {*/
            var list = GetEquipmentData(input.Paramter);
            var groupList = await crmRepository.GetListAsync();
            var productList = await productRepository.GetListAsync();
            //var installationList = installationSitesRepository.WithDetails().Where(y => y.Id != Guid.Empty).ToList();
            var manufacturerList = await manufacturerRepository.GetListAsync();
            var modelList = await modelRepository.GetListAsync();
            var orgList = await orgRepository.GetListAsync();

            var equipments = new List<EquipmentModel>();
            var equipment = new EquipmentModel();
            foreach (var item in list)
            {
                equipment = new EquipmentModel();
                if (item.OrganizationId != null && item.OrganizationId != Guid.Empty) //班组和维护单位编码
                {
                    var org = orgList.FirstOrDefault(x => x.Id == item.OrganizationId);
                    equipment.Organization = org?.Name;
                    equipment.MaintenanceOrganizationCode = org?.CSRGCode;
                }
                if (item.ProductCategoryId != null) //设备类型、单元类型、设备子类(ProductCategory)
                {
                    var pro = productList.FirstOrDefault(x => x.Id == item.ProductCategoryId);
                    equipment.Unit = pro?.Unit;
                    equipment.ProductCategory = pro?.Name;
                    if (pro.ParentId != null && pro.ParentId != Guid.Empty)
                    {
                        pro = productList.FirstOrDefault(x => x.Id == pro.ParentId);
                        equipment.ElementType = pro?.Name;
                        if (pro.ParentId != null)
                        {
                            pro = productList.FirstOrDefault(x => x.Id == pro.ParentId);
                            equipment.EquipmentSubclass = pro?.Name;
                        }
                    }
                }

                equipment.Name = item.Name; //设备名称
                if (item.ComponentCategoryId != null && item.ComponentCategoryId != Guid.Empty) //节、组合分类
                {
                    equipment.SectionIFD = item.ComponentCategory?.Name;
                    if (item.ComponentCategory.ParentId != null)
                    {
                        var group = groupList.FirstOrDefault(x => x.Id == item.ComponentCategory.ParentId);
                        equipment.Group = "_" + group.Name + equipment.Group;
                        if (group.ParentId != null)
                        {
                            group = groupList.FirstOrDefault(x => x.Id == group.ParentId);
                            equipment.Group = "_" + group.Name + equipment.Group;
                        }
                    }
                    equipment.Group = equipment.Group?.TrimStart('_');
                }
                if (item.InstallationSiteId != null && item.InstallationSiteId != Guid.Empty)
                {
                    var install = installationSitesRepository.WithDetails().Where(y => y.Id == item.InstallationSiteId).FirstOrDefault();
                    if (!string.IsNullOrEmpty(item.InstallationSite.CSRGCode))
                    {
                        if (!string.IsNullOrEmpty(item.InstallationSite?.CSRGCode))
                        {
                            if (!item.InstallationSite.CSRGCode.StartsWith("SC"))  //机房编码
                            {
                                //equipment.InstallationSiteCode = installationList.FirstOrDefault(x => x.Id == item.InstallationSiteId).CSRGCode;
                                equipment.InstallationSiteCode = item.InstallationSite.CSRGCode;
                            }
                        }

                        if (item.InstallationSite?.LocationType == InstallationSiteLocationType.StationInner) //安装位置
                        {
                            equipment.StaMachineRoomId = item.InstallationSite?.Name;
                            equipment.StaRailwayId = install?.Railway?.Name;
                            equipment.StaStationId = install?.Station?.Name;
                        }
                        else if (item.InstallationSite?.LocationType == InstallationSiteLocationType.SectionInner)
                        {
                            equipment.ZoneMachineRoomId = item.InstallationSite?.Name;
                            equipment.ZoneRailwayId = install?.Railway?.Name;
                            equipment.ZoneKilometerMark = install.KMMark.ToString();
                        }
                        else if (item.InstallationSite?.LocationType == InstallationSiteLocationType.Other)
                        {
                            equipment.OthMachineRoomId = item.InstallationSite?.Name;
                            equipment.OthRailwayId = install?.Railway?.Name;
                        }
                    }

                    if (item.InstallationSite.LocationType == InstallationSiteLocationType.StationInner) //安装位置
                    {
                        equipment.StaMachineRoomId = item.InstallationSite.Name;
                        equipment.StaRailwayId = install?.Railway?.Name;
                        equipment.StaStationId = install?.Station?.Name;
                    }
                    else if (item.InstallationSite.LocationType == InstallationSiteLocationType.SectionInner)
                    {
                        equipment.ZoneMachineRoomId = item.InstallationSite.Name;
                        equipment.ZoneRailwayId = install?.Railway?.Name;
                        equipment.ZoneKilometerMark = install.KMMark.ToString();
                    }
                    else if (item.InstallationSite.LocationType == InstallationSiteLocationType.Other)
                    {
                        equipment.OthMachineRoomId = item.InstallationSite.Name;
                        equipment.OthRailwayId = install?.Railway?.Name;
                    }
                }
                /**
                    系统名称待添加1
                 */

                if (item.ManufacturerId != null && item.ManufacturerId != Guid.Empty)
                {
                    equipment.Manufacturer = manufacturerList.FirstOrDefault(x => x.Id == item.ManufacturerId).Name;
                }
                equipment.CSRGCode = item.CSRGCode;
                equipment.UseDate = item.UseDate != null ? Convert.ToDateTime(item.UseDate.ToString()).ToString("yyyy-MM-dd") : null;
                equipment.State = item.State == EquipmentState.OnService ? "主用" : (item.State == EquipmentState.OffService ? "封存" : (item.State == EquipmentState.SpareService ? "备用" : "报废"));

                equipments.Add(equipment);
            }


            var dtoList = ObjectMapper.Map<List<EquipmentModel>, List<EquipmentModel>>(equipments);
            stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            //}
            return stream;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.Equipment.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                if (id == null || Guid.Empty == id) throw new Exception("id不正确");
                var ent = _repositoryEquipment.WithDetails(x => x.Children).FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new Exception("该设备不存在");
                if (ent.Children.Count > 0) throw new Exception("请先删除下级设备！");
                ent.IsDeleted = true;
                await _repositoryEquipment.UpdateAsync(ent);
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        /// <summary>
        /// 删除关联文件
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteFile(Guid id)
        {
            try
            {
                if (id == null || Guid.Empty == id) throw new Exception("id不正确");
                var ent = _equipmentRltFile.FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new Exception("该关联文件不存在");
                await _equipmentRltFile.DeleteAsync(id);
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }



        #region 私有方法
        private List<Manufacturer> _manufacturerList;//厂家列表
        private List<Model> _modelList;//标准设备列表
        private List<ProductCategory> _productList;//产品列表
        private readonly IRepository<StoreEquipment, Guid> _storeEquipmente;



        private List<Equipment> GetEquipmentData(EquipmentSearchDto input)
        {
            //获取当前登录用户的组织机构
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? orgRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var organizationCode = organization != null ? organization.Code : null;

            var allOrgs = new List<Guid>();
            if (!string.IsNullOrEmpty(organizationCode))
            {
                allOrgs = orgRepository.Where(x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();
            }

            var componentCategoryCode = "";
            if (input.ComponentCategoryId != null || input.ComponentCategoryId != Guid.Empty)
            {
                var componentCategory = crmRepository.FirstOrDefault(x => x.Id == input.ComponentCategoryId);
                if (componentCategory != null) componentCategoryCode = componentCategory.Code;
            }

            var result = new List<EquipmentDto>();

            var query = _repositoryEquipment.WithDetails()
            .WhereIf(organization != null && !string.IsNullOrEmpty(organizationCode), x => x.EquipmentRltOrganizations.Any(s => allOrgs.Contains(s.OrganizationId)) || x.EquipmentRltOrganizations.Count == 0)
            .WhereIf(string.IsNullOrEmpty(input.Keyword) &&
                    (input.InstallationSiteId == null || input.InstallationSiteId == Guid.Empty) &&
                    (input.OrganizationIds == null || input.OrganizationIds.Count == 0) &&
                    (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty), x => x.IsDeleted == false)
           .WhereIf(input.InstallationSiteId != null && input.InstallationSiteId != Guid.Empty, x => x.InstallationSiteId == input.InstallationSiteId)
           .WhereIf(input.OrganizationIds != null && input.OrganizationIds.Any(), x => x.EquipmentRltOrganizations.Any(y => input.OrganizationIds.Contains(y.OrganizationId)))
           .WhereIf(!string.IsNullOrEmpty(componentCategoryCode), x => x.ComponentCategory.Code.StartsWith(componentCategoryCode))
           .WhereIf(!string.IsNullOrEmpty(input.Keyword), x =>
                x.Name.Contains(input.Keyword) ||
                x.Manufacturer.Name.Contains(input.Keyword) ||
                x.ProductCategory.Name.Contains(input.Keyword) ||
                x.Code.Contains(input.Keyword)).ToList();

            return query;

        }

        /// <summary>
        /// 工程数据导入
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [UnitOfWork]
        private async Task EngineeringEquipmentImport(FileUploadDto fileData, string key)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            DataTable dt = null;
            var rowIndex = 1;  //有效数据得起始索引
            try
            {
                // 获取excel表格，判断报个是否满足模板
                workbook = fileData.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0)
                    .CheckColumnAccordTempleModel<EngineeringEquipmentModel>(rowIndex);
                dt = ExcelHelper.ImportBaseDataToDataTable(fileData.File.OpenReadStream(), fileData.File.FileName, out var workbook1);
            }
            catch (Exception)
            {
                await _fileImport.Cancel(key);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }
            if (!dt.Columns.Contains("ParentName"))
            {
                await _fileImport.Cancel(key);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            var datalist = sheet
                .TryTransToList<EngineeringEquipmentModel>(rowIndex)
                .CheckNull();


            // 定义错误信息列
            var wrongInfos = new List<WrongInfo>();
            try
            {
                if (datalist.Any())
                {
                    await _fileImport.Start(key, datalist.Count);
                    // 为了避免数据插入错误，线将父级为空的数据进行添加
                    datalist = datalist.OrderBy(a => a.ParentName).ToList();

                    foreach (var model in datalist)
                    {
                        await _fileImport.UpdateState(key, datalist.FindIndex(model));

                        var info = new WrongInfo { RowIndex = model.Index };

                        // 判断设备分组
                        var group = _repositoryEquipmentGroup.Where(x => x.Name == model.EquipmentGroup).FirstOrDefault();
                        if (group == null)
                        {
                            info.AppendInfo($"设备分组【{model.EquipmentGroup}】不存在\r\n");
                            wrongInfos.Add(info);
                            continue;
                        }

                        // 判断父级设备，判断依据为：设备分组 + 父级设备名称
                        var parent = _repositoryEquipment.Where(x => x.GroupId == group.Id && x.Name == model.ParentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(model.ParentName) && parent == null)
                        {
                            info.AppendInfo($"父级设备【{model.ParentName}】不存在\r\n");
                            wrongInfos.Add(info);
                            continue;
                        }

                        // 判断构件分类，不存在，就尝试找父级的构件分类
                        var componentCategory = _componentCategoryRepository.Where(x => x.Code == model.ComponentCategory).FirstOrDefault();
                        if (componentCategory == null && parent != null)
                        {
                            componentCategory = _componentCategoryRepository.Where(x => x.Id == parent.ComponentCategoryId).FirstOrDefault();
                        }
                        if (componentCategory == null)
                        {
                            info.AppendInfo($"构件分类【{model.ComponentCategory}】不存在\r\n");
                            wrongInfos.Add(info);
                            continue;
                        }

                        // 判断产品分类
                        var productCategory = _productCategoryRepository.Where(x => x.Code == model.ProductCategory).FirstOrDefault();
                        //if (productCategory == null)
                        //{
                        //    info.AppendInfo($"产品分类【{model.ComponentCategory}】不存在\r\n");
                        //    wrongInfos.Add(info);
                        //    continue;
                        //}

                        // 获取安装位置
                        var installation = installationSitesRepository.FirstOrDefault(x => x.Name == model.InstallationSiteName);
                        if (installation == null)
                        {
                            info.AppendInfo($"安装位置【{model.InstallationSiteName}】不存在\r\n");
                            wrongInfos.Add(info);
                            continue;
                        }

                        // 判断设备名称
                        if (string.IsNullOrEmpty(model.Name))
                        {
                            info.AppendInfo($"设备名称【{model.Index}】不能为空\r\n");
                            wrongInfos.Add(info);
                            continue;
                        }

                        using var uow = _unitOfWork.Begin(true, false);

                        // 根据设备名称、设备分组查设备
                        var exist = _repositoryEquipment.WithDetails(x => x.Group).Where(x => x.Name == model.Name && x.Group.Name == model.EquipmentGroup).FirstOrDefault();

                        // 不存在就创建
                        var entity = exist != null ? exist : new Equipment(_guidGenerator.Create());

                        // 更新设备，包括：安装位置、产品编码、构建编码、父级设备、MVD属性（删除之前的关联关系）、扩展属性（删除之前的关联关系）
                        entity.StandardName = model.StandardName;
                        entity.ComponentCategoryId = componentCategory.Id;
                        entity.ProductCategoryId = productCategory != null ? productCategory?.Id : null;
                        entity.InstallationSiteId = installation.Id;
                        entity.State = EquipmentState.OnService;
                        entity.GroupId = group.Id;
                        entity.OrganizationId = installation.OrganizationId;
                        entity.Type = EquipmentType.Default;
                        entity.Name = model.Name;

                        // 处理关联数据
                        if (exist != null)
                        {
                            info.AppendInfo($"设备【{model.Name}】已更新\r\n");
                            wrongInfos.Add(info);
                            // 库存设备、服务状态，不用更新
                            // MVD属性（删除之前的关联关系）、扩展属性（删除之前的关联关系）
                            await _equipmentPropertyRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
                        }
                        else
                        {
                            info.AppendInfo($"设备【{model.Name}】已新增\r\n");
                            await _repositoryEquipment.InsertAsync(entity);
                            // 库存设备、服务状态，需要新增
                            if (productCategory != null)
                            {

                                var storeCount = await _storeEquipmentRepository.GetCountAsync();
                                // 根据构件分类，产品分类，设备名称查询是否有仓库
                                var storeRecord = new StoreEquipment(_guidGenerator.Create())
                                {
                                    Name = model.Name,
                                    Code = (storeCount + 1).ToString(CultureInfo.InvariantCulture).PadRight(9, '0'),
                                    ProductCategoryId = productCategory.Id,
                                    ComponentCategoryId = componentCategory.Id,
                                    State = StoreEquipmentState.UnActived
                                };
                                entity.StoreEquipmentId = storeRecord.Id;
                                // 新增库存
                                await _storeEquipmentRepository.InsertAsync(storeRecord);

                                // 处理服务记录
                                var serviceRecord = new EquipmentServiceRecord(_guidGenerator.Create())
                                {
                                    StoreEquipmentId = storeRecord.Id,
                                    EquipmentId = entity.Id,
                                    Type = EquipmentServiceRecordType.Install,
                                    Date = DateTime.Now
                                };
                                await _equipmentServiceProperty.InsertAsync(serviceRecord);
                            }
                        }

                        // 更新 MVD 属性
                        if (!string.IsNullOrEmpty(model.MVDProperties))
                        {
                            try
                            {
                                var list = _repositoryMVDProperty.WithDetails(p => p.MVDCategory).ToList();
                                var mVDCategoryExportDtos = JsonConvert.DeserializeObject<List<MVDCategoryExportDto>>(model.MVDProperties);
                                foreach (var category in mVDCategoryExportDtos)
                                {
                                    foreach (var property in category.MVDProperties)
                                    {
                                        var stdProperty = list.Where(x => x.MVDCategory.Name == category.Name && x.Name == property.Name).FirstOrDefault();
                                        if (stdProperty != null)
                                        {
                                            var equipmentProperty = new EquipmentProperty(_guidGenerator.Create());

                                            equipmentProperty.MVDCategoryId = stdProperty.MVDCategory.Id;
                                            equipmentProperty.MVDPropertyId = stdProperty.Id;
                                            equipmentProperty.Name = property.Name;
                                            equipmentProperty.Value = property.Value;
                                            equipmentProperty.Order = property.Order;
                                            equipmentProperty.EquipmentId = entity.Id;

                                            await _equipmentPropertyRepository.InsertAsync(equipmentProperty);
                                        }
                                        else
                                        {
                                            info.AppendInfo($"MVD属性【{category.Name}】-【{property.Name}】不存在\r\n");
                                        }
                                    };
                                };
                            }
                            catch (Exception e)
                            {
                                info.AppendInfo($"MVD属性解析错误，格式不正确\r\n");
                            }
                        }

                        // 更新扩展属性
                        if (model.Properties.Any())
                        {
                            foreach (var prop in model.Properties)
                            {
                                var equipmentProperty = new EquipmentProperty(_guidGenerator.Create());
                                equipmentProperty.Name = prop.Key;
                                equipmentProperty.Value = prop.Value;
                                equipmentProperty.EquipmentId = entity.Id;
                                await _equipmentPropertyRepository.InsertAsync(equipmentProperty);
                            }
                        }

                        await uow.SaveChangesAsync();
                    }
                    await _fileImport.Complete(key);
                }
                // 处理错误信息
                if (wrongInfos.Any())
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), key, workbook.ConvertToBytes());
                }
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// 工程电缆导入
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [UnitOfWork]
        private async Task EngineCableImport(FileUploadDto fileData, string key)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            DataTable dt = null;
            var rowIndex = 1;
            try
            {
                workbook = fileData.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0)
                .CheckColumnAccordTempleModel<EngineeringCableModel>(rowIndex);
                dt = ExcelHelper.ImportBaseDataToDataTable(fileData.File.OpenReadStream(), fileData.File.FileName, out var workbook1);
            }
            catch (Exception)
            {
                await _fileImport.Cancel(key);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            //if (dt.Columns.Contains("ParentName") || fileData.File.FileName.IndexOf("工程电缆") == -1)
            if (dt.Columns.Contains("ParentName"))
            {
                await _fileImport.Cancel(key);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            var wrongInfos = new List<WrongInfo>();
            var dataList = sheet.TryTransToList<EngineeringCableModel>(rowIndex)
                .CheckNull();

            if (dataList.Any())
            {
                await _fileImport.Start(key, dataList.Count());
                foreach (var model in dataList)
                {
                    await _fileImport.UpdateState(key, dataList.FindIndex(model));
                    var info = new WrongInfo { RowIndex = model.Index };

                    // 判断设备分组
                    var group = _repositoryEquipmentGroup.Where(x => x.Name == model.EquipmentGroup).FirstOrDefault();
                    if (group == null)
                    {
                        info.AppendInfo($"设备分组【{model.EquipmentGroup}】不存在\r\n");
                        wrongInfos.Add(info);
                        continue;
                    }

                    // 判断构件分类，不存在，就尝试找父级的构件分类
                    var componentCategory = _componentCategoryRepository.Where(x => x.Code == model.ComponentCategory).FirstOrDefault();
                    if (componentCategory == null)
                    {
                        info.AppendInfo($"构件分类【{model.ComponentCategory}】不存在\r\n");
                        wrongInfos.Add(info);
                        continue;
                    }

                    // 判断产品分类
                    var productCategory = _productCategoryRepository.Where(x => x.Code == model.ProductCategory).FirstOrDefault();
                    //if (productCategory == null)
                    //{
                    //    info.AppendInfo($"产品分类【{model.ComponentCategory}】不存在\r\n");
                    //    wrongInfos.Add(info);
                    //    continue;
                    //}

                    // 获取安装位置
                    var installation = installationSitesRepository.FirstOrDefault(x => x.Name == model.InstallationSiteName);
                    if (installation == null)
                    {
                        info.AppendInfo($"安装位置【{model.InstallationSiteName}】不存在\r\n");
                        wrongInfos.Add(info);
                        continue;
                    }

                    // 判断设备名称
                    if (string.IsNullOrEmpty(model.Name))
                    {
                        info.AppendInfo($"设备名称【{model.Index}】不能为空\r\n");
                        wrongInfos.Add(info);
                        continue;
                    }

                    // 电缆芯数
                    int? cableExtendNumber = null;
                    if (!string.IsNullOrEmpty(model.Number))
                    {
                        try
                        {
                            cableExtendNumber = int.Parse(model.Number);
                        }
                        catch (Exception e)
                        {
                            info.AppendInfo($"电缆芯数【{model.Number}】格式错误\r\n");
                            wrongInfos.Add(info);
                            continue;
                        }
                    }

                    // 电缆备用芯数
                    int? cableExtendSpareNumber = null;
                    if (!string.IsNullOrEmpty(model.SpareNumber))
                    {
                        try
                        {
                            cableExtendSpareNumber = int.Parse(model.SpareNumber);
                        }
                        catch (Exception e)
                        {
                            info.AppendInfo($"电缆备用芯数【{model.SpareNumber}】格式错误\r\n");
                            wrongInfos.Add(info);
                            continue;
                        }
                    }

                    // 电缆路产芯数
                    int? cableExtendRailwayNumber = null;
                    if (!string.IsNullOrEmpty(model.RailwayNumber))
                    {
                        try
                        {
                            cableExtendRailwayNumber = int.Parse(model.RailwayNumber);
                        }
                        catch (Exception e)
                        {
                            info.AppendInfo($"电缆路产芯数【{model.RailwayNumber}】格式错误\r\n");
                            wrongInfos.Add(info);
                            continue;
                        }
                    }

                    // 电缆皮长
                    int? cableExtendLength = null;
                    if (!string.IsNullOrEmpty(model.Length))
                    {
                        try
                        {
                            cableExtendLength = int.Parse(model.Length);
                        }
                        catch (Exception e)
                        {
                            info.AppendInfo($"电缆皮长【{model.Length}】格式错误\r\n");
                            wrongInfos.Add(info);
                            continue;
                        }
                    }

                    using var uow = _unitOfWork.Begin(true, false);

                    // 根据设备名称、设备分组查设备
                    var exist = _repositoryEquipment.WithDetails(x => x.Group).Where(x => x.Name == model.Name && x.Group.Name == model.EquipmentGroup).FirstOrDefault();

                    // 不存在就创建
                    var entity = exist != null ? exist : new Equipment(_guidGenerator.Create());

                    // 更新设备，包括：安装位置、产品编码、构建编码、父级设备、MVD属性（删除之前的关联关系）、扩展属性（删除之前的关联关系）
                    entity.ComponentCategoryId = componentCategory.Id;
                    entity.ProductCategoryId = productCategory != null ? productCategory?.Id : null;
                    entity.InstallationSiteId = installation.Id;
                    entity.State = EquipmentState.OnService;
                    entity.GroupId = group.Id;
                    entity.OrganizationId = installation.OrganizationId;
                    entity.Type = EquipmentType.Default;
                    entity.Name = model.Name;

                    entity.CableExtend = entity.CableExtend != null ? entity.CableExtend : new CableExtend(_guidGenerator.Create());
                    entity.CableExtend.Number = cableExtendNumber;
                    entity.CableExtend.SpareNumber = cableExtendSpareNumber;
                    entity.CableExtend.RailwayNumber = cableExtendRailwayNumber;
                    entity.CableExtend.Length = cableExtendLength;


                    // 处理关联数据
                    if (exist != null)
                    {
                        info.AppendInfo($"设备【{model.Name}】已更新\r\n");
                        wrongInfos.Add(info);
                        // 库存设备、服务状态，不用更新
                        // MVD属性（删除之前的关联关系）、扩展属性（删除之前的关联关系）
                        await _equipmentPropertyRepository.DeleteAsync(x => x.EquipmentId == entity.Id);

                        // 电缆扩展属性（删除之前的关联关系）
                        await _cableExtendRepository.DeleteAsync(x => x.Id == exist.CableExtendId);
                    }
                    else
                    {
                        info.AppendInfo($"设备【{model.Name}】已新增\r\n");
                        await _repositoryEquipment.InsertAsync(entity);
                        // 库存设备、服务状态，需要新增
                        var storeCount = await _storeEquipmentRepository.GetCountAsync();
                        // 根据构件分类，产品分类，设备名称查询是否有仓库
                        if (productCategory != null)
                        {

                            var storeRecord = new StoreEquipment(_guidGenerator.Create())
                            {
                                Name = model.Name,
                                Code = (storeCount + 1).ToString(CultureInfo.InvariantCulture).PadRight(9, '0'),
                                ProductCategoryId = productCategory.Id,
                                ComponentCategoryId = componentCategory.Id,
                                State = StoreEquipmentState.UnActived
                            };
                            entity.StoreEquipmentId = storeRecord.Id;
                            // 新增库存
                            await _storeEquipmentRepository.InsertAsync(storeRecord);


                            // 处理服务记录
                            var serviceRecord = new EquipmentServiceRecord(_guidGenerator.Create())
                            {
                                StoreEquipmentId = storeRecord.Id,
                                EquipmentId = entity.Id,
                                Type = EquipmentServiceRecordType.Install,
                                Date = DateTime.Now
                            };
                            await _equipmentServiceProperty.InsertAsync(serviceRecord);
                        }
                    }

                    // 更新 MVD 属性
                    if (!string.IsNullOrEmpty(model.MVDProperties))
                    {
                        try
                        {
                            var list = _repositoryMVDProperty.WithDetails(p => p.MVDCategory).ToList();
                            var mVDCategoryExportDtos = JsonConvert.DeserializeObject<List<MVDCategoryExportDto>>(model.MVDProperties);
                            foreach (var category in mVDCategoryExportDtos)
                            {
                                foreach (var property in category.MVDProperties)
                                {
                                    var stdProperty = list.Where(x => x.MVDCategory.Name == category.Name && x.Name == property.Name).FirstOrDefault();
                                    if (stdProperty != null)
                                    {
                                        var equipmentProperty = new EquipmentProperty(_guidGenerator.Create());

                                        equipmentProperty.MVDCategoryId = stdProperty.MVDCategory.Id;
                                        equipmentProperty.MVDPropertyId = stdProperty.Id;
                                        equipmentProperty.Name = property.Name;
                                        equipmentProperty.Value = property.Value;
                                        equipmentProperty.Order = property.Order;
                                        equipmentProperty.EquipmentId = entity.Id;

                                        await _equipmentPropertyRepository.InsertAsync(equipmentProperty);
                                    }
                                    else
                                    {
                                        info.AppendInfo($"MVD属性【{category.Name}】-【{property.Name}】不存在\r\n");
                                    }
                                };
                            };
                        }
                        catch (Exception e)
                        {
                            info.AppendInfo($"MVD属性解析错误，格式不正确\r\n");
                        }
                    }

                    // 更新扩展属性
                    if (model.Properties.Any())
                    {
                        foreach (var prop in model.Properties)
                        {
                            var equipmentProperty = new EquipmentProperty(_guidGenerator.Create());
                            equipmentProperty.Name = prop.Key;
                            equipmentProperty.Value = prop.Value;
                            equipmentProperty.EquipmentId = entity.Id;
                            await _equipmentPropertyRepository.InsertAsync(equipmentProperty);
                        }
                    }

                    await uow.SaveChangesAsync();
                }
                await _fileImport.Complete(key);
            }
            // 处理错误信息
            if (wrongInfos.Any())
            {
                sheet.CreateInfoColumn(wrongInfos);
                await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), key, workbook.ConvertToBytes());
            }
        }

        /// <summary>
        /// 电缆配线导入
        /// </summary>
        /// <param name="fileData">文件信息</param>
        /// <param name="key">导入标识</param>
        /// <returns></returns>
        [UnitOfWork]
        private async Task CableWiringImport(FileUploadDto fileData, string key)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            var rowIndex = 2;

            try
            {
                workbook = fileData.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0)
                .CheckColumnAccordTempleModel<CableWiringModel>(rowIndex);
            }
            catch (Exception)
            {
                await _fileImport.Cancel(key);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            var wrongInfos = new List<WrongInfo>();
            var dataList = sheet.TryTransToList<CableWiringModel>(rowIndex)
                .CheckNull();
            if (dataList.Any())
            {
                await _fileImport.Start(key, dataList.Count());

                foreach (var item in dataList)
                {
                    await _fileImport.UpdateState(key, dataList.FindIndex(item));
                    using var uow = _unitOfWork.Begin(true, false);
                    var newInfo = new WrongInfo { RowIndex = item.Index + 1 };
                    var canInsert = true;
                    //1、这条配线信息是否存在,条件：同一位置+同一电缆+同一电缆芯+ab设备端子名称
                    if (await EquipmentManager.HasCableWiring(item))
                    {
                        newInfo.AppendInfo("此配线已存在");
                        canInsert = false;
                    }

                    // 电缆安装位置判断
                    var cableInstallation =
                        installationSitesRepository.FirstOrDefault(a => a.Name == item.InstallationSiteName);
                    if (cableInstallation == null)
                    {
                        newInfo.AppendInfo($"安装位置【{item.InstallationSiteName}】不存在");
                        canInsert = false;
                    }

                    //获取电缆信息
                    var cableModel = _repositoryEquipment.FirstOrDefault(a => a.Name == item.Name && a.InstallationSiteId == cableInstallation.Id);
                    if (cableModel == null)
                    {
                        newInfo.AppendInfo($"【{item.Name}】电缆不存在");
                        canInsert = false;
                    }

                    // 判断设备A
                    var equipmentAId = await
                        EquipmentManager.GetEquipmentGuid(item.EquipmentAName, item.EquipmentAGroupName);
                    if (equipmentAId == null)
                    {
                        newInfo.AppendInfo($"【{item.EquipmentAGroupName}】-【{item.EquipmentAName}】不存在");
                        canInsert = false;
                    }
                    // 判断设备B
                    var equipmentBId = await
                        EquipmentManager.GetEquipmentGuid(item.EquipmentBName, item.EquipmentBGroupName);
                    if (equipmentBId == null)
                    {
                        newInfo.AppendInfo($"【{item.EquipmentBGroupName}】-【{item.EquipmentBName}】不存在");
                        canInsert = false;
                    }

                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    // 处理电缆心，有则存储，无则创建
                    var cableCore = _cableCoreRepository.FirstOrDefault(a => a.Name == item.CableCore && a.CableId == cableModel.Id);
                    if (cableCore == null)
                    {
                        cableCore = new CableCore(_guidGenerator.Create())
                        {
                            Name = item.CableCore,
                            CableId = cableModel.Id
                        };
                        await _cableCoreRepository.InsertAsync(cableCore);
                        await uow.SaveChangesAsync();
                    }

                    // 创建端子A
                    var equipmentATerminal = _terminalRepository.FirstOrDefault(a =>
                        a.EquipmentId == equipmentAId && a.Name == item.EquipmentATerminalName);
                    if (equipmentATerminal == null)
                    {
                        equipmentATerminal = new Terminal(_guidGenerator.Create())
                        {
                            Name = item.EquipmentATerminalName,
                            EquipmentId = equipmentAId.GetValueOrDefault()
                        };
                        await _terminalRepository.InsertAsync(equipmentATerminal);
                        await uow.SaveChangesAsync();
                    }

                    // 创建端子B
                    var equipmentBTerminal = _terminalRepository.FirstOrDefault(a =>
                        a.EquipmentId == equipmentBId && a.Name == item.EquipmentBTerminalName);
                    if (equipmentBTerminal == null)
                    {
                        equipmentBTerminal = new Terminal(_guidGenerator.Create())
                        {
                            Name = item.EquipmentBTerminalName,
                            EquipmentId = equipmentBId.GetValueOrDefault()
                        };
                        await _terminalRepository.InsertAsync(equipmentBTerminal);
                        await uow.SaveChangesAsync();
                    }


                    //判断设备端子关联信息  当前端子或者对方端子都需要存在

                    var terminalLink = _terminalLinkRepository
                        .FirstOrDefault(a =>
                            (a.TerminalAId == equipmentATerminal.Id && a.TerminalBId == equipmentBTerminal.Id) ||
                            (a.TerminalAId == equipmentBTerminal.Id && a.TerminalBId == equipmentATerminal.Id));

                    if (terminalLink != null)
                    {
                        continue;
                    }

                    terminalLink = new TerminalLink(_guidGenerator.Create())
                    {
                        TerminalAId = equipmentATerminal.Id,
                        TerminalBId = equipmentBTerminal.Id,
                        CableCoreId = cableCore.Id
                    };
                    await _terminalLinkRepository.InsertAsync(terminalLink);
                    await uow.SaveChangesAsync();
                }
            }

            await _fileImport.Complete(key);
            if (wrongInfos.Any())
            {
                sheet.CreateInfoColumn(wrongInfos);
                await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), key, workbook.ConvertToBytes());
            }
        }

        /// <summary>
        /// 机柜配线导入
        /// </summary>
        /// <param name="fileData">文件信息</param>
        /// <param name="key">数据导入标识</param>
        /// <returns></returns>
        [UnitOfWork]
        private async Task CabinetWiringImport(FileUploadDto fileData, string key)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            var rowIndex = 1;

            try
            {
                workbook = fileData.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0)
                .CheckColumnAccordTempleModel<CabinetWiringModel>(rowIndex);
            }
            catch (Exception)
            {
                await _fileImport.Cancel(key);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            var wrongInfos = new List<WrongInfo>();

            var dataList = sheet.TryTransToList<CabinetWiringModel>(rowIndex)
                .CheckNull();
            if (dataList.Any())
            {
                await _fileImport.Start(key, dataList.Count());

                foreach (var item in dataList)
                {
                    await _fileImport.UpdateState(key, dataList.FindIndex(item));
                    var newInfo = new WrongInfo { RowIndex = item.Index };
                    var canInsert = true;

                    // 1、检查安装位置
                    var installation =
                        installationSitesRepository.FirstOrDefault(a => a.Name == item.InstallationSiteName);
                    if (installation == null)
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"【{item.InstallationSiteName}】不存在");
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    // 2、先拆解数据，得到线缆和端子的信息
                    if (!item.Name.Contains("#"))
                    {
                        newInfo.AppendInfo($"【{item.Name}】不符合导入规则");
                        canInsert = false;
                    }
                    var equipmentInfo = item.Name.Split('#');
                    var equipmentA = equipmentInfo[0];
                    var equipmentB = equipmentInfo[1];

                    // 判断安装位置下设备a 是否存在
                    var equipmentATerminalName = equipmentA.Split('`').Last();// 设备A的端子名
                    var equipmentAName = equipmentA.Substring(0, equipmentA.LastIndexOf('`'));
                    var equipmentAModel = _repositoryEquipment.FirstOrDefault(a =>
                        a.Name == equipmentAName && a.InstallationSiteId == installation.Id);
                    if (equipmentAModel == null)
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"【{equipmentAName}】 不存在");
                    }

                    // 判断安装位置下设备B是否存在
                    var equipmentBTerminalName = equipmentB.Split('`').Last();// 设备B的端子名
                    var equipmentBName = equipmentB.Substring(0, equipmentB.LastIndexOf('`'));
                    var equipmentBModel = _repositoryEquipment.FirstOrDefault(a =>
                        a.Name == equipmentBName && a.InstallationSiteId == installation.Id);

                    if (equipmentBModel == null)
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"【{equipmentBName}】不存在");
                    }
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    using var uow = _unitOfWork.Begin(true, false);
                    // 处理配线关系， 端子号不存在则新增
                    // 端子1
                    var terminalAModel = _terminalRepository.FirstOrDefault(a =>
                        a.Name == equipmentATerminalName && a.EquipmentId == equipmentAModel.Id);
                    if (terminalAModel == null)
                    {
                        terminalAModel = new Terminal(_guidGenerator.Create())
                        {
                            Name = equipmentATerminalName,
                            EquipmentId = equipmentAModel.Id
                        };
                        await _terminalRepository.InsertAsync(terminalAModel);
                        await uow.SaveChangesAsync();
                    }

                    // 端子2
                    var terminalBModel = _terminalRepository.FirstOrDefault(a =>
                        a.Name == equipmentBTerminalName && a.EquipmentId == equipmentBModel.Id);
                    if (terminalBModel == null)
                    {
                        terminalBModel = new Terminal(_guidGenerator.Create())
                        {
                            Name = equipmentBTerminalName,
                            EquipmentId = equipmentBModel.Id
                        };
                        await _terminalRepository.InsertAsync(terminalBModel);
                        await uow.SaveChangesAsync();
                    }

                    // 添加端子关联关系
                    var terminalLink = _terminalLinkRepository.FirstOrDefault(a =>
                        (a.TerminalAId == terminalAModel.Id && a.TerminalBId == terminalBModel.Id) ||
                        (a.TerminalAId == terminalBModel.Id && a.TerminalBId == terminalAModel.Id));
                    if (terminalLink != null)
                    {
                        newInfo.AppendInfo($"配线关系已存在");
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        terminalLink = new TerminalLink(_guidGenerator.Create())
                        {
                            TerminalAId = terminalAModel.Id,
                            TerminalBId = terminalBModel.Id
                        };
                        await _terminalLinkRepository.InsertAsync(terminalLink);
                        await uow.SaveChangesAsync();
                    }
                }
            }

            await _fileImport.Complete(key);
            if (wrongInfos.Any())
            {
                sheet.CreateInfoColumn(wrongInfos);
                await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), key, workbook.ConvertToBytes());
            }
        }

        /// <summary>
        /// 根据厂家名称获得厂家主键
        /// </summary>
        private async Task<Guid?> GetManufacturerIdAsync(string manufacturerName, string importKey)
        {
            //获得所有厂家列表
            if (_manufacturerList == null)
            {
                _manufacturerList = manufacturerRepository.Where(z => z.Id != Guid.Empty).ToList();
                if (_manufacturerList.Count == 0)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("未在标准库中找到厂家数据");
                }

            }
            var manufacture = this._manufacturerList.FirstOrDefault(z => z.Name == manufacturerName);
            return manufacture?.Id;
        }


        /// <summary>
        /// 获得当前节点的父级节点
        /// </summary>
        private TreeNode GetNodesParentNode(List<TreeNode> treeList, PartItem thisNode)
        {
            var arr = thisNode.ParentName.Split('✪');
            var node = (TreeNode)null;
            var index = 0;
            if (string.IsNullOrEmpty(thisNode.ParentName))
            {
                node = treeList.FirstOrDefault(z => z.Name == thisNode.Name);
                return node;
            }

            if (arr.Length > index)//第一级
            {
                node = treeList.FirstOrDefault(z => z.Name == arr[index]);
            }

            for (var i = 0; i < 20; i++)
            {
                index++;
                if (index > arr.Length) break;

                if (arr.Length > index && node != null)//第二级及其子集
                {
                    node = node.Child.FirstOrDefault(z => z.Name == arr[index]);
                    if (node == null) break;
                }
            }
            return node;
        }


        /// <summary>
        /// 设置运行状态
        /// </summary>
        /// <param name="item"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool SetRunningStates(PartImport item, string state)
        {
            if (state == EquipmentState.OnService.GetDescription())
            {
                item.State = EquipmentState.OnService;
            }
            else if (state == EquipmentState.OffService.GetDescription())
            {
                item.State = EquipmentState.OffService;
            }
            else if (state == EquipmentState.SpareService.GetDescription())
            {
                item.State = EquipmentState.SpareService;
            }
            else if (state == EquipmentState.Scrap.GetDescription())
            {
                item.State = EquipmentState.Scrap;
            }
            else
                return false;
            return true;
        }


        /// <summary>
        /// 获取标准设备
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="standardEquipment"></param>
        /// <param name="importKey"></param>
        /// <returns></returns>
        private async Task<Guid?> GetStandardEquipmentIdAsync(string manufacturer, string standardEquipment, string SectionIFD, string elementType, string EquipmentSubclass, string importKey)
        {
            //获得所有厂家列表
            if (_manufacturerList == null)
            {
                _manufacturerList = manufacturerRepository.Where(z => z.Id != Guid.Empty).ToList();
                if (_manufacturerList.Count == 0)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("未在标准库中找到厂家数据，请先导入厂家");
                }
            }
            //获得所有标准设备列表
            if (_modelList == null)
            {
                _modelList = modelRepository.WithDetails(x => x.ProductCategory).Where(z => z.Id != Guid.Empty).ToList();
                if (_modelList.Count == 0)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("未在标准库中找到标准设备数据，请先导入标准设备");
                }
            }
            //获得所有产品列表
            if (_productList == null)
            {
                _productList = productRepository.Where(z => z.Id != Guid.Empty).ToList();
                if (_productList.Count == 0)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("未在标准库中找到产品数据，请先导入产品");
                }
            }

            //厂家
            var manufacture = _manufacturerList.FirstOrDefault(z => z.Name == manufacturer);
            if (manufacture == null) return null;
            //标准设备
            var models = _modelList.Where(z => z.ManufacturerId == manufacture.Id && z.Name == standardEquipment);
            if (models == null) return null;
            //递归找到产品
            var productCategory = GetProductCategory(models, SectionIFD, elementType, EquipmentSubclass);

            return productCategory?.Id;
        }



        /// <summary>
        /// 格式化导入存在的错误消息
        /// </summary>
        private string FormartErrMsg(string sheetName, string pn, string code, string msg)
        {
            return pn.PadLeft(4, '0') + ".SheetName=" + sheetName + ";Code=" + code + msg;
        }

        /// <summary>
        /// 获取唯一产品分类
        /// </summary>
        private ProductCategory GetProductCategory(IEnumerable<Model> models, string SectionIFD, string elementType, string EquipmentSubclass)
        {
            IEnumerable<ProductCategory> product = null;
            ProductCategory productCategory = null;


            foreach (var model in models)
            {
                product = _productList.Where(z => z.Id == model.ProductCategoryId);

                foreach (var item in product)
                {
                    var parentCode = item.Code.Split(".").Take(item.Code.Split(".").Length - 1).JoinAsString(".");
                    if (SectionIFD == _productList.FirstOrDefault(z => z.Code == parentCode).Name)
                    {
                        parentCode = parentCode.Split(".").Take(parentCode.Split(".").Length - 1).JoinAsString(".");
                        if (elementType == _productList.FirstOrDefault(z => z.Code == parentCode).Name)
                        {
                            parentCode = parentCode.Split(".").Take(parentCode.Split(".").Length - 1).JoinAsString(".");
                            if (EquipmentSubclass == _productList.FirstOrDefault(z => z.Code == parentCode).Name)
                            {
                                productCategory = _productList.FirstOrDefault(z => z.Id == model.ProductCategoryId);
                            }
                        }
                    }
                }

            }
            return productCategory;
        }

        /// <summary>
        /// 构件数据写入标准库构件表中
        /// </summary>
        private readonly string _partCode = "SCC.";
        private List<ComponentCategory> _allCrmList;

        private Dictionary<Guid, Guid> InsertToStandComponentCategory(List<TreeNode> treeData)
        {
            var dic = new Dictionary<Guid, Guid>();
            //获得所有构件列表
            _allCrmList = crmRepository.Where(z => z.Id != Guid.Empty).ToList();
            var parentId = Guid.NewGuid(); ;
            //写入一级(通信专业)
            var treeNode = treeData[0];
            var model = _allCrmList.FirstOrDefault(z => z.ParentId == null && z.Name == treeNode.Name);
            if (model == null)
            {
                model = new ComponentCategory(parentId);
                model.CreationTime = DateTime.Now;
                model.CreatorId = Guid.NewGuid();
                model.Name = treeNode.Name;
                model.Code = GetStandCode(null);
                model.ExtendName = model.Name;
                model.ExtendCode = model.Code;
                model.IsDeleted = false;
                _allCrmList.Add(model);
                crmRepository.InsertAsync(model);
            }
            dic[treeNode.Id] = model.Id;
            parentId = model.Id;

            //写入二级(EXCEL文件名)
            treeNode = treeData[0].Child[0];
            model = _allCrmList.FirstOrDefault(z => z.ParentId == parentId && z.Name == treeNode.Name);
            if (model == null)
            {
                model = new ComponentCategory(Guid.NewGuid());
                model.ParentId = parentId;
                model.CreationTime = DateTime.Now;
                model.CreatorId = Guid.NewGuid();
                model.Name = treeNode.Name;
                model.Code = GetStandCode(parentId);
                model.ExtendName = model.Name;
                model.ExtendCode = model.Code;
                model.IsDeleted = false;
                dic[treeNode.Id] = model.Id;
                _allCrmList.Add(model);
                crmRepository.InsertAsync(model);
            }
            dic[treeNode.Id] = model.Id;
            //递归加载第三级
            DgInsertToStandComponentCategory(treeNode.Child, model.Id, dic);
            return dic;
        }


        /// <summary>
        /// 递归写入标准库构件数据
        /// </summary>
        private void DgInsertToStandComponentCategory(List<TreeNode> treeData, Guid parentId, Dictionary<Guid, Guid> dic)
        {
            foreach (var item in treeData)
            {
                var model = _allCrmList.FirstOrDefault(z => z.ParentId == parentId && z.Name == item.Name);
                if (model == null)
                {
                    model = new ComponentCategory(Guid.NewGuid());
                    model.ParentId = parentId;
                    model.CreationTime = DateTime.Now;
                    model.CreatorId = Guid.NewGuid();
                    model.Name = item.Name;
                    model.Code = GetStandCode(parentId);
                    model.ExtendName = model.Name;
                    model.ExtendCode = model.Code;
                    _allCrmList.Add(model);
                    crmRepository.InsertAsync(model);
                }
                DgInsertToStandComponentCategory(item.Child, model.Id, dic);
                dic[item.Id] = model.Id;
            }
        }


        /// <summary>
        /// 获得新增构件数据节点的编号
        /// </summary>
        private string GetStandCode(Guid? parentId)
        {
            var parentModle = (ComponentCategory)null;
            if (parentId.HasValue)
            {
                parentModle = _allCrmList.FirstOrDefault(z => z.Id == parentId);
                if (parentModle == null) return null;
            }

            var model = _allCrmList.Where(z => z.ParentId == parentId).OrderByDescending(z => z.Code).FirstOrDefault();//找到同级最大的编号
            if (model == null)
            {
                if (!parentId.HasValue)
                {
                    return _partCode + "001";
                }
                else
                {
                    return parentModle.Code + ".001";
                }
            }
            var arr = model.Code.Split('.');

            var val = (int)0;
            if (!int.TryParse(arr.Last(), out val))
            {
                return null;
            }
            val++;
            var code = "";
            if (arr.Length > 2)
            {
                for (var i = 0; i < arr.Length - 1; i++)
                {
                    code += arr[i] + ".";
                }
            }
            else
            {
                code = this._partCode;
            }
            code += val.ToString().PadLeft(3, '0');
            return code.Trim('.');
        }


        /// <summary>
        /// 同一级别下设备名称不能重复
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        async Task<bool> CheckSameName(Guid? parentId, Guid? id, string name)
        {
            return await Task.Run(() =>
            {
                if (parentId == null) return true;

                var sameNames =
                    _repositoryEquipment.FirstOrDefault(a =>
                        a.Name == name && a.ParentId == parentId && a.Id != id);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("当前设备级别下已存在该名称的设备！");
                }

                return true;
            });
        }

        /// <summary>
        /// 验证设备编码唯一性
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        async Task<bool> CheckSameCode(Guid? id, string code)
        {
            return await Task.Run(() =>
            {
                var sameCode =
                    _repositoryEquipment.FirstOrDefault(a =>
                        a.Code == code && a.Id != id);
                if (sameCode != null)
                {
                    throw new UserFriendlyException("该设备编码已存在！");
                }
                return true;
            });
        }

        /// <summary>
        /// 一键入库
        /// </summary>
        /// <returns></returns>
        public async Task<bool> OneTouchInStorage()
        {
            var orgId = new Guid(_httpContextAccessor.HttpContext.Request.Headers["OrganizationId"]);

            var allOrgIds = new List<Guid>();
            if (Guid.Empty != orgId)
            {
                var org = await orgManager.GetAsync(orgId);
                allOrgIds = orgRepository.Where(s => s.Code.StartsWith(org.Code)).Select(x => x.Id).ToList();
            }

            //查询维护单位
            var equipmentIds = equipmentOrgRepository.Where(x => allOrgIds.Contains(x.OrganizationId)).Select(x => x.EquipmentId).ToList();
            using (DataFilter.Disable<ISoftDelete>())
            {
                var waitingStorages = _repositoryEquipment
                   .Where(x => x.CheckState == EquipmentCheckState.UnCheck && (allOrgIds.Contains((Guid)x.OrganizationId) || equipmentIds.Contains(x.Id)))
                   .ToList();


                foreach (var item in waitingStorages)
                {
                    item.CheckState = EquipmentCheckState.Checked;

                    await _repositoryEquipment.UpdateAsync(item);
                }
            }
            return true;

        }

        ////查询站点
        //private async Task<Guid> SelectStationAsync(string stationName, PartImport item)
        //{
        //    var railwayId =await _railways.Where(x => x.Name == item.StaRailwayId).FirstOrDefault().Id;

        //    return railwayId;
        //}
        #endregion
    }
}
