using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Common.Dtos.Task
{
    public class BackgroundTaskDto
    {
        /// <summary>
        /// 任务标识
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public decimal Count { get; set; } = 1;

        /// <summary>
        /// 索引
        /// </summary>
        public decimal Index { get; set; } = 0;

        /// <summary>
        /// 进度
        /// </summary>
        public decimal Progress
        {
            get
            {
                return Index / Count;
            }
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsDone { get; set; } = false;

        /// <summary>
        /// 是否发生错误
        /// </summary>
        public bool HasError { get; set; } = false;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
