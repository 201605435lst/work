using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Atp;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.Utils;
using SnAbp.Utils.DataImport;
using SnAbp.Utils.ExcelHelper;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Dtos;
using Volo.Abp.Identity.Repository;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Uow;


namespace SnAbp.Identity
{
    [Authorize]
    public class AppUserAppService : IdentityAppServiceBase, IIdentityUserAppService
    {
        protected IdentityUserManager UserManager { get; }
        protected IdentityRoleManager RoleManager { get; }
        protected OrganizationManager OrganizationRepository { get; }
        protected IDataDictionaryRepository DataDictionaryRepository { get; }
        public IRepository<IdentityUserRltProject, Guid> UserRltRepository { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected IOrganizationRepository IOrganizationRepository { get; }
        protected IIdentityUserRltOrganizationRepository IdentityUserRltOrganizationRepository;
        public IIdentityRoleRepository RoleRepository { get; }
        public IIdentityUserRoleRepository UserRoleRepository { get; }


        private IOrganizationRltRoleRepository _organizationRltRoleRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private IdentityUserManager _identityUserManager;
        private readonly IFileImportHandler _fileImport;
        private readonly IUnitOfWorkManager _unitOfWork;
        //private readonly IRepository<IdentityUserRltProject, Guid> userRltRepository;

        public AppUserAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IOrganizationRepository iOrganizationRepository,
            IIdentityUserRoleRepository userRoleRepository,
            IOrganizationRltRoleRepository organizationRltRoleRepository,
            IIdentityRoleRepository roleRepository,
           IIdentityUserRltOrganizationRepository identityUserRltOrganizationRepository,
            IHttpContextAccessor httpContextAccessor,
            IdentityUserManager identityUserManager,
            OrganizationManager organizationRepository,
            IFileImportHandler fileImport,
            IdentityRoleManager roleManager,
            IDataDictionaryRepository dataDictionaryRepository,
            IUnitOfWorkManager unitOfWork
          , IRepository<IdentityUserRltProject, Guid> _userRltRepository
            )
        {
            UserManager = userManager;
            UserRepository = userRepository;
            IOrganizationRepository = iOrganizationRepository;
            IdentityUserRltOrganizationRepository = identityUserRltOrganizationRepository;
            RoleRepository = roleRepository;
            UserRoleRepository = userRoleRepository;
            _organizationRltRoleRepository = organizationRltRoleRepository;
            _httpContextAccessor = httpContextAccessor;
            _identityUserManager = identityUserManager;
            OrganizationRepository = organizationRepository;
            _fileImport = fileImport;
            RoleManager = roleManager;
            DataDictionaryRepository = dataDictionaryRepository;
            _unitOfWork = unitOfWork;
            UserRltRepository = _userRltRepository;
        }

        //TODO: [Authorize(IdentityPermissions.Users.Default)] should go the IdentityUserAppService class.
        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<IdentityUserDto> GetAsync(Guid id)
        {
            var user = await UserManager.GetByIdAsync(id);
            var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
            if (userDto.PositionId != null)
            {
                var position = DataDictionaryRepository.FirstOrDefault(x => x.Id == userDto.PositionId);
                if (position == null) userDto.PositionId = null;
            }
            // 查询当前用户的项目信息

            var project = UserRltRepository.Where(a => a.UserId == userDto.Id);
            if (project.Any())
            {
                userDto.ProjectIds = project.Select(a => a.ProjectTagId.Value).ToList();
            }
            return userDto;
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            var count = await UserRepository.GetCountAsync(input.Filter);

            var list = new List<IdentityUser>();
            var isSystemUser = await _identityUserManager.isSystem(CurrentUser.Id.Value);
            //查询全部组织机构用户
            if (input.IsAllOrganization)
            {
                list = await UserRepository.GetUserList(x =>
                      x.Name.Contains(input.Filter) ||
                      x.UserName.Contains(input.Filter), true);

                var identityUserDto = input.IsAll ?
                    ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list ?? new List<IdentityUser>())
                    : ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList() ?? new List<IdentityUser>());

