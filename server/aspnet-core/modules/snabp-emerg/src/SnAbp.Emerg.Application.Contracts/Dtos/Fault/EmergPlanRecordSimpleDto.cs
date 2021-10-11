using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class EmergPlanRecordSimpleDto:EntityDto<Guid>
    {
        /// <summary>
        /// 预案名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预案等级id
        /// </summary>
        public Guid LevelId { get; set; }

        /// <summary>
        /// 预案等级
        /// </summary>
        public DataDictionaryGetDto Level { get; set; }


        /// <summary>
        /// 构件分类
        /// </summary>
        public List<string> EmergPlanRecordRltComponentCategories { get; set; }

        /// <summary>
        /// 预案摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 预案流程
        /// </summary>
        public string Flow { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> EmergPlanRecordRltFiles { get; set; }

        /// <summary>
        /// 图文资料
        /// </summary>
        public string Content { get; set; }
    }
}
