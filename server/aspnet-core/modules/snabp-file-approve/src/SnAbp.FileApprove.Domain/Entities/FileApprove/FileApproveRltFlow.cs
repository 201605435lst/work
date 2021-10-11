using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.FileApprove.Entities
*文件名：FileApproveRltFlow
*创建人： liushengtao
*创建时间：2021/8/31 17:00:35
*描述：文件与工作流相关联的审批流程
*
***********************************************************************/
namespace SnAbp.FileApprove.Entities
{
  public  class FileApproveRltFlow : SingleFlowRltEntity
    {
        public FileApproveRltFlow(Guid id) => Id = id;
        public virtual Guid FileApproveId { get; set; }
        public virtual FileApprove FileApprove { get; set; }
    }
}
