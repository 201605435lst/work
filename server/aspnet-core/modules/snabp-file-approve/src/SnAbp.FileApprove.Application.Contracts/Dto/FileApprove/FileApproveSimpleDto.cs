using SnAbp.FileApprove.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.FileApprove.Dto.FileApprove
*文件名：FileApproveSimpleDto
*创建人： liushengtao
*创建时间：2021/8/31 17:46:47
*描述：
*
***********************************************************************/
namespace SnAbp.FileApprove.Dto
{
   public class FileApproveSimpleDto : EntityDto<Guid>
    {
        public Guid? WorkflowId { get; set; }
        public Guid? WorkflowTemplateId { get; set; }
        /// <summary>
        /// 所属文件
        /// </summary>
        public Guid FileId { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public FileApprovalStatus Status { get; set; }
    }
}
