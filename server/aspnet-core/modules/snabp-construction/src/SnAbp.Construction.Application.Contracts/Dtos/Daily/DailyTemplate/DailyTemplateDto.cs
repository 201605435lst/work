using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.DailyTemplate
*文件名：DailyTemplateDto
*创建人： liushengtao
*创建时间：2021/7/21 11:23:00
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public class DailyTemplateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 模板说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 设为默认
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
