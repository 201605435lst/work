using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NPOI.HPSF;
using SnAbp.Bpm.Entities;
using SnAbp.Identity;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowImformationDto
    {
        /// <summary>
        ///处理建议 
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime Time  { get; set; }

        /// <summary>
        /// 处理简报
        /// </summary>
        public List<WorkflowInfo> Infos { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeLabel { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public string NodeType { get; set; }

        /// <summary>
        /// 节点处理状态
        /// </summary>
        public WorkflowStepState? State { get; set; }

        /// <summary>
        /// 当前处理节点是否为判断节点
        /// </summary>
        public bool IsBpmApprove { get; set; }

        /// <summary>
        /// 当前处理节点是否为判断节点
        /// </summary>
        public bool IsBpmEnd { get; set; }

        /// <summary>
        /// 操作用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 待处理的用户信息
        /// </summary>
        public List<MemberDto> PendingUserInfos { get; set; }

        /// <summary>
        /// 操作用户名称
        /// </summary>
        public Guid? UserId { get; set; }
    }
}
