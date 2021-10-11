/**********************************************************************
*******命名空间： Volo.Abp.Identity
*******类 名 称： SnAbpServiceConvention
*******类 说 明： 服务转换
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/5 16:26:20
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace SnAbp.Identity
{
    public class SnAbpServiceConvention : AbpServiceConvention
    {
        public SnAbpServiceConvention(IOptions<AbpAspNetCoreMvcOptions> options) : base(options)
        {

        }

        protected override string CalculateRouteTemplate(string rootPath, string controllerName, ActionModel action, string httpMethod, ConventionalControllerSetting configuration)
        {
            var controllerNameInUrl = NormalizeUrlControllerName(rootPath, controllerName, action, httpMethod, configuration);

            var url = $"api/{rootPath}/{controllerNameInUrl.ToCamelCase()}";

            //Add action name if needed
            var actionNameInUrl = NormalizeUrlActionName(rootPath, controllerName, action, httpMethod, configuration);
            if (!actionNameInUrl.IsNullOrEmpty())
            {
                url += $"/{actionNameInUrl.ToCamelCase()}";
            }

            return url;
        }

        protected override string NormalizeUrlActionName(string rootPath, string controllerName, ActionModel action, string httpMethod, ConventionalControllerSetting configuration)
        {
            //abp使用了一些替换规则，这里全部不要，就用actionName生成；
            //return base.NormalizeUrlActionName(rootPath, controllerName, action, httpMethod, configuration);
            return action.ActionName;
        }

    }
}
