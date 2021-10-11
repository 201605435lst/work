using SnAbp.Emerg.Enums;
using System;
using SnAbp.Identity;
using Volo.Abp.Domain.Entities;
using SnAbp.MultiProject.MultiProject;

namespace SnAbp.Emerg.Entities
{
    public class EmergPlanRecordRltMember : Entity<Guid>
    {
        protected EmergPlanRecordRltMember() { }
        public EmergPlanRecordRltMember(Guid id) { Id = id; }
        /// <summary>
        /// 预案记录
        /// </summary>
        public Guid EmergPlanRecordId { get; set; }
        public virtual EmergPlanRecord EmergPlanRecord { get; set; }


        /// <summary>
        /// 成员类型
        /// </summary>
        public MemberType MemberType { get; set; }


        /// <summary>
        /// 成员 Id
        /// </summary>
        public Guid MemeberId { get; set; }


        /// <summary>
        /// 状态分组
        /// </summary>
        public EmergPlanRecordRltMemberGroup Group { get; set; }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
