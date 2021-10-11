using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Dtos
{
    public class RepairItemRltComponentCategoryDto : Entity<Guid>
    {
        /// <summary>
        /// 维修项 Id
        /// </summary>
        public Guid RepairItemId { get; set; }
        public RepairItemDto RepairItem { get; set; }

        /// <summary>
        /// 构件分类
        /// </summary>
        public Guid ComponentCategoryId { get; set; }
        public ComponentCategoryDto ComponentCategory { get; set; }
    }
}
