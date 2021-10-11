using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 设备信息表
    /// </summary>
    public class Terminal : FullAuditedEntity<Guid>
    {
        protected Terminal() { }
        public Terminal(Guid id) { Id = id; }

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
        /// 设备Id
        /// </summary>
        [MaxLength(100)]
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        /// <summary>
        /// 业务描述（即电缆芯说明）
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        public string Remark { get; set; }


        /// <summary>
        /// 和端子相连的线缆
        /// </summary>
        [InverseProperty("TerminalA")]
        public List<TerminalLink> TerminalLinkAs { get; set; }


        /// <summary>
        /// 和端子相连的线缆
        /// </summary>
        [InverseProperty("TerminalB")]
        public List<TerminalLink> TerminalLinkBs { get; set; }
    }
}
