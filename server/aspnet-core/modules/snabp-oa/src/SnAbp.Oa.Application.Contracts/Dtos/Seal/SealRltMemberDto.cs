using SnAbp.Identity;
using SnAbp.Oa.Dtos.Seal;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos
{
    public class SealRltMemberDto : EntityDto<Guid>
    {
        public Guid SealId { get; set; }
        public SealDto Seal { get; set; }

        public Guid MemberId { get; set; }
        public MemberType MemberType { get; set; }
    }
}
