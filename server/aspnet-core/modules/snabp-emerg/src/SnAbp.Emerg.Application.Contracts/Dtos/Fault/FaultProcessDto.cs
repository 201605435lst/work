using SnAbp.Emerg.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class FaultProcessDto:EntityDto<Guid>
    {
        /// <summary>
        /// 当前处理节点的id
        /// </summary>
        public Guid NodeId { get; set; }

        /// <summary>
        /// 节点处理时间
        /// </summary>
        public DateTime ProcessTime { get; set; }

        /// <summary>
        /// 判断节点的选择节点
        /// </summary>
        public Guid? DetermineTargetId { get; set; }
        /// <summary>
        /// 处理人id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 处理建议
        /// </summary>
        public string Comments { get; set; }
    }
}