/**********************************************************************
*******命名空间： Volo.Abp.Identity
*******类 名 称： IdentityOrganizationAppService
*******类 说 明： 组织机构应用接口服务 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/5 10:43:21
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NPOI.SS.UserModel;

using SnAbp.Common;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.TreeHelper;
using TemplateModel;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Dtos;
using Volo.Abp.Identity.Repository;
using Volo.Abp.Uow;


namespace SnAbp.Identity
{
    [Authorize]
    public class AppOrganizationAppService : IdentityAppServiceBase, IIdentityOrganizationAppService
    {
        protected OrganizationManager _organizationMananer { get; }

        private IOrganizationRepository _organizationRepository;

        protected IdentityUserManager UserRepository { get; }
        protected new IGuidGenerator GuidGenerator { get; }
        protected IIdentityUserRltOrganizationRepository IdentityUserRltOrganizationRepository;

        protected IDistributedCache<List<OrganizationSelectDto>> OrganizationCache { get; }
        protected IDataFilter DataFilter { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IIdentityUserRoleRepository _userRoleRepository;

        protected IUnitOfWorkManager UntiOfWorkManager { get; }
        private readonly IFileImportHandler _fileImport;
        public AppOrganizationAppService(
            OrganizationManager organizationManager,
            IOrganizationRepository organizationRepository,
            IGuidGenerator guidGenerator,
            IDistributedCache<List<OrganizationSelectDto>> organizationCache,
            IDataFilter dataFilter,
            IUnitOfWorkManager untiOfWorkManager,
            IdentityUserManager userRepository,
            IIdentityUserRltOrganizationRepository identityUserRltOrganizationRepository,
            IFileImportHandler fileImport,
            IHttpContextAccessor httpContextAccessor,
            IdentityUserManager userManager,
            IIdentityUserRoleRepository userRoleRepository
            )
        {
            _organizationRepository = organizationRepository;
            GuidGenerator = guidGenerator;
            OrganizationCache = organizationCache;
            DataFilter = dataFilter;
            UntiOfWorkManager = untiOfWorkManager;
            IdentityUserRltOrganizationRepository = identityUserRltOrganizationRepository;
            UserRepository = userRepository;
            _fileImport = fileImport;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userRoleRepository = userRoleRepository;
            _organizationMananer = organizationManager;
        }

        public Task<OrganizationDto> GetAsync(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var organization = _organizationRepository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (organization == null)
            {
                throw new UserFriendlyException("该机构不存在");
            }

            return Task.FromResult(ObjectMapper.Map<Organization, OrganizationDto>(organization));
        }

        /// <summary>
        /// 根据条件获取组织机构列表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrganizationDto>> GetList(OrganizationGetListDto input)
        {
            // 新增条件过滤
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var organizationCode = organization != null ? organization.Code : null;

            var result = new PagedResultDto<OrganizationDto>();
            var list = new List<Organization>();

            var dtos = new List<OrganizationDto>();

            // ids 反向查找不能进行全局数据过滤，会影响显示
            if (organization != null && input.Ids != null && input.Ids.Count > 0)
            {
                var query = _organizationRepository
                    .WithDetails();
                foreach (var id in input.Ids)
                {
                    //获取当前数据
                    var _org = _organizationRepository.FirstOrDefault(x => x.Id == id);
                    if (_org == null) continue;
                    string code = "";
                    if (_org != null)
                    {
                        code = _org.Code;
                    }
                    var filterLists = query.Where(x => x.Code == organization.Code).ToList();
                    list.AddRange(filterLists);
                    if (_org.Code != organization.Code)
                    {
                        // 获取父级组织机构的id,排除当前组织机构
                        var organizationParentId = query.Where(x => code.StartsWith(x.Code) && code != x.Code && x.Code.StartsWith(organization.Code)).Select(x => x.Id)
                                .ToList();
                        var parentOrganization = query.Where(x => organizationParentId.Contains(x.ParentId.Value)).ToList();
                        list.AddRange(parentOrganization);
                    }
                }

                // 数据去重并转成dto
                var listDtos = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(list.Distinct().ToList());

                //如果子集为空设置children为null
                foreach (var item in listDtos)
                {
                    item.Children = item.Children.Count == 0 ? null : new List<OrganizationDto>();
                }

                dtos = GuidKeyTreeHelper<OrganizationDto>.GetTree(listDtos);
            }
            else //按需加载
            {
                //根据关键字查询
                if (!string.IsNullOrEmpty(input.KeyWords))
                {
                    var query = _organizationRepository
                        .WithDetails();

                    if (!string.IsNullOrEmpty(input.KeyWords))
                    {
                        if (input.isTreeSearch)
                        {
                            query = query.Where(x => x.Name.Contains(input.KeyWords)).Take(200);
                        }
                        else
                        {
                            query = query.Where(x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords) || x.CSRGCode.Contains(input.KeyWords));
                        }

                    }

                    if (organization != null)
                    {
                        query = query.Where(x => x.Code.StartsWith(organization.Code));
                    }

                    list = query.ToList();
                    dtos = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(list);
                    foreach (var item in dtos)
                    {
                        item.Children = null;
                    }
                }

                else
                {
                    var query = _organizationRepository.WithDetails();
                    //如果是顶部组织机构框的按需加载
                    if (input.IsCurrent)
                    {
                        if (input.ParentId != null && input.ParentId != Guid.Empty)
                        {
                            query = query.Where(x => x.ParentId == input.ParentId);
                        }
                        else
                        {
                            query = query.Where(x => x.ParentId == null);
                        }
                    }
                    else
                    {
                        if (input.ParentId != null && input.ParentId != Guid.Empty)
                        {
                            query = query.Where(x => x.ParentId == input.ParentId);
                        }
                        else if (organization != null)
                        {
                            query = query.Where(x => x.Id == organization.Id);
                        }
                    }

                    list = query.ToList();
                    var parentDto = new OrganizationDto();
                    if (list.Count > 0)
                    {
                        parentDto = ObjectMapper.Map<Organization, OrganizationDto>(_organizationRepository.FirstOrDefault(x => x.Id == list.FirstOrDefault().ParentId));
                    }

                    dtos = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(list);
                    foreach (var item in dtos)
                    {
                        item.Children = item.Children.Count == 0 ? null : new List<OrganizationDto>();
                        item.Parent = parentDto;
                    }

                    dtos = GuidKeyTreeHelper<OrganizationDto>.GetTree(dtos);
                }
            }

            result.TotalCount = dtos.Count();
            result.Items = input.IsAll ? dtos.OrderBy(x => x.Code).ToList() :
                            (input.ParentId != null || input.ParentId != Guid.Empty) ? dtos.OrderBy(x => x.Code).ToList()
                            : dtos.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return result;
        }


        /// <summary>
        /// 获取当前登录用户的组织机构ids
        /// </summary>
        /// <returns></returns>
        public async Task<List<Guid>> GetLoginUserOrganizationIds() => (await UserRepository.GetOrganizationsAsync(CurrentUser.Id, true))
            .Select(a => a.Id)
            .ToList();

        /// <summary>
        /// 获取当前登录用户所属组织机构的根级组织机构
        /// </summary>
        /// <returns></returns>
        public Task<string> GetLoginUserOrganizationRootTag(Guid id)
        {
            var currentOrganizationCode = _organizationRepository.FirstOrDefault(x => x.Id == id)?.Code;

            var rootId = "";
            if (!string.IsNullOrEmpty(currentOrganizationCode))
            {
                var rootCode = currentOrganizationCode.Split(".").First();
                var rootList = _organizationRepository.WithDetails()
                    .Where(x => x.IsRoot && x.Code.StartsWith(rootCode) &&
                              x.Code.Length <= currentOrganizationCode.Length)
                    .OrderByDescending(s => s.Code.Length).ToList();
                if (rootList.Count > 0)
                {
                    rootId = rootList.First()?.Id.ToString();
                }
            }

            return Task.FromResult(rootId);
        }

        /// <summary>
        ///  获取当前用户的组织机构数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrganizationDto>> GetCurrentUserOrganizations(Guid? ids)
        {
            var isSystemUser = await UserRepository.isSystem(CurrentUser.Id.Value);
            var listDto = new List<OrganizationDto>();
            var treeList = new List<OrganizationDto>();

            // 非系统用户返回该用户的组织机构
            if (!isSystemUser)
            {
                //  分析：如果不是系统用户：
                // 1、获取当前用户的信息
                var currentUser = await UserRepository.GetByIdAsync(CurrentUser.Id.GetValueOrDefault());
                if (currentUser != null)
                {
                    // 2、在用户与组织机构表里面根据用户id获取组织机构id
                    var userAltOrgIds = IdentityUserRltOrganizationRepository
                        .Where(x => x.UserId == currentUser.Id).Select(x => x.OrganizationId)
                        .ToList().Distinct();
                    // 3、在组织机构表里面获取当前用户所处的组织机构
                    var userOrg = _organizationRepository
                        .WithDetails(x => x.Children).Where(x => userAltOrgIds.Contains(x.Id)).ToList();
                    listDto = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(userOrg);
                    listDto.ForEach(item =>
                    {
                        item.Children = null;
                        item.isGranted = true;
                    });
                }
            }
            // 系统用户返回所有顶级组织机构
            else
            {
                var query = _organizationRepository
                        .WithDetails(x => x.Children);
                var list = new List<Organization>();
                // 获得当前选择的组织机构
                var organization = _organizationRepository.FirstOrDefault(x => x.Id == ids);
                // 分析：如果当前组织机构的ParentId为空，则只需查出parentId为null的数据
                //           如果当前组织机构的parentId不为空,则需查出parentId为空的数据和他上一级的数据。
                //1、如果parentId为空的数据
                var filterList = query.Where(x => x.ParentId == null).ToList();
                list.AddRange(filterList);
                if (organization != null && organization.ParentId.HasValue)
                {
                    // 查到当前组织机构的上一级的数据
                    var parentsOrganizations = query.Where(x => organization.Code.StartsWith(x.Code) && x.Code != organization.Code);
                    var parentIds = parentsOrganizations.Select(s => s.Id).ToList();
                    var parentsOrganizationsList = query.Where(x => parentIds.Contains(x.ParentId.Value)).ToList();
                    list.AddRange(parentsOrganizationsList);
                }
                list = list.Distinct().ToList();
                listDto = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(list);
                listDto.ForEach(item =>
               {
                   item.isGranted = true;
                   item.Children = item.Children != null && item.Children.Count > 0 ? item.Children : null;
               });
            }
            treeList = CodeTreeHelper<OrganizationDto>
                .GetTree(listDto, ".")
                .OrderBy(a => a.Order)
                .ToList();


            return treeList;
        }

        /// <summary>
        /// 根据已有的code 计算出父级的code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private List<string> CalculateOrganizationCode(string code)
        {
            var list = new List<string>();
            var arr = code.Split(".");
            for (var i = 1; i <= arr.Length; i++)
                list.Add(arr.Take(i).JoinAsString("."));
            return list;
        }

        /// <summary>
        /// 判断指定的组织机构中是否含有用户。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> HasUser(Guid id) => (await UserRepository.GetUsersInOrganizationAsync(id)).Any();


        [Authorize(IdentityPermissions.Organization.Create)]
        public async Task<OrganizationDto> CreateAsync(OrganizationInputDto input)
        {
            //1、得到当前登录用户的选择的组织机构
            var glabalOrganizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var glabalOrganization = !string.IsNullOrEmpty(glabalOrganizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(glabalOrganizationIdString)) : null;

            await CheckSameName(input.Name, null, input.ParentId);
            var organization = ObjectMapper.Map<OrganizationInputDto, Organization>(input);
            //分析：（1）当parentId为null时，会出现两张情况：1.当是系统用户，将数据添加到最顶层，不需要对parentId进行单独处理
            //                                               2.当是普通用户的时候，默认的parentId是当前用户选择的组织机构，需要对parentId进行单独处理
            //      （2）当parentId不为null时，parentId就是选择的组织机构,不需要对parentId进行单独处理
            // 2、当parentId为null时
            if (input.ParentId == null || input.ParentId == Guid.Empty)
            {
                //3、如果当前用户不是系统用户的时候
                if (!await UserRepository.isSystem(CurrentUser.Id.Value) && glabalOrganization != null)
                {
                    organization.ParentId = glabalOrganization.Id;
                }
            }
            organization.SetId(GuidGenerator.Create());
            await _organizationMananer.CreateAsync(organization);
            return ObjectMapper.Map<Organization, OrganizationDto>(organization);
        }

        [Authorize(IdentityPermissions.Organization.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _organizationMananer.DeleteAsync(id);
        }

        [Authorize(IdentityPermissions.Organization.Update)]
        public async Task<OrganizationDto> UpdateAsync(OrganizationUpdateDto input)
        {
            await CheckSameName(input.Name, input.Id, input.ParentId);

            var organization = await _organizationMananer.GetAsync(input.Id);

            if (organization == null) throw new UserFriendlyException("更新实体不存在！！！");

            //判断是否根级组织机构，如果是，更改其上下级所有组织机构的isRoot属性为false
            if (input.IsRoot)
            {
                var organizationRoot = _organizationRepository.FirstOrDefault(x => x.IsRoot && x.Code.StartsWith(input.Code));
                if (organizationRoot != null)
                {
                    organizationRoot.IsRoot = false;
                    await _organizationRepository.UpdateAsync(organizationRoot);
                }
            }

            organization.Name = input.Name;
            organization.Nature = input.Nature;
            organization.Order = input.Order;
            organization.Description = input.Description;
            organization.ParentId = input.ParentId;
            organization.Remark = input.Remark;
            organization.IsRoot = input.IsRoot;
            organization.SealImageUrl = input.SealImageUrl;
            organization.CSRGCode = input.CSRGCode;
            organization.TypeId = input.TypeId;
            await _organizationMananer.UpdateAsync(organization);
            return ObjectMapper.Map<Organization, OrganizationDto>(organization);
        }


        /// <summary>
        /// 批量编辑组织类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Organization.Update)]
        public async Task<bool> BatchUpdateTypeAsync(OrganizationBatchUpdateTypeDto input)
        {
            var orgs = (await _organizationMananer.Where(s => input.OrganizationIds.Contains(s.Id))).ToList();
            foreach (var item in orgs)
            {
                item.TypeId = input.TypeId;
            }
            await _organizationMananer.UpdateRangesAsync(orgs);
            return true;
        }

        //判断是否存在该名称组织机构
        private async Task<bool> CheckSameName(string name, Guid? id, Guid? parentId)
        {
            var sameOrgs = await _organizationMananer.Where(o => o.Name == name);
            if (parentId != null && parentId != Guid.Empty)
            {
                sameOrgs = sameOrgs.Where(x => x.ParentId == parentId);
            }
            else
            {
                sameOrgs = sameOrgs.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }
            if (id.HasValue)
            {
                sameOrgs = sameOrgs.Where(o => o.Id != id.Value);
            }
            if (sameOrgs.Any())
            {
                throw new UserFriendlyException("当前机构下已存在相同名称的机构！！！");
            }
            return true;
        }

        //判断是否存在相同编码的组织机构
        private async Task<bool> CheckSameCSRGCode(string CSRGCode, Guid? id)
        {
            var sameOrgs = await _organizationMananer.Where(o => string.Equals(o.CSRGCode, CSRGCode, StringComparison.CurrentCultureIgnoreCase));
            if (id.HasValue)
            {
                sameOrgs = sameOrgs.Where(o => o.Id != id.Value);
            }
            if (sameOrgs.Any())
            {
                throw new UserFriendlyException("已存在相同CSRGCode编码的组织机构！！！");
            }
            return true;
        }

        private async Task<string> CreateNewOrgCode(Guid? parentId)
        {
            Organization parent = null;
            var parentCode = "";
            if (parentId.HasValue)
            {
                parent = await _organizationMananer.GetAsync(parentId.Value);
                parentCode = parent != null ? parent.Code : "";
            }
            var orgList = (await _organizationMananer.Where(a => true)).ToList();

            var orgChildren = orgList.FindAll(o => o.Code.Length == parentCode.Length + 4 && o.Code.Left(parentCode.Length) == parentCode);
            if (orgChildren.Count > 0)
            {
                var lastOrg = orgChildren.OrderBy<Organization, string>(o => o.Code).Last();
                return parentCode + (Convert.ToInt32(lastOrg.Code.Right(4)) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                return parentCode + "0001";
            }
        }


        /// <summary>
        /// 组织机构导入
        /// </summary>
        /// <param name="CSRGCode"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Organization.Import)]
        public async Task Import([FromForm] ImportData input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 0);
            const int rowIndex = 5;
            IWorkbook workbook = null;
            ISheet sheet = null;
            var dataList = new List<OrganizationImportTemplate>();
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0)
               .CheckColumnAccordTempleModel<OrganizationImportTemplate>(rowIndex);
                dataList = sheet.TryTransToList<OrganizationImportTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception e)
            {
                await _fileImport.RemoveCache(input.ImportKey);
                await _fileImport.UpdateState(input.ImportKey, 0);
                throw new UserFriendlyException(e.Message);
            }

            if (dataList == null)
            {
                await _fileImport.RemoveCache(input.ImportKey);
                await _fileImport.UpdateState(input.ImportKey, 0);
                throw new UserFriendlyException("未找到任何数据,请检查文件格式");
            }

            var wrongInfos = new List<WrongInfo>();
            var importOrganizations = new List<Organization>();// 根据表格数据组装的数据
            var updateOrganizations = new List<Organization>();// 组装过程中需要更新的数据
            List<Organization> currentOrganizations; // 数据库中已经存在的数据。
            using (DataFilter.Disable<ISoftDelete>())
            {
                currentOrganizations = (await _organizationMananer.Where(a => true)).ToList();
            }

            if (dataList.Any())
            {

                var CSRGCodeNullList = dataList.FindAll(x => x.CSRGCode.IsNullOrEmpty());
                if (CSRGCodeNullList.Count > 0) throw new UserFriendlyException("请检查是否存在编码为空的数据");
                // 组织数据
                var dataGroup = dataList.GroupBy(a => a.CSRGCode.Length).ToList();
                await _fileImport.ChangeTotalCount(input.ImportKey, dataGroup.Count());
                for (var i = 0; i < dataGroup.Count(); i++)
                {
                    await _fileImport.UpdateState(input.ImportKey, i);
                    var list = dataGroup[i].ToList();
                    if (!list.Any())
                    {
                        continue;
                    }
                    var count = rowIndex;
                    foreach (var item in list)
                    {
                        var index = dataList.FindIndex(item);
                        var newInfo = new WrongInfo(index + rowIndex);
                        count++;
                        var canInsert = true;
                        if (item.CSRGCode.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"编码数据不能为空");
                        }
                        if (item.Name.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"名称数据不能为空");
                        }
                        if ((GetNameLength(item.Name).Length == 1 && item.CSRGCode.Length != 2) || (GetNameLength(item.Name).Length == 2 && item.CSRGCode.Length != 5)
                            || (GetNameLength(item.Name).Length == 3 && item.CSRGCode.Length != 8) || (GetNameLength(item.Name).Length == 4 && item.CSRGCode.Length != 10))
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"编码与名称级数不对应");
                        }
                        if (!canInsert)
                        {
                            wrongInfos.Add(newInfo);
                            continue;
                        }

                        //判断GsrCode 是否存在数据库，存在则不添加，需要更新
                        var hasOrganization = currentOrganizations.FirstOrDefault(a => a.CSRGCode == item.CSRGCode);
                        if (hasOrganization != null)
                        {
                            newInfo.AppendInfo($"【{item.CSRGCode}】已存在数据库");
                            hasOrganization.IsDeleted = false;
                            hasOrganization.Name = GetOrganizationName(hasOrganization.Code, item.Name);
                            updateOrganizations.Add(hasOrganization);
                            wrongInfos.Add(newInfo);
                            continue;
                        }

                        // 组织机构逻辑处理，当导入的表格内容是从一级往后的数据，需要重新判断处理


                        var parentGsrCode = GetParentGsrCode(i, item.CSRGCode);
                        var parentId = GetParentGuidByGsrCode(importOrganizations, parentGsrCode, currentOrganizations, i);
                        var organization = new Organization();
                        organization.SetId(GuidGenerator.Create());
                        var codeine = GetNextChildCodeAsync(importOrganizations, parentId, currentOrganizations);
                        // 判断新的code 是否存在数据库中，是则重新生成
                        var sameCodeOrganization = currentOrganizations.FirstOrDefault(a => a.Code == codeine);
                        if (sameCodeOrganization != null)
                        {
                            codeine = CalculateNextCode(codeine);
                        }
                        organization.CSRGCode = item.CSRGCode;
                        organization.Nature = item.Nature;
                        organization.Code = codeine;
                        organization.Name = GetOrganizationName(codeine, item.Name);
                        organization.ParentId = parentId;
                        importOrganizations.Add(organization);
                    }
                }

                // 数据组装完成，执行其他的操作
                //1、更新已将存在数据，批量更新操作
                if (updateOrganizations.Any())
                {
                    await _organizationMananer.UpdateRangesAsync(updateOrganizations);
                }
                //2、对组装后的数据数据分组批量添加
                if (importOrganizations.Any())
                {
                    var lastGroup = importOrganizations.GroupBy(a => a.Code.Split('.').Length);
                    foreach (var item in lastGroup)
                    {
                        await _organizationMananer.InsertRangesAsync(item.ToList());
                    }
                }
                // 3、完成导入
                await _fileImport.Complete(input.ImportKey);
            }
            if (wrongInfos.Any())
            {
                sheet.CreateInfoColumn(wrongInfos);
                await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
            }
        }

        [Produces("application/octet-stream")]
        [Authorize(IdentityPermissions.Organization.Export)]
        [UnitOfWork]
        public async Task<Stream> Export(OrganizationData input)
        {
            using (var uow = UntiOfWorkManager.Begin(true, false))
            {
                // 具体的业务实现，下面为实例，仅供参考
                //IReadOnlyCollection<Organization> list = await _organizationRepository.GetListAsync();
                var orgList = await _organizationRepository.GetListAsync();
                var list = GetOrganizationsData(input.Paramter);
                list = list.OrderBy(a => a.Code.Split(".").Length).ToList();
                var organizations = new List<OrganizationModel>();
                var organization = new OrganizationModel();
                foreach (var item in list)
                {
                    organization = new OrganizationModel();
                    if (item.ParentId != null && item.ParentId != Guid.Empty)
                    {
                        organization.Name = item.Name;
                        DgGetToOrganizationName(organization, item, orgList); //递归获取组织机构名称
                    }
                    else
                    {
                        organization.Name = item.Name;
                    }
                    organization.CSRGCode = item.CSRGCode;
                    organization.Nature = item.Nature;
                    organizations.Add(organization);
                }

                var orgImportTemplates = organizations.OrderBy(x => x.CSRGCode).ToList();
                var stream = ExcelHelper.ExcelExportStream(orgImportTemplates, input.TemplateKey, input.RowIndex);
                return stream;
            }
        }


        #region 计算组织机构code 私有方法

        private void DgGetToOrganizationName(OrganizationModel organization, Organization item, List<Organization> orgList)
        {
            var org = orgList.FirstOrDefault(a => a.Id == item.ParentId);
            organization.Name = org.Name + "-" + organization.Name;
            if (org.ParentId != null && org.ParentId != Guid.Empty)
            {
                DgGetToOrganizationName(organization, org, orgList);
            }
            else
            {
                organization.Name = organization.Name.TrimStart('-');
            }


        }

        private List<Organization> GetOrganizationsData(OrganizationGetListDto input)
        {
            // 新增条件过滤
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var organizationCode = organization != null ? organization.Code : null;

            var list = new List<Organization>();
            var dtos = new List<OrganizationDto>();

            //根据关键字查询
            if (!string.IsNullOrEmpty(input.KeyWords))
            {
                var query = _organizationRepository
                    .WithDetails();

                if (!string.IsNullOrEmpty(input.KeyWords))
                {
                    if (input.isTreeSearch)
                    {
                        query = query.Where(x => x.Name.Contains(input.KeyWords)).Take(200);
                    }
                    else
                    {
                        query = query.Where(x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords) || x.CSRGCode.Contains(input.KeyWords));
                    }

                }

                if (organization != null)
                {
                    query = query.Where(x => x.Code.StartsWith(organization.Code));
                }

                list = query.ToList();
                dtos = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(list);
                foreach (var item in dtos)
                {
                    item.Children = null;
                }
            }
            else
            {
                var query = _organizationRepository
                     .WithDetails()
                     .Where(x => x.Id == organization.Id || x.Code.StartsWith(organizationCode)).ToList();

                dtos = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(query);
            }

            var result = ObjectMapper.Map<List<OrganizationDto>, List<Organization>>(dtos);
            return result;
        }

        private static string GetParentGsrCode(int index, string code)
        {
            if (code.Length == 2)
            {
                return string.Empty;
            }
            else
            {
                return code.Length == 10 ? code.Substring(0, code.Length - 2) : code.Substring(0, code.Length - 3);
            }
            //// 判断code 长度
            //if (index == 0)
            //{
            //    if (code.Length != 2)
            //    {
            //        //不是完整的数据

            //    }
            //    else
            //    {
            //        return string.Empty;
            //    }
            //}
            //else
            //{
            //    return code.Substring(0, length: 2 + 3 * (index - 1));

            //}
        }

        private static Guid? GetParentGuidByGsrCode(IEnumerable<Organization> list, string code,
            IEnumerable<Organization> currentList, int index)
        {
            var id = currentList.FirstOrDefault(a => a.CSRGCode == code)?.Id;
            return id ?? list.FirstOrDefault(a => a.CSRGCode == code)?.Id;
        }

        private static string GetOrganizationName(string code, string name) => name.Replace("GSM-R", "GSMR").Split('-')[code.Split('.').Length - 1].Replace("GSMR", "GSM-R");

        private static string[] GetNameLength(string name) => name.Replace("GSM-R", "GSMR").Split('-');

        private static string GetNextChildCodeAsync(List<Organization> Organizations, Guid? parentId, List<Organization> currentList)
        {
            var lastChild = GetLastChildOrNullAsync(Organizations, parentId, currentList);
            if (lastChild != null)
            {
                return Organization.CalculateNextCode(lastChild.Code);
            }

            var parentCode = parentId != null
                ? GetCodeOrDefaultAsync(Organizations, parentId.Value, currentList)
                : null;

            return Organization.AppendCode(
                parentCode,
                Organization.CreateCode(1)
            );
        }

        private static string AppendCode(string parentCode, string childCode)
        {
            if (string.IsNullOrEmpty(childCode))
            {
                throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
            }
            if (string.IsNullOrEmpty(parentCode))
            {
                return childCode;
            }
            return parentCode + "." + childCode;
        }

        private static string CreateCode(params int[] numbers)
        {
            if (!numbers.Any())
            {
                return null;
            }
            var aa = numbers.Select(number => number.ToString(new string('0', 3)));
            return string.Join(".", aa);
        }

        private static Organization GetLastChildOrNullAsync(List<Organization> Organizations, Guid? pid, List<Organization> currentList)
        {
            var organization = currentList.Where(a => a.ParentId == pid);
            var organizations = organization as Organization[] ?? organization.ToArray();
            if (organizations.Any())
            {
                return organizations.OrderBy(c => c.Code).LastOrDefault();
            }
            else
            {
                var children = Organizations.Where(a => a.ParentId == pid);
                return children.OrderBy(c => c.Code).LastOrDefault();
            }
        }

        private static string CalculateNextCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);
            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        private static string GetLastUnitCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }
            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        private static string GetParentCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }
            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return string.Join(".", splittedCode.Take(splittedCode.Length - 1));
        }

        private static string GetCodeOrDefaultAsync(List<Organization> Organizations, Guid id, List<Organization> currentList)
        {
            var code = currentList.FirstOrDefault(a => a.Id == id)?.Code;
            if (!code.IsNullOrEmpty())
            {
                return code;
            }
            else
            {
                var data = Organizations.FirstOrDefault(a => a.Id == id);
                return data?.Code;
            }

        }

        private string GetParentOrganizationCode(string code)
        {
            return code.Split(".").Take(code.Split(".").Length - 1).JoinAsString(".");
        }

        #endregion

    }
}