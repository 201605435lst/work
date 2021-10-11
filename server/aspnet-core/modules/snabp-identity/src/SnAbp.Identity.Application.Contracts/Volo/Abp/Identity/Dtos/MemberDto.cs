/**********************************************************************
*******命名空间： Volo.Abp.Identity.Dtos
*******类 名 称： MemberDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/28 11:12:59
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Identity
{
    public class MemberDto
    {
        /// <summary>
        /// 成员 Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 成员类型
        /// </summary>
        public MemberType Type { get; set; }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string? Name { get; set; }
    }
}
