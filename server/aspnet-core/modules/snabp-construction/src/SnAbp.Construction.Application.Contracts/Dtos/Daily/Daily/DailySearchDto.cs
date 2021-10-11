using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.Daily
*文件名：DailySearchDto
*创建人： liushengtao
*创建时间：2021/7/21 11:16:31
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public class DailySearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否全部显示
        /// </summary>
        public bool IsAll { get; set; }
        /// <summary>
        /// 是否查询审批数据（根据此字段过滤）
        /// </summary>
        public bool Approval { get; set; }
        /// <summary>
        /// 是否获取待我审批的数据
        /// </summary>
        public bool Waiting { get; set; }
    }
}
