using SnAbp.Exam.Entities;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;


namespace SnAbp.Exam.Dtos
{
    public class KnowledgePointDto:AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
  
        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        public List<KnowledgePointDto> Children { get; set; } = new List<KnowledgePointDto>();

        /// <summary>
        /// 分类 Id
        /// </summary>

        public List<KnowledgePointRltCategorySimpleDto> KnowledgePointRltCategories { get; set; } = new List<KnowledgePointRltCategorySimpleDto>();
    }
}
