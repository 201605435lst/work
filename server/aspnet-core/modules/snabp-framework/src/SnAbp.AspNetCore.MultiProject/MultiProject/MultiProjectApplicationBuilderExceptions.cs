/**********************************************************************
*******命名空间： SnAbp.MultiProject
*******类 名 称： MultiProjectApplicationBuilderExceptions
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/17/2021 2:00:15 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.AspNetCore.Builder;


using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.AspNetCore.MultiProject
{
    /// <summary>
    ///  多项目中间件扩展方法 
    /// </summary>
    public static class MultiProjectApplicationBuilderExceptions
    {
        public static IApplicationBuilder UseMultiProject(this IApplicationBuilder app)
        {
            app.UseMiddleware<MultiProjectMiddleware>();
            app.UseMiddleware<OrganizationMiddleware>();
            return app;
        }
    }
}
