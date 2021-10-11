/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： ResourceSearchInput
*******类 说 明： 资源搜索输入条件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 15:15:07
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class ResourceSearchInput:EntityDto<Guid>
    {
        /// <summary>
        /// 文件名称，支持模糊查询
        /// </summary>
        [Required]public string Name { get; set; }

        /// <summary>
        /// 树节点id，可以是树，可以是文件夹
        /// </summary>
        [Required] public Guid NodeId { get; set; }

        /// <summary>
        /// 节点类型，具体查看：<see cref="ResourceNodeType"/>
        /// </summary>
        [Required] public ResourceNodeType NodeType { get; set; }

        /// <summary>
        /// 分页数
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页显示的条数
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 标签id
        /// </summary>
        public Guid TagId { get; set; }

        /// <summary>
        /// 是否是审批状态
        /// </summary>
        public bool IsApprove { get; set; }
        public string StaticKey { get; set; }
    }
}
