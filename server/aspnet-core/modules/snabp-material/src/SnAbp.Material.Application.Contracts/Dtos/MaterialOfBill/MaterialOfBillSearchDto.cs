using SnAbp.Material.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public  class MaterialOfBillSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 施工队
        /// </summary>
        public string ConstructionTeam { get; set; }

        /// <summary>
        /// 施工区段Id
        /// </summary>
        public Guid? SectionId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public MaterialOfBillState? State { get; set; }

        /// <summary>
        ///后台审核数据列表
        /// </summary>
        public bool IsChecking { get; set; }

        /// <summary>
        /// 全部
        /// </summary>
        public bool IsAll { get; set; }
    }
}
