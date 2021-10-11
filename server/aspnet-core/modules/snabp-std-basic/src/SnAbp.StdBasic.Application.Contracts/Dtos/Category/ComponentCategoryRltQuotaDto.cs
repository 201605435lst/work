using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ComponentCategoryRltQuotaDto : EntityDto<Guid>
    {
        public  Guid ComponentCategoryId { get; set; }
        public  Guid QuotaId { get; set; }

        /// <summary>
        /// 定额名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// 定额编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public float Weight { get; set; }
    }
}
