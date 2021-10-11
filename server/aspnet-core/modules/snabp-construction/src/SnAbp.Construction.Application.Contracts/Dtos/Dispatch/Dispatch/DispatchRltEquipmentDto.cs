using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 计划内容关联设备dto
    /// </summary>
    public class DispatchRltEquipmentDto : EntityDto<Guid>
    {
        /// <summary>
        /// 构件id
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public string Spec { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
    }
}