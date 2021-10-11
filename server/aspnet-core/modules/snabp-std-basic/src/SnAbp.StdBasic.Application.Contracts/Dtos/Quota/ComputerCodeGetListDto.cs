using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ComputerCodeGetListDto : PagedAndSortedResultRequestDto
    {

        /// <summary>
        /// 名称
        /// </summary>
        public string Keyword { get; set; }

        public bool IsAll { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ComputerCodeType? Type { get; set; }

        /// <summary>
        /// 关联时传入的Ids
        /// </summary>
        public List<Guid>? Ids { get; set; }

        /// <summary>
        ///是否关联电算代号使用
        /// </summary>
        public bool? IsRltMaterial { get; set; }
    }
}
