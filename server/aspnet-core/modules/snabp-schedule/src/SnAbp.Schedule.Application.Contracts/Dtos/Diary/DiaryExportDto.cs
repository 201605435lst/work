using SnAbp.Identity;
using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class DiaryExportDto : EntityDto<Guid>
    {
        /// <summary>
        /// 日志编号
        /// </summary>
        public string DiaryCode { get; set; }
        /// <summary>
        /// 施工单位
        /// </summary>
        public string Organization { get; set; }
        /// <summary>
        /// 施工日期
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 填报日期
        /// </summary>
        public DateTime FillTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StatusType State { get; set; }
        public string Weathers { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public virtual DataDictionary Profession { get; set; }

        /// <summary>
        /// 施工部位
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Directors { get; set; }
        /// <summary>
        /// 施工员
        /// </summary>
        public string Builders { get; set; }
        public List<DiaryRltBuilderDto> DirectorsRltBuilders { get; set; } = new List<DiaryRltBuilderDto>();
        public int MemberNum { get; set; }
        public int ReaLMemberNum { get; set; }
        /// <summary>
        /// 施工描述
        /// </summary>
        public string Discription { get; set; }
        /// <summary>
        /// 存在的问题
        /// </summary>
        public string Problem { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
