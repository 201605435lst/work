using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model.ModelMVD
{
    public class MVDCategoryExportDto : EntityDto<Guid> 
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 代号
        /// </summary>
        [MaxLength(50)]
        [Description("代号")]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public List<MVDPropertyExportDto> MVDProperties { get; set; }
    }
}
