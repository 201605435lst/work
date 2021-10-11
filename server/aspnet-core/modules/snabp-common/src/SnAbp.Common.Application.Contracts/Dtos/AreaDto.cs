using SnAbp.Utils.TreeHelper;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Common.Dtos
{
    public class AreaDto : EntityDto<int>, ICodeTree<AreaDto>
    {

        public int? ParentId { get; set; }
        public virtual AreaDto Parent { get; set; }
        public virtual List<AreaDto> Children { get; set; } = new List<AreaDto>();

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 完整编码
        /// </summary>
        public string FullCode { get; set; }

        /// <summary>
        /// 层级深度；0：省，1：市，2：区，3：镇
        /// </summary>
        public int Deep { get; set; }

        /// <summary>
        /// 完整名称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称的完整拼音
        /// </summary>
        public string PinYin { get; set; }

        /// <summary>
        /// 名称的拼音前缀
        /// </summary>
        public string PinYinPrefix { get; set; }
    }
}
