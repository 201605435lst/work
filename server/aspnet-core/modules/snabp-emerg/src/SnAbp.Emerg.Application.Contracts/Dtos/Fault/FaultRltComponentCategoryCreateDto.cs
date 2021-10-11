using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class FaultRltComponentCategoryCreateDto : EntityDto
    {
        /// <summary>
        /// 构件分类ID
        /// </summary>
        public Guid Id { get; set; }
    }
}
