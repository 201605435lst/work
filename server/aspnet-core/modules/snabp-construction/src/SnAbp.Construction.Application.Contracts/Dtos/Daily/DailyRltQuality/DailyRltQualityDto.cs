using SnAbp.Construction.Entities;
using SnAbp.Quality.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.DailyRltQuality
*文件名：DailyRltQualityDto
*创建人： liushengtao
*创建时间：2021/7/21 11:18:34
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public class DailyRltQualityDto : EntityDto<Guid>
    {
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid DailyId { get; set; }
        public virtual Daily Daily { get; set; }
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid QualityProblemId { get; set; }
        public virtual QualityProblem QualityProblem { get; set; }
    }
}
