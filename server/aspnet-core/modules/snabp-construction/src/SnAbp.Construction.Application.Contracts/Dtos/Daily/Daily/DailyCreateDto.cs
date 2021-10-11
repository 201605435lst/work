using SnAbp.Construction.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.Daily
*文件名：DailyCreateDto
*创建人： liushengtao
*创建时间：2021/7/21 11:15:53
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public class DailyCreateDto : EntityDto<Guid>   
    {
        /// <summary>
        /// 日志编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid? DispatchId { get; set; }

        /// <summary>
        /// 填报日期
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// 填报人
        /// </summary>
        public virtual Guid? InformantId { get; set; }
        /// <summary>
        /// 天气
        /// </summary>
        public string Weathers { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }
        /// <summary>
        /// 风力风向
        /// </summary>
        public string WindDirection { get; set; }
        /// <summary>
        /// 空气质量
        /// </summary>
        public string AirQuality { get; set; }
        /// <summary>
        /// 施工班组
        /// </summary>
        public string Team { get; set; }
        /// <summary>
        /// 施工人员
        /// </summary>
        public int BuilderCount { get; set; }
        /// <summary>
        /// 审核状态（审核状态）
        /// </summary>
        public DailyStatus Status { get; set; }
        /// <summary>
        /// 施工部位
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 施工总结
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 临时任务
        /// </summary>
        public List<UnplannedTaskSimpleDto> UnplannedTask { get; set; } = new List<UnplannedTaskSimpleDto>();
        /// <summary>
        /// 存在的安全问题
        /// </summary>
        public List<DailyRltSafeSimpleDto> DailyRltSafe { get; set; } = new List<DailyRltSafeSimpleDto>();
        /// <summary>
        /// 存在的质量问题
        /// </summary>
        public List<DailyRltQualitySimpleDto> DailyRltQuality { get; set; } = new List<DailyRltQualitySimpleDto>();
        /// <summary>
        /// 施工现场照片
        /// </summary>
        public List<DailyRltFileSimpleDto> DailyRltFiles { get; set; } = new List<DailyRltFileSimpleDto>();
        /// <summary>
        /// 施工任务
        /// </summary>
        public List<DailyRltPlanMaterialSimpleDto> DailyRltPlan { get; set; } = new List<DailyRltPlanMaterialSimpleDto>();
        /// <summary>
        /// 其他内容
        /// </summary>
        public string Remark { get; set; }
    }
}
