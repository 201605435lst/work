using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class MaterialAcceptanceSearchDto :PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 检测机构（数据字典）
        /// </summary>
        public virtual Guid TestingOrganizationId { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 检测类型
        /// </summary>
        public TestingType ? TestingType { get; set; }
        /// 检测状态
        /// </summary>
        public TestingStatus ? TestingStatus { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
