using Microsoft.AspNetCore.Mvc;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity.Dtos;

namespace SnAbp.Identity
{
    public interface IIdentityUserAppService : ICrudAppService<IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>
    {
        /// <summary>
        /// 获取一个用户的角色列表
        /// </summary>
        /// <param name="id"></param> 用户 Id
        /// <param name="organizationId"></param> 组织机构 Id
        /// <returns></returns>
        Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, Guid? organizationId);

        Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync();

        Task UpdateRolesAsync(IdentityUserUpdateRolesDto input);

        Task<IdentityUserDto> FindByUsernameAsync(string username);

        Task<IdentityUserDto> FindByEmailAsync(string email);

        /// <summary>
        /// 获取当前用户权限
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetUserPermissions();

        Task<string> Upload([FromForm] ImportData input);

        Task<bool> Remove(Guid id);

        Task<bool> RemoveFromOrganization(Guid id, Guid organizationId);
        Task<bool> SetUserInfo(IdentityUserSetDto input);

        Task<bool> UpdatePassword(string oldPassword, string newPassword);

        Task<Stream> Export(IdentityUserData input);
        Task<List<IdentityUserDto>> GetListByIds(List<Guid> ids);
    }
}