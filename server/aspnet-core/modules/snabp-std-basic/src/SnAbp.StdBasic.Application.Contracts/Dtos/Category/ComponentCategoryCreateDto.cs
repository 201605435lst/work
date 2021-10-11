using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
  public  class ComponentCategoryCreateDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级构件分类id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 层级名称
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 扩展编码
        /// </summary>
        public string ExtendCode { get; set; }

        /// <summary>
        /// 扩展名称
        /// </summary>
        public string ExtendName { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public virtual List<ComponentCategoryRltProductCategoryCreateDto> ComponentCategoryRltProductCategories { get; set; }
        /// <summary>
        /// 产品分类id
        /// </summary>
        public Guid? ProductCategoryId { get; set; }

    }
}
