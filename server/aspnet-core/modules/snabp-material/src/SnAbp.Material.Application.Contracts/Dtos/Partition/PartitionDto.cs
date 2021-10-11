/**********************************************************************
*******命名空间： SnAbp.Material.Dtos.Partition
*******类 名 称： PartitionDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/1 14:33:03
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using SnAbp.Material.Enums;
using SnAbp.Utils.TreeHelper;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// $$
    /// </summary>
    public class PartitionDto : EntityDto<Guid>, IGuidKeyTree<PartitionDto>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal X { get; set; }
        public virtual decimal Y { get; set; }
        public virtual PartitionType Type { get; set; }
        public virtual PartitionDto Parent { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual SnAbp.File.Dtos.FileSimpleDto File { get; set; }
        public virtual Guid? FileId { get; set; }
        public virtual Guid? TopId { get; set; }
        public virtual string Remark { get; set; }
        public List<PartitionDto> Children { get; set; }
    }
}
