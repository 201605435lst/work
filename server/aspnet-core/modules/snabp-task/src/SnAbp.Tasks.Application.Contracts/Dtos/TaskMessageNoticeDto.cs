using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Tasks.Dtos
{
    public class TaskMessageNoticeDto 
    {
        //提交任务的人
        public string Name { get; set; }
        /// <summary>
        /// 任务的状态
        /// </summary>
        public string TaskType { get; set; }
    }
}
