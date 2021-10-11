using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
   public class ComponentCategoryRltProductCategoryCreateDto
    {
        /// <summary>
        /// 构件分类Id
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }

        /// <summary>
        /// 产品分类Id
        /// </summary>
        public Guid? ProductionCategoryId { get; set; }
    }
}
