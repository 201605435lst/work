using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemRecordCreateDto
    {
        /// <summary>
        /// 关联的问题
        /// </summary>
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
        /// <summary>
        /// 整改或者验证的文件
        /// </summary>
        public List<QualityProblemRecordRltFileCreateDto> Files { get; set; } = new List<QualityProblemRecordRltFileCreateDto>();
    }
}
