using System;

namespace SnAbp.Identity
{
    public class MemberInfoDto
    {
        /// <summary>
        /// 成员 Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 成员类型
        /// </summary>
        public MemberType Type { get; set; }
    }
}