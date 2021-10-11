using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Basic.Enums;
using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class GetListByScopeOutDto : EntityDto<Guid>, IGuidKeyTree<GetListByScopeOutDto>
    {
        /// <summary>
        /// 父级
        /// </summary>
        public virtual Guid? ParentId { get; set; }
        public virtual GetListByScopeOutDto Parent { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        public virtual List<GetListByScopeOutDto> Children { get; set; }

        /// <summary>
        /// Id对应的name值
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ScopeType Type { get; set; }


        /// <summary>
        /// 范围编码
        /// </summary>
        public string ScopeCode { get; set; }

        public static implicit operator List<object>(GetListByScopeOutDto v)
        {
            throw new NotImplementedException();
        }
    }
}
