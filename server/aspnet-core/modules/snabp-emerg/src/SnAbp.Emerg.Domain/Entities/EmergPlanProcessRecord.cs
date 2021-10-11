using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Emerg.Entities
{
    /// <summary>
    /// 应急预案
    /// </summary>
    public class EmergPlanProcessRecord : Entity<Guid>
    {
        protected EmergPlanProcessRecord() { }
        public EmergPlanProcessRecord(Guid id) { Id = id; }


        /// <summary>
        /// 预案记录
        /// </summary>
        public Guid EmergPlanRecordId { get; set; }
        public EmergPlanRecord EmergPlanRecord { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public Guid UserId { get; set; }
        //public IdentityUser User { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 节点Id
        /// </summary>
        public Guid NodeId { get; set; }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
