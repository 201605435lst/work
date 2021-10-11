using System;

using Microsoft.EntityFrameworkCore;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.Settings;
using SnAbp.Material.Entities;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Settings;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Material.EntityFrameworkCore
{
    public static class MaterialDbContextModelCreatingExtensions
    {
        public static void ConfigureMaterial(
            this ModelBuilder builder,
            Action<MaterialModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new MaterialModelBuilderConfigurationOptions(
                MaterialDbProperties.DbTablePrefix,
                MaterialDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
                b.ConfigureByConvention();
            
                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */

            // 供应商管理
            builder.Entity<Supplier>(b => b.ToTable(options.TablePrefix + nameof(Supplier), options.Schema));
            builder.Entity<SupplierRltAccessory>(b => b.ToTable(options.TablePrefix + nameof(SupplierRltAccessory), options.Schema));
            builder.Entity<SupplierRltContacts>(b => b.ToTable(options.TablePrefix + nameof(SupplierRltContacts), options.Schema));

            //料库管理-分区管理
            builder.Entity<Partition>(b => b.ToTable(options.TablePrefix + nameof(Partition), options.Schema));

            //采购计划
            builder.Entity<PurchasePlan>(b => b.ToTable(options.TablePrefix + nameof(PurchasePlan), options.Schema).ConfigureByConvention());
            builder.Entity<PurchasePlanRltFile>(b => b.ToTable(options.TablePrefix + nameof(PurchasePlanRltFile), options.Schema).ConfigureByConvention());
            builder.Entity<PurchasePlanRltFlow>(b => b.ToTable(options.TablePrefix + nameof(PurchasePlanRltFlow), options.Schema).ConfigureByConvention());
            builder.Entity<PurchasePlanRltMaterial>(b => b.ToTable(options.TablePrefix + nameof(PurchasePlanRltMaterial), options.Schema).ConfigureByConvention());

            //采购清单
            builder.Entity<PurchaseList>(b => b.ToTable(options.TablePrefix + nameof(PurchaseList), options.Schema).ConfigureByConvention());
            builder.Entity<PurchaseListRltFile>(b => b.ToTable(options.TablePrefix + nameof(PurchaseListRltFile), options.Schema).ConfigureByConvention());
            builder.Entity<PurchaseListRltFlow>(b => b.ToTable(options.TablePrefix + nameof(PurchaseListRltFlow), options.Schema).ConfigureByConvention());
            builder.Entity<PurchaseListRltMaterial>(b => b.ToTable(options.TablePrefix + nameof(PurchaseListRltMaterial), options.Schema).ConfigureByConvention());
            builder.Entity<PurchaseListRltPurchasePlan>(b => b.ToTable(options.TablePrefix + nameof(PurchaseListRltPurchasePlan), options.Schema).ConfigureByConvention());




            //料库管理-库存管理
            builder.Entity<Inventory>(b => b.ToTable(options.TablePrefix + nameof(Inventory), options.Schema).ConfigureByConvention());

            //入库记录
            builder.Entity<EntryRecord>(b => b.ToTable(options.TablePrefix + nameof(EntryRecord), options.Schema).ConfigureByConvention());
            builder.Entity<EntryRecordRltQRCode>(b => b.ToTable(options.TablePrefix + nameof(EntryRecordRltQRCode), options.Schema).ConfigureByConvention());
            builder.Entity<EntryRecordRltFile>(b => b.ToTable(options.TablePrefix + nameof(EntryRecordRltFile), options.Schema).ConfigureByConvention());
            builder.Entity<EntryRecordRltMaterial>(b => b.ToTable(options.TablePrefix + nameof(EntryRecordRltMaterial), options.Schema).ConfigureByConvention());

            //出库记录
            builder.Entity<OutRecord>(b => b.ToTable(options.TablePrefix + nameof(OutRecord), options.Schema).ConfigureByConvention());
            builder.Entity<OutRecordRltQRCode>(b => b.ToTable(options.TablePrefix + nameof(OutRecordRltQRCode), options.Schema).ConfigureByConvention());
            builder.Entity<OutRecordRltFile>(b => b.ToTable(options.TablePrefix + nameof(OutRecordRltFile), options.Schema).ConfigureByConvention());
            builder.Entity<OutRecordRltMaterial>(b => b.ToTable(options.TablePrefix + nameof(OutRecordRltMaterial), options.Schema).ConfigureByConvention());

            // 物资合同
            builder.Entity<Contract>(b => b.ToTable(options.TablePrefix + nameof(Contract), options.Schema).ConfigureByConvention());
            builder.Entity<ContractRltFile>(b => b.ToTable(options.TablePrefix + nameof(ContractRltFile), options.Schema).ConfigureByConvention());
            

            builder.Entity<Equipment>(b =>
            {
                b.ToTable(ResourceSettings.DbTablePrefix + nameof(Equipment), ResourceSettings.DbSchema);
            });

            //物资验收
            builder.Entity<MaterialAcceptance>(b => b.ToTable(options.TablePrefix + nameof(MaterialAcceptance), options.Schema).ConfigureFullAudited());
            builder.Entity<MaterialAcceptanceRltMaterial>(b => b.ToTable(options.TablePrefix + nameof(MaterialAcceptanceRltMaterial), options.Schema).ConfigureAudited());
            builder.Entity<MaterialAcceptanceRltFile>(b => b.ToTable(options.TablePrefix + nameof(MaterialAcceptanceRltFile), options.Schema).ConfigureAudited());
            builder.Entity<MaterialAcceptanceRltQRCode>(b => b.ToTable(options.TablePrefix + nameof(MaterialAcceptanceRltQRCode), options.Schema).ConfigureByConvention());
            builder.Entity<MaterialAcceptanceRltPurchase>(b => b.ToTable(options.TablePrefix + nameof(MaterialAcceptanceRltPurchase), options.Schema).ConfigureByConvention());
            
            //领料单
            builder.Entity<MaterialOfBill>(b => b.ToTable(options.TablePrefix + nameof(MaterialOfBill), options.Schema).ConfigureFullAudited());
            //领料单-附件
            builder.Entity<MaterialOfBillRltAccessory>(b => b.ToTable(options.TablePrefix + nameof(MaterialOfBillRltAccessory), options.Schema).ConfigureByConvention());
            //领料单-材料
            builder.Entity<MaterialOfBillRltMaterial>(b => b.ToTable(options.TablePrefix + nameof(MaterialOfBillRltMaterial), options.Schema).ConfigureByConvention());

            //领料单-施工区段
            builder.Entity<Section>(b => b.ToTable(ConstructionBaseSettings.DbTablePrefix + nameof(Section), ConstructionBaseSettings.DbSchema).ConfigureByConvention());
        }
    }
}