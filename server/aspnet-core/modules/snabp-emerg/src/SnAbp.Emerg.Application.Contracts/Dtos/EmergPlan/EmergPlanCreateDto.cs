using SnAbp.Emerg.Dtos;
using System;
using System.Collections.Generic;

namespace SnAbp.Emerg.Dtos
{
    public class EmergPlanCreateDto
    {
        /// <summary>
        /// 预案名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预案等级id
        /// </summary>
        public Guid LevelId { get; set; }

        /// <summary>
        /// 构件分类
        /// </summary>
        public List<Guid> ComponentCategoryIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 预案摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 预案流程
        /// </summary>
        public string Flow { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 主要附件
        /// </summary>
        public List<Guid> FileIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 图文资料
        /// </summary>
        public string Content { get; set; }
    }
}
