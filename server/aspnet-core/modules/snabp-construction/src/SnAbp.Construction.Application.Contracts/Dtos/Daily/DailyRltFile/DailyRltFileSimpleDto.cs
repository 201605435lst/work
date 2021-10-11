using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.DailyRltFile
*文件名：DailyRltFileSimpleDto
*创建人： liushengtao
*创建时间：2021/7/21 11:18:17
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public class DailyRltFileSimpleDto : EntityDto<Guid>
    {
        public virtual Guid DailyId { get; set; }
        public virtual Guid FileId { get; set; }
    }
}
