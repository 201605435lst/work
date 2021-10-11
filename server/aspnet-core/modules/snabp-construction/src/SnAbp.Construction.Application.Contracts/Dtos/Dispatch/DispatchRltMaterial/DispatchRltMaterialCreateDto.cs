using System;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联材料
    /// </summary>
    public class DispatchRltMaterialCreateDto
    {
        /// <summary>
        /// 材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal Count { get; set; }
    }
}
