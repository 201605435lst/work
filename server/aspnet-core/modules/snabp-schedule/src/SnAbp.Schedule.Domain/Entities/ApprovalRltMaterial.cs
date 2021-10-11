using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Schedule.Entities
{
    public class ApprovalRltMaterial : FullAuditedEntity<Guid>
    {
        public ApprovalRltMaterial(Guid id) => Id = id;
        public ApprovalRltMaterial()
        {

        }
        /// <summary>
        /// 审批Id
        /// </summary>
        public Guid ApprovalId { get; set; }
        public Approval Approval { get; set; }

        /// <summary>
        /// 材料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecModel { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 材料类别
        /// </summary>
        public MaterialsType Type { get; set; }
    }
}
