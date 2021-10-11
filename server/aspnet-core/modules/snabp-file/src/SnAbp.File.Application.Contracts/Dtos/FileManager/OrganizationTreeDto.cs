/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： OrganizationDto
*******类 说 明： 文件管理-->组织机构+文件夹树结构，数据节点既是组织，又是文件夹
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 9:31:48
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.File.Dtos
{
    public class OrganizationTreeDto:EntityDto<Guid>
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节点id
        /// </summary>
        /// public  Guid NodeId { get; set; }

        /// <summary>
        /// 父节点id
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 是否分享
        /// </summary>
        public bool IsShare { get; set; }

        /// <summary>
        /// 节点类型，具体查看<see cref="ResourceNodeType"/>
        /// </summary>
        public ResourceNodeType Type { get; set; }

        /// <summary>
        /// 字段，存储当类型是文件夹时的文件夹路径
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<OrganizationTreeDto> Children { get; set; }
    }
}
