using SnAbp.StdBasic.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class FaultRltComponentCategorySimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 构件
        /// </summary>
        public Guid ComponentCategoryId { get; set; }
                  
        public ComponentCategoryDto ComponentCategory { get; set; }
    }
}