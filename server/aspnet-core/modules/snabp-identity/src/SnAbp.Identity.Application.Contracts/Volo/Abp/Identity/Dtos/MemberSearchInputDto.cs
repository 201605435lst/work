using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SnAbp.Identity
{
    public class MemberSearchInputDto
    {
        /// <summary>
        /// 成员信息列表
        /// </summary>
        [Required]
        public List<MemberInfoDto> MemberInfos { get; set; } = new List<MemberInfoDto>();
    }
}