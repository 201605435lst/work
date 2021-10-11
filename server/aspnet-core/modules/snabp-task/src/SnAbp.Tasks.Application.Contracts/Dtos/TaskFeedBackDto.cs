using SnAbp.Tasks.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Tasks.Dtos
{
    public class TaskFeedBackDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 进度
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// 情况描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 关联相关附件表
        /// </summary>
        public List<TaskRltFile> TaskRltFiles { get; set; }
    }
}
