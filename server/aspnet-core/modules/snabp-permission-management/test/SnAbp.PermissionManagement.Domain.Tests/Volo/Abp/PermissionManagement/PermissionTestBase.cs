﻿using System;
using SnAbp.PermissionManagement.EntityFrameworkCore;

namespace SnAbp.PermissionManagement
{
    public abstract class PermissionTestBase : PermissionManagementTestBase<AbpPermissionManagementTestModule>
    {
        protected virtual void UsingDbContext(Action<IPermissionManagementDbContext> action)
        {
            using (var dbContext = GetRequiredService<IPermissionManagementDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<IPermissionManagementDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<IPermissionManagementDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }
    }
}
