/**********************************************************************
*******命名空间： SnAbp.Material.Dtos.Inventory
*******类 名 称： InventoryDto
*******类 说 明： 库存返回结果
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/3 14:54:48
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
    public class InventoryDto : EntityDto<Guid>
    {
        /// <summary>
        /// 材料表
        /// </summary>
        public virtual Guid MaterialId { get; set; }
        public virtual Technology.Dtos.MaterialDto Material { get; set; }

        public virtual int Order { get; set; }

        /// <summary>
        /// 生产批号
        /// </summary>
        public virtual string BatchNumber { get; set; }
        /// <summary>
        /// 库存位置
        /// </summary>
        public virtual Guid? PartitionId { get; set; }
        public virtual PartitionDto Partition { get; set; }

        /// <summary>
        /// 库存量
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 供应商信息
        /// </summary>
        public virtual Guid? SupplierId { get; set; }
        public virtual SupplierDto Supplier { get; set; }

        /// <summary>
        /// 入库登记时间
        /// </summary>
        public virtual DateTime EntryTime { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public virtual DateTime ProductionDate { get; set; }
    }
}
