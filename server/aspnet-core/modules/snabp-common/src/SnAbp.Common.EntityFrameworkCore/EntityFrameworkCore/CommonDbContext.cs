using Microsoft.EntityFrameworkCore;

using SnAbp.Common.Entities;
//using SnAbp.Common.Eitities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Common.EntityFrameworkCore
{
    [ConnectionStringName(CommonDbProperties.ConnectionStringName)]
    public class CommonDbContext : AbpDbContext<CommonDbContext>, ICommonDbContext
    {
        public DbSet<Entities.Area> Area { get; set; }
        public DbSet<QRCode> QRCode { get; set; }
        public CommonDbContext(DbContextOptions<CommonDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCommon();
        }
    }
}