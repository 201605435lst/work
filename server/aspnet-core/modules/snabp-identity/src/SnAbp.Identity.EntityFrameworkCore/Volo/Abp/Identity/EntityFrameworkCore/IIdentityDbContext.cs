using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Identity.EntityFrameworkCore
{
    [ConnectionStringName(SnAbpIdentityDbProperties.ConnectionStringName)]
    public interface IIdentityDbContext : IEfCoreDbContext
    {
        DbSet<IdentityUser> Users { get; set; }

        DbSet<IdentityRole> Roles { get; set; }

        DbSet<IdentityClaimType> ClaimTypes { get; set; }

        DbSet<Organization> Organization { get; set; }
        DbSet<DataDictionary> DataDictionary { get; set; }
        DbSet<IdentityUserRltProject> IdentityUserRltProject { get; set; }
    }
}