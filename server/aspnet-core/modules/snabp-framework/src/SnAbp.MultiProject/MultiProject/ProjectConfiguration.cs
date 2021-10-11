/**********************************************************************
*******命名空间： SnAbp.MultiProject.MultiProject
*******类 名 称： ProjectConfiguration
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/17/2021 2:23:35 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using JetBrains.Annotations;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp;
using Volo.Abp.Data;

namespace SnAbp.MultiProject.MultiProject
{
    /// <summary>
    /// 项目配置信息  
    /// </summary>
    [Serializable]
    public class ProjectConfiguration
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public ProjectConfiguration()
        {

        }
        public ProjectConfiguration(Guid id, [NotNull] string name)
        {
            Check.NotNull(name, nameof(name));
            Id = id;
            Name = name;
            ConnectionStrings = new ConnectionStrings();
        }

    }
}
