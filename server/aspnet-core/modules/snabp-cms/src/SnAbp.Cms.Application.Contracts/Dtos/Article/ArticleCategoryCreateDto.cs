using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class ArticleCategoryCreateDto : EntityDto
    {
        /// <summary>
        /// 栏目id
        /// </summary>
        public Guid Id { get; set; }
    }
}
