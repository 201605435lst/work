using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.CostManagement.Entities
{
    public class MoneyList : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 资金类别
        /// </summary>
        public virtual Guid TypeId { get; set; }
        public virtual DataDictionary Type { get; set; }
        /// <summary>
        /// 应收
        /// </summary>
        public decimal Receivable { get; set; }
        /// <summary>
        /// 已收
        /// </summary>
        public decimal Received { get; set; }
        /// <summary>
        /// 应付
        /// </summary>
        public decimal Due { get; set; }
        /// <summary>
        /// 已付
        /// </summary>
        public decimal Paid { get; set; }
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
        public MoneyList() { }
        public MoneyList(Guid id)
        {
            Id = id;
        }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
