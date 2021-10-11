using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

using SnAbp.EntityFrameworkCore.DependencyInjection;

namespace SnAbp.EntityFrameworkCore.PostgreSql
{
    public static class AbpDbContextConfigurationContextPostgreSqlExtensions
    {
        [Obsolete("Use 'UseNpgsql(...)' method instead. This will be removed in future versions.")]
        public static DbContextOptionsBuilder UsePostgreSql(
            [NotNull] this AbpDbContextConfigurationContext context,
            [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> postgreSqlOptionsAction = null)
        {
            return context.UseNpgsql(postgreSqlOptionsAction);
        }

        public static DbContextOptionsBuilder UseNpgsql(
            [NotNull] this AbpDbContextConfigurationContext context,
            [CanBeNull] Action<NpgsqlDbContextOptionsBuilder> postgreSqlOptionsAction = null)
        {
            if (context.ExistingConnection != null)
            {
                return context.DbContextOptions.UseNpgsql(context.ExistingConnection, postgreSqlOptionsAction);
            }
            else
            {
                return context.DbContextOptions.UseNpgsql(context.ConnectionString, postgreSqlOptionsAction);
            }
        }
    }
}
