using SnAbp.Safe.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    public class SafeProblemRecordDto : EntityDto<Guid>  
    {
        /// <summary>
        /// 关联的问题
        /// </summary>
        public SafeProblem SafeProblem { get; set; }
        public Guid SafeProblemId { get; set; }
        /// <summary>
        /// 记录类型
        /// </summary>
        public SafeRecordType Type { get; set; }
        /// <summary>
        /// 记录状态
        /// </summary>
        public SafeRecordState State { get; set; }
        /// <summary>
        /// 整改或验证时间
        /// </summary>

        public DateTime? Time { get; set; }
        /// <summary>
        /// 整改或验证内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public Guid UserId { get; set; }
        public Identity.IdentityUser User { get; set; }

        /// <summary>
        /// 整改或者验证的文件
        /// </summary>
        public List<SafeProblemRecordRltFileDto> Files { get; set; } = new List<SafeProblemRecordRltFileDto>();
    }
}
