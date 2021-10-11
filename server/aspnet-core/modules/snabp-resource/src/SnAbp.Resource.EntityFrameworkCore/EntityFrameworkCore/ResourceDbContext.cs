using Microsoft.EntityFrameworkCore;
using SnAbp.Resource.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Resource.EntityFrameworkCore
{
    [ConnectionStringName(ResourceDbProperties.ConnectionStringName)]
    public class ResourceDbContext : AbpDbContext<ResourceDbContext>, IResourceDbContext
    {

        public DbSet<CableCore> CableCore { get; set; }
        public DbSet<CableExtend> CableExtend { get; set; }
        public DbSet<CableLocation> CableLocation { get; set; }

        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentGroup> EquipmentGroup { get; set; }
        public DbSet<EquipmentProperty> EquipmentProperty { get; set; }
        public DbSet<EquipmentRltFile> EquipmentRltFile { get; set; }

        public DbSet<EquipmentRltOrganization> EquipmentRltOrganization { get; set; }
        public DbSet<EquipmentServiceRecord> EquipmentServiceRecord { get; set; }
        public DbSet<StoreEquipment> StoreEquipment { get; set; }
        public DbSet<StoreEquipmentTest> StoreEquipmentTest { get; set; }
        public DbSet<StoreEquipmentTestRltEquipment> StoreEquipmentTestRltEquipment { get; set; }
        public DbSet<StoreEquipmentTransfer> StoreEquipmentTransfer { get; set; }
        public DbSet<StoreEquipmentTransferRltEquipment> StoreEquipmentTransferRltEquipment { get; set; }
        public DbSet<StoreHouse> StoreHouse { get; set; }

        public DbSet<Terminal> Terminal { get; set; }
        public DbSet<TerminalLink> TerminalLink { get; set; }
        public DbSet<TerminalBusinessPath> TerminalBusinessPath { get; set; }
        public DbSet<TerminalBusinessPathNode> TerminalBusinessPathNode { get; set; }
        public  DbSet<ComponentRltQRCode> ComponentRltQRCode { get; set; }

        public DbSet<ComponentTrackRecord> ComponentTrackRecord { get; set; }

        public DbSet<OrganizationRltLayer> OrganizationRltLayer { get; set; }

        public ResourceDbContext(DbContextOptions<ResourceDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureResource();
        }
    }
}