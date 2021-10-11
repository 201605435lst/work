
using SnAbp.Resource.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class ComponentTrackRecordUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 二维码表Id
        /// </summary>
        public virtual Guid ComponentRltQRCodeId { get; set; }
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
        public virtual Guid? UserId { get; set; }
    }
}