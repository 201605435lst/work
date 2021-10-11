/**********************************************************************
*******命名空间： SnAbp.Technology.Entities
*******类 名 称： ConstructInterfaceInfoRltMarkFile
*******类 说 明： 接口标记信息，标记文件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/23 17:57:57
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using SnAbp.Technology.Enums;
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Technology.Entities
{
    /// <summary>
    /// 接口标记信息，标记文件
    /// </summary>
    public class ConstructInterfaceInfoRltMarkFile:Entity<Guid>
    {
        public ConstructInterfaceInfoRltMarkFile(Guid id)
        {
            Id = id;
        }
        public Guid ConstructInterfaceInfoId { get; set; }
        public ConstructInterfaceInfo ConstructInterfaceInfo { get; set; }
        public Guid MarkFileId { get; set; }
        public File.Entities.File MarkFile { get; set; }
        /// <summary>
        /// 文件类
        /// </summary>
        public InterfaceFlagType Type { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { ConstructInterfaceInfoId , MarkFileId };
        }
    }
}
