using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ComponentCategoryDto : EntityDto<Guid>, IGuidKeyTree<ComponentCategoryDto>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(200)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 上级构件分类id
        /// </summary>
        public Guid? ParentId { get; set; }
        public ComponentCategoryDto Parent { get; set; }
        public List<ComponentCategoryDto> Children { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(50)]
        [Description("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 层级名称
        /// </summary>
        [MaxLength(50)]
        [Description("层级名称")]
        public string LevelName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [MaxLength(30)]
        [Description("单位")]
        public string Unit { get; set; }

        /// <summary>
        /// 扩展编码
        /// </summary>
        [MaxLength(50)]
        [Description("扩展编码")]
        public string ExtendCode { get; set; }

        /// <summary>
        /// 扩展名称
        /// </summary>
        [MaxLength(50)]
        [Description("扩展名称")]
        public string ExtendName { get; set; }

        public bool IsDeleted { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public virtual List<ComponentCategoryRltProductCategoryDto> ComponentCategoryRltProductCategories { get; set; } = new List<ComponentCategoryRltProductCategoryDto>();
    }
}
