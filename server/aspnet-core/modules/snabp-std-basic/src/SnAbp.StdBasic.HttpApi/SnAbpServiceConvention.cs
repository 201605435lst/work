using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace SnAbp.StdBasic
{
    public class SnAbpServiceConvention: AbpServiceConvention
    {
        public SnAbpServiceConvention(IOptions<AbpAspNetCoreMvcOptions> options):base(options)
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

                ////Add secondary Id
                //var secondaryIds = action.Parameters.Where(p => p.ParameterName.EndsWith("Id", StringComparison.Ordinal)).ToList();
                //if (secondaryIds.Count == 1)
                //{
                //    url += $"/{{{secondaryIds[0].ParameterName}}}";
                //}
            }

            ////Add {id} path if needed
            //if (action.Parameters.Any(p => p.ParameterName == "id"))
            //{
            //    url += "/{id}";
            //}


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
