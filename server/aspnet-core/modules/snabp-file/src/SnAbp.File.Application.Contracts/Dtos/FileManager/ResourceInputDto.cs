/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： ResourceInputDto
*******类 说 明： 资源查询条件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 17:53:55
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class ResourceInputDto:EntityDto<Guid>
    {
        /// <summary>
        /// id的类型
        /// <code>组织机构id：Type=1</code>
        /// <code>文件夹id：Type=2</code>
        /// <code>标签Id：Type=3</code>
        /// <code>审批Id：Type=4</code>
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 分页数
        /// </summary>
      
        public int Page { get; set; }
        
        /// <summary>
        /// 每页显示的条数
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 是否为“我的”，获取私有的文件列表
        /// </summary>
        public bool IsMine { get; set; }

        /// <summary>
        /// 共享的数据
        /// </summary>
        public  bool IsShare { get; set; }

        /// <summary>
        /// 是否被删除，用于回收站获取文件列表
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 是否是审批状态
        /// </summary>
        public bool IsApprove { get; set; }
        public string StaticKey { get; set; }

        /// <summary>
        /// 是否查询审批数据（根据此字段过滤）
        /// </summary>
        public bool Approval { get; set; }
        /// <summary>
        /// 是否获取待我审批的数据
        /// </summary>
        public bool Waiting { get; set; }
        /// <summary>
        /// 文件名称，支持模糊查询
        /// </summary>
         public string Name { get; set; }
    }
}
