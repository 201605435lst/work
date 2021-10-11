using SnAbp.Bpm.Entities;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Construction.Entities.Daily
*文件名：DailyFlowInfo
*创建人： liushengtao
*创建时间：2021/7/26 11:39:39
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Entities
{
   public class DailyFlowInfo : SingleFlowRltEntity
    {
        public DailyFlowInfo(Guid id) => Id = id;

        public virtual Guid DailyId { get; set; }
        public virtual Daily Daily { get; set; }
    }
}