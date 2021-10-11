using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class ArticleAccessoryCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public Guid FileId { get; set; }
    }
}
