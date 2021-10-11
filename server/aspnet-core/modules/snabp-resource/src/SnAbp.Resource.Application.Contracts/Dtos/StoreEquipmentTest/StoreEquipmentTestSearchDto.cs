using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentTestSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public Guid? StoreHouseId { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        public Guid? OrganizationId { get; set; }
        /// <summary>
        /// 是否合格
        /// </summary>
        public bool ? Passed { get; set; }

        /// <summary>
        /// 检测单编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }
}
