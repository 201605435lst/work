using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
    public class IndividualProjectCreateDto 
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级产品分类id
        /// </summary>
        public Guid? ParentId { get; set; }
      
        /// <summary>
        /// 编码
        /// </summary>
        public  string Code { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public Guid SpecialtyId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public  string Remark { get; set; }
    }
}
