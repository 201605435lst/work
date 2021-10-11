﻿using SnAbp.Schedule.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Schedule
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(ScheduleEntityFrameworkCoreTestModule)
        )]
    public class ScheduleDomainTestModule : AbpModule
    {
        
    }
}
