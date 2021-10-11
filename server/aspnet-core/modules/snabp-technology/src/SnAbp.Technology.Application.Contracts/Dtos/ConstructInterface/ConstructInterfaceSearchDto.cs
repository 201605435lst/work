using SnAbp.Technology.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
  public class ConstructInterfaceSearchDto : PagedAndSortedResultRequestDto
    {
        //// 施工地点
        //public virtual Guid? ConstructionSectionId { get; set; }
        /// <summary>
        /// 专业 DataDictionaryId
        /// </summary>
        public virtual Guid? ProfessionId { get; set; }
        /// <summary>
        /// 土建单位
        /// </summary>
        public virtual Guid? BuilderId { get; set; }
        /// <summary>
        /// 接口检查情况
        /// </summary>
        public virtual MarkType? MarkType { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public virtual Guid? InterfaceManagementTypeId { get; set; }
    }
}
