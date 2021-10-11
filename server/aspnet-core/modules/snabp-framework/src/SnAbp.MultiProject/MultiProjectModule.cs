/**********************************************************************
*******命名空间： SnAbp.MultiProject
*******类 名 称： MultiProjectModule
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/17/2021 1:58:26 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.Extensions.DependencyInjection;

using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace SnAbp.MultiProject
{
    /// <summary>
    /// 多项目模块
    /// </summary>
    [DependsOn(
      typeof(AbpDataModule),
      typeof(AbpSecurityModule)
      )]
    public class MultiProjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<DefaultProjectStoreOptions>(configuration);
            // 注册一下
            context.Services.AddSingleton<ICurrentProject, CurrentProject>();
            context.Services.AddSingleton<IOrganizationRoot, OrganizationRoot>();
        }
    }
}
