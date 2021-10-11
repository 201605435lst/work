using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.CostManagement.Entities
{
   public class PeopleCost: FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 专业
        /// </summary>
        public virtual Guid ProfessionalId { get; set; }
        public virtual DataDictionary Professional { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 收款单位
        /// </summary>
        public virtual Guid PayeeId { get; set; }
        public virtual DataDictionary Payee { get; set; }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public PeopleCost(){}
        public PeopleCost(Guid id)
        {
            Id = id;
        }

        public static explicit operator DateTime(PeopleCost v)
        {
            throw new NotImplementedException();
        }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
