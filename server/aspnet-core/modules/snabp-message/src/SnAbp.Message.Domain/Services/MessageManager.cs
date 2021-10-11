/**********************************************************************
*******命名空间： SnAbp.Message.Services
*******类 名 称： MessageManager
*******类 说 明： 消息管理类，实现与系统模块的数据交换和相应的处理。
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/17 9:19:16
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.Identity;
using Volo.Abp.Domain.Services;

namespace SnAbp.Message.Services
{
    public class MessageManager : IDomainService
    {
        readonly IIdentityUserRepository _identityUserRepository;

        public MessageManager(
            IIdentityUserRepository identityUserRepository
        ) => _identityUserRepository = identityUserRepository;

        /// <summary>
        ///     通过组织机构id 获取用户的id
        /// </summary>
        /// <param name="organizationIds"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<Guid>> GetUserIdsByOrganization(IReadOnlyList<Guid> organizationIds) =>
            (await _identityUserRepository.GetUsersInOrganizationsListAsync(organizationIds.ToList()))
            .Select(a => a.Id)
            .ToList();

        /// <summary>
        ///     根据角色获取用户id数据
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<Guid>> GetUserIdsByRole(IReadOnlyList<Guid> roleIds) =>
            (await _identityUserRepository.GetUserListAsync(a => a.Roles.Any(b => roleIds.Contains(b.RoleId))))
            .Select(a => a.Id)
            .ToList();

        /// <summary>
        ///     根据不同类型ids获取用户ids
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="organizationIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<Guid>> GetUserIdsByMember(
            List<Guid> userIds,
            IReadOnlyList<Guid> organizationIds,
            IReadOnlyList<Guid> roleIds
        ) => userIds.AddNewRange(await GetUserIdsByOrganization(organizationIds))
            .AddNewRange(await GetUserIdsByRole(roleIds));
    }
}