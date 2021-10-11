/**********************************************************************
*******命名空间： SnAbp.Bpm.Dtos
*******类 名 称： SingleFlow
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/15 15:10:48
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;

namespace SnAbp.Bpm.Dtos
{
    /// <summary>
    /// $$
    /// </summary>
    public class SingleFlowNodeDto : IGuidKeyTree<SingleFlowNodeDto>
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 节点的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 节点是否激活
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// 节点的类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 改节点的审批人
        /// </summary>
        public List<IdentityUserDto> Approvers { get; set; }

        /// <summary>
        /// 节点的审批意见,考虑到了一个节点多个人的情况
        /// </summary>
        public List<CommentDto> Comments { get; set; }
        public SingleFlowNodeDto Parent { get; set; }
        public List<SingleFlowNodeDto> Children { get; set; }
    }

    /// <summary>
    /// 审批意见
    /// </summary>
    public class CommentDto
    {
        public IdentityUserDto User { get; set; }
        public string Content { get; set; }
        public DateTime ApproveTime { get; set; }
        public WorkflowState State { get; set; }
    }
}
