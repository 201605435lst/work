using SnAbp.FileApprove.Enums;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.FileApprove.Dto.FileApprove
*文件名：FileApproveProcessDto
*创建人： liushengtao
*创建时间：2021/9/1 10:12:02
*描述：
*
***********************************************************************/
namespace SnAbp.FileApprove.Dto
{
  public  class FileApproveProcessDto
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public Guid FileId { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 审批状态（pass 和unpass)
        /// </summary>
        public FileApprovalStatus Status { get; set; }
    }
}
