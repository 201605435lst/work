using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
   public class QuotaCategoryGetListByIdsDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 获取兄弟及父级元素的Id集合
        /// </summary>
        public List<Guid>? Ids { get; set; }

        /// <summary>
        /// 是否全部加载
        /// </summary>
        public bool IsAll { get; set; }
        /// <summary>
        /// 关键字：名称或者编码
        /// </summary>
        public string KeyWords { get; set; }


        /// <summary>
        /// 获取定额的Id集合
        /// </summary>
        public List<Guid>? QuotaIds { get; set; }
    }
}
