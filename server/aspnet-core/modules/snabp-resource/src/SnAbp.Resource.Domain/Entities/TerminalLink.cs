using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 端子配线
    /// </summary>
    public class TerminalLink : FullAuditedEntity<Guid>
    {
        protected TerminalLink() { }
        public TerminalLink(Guid id) { Id = id; }


        /// <summary>
        /// 端子ID
        /// </summary>
        public Guid TerminalAId { get; set; }
        public Terminal TerminalA { get; set; }


        /// <summary>
        /// 对方端子ID
        /// </summary>
        public Guid TerminalBId { get; set; }
        public Terminal TerminalB { get; set; }


        /// <summary>
        /// 线缆芯
        /// </summary>
        public Guid? CableCoreId { get; set; }
        public CableCore CableCore { get; set; }


        /// <summary>
        /// 业务功能
        /// </summary>
        public string BusinessFunction { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }

    }
}
