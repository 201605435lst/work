using Microsoft.EntityFrameworkCore;
using SnAbp.ConstructionBase.Entities;
using SnAbp.Material.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Material.EntityFrameworkCore
{
    [ConnectionStringName(MaterialDbProperties.ConnectionStringName)]
    public interface IMaterialDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<Supplier> Suppliers { get; set; }
        DbSet<SupplierRltAccessory> SupplierRltAccessories { get; set; }
        DbSet<SupplierRltContacts> SupplierRltContacts { get; set; }
        DbSet<Partition> Partition { get; set; }

        //库存
        DbSet<Inventory> Inventory { get; set; }

        //入库记录
        DbSet<EntryRecord> EntryRecord { get; set; }
        DbSet<EntryRecordRltFile> EntryRecordRltFile { get; set; }
        DbSet<EntryRecordRltMaterial> EntryRecordRltMaterial { get; set; }
        DbSet<EntryRecordRltQRCode> EntryRecordRltQRCode { get; set; }

        //出库记录
        DbSet<OutRecord> OutRecord { get; set; }
        DbSet<OutRecordRltFile> OutRecordRltFile { get; set; }
        DbSet<OutRecordRltMaterial> OutRecordRltMaterial { get; set; }
        DbSet<OutRecordRltQRCode> OutRecordRltQRCode { get; set; }

        //采购计划
        DbSet<PurchasePlan> PurchasePlan { get; set; }
        DbSet<PurchasePlanRltFlow> PurchasePlanRltFlow { get; set; }
        DbSet<PurchasePlanRltMaterial> PurchasePlanRltMaterial { get; set; }
        DbSet<PurchasePlanRltFile> PurchasePlanRltFile { get; set; }

        //采购清单
        DbSet<PurchaseList> PurchaseList { get; set; }
        DbSet<PurchaseListRltFlow> PurchaseListRltFlow { get; set; }
        DbSet<PurchaseListRltMaterial> PurchaseListRltMaterial { get; set; }
        DbSet<PurchaseListRltFile> PurchaseListRltFile { get; set; }
        
        DbSet<PurchaseListRltPurchasePlan> PurchaseListRltPurchasePlan { get; set; }

        DbSet<Contract> Contract { get; set; }
        DbSet<ContractRltFile> ContractRltFile { get; set; }

        //DbSet<ConstructionSection> ConstructionSection { get; set; }
        //DbSet<ConstructionTeam> ConstructionTeam { get; set; }


        //物资验收
        DbSet<MaterialAcceptance> MaterialAcceptance { get; set; }
        DbSet<MaterialAcceptanceRltMaterial> MaterialAcceptanceRltMaterial { get; set; }
        DbSet<MaterialAcceptanceRltFile> MaterialAcceptanceRltFile { get; set; }
        DbSet<MaterialAcceptanceRltQRCode> MaterialAcceptanceRltQRCode { get; set; }
        DbSet<MaterialAcceptanceRltPurchase> MaterialAcceptanceRltPurchase { get; set; }


        DbSet<MaterialOfBill> MaterialOfBill { get; set; }
        DbSet<MaterialOfBillRltAccessory> MaterialOfBillRltAccessory { get; set; }
        DbSet<MaterialOfBillRltMaterial> MaterialOfBillRltMaterial { get; set; }
    }
}