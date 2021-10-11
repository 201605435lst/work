/**********************************************************************
*******命名空间： Volo.Abp.Identity
*******类 名 称： IdentityController
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/5 16:34:34
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.Identity.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Identity
{
    public class IdentityController : AbpController
    {
        protected IdentityController()
        {
            LocalizationResource = typeof(IdentityResource);
        }
    }
}
