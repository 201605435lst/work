/**********************************************************************
*******命名空间： SnAbp.Material.Entities.Partition
*******类 名 称： Partition
*******类 说 明： 分区管理-分区信息表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/1 13:50:21
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Material.Enums;
using SnAbp.MultiProject.MultiProject;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 分区管理-分区信息表
    /// </summary>
    public class Partition:Entity<Guid>
    {
        public Partition(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;
        /// <summary>
        /// 分组名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 坐标x
        /// </summary>
        public virtual decimal X { get; set; }
        /// <summary>
        /// 坐标y
        /// </summary>
        public virtual decimal Y { get; set; }
        /// <summary>
        /// 分区类型
        /// </summary>
        public virtual PartitionType Type { get; set; }

        public virtual Partition Parent { get; set; }
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// 关联文件
        /// </summary>
        public virtual File.Entities.File File { get; set; }
        public virtual Guid? FileId { get; set; }

        public virtual string Remark { get; set; }

        /// <summary>
        /// 顶级id,用来记录顶级id
        /// </summary>
        public virtual Guid? TopId { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
