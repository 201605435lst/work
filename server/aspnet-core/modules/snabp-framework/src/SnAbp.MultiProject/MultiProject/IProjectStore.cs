/**********************************************************************
*******命名空间： SnAbp.MultiProject.MultiProject
*******接口名称： IProjectStore
*******接口说明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/17/2021 2:28:22 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnAbp.MultiProject.MultiProject
{
    /// <summary>
    ///  项目信息存储接口
    /// </summary>
    public interface IProjectStore
    {
        Task<ProjectConfiguration> FindAsync(string name);
        Task<ProjectConfiguration> FindAsync(Guid id);
        Task<ProjectConfiguration> Find(string name);
        ProjectConfiguration Find(Guid id);
    }
}
