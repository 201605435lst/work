using SnAbp.Construction.Entities;
using SnAbp.Construction.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.UnplannedTask
*文件名：UnplannedTaskDto
*创建人： liushengtao
*创建时间：2021/7/21 11:23:56
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
   public class UnplannedTaskDto : EntityDto<Guid>
    {
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
