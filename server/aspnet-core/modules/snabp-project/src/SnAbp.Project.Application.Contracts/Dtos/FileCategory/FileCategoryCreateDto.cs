using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Project.Dtos
{
    public class FileCategoryCreateDto:EntityDto<Guid>
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
    }
}
