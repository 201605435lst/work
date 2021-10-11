/**********************************************************************
*******命名空间： SnAbp.MultiProject.MultiProject
*******类 名 称： DefaultProjectStoreOptions
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/19/2021 1:58:50 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.MultiProject.MultiProject
{
    /// <summary>
    /// 项目存储配置项  
    /// </summary>
    public class DefaultProjectStoreOptions
    {
        public ProjectConfiguration[] Projects { get; set; }
        public DefaultProjectStoreOptions()
        {
            this.Projects = new ProjectConfiguration[0];
        }
    }
}
