using SnAbp.Oa.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos.Seal
{
    public class SealUpdateDto : EntityDto<Guid>
    {

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 签章密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 批量设为无效
        /// </summary>
        public List<Guid> Ids { get; set; }

        /// <summary>
        /// 更新类型
        /// </summary>
        public UpdateType Type { get; set; }
    }
}
