using SnAbp.ConstructionBase.Entities;
using SnAbp.Material.Enums;
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    public class MaterialOfBill : FullAuditedEntity<Guid>
    {
        public MaterialOfBill(Guid id) => Id = id;

        /// <summary>
        /// 施工队
        /// </summary>
        public string ConstructionTeam { get; set; }

        /// <summary>
        /// 施工区段
        /// </summary>
        public Guid SectionId { get; set; }
        public virtual Section Section { get; set; }

        /// <summary>
        /// 领料日期
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 附件关联表
        /// </summary>
        public List<MaterialOfBillRltAccessory> MaterialOfBillRltAccessories { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 物料关联表
        /// </summary>
        public List<MaterialOfBillRltMaterial> MaterialOfBillRltMaterials { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public MaterialOfBillState State { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
