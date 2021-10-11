using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model.ModelMVD
{
    public class MVDSearchDto : PagedAndSortedResultRequestDto
    {

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 是否获取所有
        /// </summary>
        public bool IsAll { get; set; }

        /// <summary>
        /// 信息交换模板分类Id
        /// </summary>
        public Guid? mvdCategoryId { get; set; }

        public List<Guid>? Ids { get; set; }
    }
}
