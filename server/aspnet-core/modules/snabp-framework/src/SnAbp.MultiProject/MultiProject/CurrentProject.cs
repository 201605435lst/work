/**********************************************************************
*******命名空间： SnAbp.MultiProject.MultiProject
*******接口名称： CurrentProject
*******接口说明： 多项目实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/17/2021 1:46:58 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace SnAbp.MultiProject.MultiProject
{
    /// <summary>
    /// 当前项目实例
    /// </summary>
    public class CurrentProject : ICurrentProject, ITransientDependency
    {
        BasicProjectInfo Current { get; set; }
        public CurrentProject()
        {
        }
        public bool IsAvailable => Id.HasValue;

        public Guid? Id => Current?.ProjectId;

        public string Name =>Current?.Name;

        public IDisposable Change(Guid? id, string name = null)
        {
            var parentScop = new BasicProjectInfo(id, name);
            return new DisposeAction(() => Current = parentScop);
        }
    }
}
