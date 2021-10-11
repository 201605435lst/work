/**********************************************************************
*******命名空间： Volo.Abp.Identity.Dtos
*******类 名 称： DataDictionaryCreateDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/18 14:26:08
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Identity
{
    public class DataDictionaryCreateDto:EntityDto
    {
        /// <summary>
        /// 字典值
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否系统使用
        /// </summary>
        public bool IsStatic { get; set; }
    }
}
