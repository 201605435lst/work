using System;

namespace SnAbp.Identity
{
    public class Member
    {
        /// <summary>
        /// 成员 Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 成员类型
        /// </summary>
        public MemberType Type { get; set; }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string Name { get; set; }
    }
}