using SnAbp.Bpm.Entities;
using SnAbp.FileApprove.Enums;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.FileApprove.Entities
*文件名：FileApprove
*创建人： liushengtao
*创建时间：2021/8/31 17:21:04
*描述：文件与工作流相关联
*
***********************************************************************/
namespace SnAbp.FileApprove
{
    public class FileApprove : SingleFlowEntity
    {
        public FileApprove() { }
        public FileApprove(Guid id) { Id = id; }
        public void SetId(Guid id) { Id = id; }
        /// <summary>
        /// 文件
        /// </summary>
        public Guid FileId { get; set; }
        public File.Entities.File File { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public FileApprovalStatus Status { get; set; }
    }
}
