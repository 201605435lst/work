using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.DailyRltSafe
*文件名：DailyRltSafeSimpleDto
*创建人： liushengtao
*创建时间：2021/7/21 11:22:06
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public class DailyRltSafeSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 施工日志
        /// </summary>
        public virtual Guid DailyId { get; set; }
        /// <summary>
        /// 安全问题
        /// </summary>
        public virtual Guid SafeProblemId { get; set; }
    }
}
