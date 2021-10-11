/**********************************************************************
*******命名空间： SnAbp.Safe.Entities
*******类 名 称： SafeProblemRecord
*******类 说 明： 安全问题记录表，包括整改内容和验证内容
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 19:05:29
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Safe.Entities
{
    /// <summary>
    ///  安全问题记录表，包括整改内容和验证内容
    /// </summary>
    public class SafeProblemRecord : AuditedEntity<Guid>
    {
        public SafeProblemRecord(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;
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
        public List<SafeProblemRecordRltFile> Files { get; set; }

    }
}
