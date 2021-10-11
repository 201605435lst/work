using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.DailyRltQuality
*文件名：DailyRltQualitySimpleDto
*创建人： liushengtao
*创建时间：2021/7/21 11:19:05
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public class DailyRltQualitySimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid DailyId { get; set; }
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid QualityProblemId { get; set; }
    }
}
