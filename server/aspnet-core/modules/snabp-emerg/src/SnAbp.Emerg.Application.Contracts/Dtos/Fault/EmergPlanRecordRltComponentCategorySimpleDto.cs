using SnAbp.StdBasic.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class EmergPlanRecordRltComponentCategorySimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 构件分类Id
        /// </summary>
        public Guid ComponentCategoryId { get; set; }
        public ComponentCategoryDto ComponentCategory { get; set; }
    }
}