/**********************************************************************
*******命名空间： SnAbp.Construction.Entities.Daily
*******类 名 称： UnplannedTask
*******类 说 明： 计划外任务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 7/16/2021 9:34:58 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Construction.Enums;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    ///  计划外任务
    /// </summary>
    public class UnplannedTask : Entity<Guid>
    {
        public UnplannedTask(Guid id)
        {
            Id = id;

        }
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid DailyId { get; set; }
        public virtual Daily Daily { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public UnplannedTaskType TaskType { get; set; }
        /// <summary>
        /// 任务说明
        /// </summary>
        public string Content { get; set; }
    }
}
