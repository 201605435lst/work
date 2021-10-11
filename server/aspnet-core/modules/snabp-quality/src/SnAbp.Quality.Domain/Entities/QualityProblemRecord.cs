using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Quality.Entities
{
   public class QualityProblemRecord : AuditedEntity<Guid>
    {
        public QualityProblemRecord(Guid id) => this.Id = id;

        /// <summary>
        /// 关联的问题
        /// </summary>
        public QualityProblem QualityProblem { get; set; }
        public Guid QualityProblemId { get; set; }
        /// <summary>
        /// 记录类型
        /// </summary>
        public QualityRecordType Type { get; set; }
        /// <summary>
        /// 记录状态
        /// </summary>
        public QualityRecordState State { get; set; }
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
        public List<QualityProblemRecordRltFile> Files { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}