using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos
{
   public class RepairGroupGetListDto
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 是否树选择
        /// </summary>
        public bool TreeSelect { get; set; } 
    }
}
