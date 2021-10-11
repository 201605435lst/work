using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.DailyTemplate
*文件名：DailyTemplateSearchDto
*创建人： liushengtao
*创建时间：2021/7/30 13:45:03
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public  class DailyTemplateSearchDto : PagedAndSortedResultRequestDto
    {

        /// <summary>
        /// 模糊搜索 
        /// </summary>
        public string KeyWords { get; set; }
        /// <summary>
        /// 是否全部
        /// </summary>
        public bool IsAll { get; set; }

    }
}