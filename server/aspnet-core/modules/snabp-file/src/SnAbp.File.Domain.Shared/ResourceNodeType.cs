/**********************************************************************
*******命名空间： SnAbp.File
*******类 名 称： ResourceNodeType
*******类 说 明： 资源树节点类型，定义树结构时绑定一个节点类型，用来做查询使用
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 15:18:43
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.File
{
    /// <summary>
    /// 资源树节点类型，定义树结构时绑定一个节点类型，用来做查询使用
    /// <para>资源树节点类型只能是组织机构或组织结构下的文件夹</para>
    /// <para>组织机构：value=0</para>
    /// <para>文件夹：value=1</para>
    /// </summary>
    public enum ResourceNodeType
    {
        /// <summary>
        /// 组织机构
        /// </summary>
        Organization = 0,
        /// <summary>
        /// 文件夹
        /// </summary>
        Folder = 1,
        /// <summary>
        /// 未知节点类型
        /// </summary>
        Undefined = 2
    }
}
