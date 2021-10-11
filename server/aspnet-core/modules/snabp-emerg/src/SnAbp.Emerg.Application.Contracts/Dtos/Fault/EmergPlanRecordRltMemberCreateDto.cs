using SnAbp.Emerg.Enums;
using System;
using SnAbp.Identity;

namespace SnAbp.Emerg.Dtos.EmergPlanRecord
{
    public class EmergPlanRecordRltMemberCreateDto
    {
        /// <summary>
        /// 预案记录
        /// </summary>
        public Guid EmergPlanRecordId { get; set; }


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
    }
}
