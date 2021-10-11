/**********************************************************************
*******命名空间： SnAbp.Material.Dtos.Partition
*******类 名 称： PartitionCreateDto
*******类 说 明： 分区创建
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/1 14:26:53
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// $$
    /// </summary>
    public class PartitionCreateDto:EntityDto<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal X { get; set; }
        public virtual decimal Y { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual Guid? FileId { get; set; }
        public virtual Guid? TopId { get; set; }
        public virtual string Remark { get; set; }
    }
}
