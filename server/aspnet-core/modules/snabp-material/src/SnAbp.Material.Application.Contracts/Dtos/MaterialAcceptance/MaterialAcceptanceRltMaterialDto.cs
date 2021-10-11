using SnAbp.Material.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class MaterialAcceptanceRltMaterialDto : EntityDto<Guid>
    {
        /// 物料检验状态
        public TestState TestState { get; set; }
        /// <summary>
        /// 物料检验单
        /// </summary>
        public virtual Guid MaterialAcceptanceId { get; set; }
        public virtual MaterialAcceptanceDto MaterialAcceptance { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 关联材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }
        public virtual Technology.Dtos.MaterialDto Material { get; set; }
    }
}
