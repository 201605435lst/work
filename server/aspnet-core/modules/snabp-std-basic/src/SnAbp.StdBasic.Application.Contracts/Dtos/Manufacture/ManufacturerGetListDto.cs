using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ManufacturerGetListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 厂家名称\简称
        /// </summary>
        public string Keyword { get; set; }


        /// <summary>
        /// 父级 Id
        /// </summary>
        public Guid? ParentId { get; set; }


        public bool IsAll { get; set; } = false;
    }
}
