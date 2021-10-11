/**********************************************************************
*******命名空间： SnAbp.Construction.Daily
*******类 名 称： Daily
*******类 说 明： 施工日志实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 7/16/2021 9:31:04 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Bpm.Entities;
using SnAbp.Construction.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    ///  施工日志实体
    /// </summary>
    public class Daily : SingleFlowEntity
    {
        public Daily()
        {

        }
        public Daily(Guid id) => Id = id;
        public void SetId(Guid id) => Id = id;
        /// <summary>
        /// 日志编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid? DispatchId { get; set; }
        public virtual Dispatch  Dispatch  { get; set; }
        /// <summary>
        /// 日志模板
        /// </summary>
        public virtual Guid? DailyTemplateId { get; set; }
        public virtual DailyTemplate DailyTemplate { get; set; }
        /// <summary>
        /// 填报日期
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// 填报人
        /// </summary>
        public virtual IdentityUser Informant { get; set; }
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
        public int  BuilderCount{ get; set; }
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
        public List<UnplannedTask> UnplannedTask { get; set; }
        /// <summary>
        /// 存在的安全问题
        /// </summary>
        public List<DailyRltSafe> DailyRltSafe { get; set; }
        /// <summary>
        /// 存在的质量问题
        /// </summary>
        public List<DailyRltQuality> DailyRltQuality { get; set; }
        /// <summary>
        /// 施工现场照片
        /// </summary>
        public List<DailyRltFile> DailyRltFiles { get; set; }
        /// <summary>
        /// 施工任务
        /// </summary>
        public List<DailyRltPlanMaterial> DailyRltPlan { get; set; }
        /// <summary>
        /// 其他内容
        /// </summary>
        public string Remark { get; set; }
    }
}
