/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos
*******类 名 称： QuantitiesDto
*******类 说 明： 工程量统计dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/23/2021 3:11:08 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
    /// <summary>
    ///  工程量统计dto
    /// </summary>
    public class QuantitiesDto : EntityDto<Guid>
    {
        public Guid SpecialityId { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Speciality { get; set; }
        /// <summary>
        /// 系统1
        /// </summary>
        public string System1 { get; set; }
        /// <summary>
        /// 系统2
        /// </summary>
        public string System2 { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品id -(与物资对应)
        /// </summary>
        public Guid ProductCategoryId { get; set; }
        /// <summary>
        /// 产品分类 名称
        /// </summary>
        public string ProductCategoryName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Spec { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Count { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
    }
}
