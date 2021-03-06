using System;
using JetBrains.Annotations;
using SnAbp.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.IdentityServer.EntityFrameworkCore
{
    public class IdentityServerModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        [Obsolete("No need to manually set database provider after v2.9+. If it doesn't set automatically, use modelBuilder.UseXXX() in the OnModelCreating method of your DbContext (XXX is your database provider name).")]
        public EfCoreDatabaseProvider? DatabaseProvider { get; set; }

        public IdentityServerModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}