using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 端子业务径路表
    /// </summary>
    public class TerminalBusinessPathNode : FullAuditedEntity<Guid>
    {
        protected TerminalBusinessPathNode() { }
        public TerminalBusinessPathNode(Guid id) { Id = id; }


        /// <summary>
        /// 端子业务表
        /// </summary>
        public TerminalBusinessPath TerminalBusinessPath { get; set; }
        public Guid TerminalBusinessPathId { get; set; }


        /// <summary>
        /// 端子 Id
        /// </summary>
        public Terminal Terminal { get; set; }
        public Guid TerminalId { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// 电缆芯 Id
        /// </summary>
        public Guid? CableCoreId { get; set; }
        public CableCore CableCore { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
    }
}
