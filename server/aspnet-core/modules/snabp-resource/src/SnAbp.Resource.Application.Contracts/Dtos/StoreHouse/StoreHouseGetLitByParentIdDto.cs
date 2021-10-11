using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.Dtos.StoreHouse
{
    public class StoreHouseGetLitByParentIdDto
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 获取兄弟及父级元素的Id集合
        /// </summary>
        public List<Guid> ? Ids { get; set; }

        /// <summary>
        /// 是否全部加载
        /// </summary>
        public bool IsAll { get; set; }
    }
}
