using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 端子业务表
    /// </summary>
    public class TerminalBusinessPath : FullAuditedEntity<Guid>
    {
        protected TerminalBusinessPath() { }
        public TerminalBusinessPath(Guid id) { Id = id; }


        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }


        /// <summary>
        /// 配线径路
        /// </summary>
        public List<TerminalBusinessPathNode> Nodes { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
    }
}
