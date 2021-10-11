using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Cms.Dtos
{
    public class CategoryUpdateEnableDto: EntityDto<Guid>
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
