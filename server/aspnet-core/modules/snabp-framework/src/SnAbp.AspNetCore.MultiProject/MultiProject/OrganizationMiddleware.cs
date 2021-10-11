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
    public class OrganizationMiddleware
    {
        private readonly IOrganizationRoot _organizationRoot;
        readonly RequestDelegate _next;
        public OrganizationMiddleware(
             RequestDelegate next,
             IOrganizationRoot organizationRoot = null
            )
        {
            _next = next;
            _organizationRoot = organizationRoot;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var organizationTagId = context.Request.Headers["OrganizationTagId"].FirstOrDefault();

            if (!organizationTagId.IsNullOrEmpty())
            {
                Guid.TryParse(organizationTagId, out Guid guid);
                if (guid != Guid.Empty)
                {
                    using (_organizationRoot.Change(guid))
                    {
                        await _next(context);
                    }
                }
            }
            else
            {
                await _next(context);
            }

            //
        }
    }
}
