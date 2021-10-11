using Microsoft.EntityFrameworkCore;
using SnAbp.StdBasic.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.StdBasic.EntityFrameworkCore
{
    [ConnectionStringName(StdBasicDbProperties.ConnectionStringName)]
    public interface IStdBasicDbContext : IEfCoreDbContext
    {
        // Category
        DbSet<ComponentCategory> ComponentCategory { get; set; }
        DbSet<ComponentCategoryRltMVDProperty> ComponentCategoryRltMVDProperty { get; set; }
        DbSet<ComponentCategoryRltProductCategory> ComponentCategoryRltProductCategory { get; set; }
        DbSet<ProductCategory> ProductCategory { get; set; }
        DbSet<ProductCategoryRltMVDProperty> ProductCategoryRltMVDProperty { get; set; }


        // Manufacturer
        DbSet<Manufacturer> Manufacturer { get; set; }
        DbSet<EquipmentControlType> EquipmentControlType { get; set; }


        // Model
        DbSet<Block> Block { get; set; }
        DbSet<BlockCategory> BlockCategory { get; set; }
        DbSet<ModelRltBlock> ModelRltBlock { get; set; }

        DbSet<ModelRltMVDProperty> ModelRltMVDProperty { get; set; }
        DbSet<MVDCategory> MVDCategory { get; set; }
        DbSet<MVDProperty> MVDProperty { get; set; }

        DbSet<RevitConnector> RevitConnector { get; set; }
        DbSet<Model> Model { get; set; }
        DbSet<ModelFile> ModelFile { get; set; }
        DbSet<ModelRltModel> ModelRltModel { get; set; }
        DbSet<ModelTerminal> ModelTerminal { get; set; }

        // Repair
        DbSet<RepairGroup> RepairGroup { get; set; }
        DbSet<RepairItem> RepairItem { get; set; }
        DbSet<RepairItemRltComponentCategory> RepairItemRltComponentCategory { get; set; }
        DbSet<RepairItemRltOrganizationType> RepairItemRltOrganizationType { get; set; }
        DbSet<RepairTestItem> RepairTestItem { get; set; }

        //InfluenceRange
        DbSet<InfluenceRange> InfluenceRange { get; set; }
        //WorkAttention
        DbSet<WorkAttention> WorkAttention { get; set; }


        // quota
        DbSet<BasePrice> BasePrice { get; set; }
        DbSet<ComponentCategoryRltMaterial> ComponentCategoryRltMaterial { get; set; }
        DbSet<ComponentCategoryRltQuota> ComponentCategoryRltQuota { get; set; }
        DbSet<ComputerCode> ComputerCode { get; set; }
        DbSet<ProductCategoryRltMaterial> ProductCategoryRltMaterial { get; set; }
        DbSet<ProductCategoryRltQuota> ProductCategoryRltQuota { get; set; }
        DbSet<Quota> Quota { get; set; }
        DbSet<QuotaCategory> QuotaCategory { get; set; }
        DbSet<QuotaItem> QuotaItem { get; set; }

        // WBS/EBS
        DbSet<IndividualProject> IndividualProject { get; set; }
        DbSet<ProcessTemplate> ProcessTemplate { get; set; }
        DbSet<ProjectItem> ProjectItem { get; set; }
        DbSet<ProjectItemRltComponentCategory> ProjectItemRltComponentCategory { get; set; }
        DbSet<ProjectItemRltIndividualProject> ProjectItemRltIndividualProject { get; set; }
        DbSet<ProjectItemRltProcessTemplate> ProjectItemRltProcessTemplate { get; set; }
        DbSet<ProjectItemRltProductCategory> ProjectItemRltProductCategory { get; set; }

    }
}