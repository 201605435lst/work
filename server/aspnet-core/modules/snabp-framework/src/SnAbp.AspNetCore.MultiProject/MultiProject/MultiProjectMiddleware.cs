/**********************************************************************
*******命名空间： SnAbp.MultiProject.MultiProject
*******类 名 称： MultiProjectMiddleware
*******类 说 明： 多项目中间件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/17/2021 11:59:31 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.AspNetCore.Http;
using SnAbp.MultiProject;
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace SnAbp.AspNetCore.MultiProject
{
    /// <summary>
    ///  多项目中间件
    /// </summary>
    public class MultiProjectMiddleware
    {
        private readonly ICurrentProject _currentProject;
        private readonly IOrganizationRoot _organizationRoot;
        readonly RequestDelegate _next;
        public MultiProjectMiddleware(
             RequestDelegate next,
             ICurrentProject currentProject = null
, IOrganizationRoot organizationRoot = null)
        {
            _next = next;
            _currentProject = currentProject;
            _organizationRoot = organizationRoot;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // 关键部分，解析当前的项目id，并写入到全局的项目服务中即可
            var projectId = context.Request.Headers["ProjectId"].FirstOrDefault();
            var organizationTagId = context.Request.Headers["OrganizationTagId"].FirstOrDefault();


            if (!projectId.IsNullOrEmpty())
            {
                Guid.TryParse(projectId, out Guid pid);
                Guid.TryParse(organizationTagId, out Guid gid);
                using (_currentProject.Change(pid)) { }
                using (_organizationRoot.Change(gid)) { }
                await _next(context);
            }
            else
            {
                await _next(context);
            }

        }

        private void SetValue(Guid pid, Guid oId)
        {
            using (_currentProject.Change(pid)) { }
            using (_organizationRoot.Change(oId)) { }
        }
    }
}
