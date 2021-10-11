using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ProjectItemRltProductCategoryDto : EntityDto<Guid>
    {
        public Guid ProjectItemId { get; set; }
        public Guid ProductCategoryId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 扩展编码
        /// </summary>
        public string ExtendCode { get; set; }

        /// <summary>
        /// 扩展名称
        /// </summary>
        public string ExtendName { get; set; }
    }
}
