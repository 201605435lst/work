using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Problem.Dtos
{
    public class ProblemDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 详情
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// 问题分类关联表
        /// </summary>
        [InverseProperty("Problem")]
        public virtual List<ProblemRltProblemCategoryDto> ProblemRltProblemCategories { get; set; }
    }
}
