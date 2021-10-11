using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Basic.Enums;

namespace SnAbp.Basic.Dtos
{
    public class GetListByScopeInputDto
    {
        /// <summary>
        /// 父级范围编码，如果为空时返回根节点的数据，不为空时返回该范围下的子节点
        /// </summary>
        public string? ParentScopeCode { get; set; }


        /// <summary>
        /// 初始范围编码，如果存在返回该范围的父级及兄弟节点，此时不用考虑 ParentScopeCode
        /// </summary>
        public string? InitialScopeCode { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        public ScopeType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? Id { get; set; }
    }
}
