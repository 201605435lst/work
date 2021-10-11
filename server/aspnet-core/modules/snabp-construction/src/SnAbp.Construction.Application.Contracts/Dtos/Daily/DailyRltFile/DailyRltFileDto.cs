using SnAbp.Construction.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.DailyRltFile
*文件名：DailyRltFileDto
*创建人： liushengtao
*创建时间：2021/7/21 11:16:47
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public  class DailyRltFileDto : EntityDto<Guid>
    {
        public virtual Guid DailyId { get; set; }
        public virtual Daily Daily { get; set; }
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }
    }
}
