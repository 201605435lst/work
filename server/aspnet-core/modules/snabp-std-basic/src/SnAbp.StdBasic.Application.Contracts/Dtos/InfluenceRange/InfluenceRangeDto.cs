using SnAbp.StdBasic.Enums;
using SnAbp.Utils.EnumHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class InfluenceRangeDto : EntityDto<Guid>
    {
        /// <summary>
        /// 维修级别
        /// </summary>
        public SkylightPlanRepairLevel RepairLevel { get; set; }

        public string RepairLevelStr
        {
            get { return EnumHelper.GetDescription(RepairLevel); }
        }


        /// <summary>
        /// 影响范围内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public Guid? TagId { get; set; }
    }
}
