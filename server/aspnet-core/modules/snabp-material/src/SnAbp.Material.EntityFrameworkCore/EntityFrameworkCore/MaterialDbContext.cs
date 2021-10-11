using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.EntityFrameworkCore;
using SnAbp.ConstructionBase.Entities;
using SnAbp.File.EntityFrameworkCore;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Material.Entities;
using SnAbp.Project.EntityFrameworkCore;
using SnAbp.Technology.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Material.EntityFrameworkCore
{
    [ConnectionStringName(MaterialDbProperties.ConnectionStringName)]
    public class MaterialDbContext : AbpDbContext<MaterialDbContext>, IMaterialDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierRltAccessory> SupplierRltAccessories { get; set; }
        public DbSet<SupplierRltContacts> SupplierRltContacts { get; set; }
        public DbSet<Partition> Partition { get; set; }

        //库存
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<PurchaseListRltPurchasePlan> PurchaseListRltPurchasePlan { get; set; }
        //入库记录
        public DbSet<EntryRecord> EntryRecord { get; set; }
        public DbSet<EntryRecordRltQRCode> EntryRecordRltQRCode { get; set; }
        public DbSet<EntryRecordRltFile> EntryRecordRltFile { get; set; }
        public DbSet<EntryRecordRltMaterial> EntryRecordRltMaterial { get; set; }

        //出库记录
        public DbSet<OutRecord> OutRecord { get; set; }
        public DbSet<OutRecordRltQRCode> OutRecordRltQRCode { get; set; }
        public DbSet<OutRecordRltFile> OutRecordRltFile { get; set; }
        public DbSet<OutRecordRltMaterial> OutRecordRltMaterial { get; set; }

        //采购计划
        public DbSet<PurchasePlan> PurchasePlan { get; set; }
        public DbSet<PurchasePlanRltFlow> PurchasePlanRltFlow { get; set; }
        public DbSet<PurchasePlanRltMaterial> PurchasePlanRltMaterial { get; set; }
        public DbSet<PurchasePlanRltFile> PurchasePlanRltFile { get; set; }

        //采购清单
        public DbSet<PurchaseList> PurchaseList { get; set; }
        public DbSet<PurchaseListRltFlow> PurchaseListRltFlow { get; set; }
        public DbSet<PurchaseListRltMaterial> PurchaseListRltMaterial { get; set; }
        public DbSet<PurchaseListRltFile> PurchaseListRltFile { get; set; }

        public DbSet<Contract> Contract { get; set; }
        public DbSet<ContractRltFile> ContractRltFile { get; set; }
        //public DbSet<ConstructionSection> ConstructionSection { get; set; }
        //public DbSet<ConstructionTeam> ConstructionTeam { get; set; }


        //物资验收
        public DbSet<MaterialAcceptance> MaterialAcceptance { get; set; }
        public DbSet<MaterialAcceptanceRltQRCode> MaterialAcceptanceRltQRCode { get; set; }
        public DbSet<MaterialAcceptanceRltMaterial> MaterialAcceptanceRltMaterial { get; set; }
        public DbSet<MaterialAcceptanceRltFile> MaterialAcceptanceRltFile { get; set; }
        public DbSet<MaterialAcceptanceRltPurchase> MaterialAcceptanceRltPurchase { get; set; }
        

        //领料单
        public DbSet<MaterialOfBill> MaterialOfBill { get; set; }
        public DbSet<MaterialOfBillRltAccessory> MaterialOfBillRltAccessory { get; set; }
        public DbSet<MaterialOfBillRltMaterial> MaterialOfBillRltMaterial { get; set; }

        public MaterialDbContext(DbContextOptions<MaterialDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureBpm();
            builder.ConfigureMaterial();
            builder.ConfigureProject();
            builder.ConfigureTechnology();
            builder.ConfigureFile();
            builder.ConfigureIdentity();
        }
    }
}