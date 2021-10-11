using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Material.Entities;
using System;
using SnAbp.Material.Repository;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Material.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule),
        typeof(MaterialDomainModule)

    )]
    public class MaterialEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MaterialDbContext>(options =>
            {
                options.AddDefaultRepositories<IMaterialDbContext>(true);
                //options.AddRepository<Supplier, EfCoreRepository<MaterialDbContext, Supplier, Guid>>();
                //options.AddRepository<SupplierRltAccessory, EfCoreRepository<MaterialDbContext, SupplierRltAccessory, Guid>>();
                //options.AddRepository<SupplierRltContacts, EfCoreRepository<MaterialDbContext, SupplierRltContacts, Guid>>();
                //options.AddRepository<Partition, EfCoreRepository<MaterialDbContext, Partition, Guid>>();
                //options.AddRepository<Material, EfCoreRepository<MaterialDbContext, Material, Guid>>();
                //options.AddRepository<MaterialType, EfCoreRepository<MaterialDbContext, MaterialType, Guid>>();
                //options.AddRepository<InventoryRltFile, EfCoreRepository<MaterialDbContext, InventoryRltFile, Guid>>();
                //options.AddRepository<Inventory, EfCoreRepository<MaterialDbContext, Inventory, Guid>>();
                //options.AddRepository<EntryRecord, EfCoreRepository<MaterialDbContext, EntryRecord, Guid>>();
                //options.AddRepository<OutRecord, EfCoreRepository<MaterialDbContext, OutRecord, Guid>>();
                //options.AddRepository<Purchase, EfCoreRepository<MaterialDbContext, Purchase, Guid>>();
                //options.AddRepository<PurchaseFlowTemplate, EfCoreRepository<MaterialDbContext, PurchaseFlowTemplate, Guid>>();
                //options.AddRepository<PurchaseFlowInfo, EfCoreRepository<MaterialDbContext, PurchaseFlowInfo, Guid>>();
                //options.AddRepository<PurchaseRltMaterial, EfCoreRepository<MaterialDbContext, PurchaseRltMaterial, Guid>>();
                //options.AddRepository<ConstructionSection, EfCoreRepository<MaterialDbContext, ConstructionSection, Guid>>();
                //options.AddRepository<ConstructionTeam, EfCoreRepository<MaterialDbContext, ConstructionTeam, Guid>>();

                //options.AddRepository<UsePlan, EfCoreRepository<MaterialDbContext, UsePlan, Guid>>();
                //options.AddRepository<UsePlanRltMaterial, EfCoreRepository<MaterialDbContext, UsePlanRltMaterial, Guid>>();
                //options.AddRepository<MaterialAcceptance, EfCoreRepository<MaterialDbContext, MaterialAcceptance, Guid>>();
                //options.AddRepository<TestMaterials, EfCoreRepository<MaterialDbContext, TestMaterials, Guid>>();
                //options.AddRepository<Contract, EfCoreRepository<MaterialDbContext, Contract, Guid>>();
                //options.AddRepository<UsePlan, EfCoreRepository<MaterialDbContext, UsePlan, Guid>>();
                //options.AddRepository<UsePlanRltMaterial, EfCoreRepository<MaterialDbContext, UsePlanRltMaterial, Guid>>();



                options.Services.AddTransient<IMaterialRepository, MaterialRepository>();
                options.Entity<Supplier>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.SupplierRltAccessories).ThenInclude(r => r.File)
                       .Include(x => x.SupplierRltContacts)
                   );

                options.Entity<MaterialOfBill>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.Section)
                        .Include(x => x.MaterialOfBillRltAccessories).ThenInclude(s => s.File)
                        .Include(x => x.MaterialOfBillRltMaterials).ThenInclude(s => s.Inventory).ThenInclude(m => m.Supplier)
                        .Include(x => x.MaterialOfBillRltMaterials).ThenInclude(s => s.Inventory).ThenInclude(m => m.Material)
                        .Include(x => x.MaterialOfBillRltMaterials).ThenInclude(s => s.Inventory).ThenInclude(m => m.Partition)
                    );

                options.Entity<Inventory>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Material).ThenInclude(y => y.Type)
                       .Include(x => x.Supplier)
                       .Include(x => x.Partition)
                   );

                options.Entity<ContractRltFile>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.File)
                   );

                options.Entity<Contract>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Files)
                       .Include(a => a.Creator)
                   );

             

                options.Entity<PurchasePlan>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x=>x.Creator)
                        .Include(x => x.WorkflowTemplate)
                       .Include(x => x.PurchasePlanRltFlows)
                       .Include(x => x.PurchasePlanRltFiles).ThenInclude(y => y.File)
                       .Include(x => x.PurchasePlanRltMaterials).ThenInclude(y => y.Material).ThenInclude(q=>q.Type)
                   );
                options.Entity<PurchaseList>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Creator)
                       .Include(x=>x.WorkflowTemplate)
                       .Include(x => x.PurchaseListRltFlows)
                       .Include(x=>x.PurchaseListRltPurchasePlan)
                       .Include(x => x.PurchaseListRltFiles).ThenInclude(y => y.File)
                       .Include(x => x.PurchaseListRltMaterials).ThenInclude(y => y.Material).ThenInclude(q => q.Type)
                   );
                options.Entity<EntryRecord>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Partition)
                       .Include(x => x.Creator)
                       .Include(x => x.EntryRecordRltFiles).ThenInclude(y => y.File)
                       .Include(x => x.EntryRecordRltMaterials).ThenInclude(y => y.Material)
                       .Include(x => x.EntryRecordRltMaterials).ThenInclude(y => y.Supplier)
                       .Include(x => x.EntryRecordRltQRCodes)
                   );
                options.Entity<OutRecord>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Partition)
                       .Include(x => x.Creator)
                       .Include(x => x.OutRecordRltFiles).ThenInclude(y => y.File)
                       .Include(x => x.OutRecordRltMaterials).ThenInclude(y => y.Inventory).ThenInclude(z => z.Material)
                       .Include(x => x.OutRecordRltMaterials).ThenInclude(y => y.Supplier)
                       .Include(x => x.OutRecordRltQRCodes)
                   );

                options.Entity<EntryRecordRltMaterial>(x => x.DefaultWithDetailsFunc = q => q
                      .Include(x => x.Inventory)
                      .Include(x => x.Material)
                      .Include(x => x.EntryRecord).ThenInclude(y=>y.Creator)
                      .Include(x => x.Supplier)
                  );
                options.Entity<OutRecordRltMaterial>(x => x.DefaultWithDetailsFunc = q => q
                      .Include(x => x.Inventory)
                      .Include(x => x.Material)
                      .Include(x => x.OutRecord).ThenInclude(y => y.Creator)
                      .Include(x => x.Supplier)
                   );

                options.Entity<MaterialAcceptance>(x => x.DefaultWithDetailsFunc = q => q
                      .Include(x => x.TestingOrganization)
                      .Include(x => x.Creator)
                      .Include(x => x.MaterialAcceptanceRltPurchases).ThenInclude(y => y.PurchaseList)
                      .Include(x => x.MaterialAcceptanceRltFiles).ThenInclude(y => y.File)
                      .Include(x => x.MaterialAcceptanceRltMaterials).ThenInclude(y => y.Material).ThenInclude(z => z.Type)
                      .Include(x => x.MaterialAcceptanceRltQRCodes)
                    );
            });
        }
    }
}