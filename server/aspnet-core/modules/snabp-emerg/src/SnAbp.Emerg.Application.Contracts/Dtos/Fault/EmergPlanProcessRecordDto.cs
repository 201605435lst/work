using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class EmergPlanProcessRecordDto : EntityDto<Guid>
    {
        public Guid EmergPlanRecordId { get; set; }

        /// <summary>
        /// 处理人Id
        /// </summary>
        public Guid UserId { get; set; }
        //public IdentityUser User { get; set; }

        /// <summary>
        /// 处理人姓名
        /// </summary>
        public string HandleUserName { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 节点Id
        /// </summary>
        public Guid NodeId { get; set; }
    }
}
