/************************************************************************************
*命名空间：SnAbp.Resource.Entities
*文件名：ComponentTrackRecord
*创建人： liushengtao
*创建时间：2021/6/24 10:04:58
*描述：
*
***********************************************************************/
using SnAbp.Resource.Enums;
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    public class ComponentTrackRecord : Entity<Guid>
    {
        public ComponentTrackRecord() { }
        public ComponentTrackRecord(Guid id)
        {
            Id = id;
        }
        public void SetId(Guid id) => Id = id;
        /// <summary>
        /// 二维码表Id
        /// </summary>
        public virtual Guid ComponentRltQRCodeId { get; set; }
        public virtual ComponentRltQRCode ComponentRltQRCode { get; set; }
        /// <summary>
        /// 节点类型 Type（检验、入库、出库、到场检验、安装、调试）
        /// </summary>
        public NodeType NodeType { get; set; }
        /// <summary>
        /// 跟踪类型id
        /// </summary>
        public virtual Guid? TrackingId { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作时间 Time
        /// </summary>
        public DateTime? Time { get; set; }
        /// <summary>
        /// 标记人员 UserId
        /// </summary>
        public virtual Identity.IdentityUser User { get; set; }
        public virtual Guid? UserId { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectTagId { get; set; }
    }
}
