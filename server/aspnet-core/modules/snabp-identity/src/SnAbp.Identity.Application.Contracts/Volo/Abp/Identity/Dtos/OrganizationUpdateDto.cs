/**********************************************************************
*******命名空间： Volo.Abp.Identity
*******类 名 称： IdentityOrganizationUpdateDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/5 11:18:44
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Identity
{
    public class OrganizationUpdateDto: EntityDto<Guid>
    {
        /// <summary>
        /// 全称 
        /// </summary>
        [Required] public string Name { get; set; }
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
        public Guid? TypeId { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }
        public string Remark { get; set; }

        public string SealImageUrl { get; set; }
    }
}
