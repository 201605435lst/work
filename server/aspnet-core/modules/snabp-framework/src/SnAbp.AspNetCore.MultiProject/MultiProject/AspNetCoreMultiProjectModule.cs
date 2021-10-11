using SnAbp.MultiProject;
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;

namespace SnAbp.AspNetCore.MultiProject.MultiProject
{
    [DependsOn(
          typeof(MultiProjectModule),
          typeof(AbpAspNetCoreModule)
          )]
    public class AspNetCoreMultiProjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<ProjectResolveOptions>(options =>
            {
                options.ProjectResolvers.Add(new QueryStringProjectResolveContributor());
            });

            //base.ConfigureServices(context);
        }
    }
}
