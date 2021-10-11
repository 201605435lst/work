using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos.Seal
{
    public class SealDto : EntityDto<Guid>
    {
        /// <summary>
        /// 签章名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 签章类型
        /// </summary>
        public SealType Type { get; set; }
        /// <summary>
        /// 授权成员
        /// </summary>
        public List<SealRltMemberDto> SealRltMembers { get; set; }
        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// 签章密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 签名图片
        /// </summary>
        public Guid ImageId { get; set; }
        public File.Entities.File Image { get; set; }
    }
}
