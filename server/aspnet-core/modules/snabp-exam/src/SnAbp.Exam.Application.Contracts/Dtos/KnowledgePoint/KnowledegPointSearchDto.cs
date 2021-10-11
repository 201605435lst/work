using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Exam.Dtos
{
    public class KnowledegPointSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 根据名称查询
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 根据分类查询
        /// </summary>
        public List<Guid> CategoryIds { get; set; }

        /// <summary>
        /// 是否查询所有
        /// </summary>
        public bool IsAll { get; set; }
    }
}
