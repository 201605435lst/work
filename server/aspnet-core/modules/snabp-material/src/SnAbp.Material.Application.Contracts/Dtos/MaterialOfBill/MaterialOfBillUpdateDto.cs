using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class MaterialOfBillUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 施工队
        /// </summary>
        public string ConstructionTeam { get; set; }

        /// <summary>
        /// 施工区段
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// 领料日期
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 附件关联表
        /// </summary>
        public List<MaterialOfBillRltAccessoryUpdateDto> MaterialOfBillRltAccessories { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 物料关联表
        /// </summary>
        public List<MaterialOfBillRltMaterialUpdateDto> MaterialOfBillRltMaterials { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public MaterialOfBillState State { get; set; }
    }
}
