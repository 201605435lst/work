using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Project.Dtos
{
   public class BooksClassificationDto : EntityDto<Guid>
    {
        /// <summary>
        /// 案卷分类名
        /// </summary>
        public string Name { get; set; }
    }
}