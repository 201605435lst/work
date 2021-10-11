using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model.ModelMVD
{
    public class MVDPropertyCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        [Description("名称")]
        public string Name { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// 是否是实例参数
        /// </summary>
        public bool IsInstance { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        ///// <summary>
        ///// 参数类型
        ///// </summary>
        //public MVDPropertyDataType DataType { get; set; }

        /// <summary>
        /// 信息交换模板分类Id
        /// </summary>
        public Guid? MVDCategoryId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
