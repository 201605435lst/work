/**********************************************************************
*******命名空间： SnAbp.MultiProject.MultiProject
*******类 名 称： CurrentUserProjectResolveContributor
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/19/2021 3:37:33 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Users;

namespace SnAbp.MultiProject.MultiProject
{
    // TODO 当前项目的信息是通过当前用户信息中记录的项目而来的，这一块需要处理。
    /// <summary>
    ///  当前用户属于的项目
    /// </summary>
    public class CurrentUserProjectResolveContributor : ProjectResolveContributorBase
    {
        public const string ContributorName = "CurrentUser";

        public override string Name => ContributorName;

        public override void Resolve(IProjectResolveContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            if (!currentUser.IsAuthenticated) return;
            context.Handled = true;
           //  context.ProjectId=currentUser.
        }
    }
}
