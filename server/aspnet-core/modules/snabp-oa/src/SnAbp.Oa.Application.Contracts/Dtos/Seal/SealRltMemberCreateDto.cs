using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Oa.Dtos.Seal
{
    public class SealRltMemberCreateDto
    {
        public Guid Id { get; set; }

        public MemberType Type { get; set; }
    }
}
