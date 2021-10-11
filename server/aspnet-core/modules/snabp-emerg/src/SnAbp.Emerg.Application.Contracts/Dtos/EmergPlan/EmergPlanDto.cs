using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class EmergPlanDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预案等级Id
        /// </summary>
        public Guid LevelId { get; set; }

        /// <summary>
        /// 预案等级
        /// </summary>
        public DataDictionaryDto Level { get; set; }

        /// <summary>
        /// 构件分类
        /// </summary>
        public List<EmergPlanRltComponentCategorySimpleDto> EmergPlanRltComponentCategories { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 流程图
        /// </summary>
        public string Flow { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<EmergPlanRltFileSimpleDto> EmergPlanRltFiles { get; set; }

        /// <summary>
        /// 图文资料
        /// </summary>
        public string Content { get; set; }
    }
}
