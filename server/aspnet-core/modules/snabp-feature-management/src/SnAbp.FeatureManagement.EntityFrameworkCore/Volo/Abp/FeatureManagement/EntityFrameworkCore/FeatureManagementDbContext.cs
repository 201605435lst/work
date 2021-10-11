using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.FeatureManagement.EntityFrameworkCore
{
    [ConnectionStringName(FeatureManagementDbProperties.ConnectionStringName)]
    public class FeatureManagementDbContext : AbpDbContext<FeatureManagementDbContext>, IFeatureManagementDbContext
    {
        public DbSet<FeatureValue> FeatureValues { get; set; }

        public FeatureManagementDbContext(DbContextOptions<FeatureManagementDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFeatureManagement();
        }
    }
}