                foreach (var item in identityUserDto)
                {
                    var organization = IdentityUserRltOrganizationRepository.Where(x => x.UserId == item.Id).ToList();
                    item.Organizations = organization;
                }

                return new PagedResultDto<IdentityUserDto>(list?.Count ?? 0, identityUserDto);
            }
            else
            {
                // 根据查询调件得到的结果
                var query = UserRepository.WhereIf(
                         !input.Filter.IsNullOrWhiteSpace(),
                         u =>
                             u.UserName.Contains(input.Filter) ||
                             u.Email.Contains(input.Filter) ||
                             (u.Name != null && u.Name.Contains(input.Filter)) ||
                             (u.Surname != null && u.Surname.Contains(input.Filter)) ||
                             (u.PhoneNumber != null && u.PhoneNumber.Contains(input.Filter))
                     ).OrderBy(input.Sorting ?? nameof(IdentityUser.UserName));

                if (!input.ExcludeOrganizationId.HasValue)
                {
                    //1、获得组织机构的用户id
                    var userAltOrganizationIds = IdentityUserRltOrganizationRepository
                        .WhereIf(input.OrganizationId != null && input.OrganizationId != Guid.Empty, x => x.OrganizationId == input.OrganizationId)
                        .Select(u => u.UserId).ToList();
                    // 如果是系统用户，组织机构为空,在用户仓储里面排除属于组织机构的用户
                    if (isSystemUser && !input.OrganizationId.HasValue)
                    {
                        list = query.Where(a => !userAltOrganizationIds.Contains(a.Id)).ToList();
                    }
                    else
                    {
                        if (!isSystemUser && !input.OrganizationId.HasValue)
                        {
                            list = new List<IdentityUser>();
                        }
                        else
                        {
                            // 组织机构里面的用户
                            list = query.Where(a => userAltOrganizationIds.Contains(a.Id)).ToList();
                        }
                    }
                }
                else
                {
                    // 分析：如果是系统用户，则可以获取除系统用户外和不包括当前组织机构的用户
                    if (isSystemUser && input.ExcludeOrganizationId.HasValue)
                    {
                        //在IdentityUserRltOrganizationRepository仓储里面获得当前组织机构用户的id
                        var organizationUserIds = IdentityUserRltOrganizationRepository
                            .WhereIf(input.ExcludeOrganizationId != null && input.ExcludeOrganizationId != Guid.Empty, x => x.OrganizationId == input.ExcludeOrganizationId)
                            .Select(u => u.UserId).ToList();

                        //获取系统用户的id
                        // 1、获取组织机构的所有角色
                        var orgRltroleIds = _organizationRltRoleRepository.Select(x => x.RoleId).ToList();
                        // 2、获取系统角色id:=========分析：系统角色不包括系统公开角色=======（获取除组织机构外角色和系统默认角色）
                        var roleIds = RoleRepository.Where(x => !orgRltroleIds.Contains(x.Id) && !x.IsDefault && !x.IsPublic).Select(x => x.Id).ToList();
                        //3、获取系统用户的id
                        var userRoleIds = UserRoleRepository.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.UserId).ToList();
                        // 4、得到当前组织机构用户的id和系统用户的id
                        organizationUserIds.AddRange(userRoleIds);
                        // 5、在用户仓库里面拿取不属于第四步的数据
                        list = query.Where(a => !organizationUserIds.Contains(a.Id)).ToList();

                    }
                    //如果不是系统用户，则可以获取除系统用户外的他的下级组织机构人员
                    else
                    {
                        //1、获取该组织机构的code
                        // 父级组织机构
                        var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
                        var organizationIdParent = !string.IsNullOrEmpty(organizationIdString) ? IOrganizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
                        ////当前组织机构
                        //var organizationIdChild = IOrganizationRepository
                        //    .WhereIf(input.ExcludeOrganizationId != null && input.ExcludeOrganizationId != Guid.Empty, x => x.Id == input.ExcludeOrganizationId).FirstOrDefault();
                        var code = organizationIdParent?.Code;
                        //2、根据code获取属于所有的子级组织机构的id.
                        var _orgIds = IOrganizationRepository.WhereIf(!string.IsNullOrEmpty(code), x => x.Code.StartsWith(code)).Select(p => p.Id).ToList();
                        ////3、获得所有属于子级组织机构的用户id
                        var organizationAltUserIds = IdentityUserRltOrganizationRepository.Where(x => _orgIds.Contains(x.OrganizationId)).Select(u => u.UserId).ToList();
                        // 4、获得所有属于子级组织机构的id
                        var listsUser = query.Where(a => organizationAltUserIds.Contains(a.Id)).ToList();
                        //5、在IdentityUserRltOrganizationRepository仓储里面获得当前组织机构用户的id
                        var organizationUserIds = IdentityUserRltOrganizationRepository.WhereIf(input.ExcludeOrganizationId != null && input.ExcludeOrganizationId != Guid.Empty, x => x.OrganizationId == input.ExcludeOrganizationId).Select(u => u.UserId).ToList();
                        //6、排除当前组织机构的用户
                        list = listsUser.Where(x => !organizationUserIds.Contains(x.Id)).ToList();
                        //7、排除自己
                        var currentUser = await UserManager.GetByIdAsync(CurrentUser.Id.GetValueOrDefault());
                        list = list.WhereIf(currentUser != null, x => x.Id != currentUser.Id).ToList();
                    }
                }
            }

            var result = input.IsAll ? new PagedResultDto<IdentityUserDto>(list?.Count ?? 0, ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list ?? new List<IdentityUser>()))
                                   : new PagedResultDto<IdentityUserDto>(list?.Count ?? 0, ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList() ?? new List<IdentityUser>()));
            if (!input.IsAll)
            {
                foreach (var item in result.Items)
                {
                    var roleIds = UserRoleRepository.Where(x => x.UserId == item.Id).Select(y => y.RoleId).ToList();
                    item.Roles = ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(RoleRepository.Where(x => roleIds.Contains(x.Id) && !x.IsDefault).ToList());
                }

            }

            return result;

        }

        /// <summary>
        /// 获取一个用户的角色列表
        /// </summary>
        /// <param name="id"></param> 用户 Id
        /// <param name="organizationId"></param> 组织机构Id
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, Guid? organizationId)
        {
            //var roles = await UserRepository.GetRolesAsync(id);

            var roles = new List<IdentityRole>();

            // 获取这个用户在指定组织机构的角色
            if (organizationId.HasValue)
            {
                //该用户的所有角色Id
                var userRltRolesIds = UserRoleRepository.Where(x => x.UserId == id).Select(x => x.RoleId).ToList();
                // 获取该组织的所有角色
                var organizationRltRolesIds = _organizationRltRoleRepository.Where(x => x.OrganizationId == organizationId).Select(x => x.RoleId).ToList();

                // 获取用户在该组织的角色(不包含共享角色)
                var rolesIds = userRltRolesIds.Where(x => organizationRltRolesIds.Contains(x)).ToList();
                var orgRoles = RoleRepository.Where(x => rolesIds.Contains(x.Id)).ToList();
                // 获取共享角色
                var publicRolds = RoleRepository.Where(x => userRltRolesIds.Contains(x.Id) && x.IsPublic).ToList();
                //获取组织机构角色和共享角色
                foreach (var item in publicRolds)
                {
                    orgRoles.Add(item);
                }
                roles = orgRoles.Distinct().ToList();

            }
            // 认为为系统用户，该用户的系统角色
            else
            {
                // 获取所有组织机构角色Id
                var organizationRoleIds = (await _organizationRltRoleRepository.GetListAsync()).Select(x => x.RoleId).ToList();


                //获取不属于组织机构的该用户的所有角色Id
                var userRltRolesIds = UserRoleRepository.Where(x => x.UserId == id && !organizationRoleIds.Contains(x.RoleId)).Select(x => x.RoleId).ToList();


                // 获取不属于组织机构的角色(排除默认角色)
                roles = RoleRepository.Where(x => userRltRolesIds.Contains(x.Id) && !x.IsDefault).ToList();
            }

            return new ListResultDto<IdentityRoleDto>(ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles));
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
        {
            var list = await RoleRepository.GetListAsync();
            return new ListResultDto<IdentityRoleDto>(
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list));
        }

        public async Task<List<string>> GetUserPermissions()
        {
            var organizationId = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var list = new List<string>();
            if (string.IsNullOrEmpty(organizationId))
            {
                list = await _identityUserManager.GetUserPermissions(CurrentUser.Id.Value, null);
            }
            else
            {
                list = await _identityUserManager.GetUserPermissions(CurrentUser.Id.Value, new Guid(organizationId));
            }

            return list;
        }

        [Authorize(IdentityPermissions.Users.Create)]
        public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var email = string.IsNullOrEmpty(input.Email) ? $"{input.Name}@snabp.com" : input.Email;
            var user = new IdentityUser(
                GuidGenerator.Create(),
                input.UserName,
                email,
                CurrentTenant.Id,
                input.PositionId
            );

            if (input.OrganizationId.HasValue)
            {
                user.AddOrganization(input.OrganizationId.GetValueOrDefault());  //给指定组织机构添加角色
            }
            input.MapExtraPropertiesTo(user);

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
            await UpdateUserByInput(user, input);
            // 创建用户角色的关联
            await SaveUseRoleByInput(user.Id, input.RoleIds);
            await CurrentUnitOfWork.SaveChangesAsync();

            // 新增多项目 项目id判断，如果存在项目id，则需要关联一下项目
            if (input.ProjectTagId.HasValue)
            {
                var rlt = new IdentityUserRltProject(user.Id, input.ProjectTagId.Value);
                await UserManager.CreateUserProject(rlt);
            }

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        private async Task SaveUseRoleByInput(Guid userGuid, IEnumerable<Guid> roleIds)
        {
            if (roleIds != null && roleIds.Any())
            {
                var hasRoles = await UserRoleRepository.GetRoleIdsByUser(userGuid);
                if (hasRoles != null && hasRoles.Any())
                {
                    // 删除这个关联
                    await UserRoleRepository.DeleteAsync(a => a.UserId == userGuid, CancellationToken.None);
                }
                foreach (var roleId in roleIds)
                {
                    var userRole = new IdentityUserRltRole(userGuid, roleId);
                    await UserRoleRepository.InsertAsync(userRole);
                }
            }
            else
            {
                // 判断是否有默认角色
                var defaultRole = RoleRepository.FirstOrDefault(a => a.IsDefault);
                if (defaultRole != null)
                {
                    var userDefaultRole = UserRoleRepository.Where(x => x.RoleId == defaultRole.Id && x.UserId == userGuid).FirstOrDefault();
                    if (userDefaultRole == null)
                    {
                        await UserRoleRepository.InsertAsync(new IdentityUserRltRole(userGuid, defaultRole.Id));
                    }
                }
            }
        }

        public virtual async Task<IdentityUserDto> FindByUsernameAsync(string username)
        {
            var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(await UserManager.FindByNameAsync(username));
            if (userDto != null)
            {
                var organizations = IdentityUserRltOrganizationRepository.Where(x => x.UserId == userDto.Id).ToList();
                if (organizations.Count > 0)
                {
                    userDto.Organizations = organizations;
                    foreach (var item in userDto.Organizations)
                    {
                        var organization = await OrganizationRepository.GetAsync(item.OrganizationId);
                        if (organization != null) item.Organization = organization;
                    }
                }

                // 查询当前用户的项目信息
                var project = UserRltRepository.Where(a => a.UserId == userDto.Id);
                if (project.Any())
                {
                    userDto.ProjectIds = project.Select(a => a.ProjectTagId.Value).ToList();
                }
            }
            return userDto;
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public virtual async Task<IdentityUserDto> FindByEmailAsync(string email)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await UserManager.FindByEmailAsync(email)
            );
        }

        protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            }

            (await UserManager.SetTwoFactorEnabledAsync(user, input.TwoFactorEnabled)).CheckErrors();
            (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

            user.Name = input.Name;
            user.Surname = input.Surname;
            user.PositionId = input.PositionId;

            if (input.RoleIds != null)
            {
                (await UserManager.SetRolesAsync(user, input.RoleIds)).CheckErrors();
            }
        }

        /// <summary>
        /// 用户信息配置
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<bool> SetUserInfo(IdentityUserSetDto input)
        {
            if (!input.UserAndPositions.Any())
            {
                return true;
            }

            foreach (var item in input.UserAndPositions)
            {
                var user = await UserManager.GetByIdAsync(item.UserId.GetValueOrDefault());
                if (user == null)
                {
                    continue;
                }

                user.PositionId = item.PositionId;
                user.AddOrganization(input.OrganizationId);
                await UserManager.UpdateAsync(user);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// 用户批量导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> Upload([FromForm] ImportData input)
        {
            var workbook = input.File.ConvertToWorkbook();
            const int rowIndex = 2;
            var wrongInfos = new List<WrongInfo>();
            ISheet sheet = null;
            List<UserImportTemplate> dataList = new List<UserImportTemplate>();
            try
            {
                sheet = workbook.GetSheetAt(0)
                   .CheckColumnAccordTempleModel<UserImportTemplate>(rowIndex);
                dataList = sheet.TryTransToList<UserImportTemplate>(rowIndex)
                   .CheckNull();
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            var addCount = 0;
            var updateCount = 0;
            if (!dataList.Any())
            {
                return string.Format($"成功导入{addCount}条用户数据，更新{updateCount}条数据。");
            }
            await _fileImport.Start(input.ImportKey, dataList.Count);
            using var uow = _unitOfWork.Begin(true, false);
            foreach (var item in dataList)
            {
                await _fileImport.UpdateState(input.ImportKey, dataList.FindIndex(item));
                var newInfo = new WrongInfo(dataList.FindIndex(item) + rowIndex);
                var canInsert = true;
                if (item.OrgCode.IsNullOrEmpty() && item.OrgName.IsNullOrEmpty())
                {
                    canInsert = false; newInfo.AppendInfo($"OrgCode或OrgName不能为空");
                }
                //if (item.OrgName.IsNullOrEmpty())
                //{
                //    canInsert = false; newInfo.AppendInfo($"OrgName不能为空");
                //}
                if (item.Account.IsNullOrEmpty())
                {
                    canInsert = false; newInfo.AppendInfo($"Account不能为空");
                }
                if (item.Name.IsNullOrEmpty())
                {
                    canInsert = false; newInfo.AppendInfo($"Name不能为空");
                }
                if (item.Email.IsNullOrEmpty())
                {
                    item.Email = $"{item.Account}@SnAbp.com";
                }
                // 判断组织机构是否存在
                var codeOrganization = await OrganizationRepository.GetAsync(a => a.CSRGCode == item.OrgCode);
                var nameOrganization = await OrganizationRepository.GetAsync(a => a.Name == item.OrgName);
                if (codeOrganization == null && nameOrganization == null)
                {
                    canInsert = false;
                    newInfo.AppendInfo($"{item.OrgCode}{item.OrgName}的组织机构不存在");
                }
                Guid orgGuid = codeOrganization != null ? codeOrganization.Id : nameOrganization.Id;
                if (item.Password.IsNullOrEmpty())
                {
                    item.Password = "123456";
                }
                else if (item.Password.Length < 6)
                {
                    canInsert = false;
                    newInfo.AppendInfo($"密码：{item.Password}长度应大于6位");
                }
                if (!canInsert)
                {
                    wrongInfos.Add(newInfo);
                    continue;
                }

                // 用户存在，更新其值
                var user = await UserManager.GetUserByUserName(item.Account);
                try
                {
                    if (user != null)
                    {
                        (await UserManager.SetUserNameAsync(user, item.Account)).CheckErrors();
                        if (!string.Equals(user.Email, item.Email, StringComparison.InvariantCultureIgnoreCase))
                        {
                            (await UserManager.SetEmailAsync(user, item.Email)).CheckErrors();
                        }
                        if (!string.Equals(user.PhoneNumber, item.PhoneNum, StringComparison.InvariantCultureIgnoreCase))
                        {
                            (await UserManager.SetPhoneNumberAsync(user, item.PhoneNum)).CheckErrors();
                        }
                        if (user.Organizations.FirstOrDefault(s => s.OrganizationId == orgGuid) == null)
                        {
                            user.AddOrganization(orgGuid);
                        }
                        user.Name = item.Name;
                        (await UserManager.UpdateAsync(user)).CheckErrors();
                        (await UserManager.RemovePasswordAsync(user)).CheckErrors();
                        (await UserManager.AddPasswordAsync(user, item.Password)).CheckErrors();
                        //await CurrentUnitOfWork.SaveChangesAsync();
                        updateCount++;
                    }
                    else
                    {
                        user = new IdentityUser(GuidGenerator.Create(), item.Account, item.Email) { Name = item.Name };
                        user.AddOrganization(orgGuid);
                        // 给用户添加一个默认的角色
                        var role = RoleRepository.FirstOrDefault(a => a.IsDefault);
                        if (role != null)
                        {
                            user.AddRole(role.Id);
                        }
                        (await UserManager.CreateAsync(user, item.Password)).CheckErrors();
                        (await UserManager.SetPhoneNumberAsync(user, item.PhoneNum)).CheckErrors();
                        //await CurrentUnitOfWork.SaveChangesAsync();
                        addCount++;
                    }
                }
                catch (Exception e)
                {
                    newInfo.AppendInfo(e.Message);
                    wrongInfos.Add(newInfo);
                }
            }
            await uow.SaveChangesAsync();
            if (wrongInfos.Any())
            {
                sheet.CreateInfoColumn(wrongInfos);
                await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey,
                    fileBytes: workbook.ConvertToBytes());
            }
            await _fileImport.Complete(input.ImportKey);
            return string.Format($"成功导入{addCount}条用户数据，更新{updateCount}条数据。");
        }
        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await UserManager.GetByIdAsync(id);
            user.ConcurrencyStamp = input.ConcurrencyStamp;

            (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();

            await UpdateUserByInput(user, input);
            input.MapExtraPropertiesTo(user);

            (await UserManager.UpdateAsync(user)).CheckErrors();

            if (!input.Password.IsNullOrEmpty())
            {
                (await UserManager.RemovePasswordAsync(user)).CheckErrors();
                (await UserManager.AddPasswordAsync(user, input.Password)).CheckErrors();
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(IdentityPermissions.Users.Update)]
        public virtual async Task UpdateRolesAsync(IdentityUserUpdateRolesDto input)
        {
            //var user = await UserManager.GetByIdAsync(id);
            //if (user != null)
            //{
            //    await SaveUseRoleByInput(user.Id, input.RoleIds);
            //}

            //(await UserManager.SetRolesAsync(user, input.RoleIds)).CheckErrors();
            //await UserRepository.UpdateAsync(user);

            // 当前范围内的角色
            var allRoleIds = new List<Guid>();

            // 更新指定组织机构的角色
            if (input.OrganizationId.HasValue)
            {
                // 获取该组织机构的所有角色
                allRoleIds.AddRange(_organizationRltRoleRepository.Where(x => x.OrganizationId == input.OrganizationId).Select(x => x.RoleId).ToList());
                // 获取共享角色
                allRoleIds.AddRange(RoleRepository.Where(x => x.IsPublic).Select(x => x.Id).ToList());
                // 排除未改变的角色
                allRoleIds = allRoleIds.Distinct().ToList();
                // 清除该用户的系统角色关系
                await UserRoleRepository.DeleteAsync(x => input.UserIds.Contains(x.UserId) && allRoleIds.Contains(x.RoleId), CancellationToken.None);
            }
            // 更新系统角色
            else
            {
                // 获取所有组织机构角色Id
                var organizationRoleIds = (await _organizationRltRoleRepository.GetListAsync()).Select(x => x.RoleId).ToList();
                // 获取不属于组织机构的角色(排除默认角色)
                var systemRoleIds = RoleRepository.Where(x => !organizationRoleIds.Contains(x.Id) && !x.IsDefault).Select(x => x.Id).ToList();
                // 清除该用户的系统角色关系
                await UserRoleRepository.DeleteAsync(x => input.UserIds.Contains(x.UserId) && systemRoleIds.Contains(x.RoleId), CancellationToken.None);
            }


            foreach (var roleId in input.RoleIds)
            {
                foreach (var uId in input.UserIds)
                {
                    var userRole = new IdentityUserRltRole(uId, roleId);
                    await UserRoleRepository.InsertAsync(userRole);
                }

            }

            return;
        }

        /// <summary>
        /// 用用户修改自己的密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [Authorize(IdentityPermissions.Users.Default)]
        public async Task<bool> UpdatePassword(string oldPassword, string newPassword)
        {
            if (CurrentUser.Id == null)
            {
                throw new UserFriendlyException("当前用户异常，无法执行");
            }

            if (newPassword.IsNullOrEmpty())
            {
                throw new UserFriendlyException("新密码不能为空!");
            }

            var user = await UserManager.GetByIdAsync(CurrentUser.Id.GetValueOrDefault());

            if (!await UserManager.CheckPasswordAsync(user, oldPassword))
            {
                throw new UserFriendlyException("原密码输入有误!");
            }

            await UserManager.ResetPasswordAsync(user, await UserManager.GeneratePasswordResetTokenAsync(user),
                newPassword);

            user.IsChangePassword = true;
            await UserRepository.UpdateAsync(user);

            return true;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Remove(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请选择用户");
            //分析：如果当前用户是系统用户，则不能删除
            // 1、得到当前用户的角色id
            var roleId = UserRoleRepository.Where(x => x.UserId == id).Select(x => x.RoleId).ToList();
            // 2、获取系统角色
            var role = RoleRepository.Where(x => roleId.Contains(x.Id) && !x.IsDefault && x.IsStatic);
            if (role.Any())
            {
                throw new UserFriendlyException("当前用户是系统用户，不能删除");
            }
            else
            {
                //用户不能删除自己
                if (id == CurrentUser.Id)
                {
                    throw new UserFriendlyException("不能删除当前登录用户");
                }
                else
                {
                    await UserManager.DeleteAsync(a => a.Id == id);
                }
            }
            return true;
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveFromOrganization(Guid id, Guid organizationId)
        {
            //用户不能移出自己
            if (id == CurrentUser.Id)
            {
                throw new UserFriendlyException("不能移出当前登录用户");
            }
            else
            {
                // 分析：
                // 1、移出当前组织机构的时候，第一步：需要删除这个用户在当前组织机构所拥有的角色
                // 1.1 获取当前组织机构的角色
                var organizationRoleId = _organizationRltRoleRepository
                 .Where(x => x.OrganizationId == organizationId).Select(x => x.RoleId).ToList();
                await UserRoleRepository.DeleteAsync(x => organizationRoleId.Contains(x.RoleId) && x.UserId == id, CancellationToken.None);
                // 2、第二步：将用户从当前组织机构移出
                await UserManager.RemoveFromOrganizationAsync(id, organizationId);
            }

            return true;
        }

        [Authorize(IdentityPermissions.Users.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            if (CurrentUser.Id == id)
            {
                throw new BusinessException(code: IdentityErrorCodes.UserSelfDeletion);
            }

            var user = await UserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return;
            }

            (await UserManager.DeleteAsync(user)).CheckErrors();
        }

        [Produces("application/octet-stream")]
        [Authorize(IdentityPermissions.Organization.Export)]
        [UnitOfWork]
        public async Task<Stream> Export(IdentityUserData input)
        {
            //var users = await UserRepository.GetListAsync();
            List<IdentityUser> users = await GetIdentityUsersDataAsync(input.Paramter);
            var importTemplates = new List<UserImportTemplate>();

            foreach (var ite in users)
            {
                var userImportTemplate = new UserImportTemplate
                {
                    Account = ite.UserName,
                    Name = ite.Name,
                    PhoneNum = ite.PhoneNumber,
                    Email = ite.Email
                };
                var organizationList = await UserManager.GetOrganizationsAsync(ite.Id);
                if (organizationList.Any())
                {
                    foreach (var organization in organizationList)
                    {
                        userImportTemplate.OrgCode = organization.CSRGCode;
                        userImportTemplate.OrgName = organization.Name;
                    }
                }
                importTemplates.Add(userImportTemplate);
            }
            var dtoList = ObjectMapper.Map<List<UserImportTemplate>, List<UserImportTemplate>>(importTemplates);
            var userImportTemplates = dtoList.OrderBy(x => x.OrgCode).ToList();
            var stream = ExcelHelper.ExcelExportStream(userImportTemplates, input.TemplateKey, input.RowIndex);
            return stream;
        }


        #region 私有方法

        private async Task<List<IdentityUser>> GetIdentityUsersDataAsync(GetIdentityUsersInput input)
        {
            var list = new List<IdentityUser>();
            var isSystemUser = await _identityUserManager.isSystem(CurrentUser.Id.Value);
            // 根据查询调件得到的结果
            var query = UserRepository.WhereIf(
                     !input.Filter.IsNullOrWhiteSpace(),
                     u =>
                         u.UserName.Contains(input.Filter) ||
                         u.Email.Contains(input.Filter) ||
                         (u.Name != null && u.Name.Contains(input.Filter)) ||
                         (u.Surname != null && u.Surname.Contains(input.Filter)) ||
                         (u.PhoneNumber != null && u.PhoneNumber.Contains(input.Filter))
                 );

            //1、ExcludeOrganizationId为空，获得组织机构的用户id
            var userAltOrganizationIds = IdentityUserRltOrganizationRepository
                .WhereIf(input.OrganizationId != null && input.OrganizationId != Guid.Empty, x => x.OrganizationId == input.OrganizationId)
                .Select(u => u.UserId).ToList();
            // 如果是系统用户，组织机构为空,在用户仓储里面排除属于组织机构的用户
            if (isSystemUser && !input.OrganizationId.HasValue)
            {
                list = query.Where(a => !userAltOrganizationIds.Contains(a.Id)).ToList();
            }
            else
            {
                if (!isSystemUser && !input.OrganizationId.HasValue)
                {
                    list = new List<IdentityUser>();
                }
                else
                {
                    // 组织机构里面的用户
                    list = query.Where(a => userAltOrganizationIds.Contains(a.Id)).ToList();
                }
            }
            return list;
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<List<IdentityUserDto>> GetListByIds(List<Guid> ids)
        {
            var user = UserRepository.Where(x => ids.Contains(x.Id)).ToList();
            return Task.FromResult(ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(user));
        }

        #endregion
    }
}