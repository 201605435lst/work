/**********************************************************************
*******命名空间： Volo.Abp.Account
*******类 名 称： ResetDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/25 14:13:01
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SnAbp.Identity;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace SnAbp.Account
{
    public class ResetDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required] public Guid UserId { get; set; }

        /// <summary>
        /// 需要重置的密码
        /// </summary>
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }
    }
}
