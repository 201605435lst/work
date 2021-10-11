﻿using SnAbp.EntityFrameworkCore;

using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace SnAbp.EntityFrameworkCore.PostgreSql
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCorePostgreSqlModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                if (options.DefaultSequentialGuidType == null)
                {
                    options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
                }
            });
        }
    }
}
