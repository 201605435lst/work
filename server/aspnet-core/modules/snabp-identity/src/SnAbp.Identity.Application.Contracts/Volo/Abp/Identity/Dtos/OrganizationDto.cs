/**********************************************************************
*******命名空间： Volo.Abp.Identity
*******类 名 称： OrganizationDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/5 11:23:36
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Identity
{
    public class OrganizationDto : EntityDto<Guid>, IGuidKeyTree<OrganizationDto>, ICodeTree<OrganizationDto>
    {
        /// <summary>
        /// 全称 
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 简介 
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// 铁路编码
        /// </summary>
        public string CSRGCode { get; set; }


        /// <summary>
        /// 系统编码 
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// 排序 
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否根级
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// 单位性质
        /// </summary>
        public string Nature { get; set; }


        /// <summary>
        /// 组织机构类型
        /// </summary>
        public DataDictionaryDto Type { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentId { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public OrganizationDto Parent { get; set; }

        List<OrganizationDto> children;
        /// <summary>
        /// 子集
        /// </summary>
        public List<OrganizationDto> Children { get; set; }

        public string Remark { get; set; }
        public OrganizationDto Parent { get; set; }

        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 是否为当前用户
        /// </summary>
        public bool isGranted { get; set; } = false;

        public string SealImageUrl { get; set; }
    }
}
