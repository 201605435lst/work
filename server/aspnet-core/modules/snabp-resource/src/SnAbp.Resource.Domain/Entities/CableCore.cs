using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 电缆芯
    /// </summary>
    public class CableCore : FullAuditedEntity<Guid>
    {
        protected CableCore() { }
        public CableCore(Guid id) { Id = id; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 电缆
        /// </summary>
        public Guid CableId { get; set; }
        public Equipment Cable { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public CableCoreType Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Rremark { get; set; }

        ///// <summary>
        ///// 配线关系
        ///// </summary>
        //public List<TerminalLink> TerminalLinks { get; set; }
    }
}
