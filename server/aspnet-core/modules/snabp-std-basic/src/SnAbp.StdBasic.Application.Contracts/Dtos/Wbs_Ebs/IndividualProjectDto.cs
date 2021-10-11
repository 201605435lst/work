using SnAbp.Identity;
using SnAbp.StdBasic.Entities;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    /// <summary>
    /// 单项工程
    /// </summary>
    public class IndividualProjectDto : EntityDto<Guid>, IGuidKeyTree<IndividualProjectDto>
    {

        public  IndividualProjectDto Parent { get; set; }
        public  List<IndividualProjectDto> Children { get; set; } = new List<IndividualProjectDto>();
        /// <summary>
        /// 上级单项工程id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(200)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(50)]
        [Description("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string SpecialtyName { get; set; }

        public Guid SpecialtyId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
