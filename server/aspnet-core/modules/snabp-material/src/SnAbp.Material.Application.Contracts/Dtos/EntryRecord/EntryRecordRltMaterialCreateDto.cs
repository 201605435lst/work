using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 入库关联物资
    /// </summary>
    public class EntryRecordRltMaterialCreateDto
    {
        /// <summary>
        /// 材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public virtual decimal Count { get; set; }

        /// <summary>
        /// 采购价格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 到货日期
        /// </summary>
        public DateTime? Time { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual Guid? SupplierId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
