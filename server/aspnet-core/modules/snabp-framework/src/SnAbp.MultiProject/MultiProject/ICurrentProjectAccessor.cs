/**********************************************************************
*******命名空间： SnAbp.MultiProject.MultiProject
*******接口名称： ICurrentProjectAccessor
*******接口说明： 当前项目信息存取器
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 8/17/2021 1:49:07 PM
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
    /// 项目信息存储区
    /// </summary>
    public interface ICurrentProjectAccessor
    {
        BasicProjectInfo Current { get; set; }
    }
}
