using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    [ConnectionStringName(SnAbpIdentityDbProperties.ConnectionStringName)]
    public class IdentityDbContext : AbpDbContext<IdentityDbContext>, IIdentityDbContext
    {
        public DbSet<IdentityUser> Users { get; set; }

        public DbSet<IdentityRole> Roles { get; set; }

        public DbSet<IdentityClaimType> ClaimTypes { get; set; }

        public DbSet<Organization> Organization { get; set; }
        public DbSet<DataDictionary> DataDictionary { get; set; }
        public DbSet<IdentityUserRltProject> IdentityUserRltProject { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureIdentity();
        }
    }
}