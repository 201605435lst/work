/**********************************************************************
*******命名空间： Volo.Abp.Identity
*******接口名称： IIdentityUserRoleRepository
*******接口说明： 用户角色仓储，用来重写ef 封装的用户和角色的管理方法
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/4 15:21:35
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SnAbp.Identity;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Identity
{
    public interface IIdentityOrganizationRltRoleRepository : IBasicRepository<OrganizationRltRole>, IQueryable<OrganizationRltRole>
    {

    }
}
