using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.CostManagement.Entities
{
   public class CostOther : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 费用类型
        /// </summary>
        public virtual Guid TypeId { get; set; }
        public virtual DataDictionary Type { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

       
        public void SetId(Guid id)
        {
            Id = id;
        }
        public CostOther() { }
        public CostOther(Guid id)
        {
            Id = id;
        }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
