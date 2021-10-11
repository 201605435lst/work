using SnAbp.StdBasic.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 信息交换模板属性表
    /// </summary>
    public class MVDProperty : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [MaxLength(50)]
        [Description("值")]
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
        //[MaxLength(50)]
        //[Description("数据类型")]
        //public MVDPropertyDataType DataType { get; set; }

        /// <summary>
        /// 信息交换模板分类Id
        /// </summary>
        public Guid? MVDCategoryId { get; set; }
        public virtual MVDCategory MVDCategory { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        protected MVDProperty() { }
        public MVDProperty(Guid id)
        {
            Id = id;
        }
    }
}