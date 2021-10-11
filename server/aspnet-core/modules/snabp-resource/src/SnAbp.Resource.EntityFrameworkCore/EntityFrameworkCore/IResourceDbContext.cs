using Microsoft.EntityFrameworkCore;
using SnAbp.Resource.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Resource.EntityFrameworkCore
{
    [ConnectionStringName(ResourceDbProperties.ConnectionStringName)]
    public interface IResourceDbContext : IEfCoreDbContext
    {
        DbSet<CableCore> CableCore { get; set; }
        DbSet<CableExtend> CableExtend { get; set; }
        DbSet<CableLocation> CableLocation { get; set; }

        DbSet<Equipment> Equipment { get; set; }
        DbSet<EquipmentGroup> EquipmentGroup { get; set; }
        DbSet<EquipmentProperty> EquipmentProperty { get; set; }
        DbSet<EquipmentRltFile> EquipmentRltFile { get; set; }

        DbSet<EquipmentRltOrganization> EquipmentRltOrganization { get; set; }
        DbSet<EquipmentServiceRecord> EquipmentServiceRecord { get; set; }
        DbSet<StoreEquipment> StoreEquipment { get; set; }
        DbSet<StoreEquipmentTest> StoreEquipmentTest { get; set; }
        DbSet<StoreEquipmentTestRltEquipment> StoreEquipmentTestRltEquipment { get; set; }
        DbSet<StoreEquipmentTransfer> StoreEquipmentTransfer { get; set; }
        DbSet<StoreEquipmentTransferRltEquipment> StoreEquipmentTransferRltEquipment { get; set; }
        DbSet<StoreHouse> StoreHouse { get; set; }

        DbSet<Terminal> Terminal { get; set; }
        DbSet<TerminalLink> TerminalLink { get; set; }
        DbSet<TerminalBusinessPath> TerminalBusinessPath { get; set; }
        DbSet<TerminalBusinessPathNode> TerminalBusinessPathNode { get; set; }
        DbSet<ComponentRltQRCode> ComponentRltQRCode { get; set; }

        DbSet<ComponentTrackRecord> ComponentTrackRecord { get; set; }

        DbSet<OrganizationRltLayer> OrganizationRltLayer { get; set; }

    }
}