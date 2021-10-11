using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 物资验收关联材料实体
    /// </summary>
    public class MaterialAcceptanceRltMaterial : AuditedEntity<Guid>
    {
        /// 物料检验状态（合格、不合格）
        public TestState TestState { get; set; }
        /// <summary>
        /// 物料检验单
        /// </summary>
        public virtual Guid MaterialAcceptanceId { get; set; }
        public virtual MaterialAcceptance MaterialAcceptance { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 关联材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }
        public virtual Technology.Entities.Material Material { get; set; }

        public MaterialAcceptanceRltMaterial() { }
        public MaterialAcceptanceRltMaterial(Guid id)
        {
            Id = id;
        }
    }
}
