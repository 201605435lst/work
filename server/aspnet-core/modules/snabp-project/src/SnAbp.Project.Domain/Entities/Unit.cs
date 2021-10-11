using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Project.enums;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Project
{
    /// <summary>
    /// 建设单位实体
    /// </summary>
    public class Unit : FullAuditedEntity<Guid>
    {
        public Unit() { }
        public Unit(Guid id) { Id = id; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public UnitType Type { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankAccount { get; set; }
        /// <summary>
        /// 银行代码
        /// </summary>
        public string BankCode { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
