using SnAbp.File.Dtos;
using SnAbp.FileApprove.Enums;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.FileApprove.Dto.FileApprove
*文件名：FileRltFileApproveDto
*创建人： liushengtao
*创建时间：2021/9/2 11:49:55
*描述：
*
***********************************************************************/
namespace SnAbp.FileApprove.Dto
{
   public class FileRltFileApproveDto: ResourceDto
    {
        public Guid WorkflowId { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public FileApprovalStatus Status { get; set; }
    }
}
