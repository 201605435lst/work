using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
   public class ProjectItemDto : EntityDto<Guid>
    {
        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(50)]
        [Description("编码")]
        public  string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(200)]
        [Description("名称")]
        public  string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public  string Remark { get; set; }
    }
}
