using Microsoft.EntityFrameworkCore;
using SnAbp.StdBasic.Entities;
//using SnAbp.StdBasic.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.StdBasic.EntityFrameworkCore
{
    [ConnectionStringName(StdBasicDbProperties.ConnectionStringName)]
    public class StdBasicDbContext : AbpDbContext<StdBasicDbContext>, IStdBasicDbContext
    {
        // Category
        public DbSet<ComponentCategory> ComponentCategory { get; set; }
        public DbSet<ComponentCategoryRltProductCategory> ComponentCategoryRltProductCategory { get; set; }
        public DbSet<ComponentCategoryRltMVDProperty> ComponentCategoryRltMVDProperty { get; set; }

        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductCategoryRltMVDProperty> ProductCategoryRltMVDProperty { get; set; }



        // Manufacturer
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<EquipmentControlType> EquipmentControlType { get; set; }


        // Model
        public DbSet<Block> Block { get; set; }
        public DbSet<BlockCategory> BlockCategory { get; set; }
        public DbSet<ModelRltBlock> ModelRltBlock { get; set; }

        public DbSet<ModelRltMVDProperty> ModelRltMVDProperty { get; set; }
        public DbSet<MVDCategory> MVDCategory { get; set; }
        public DbSet<MVDProperty> MVDProperty { get; set; }

        public DbSet<RevitConnector> RevitConnector { get; set; }
        public DbSet<Model> Model { get; set; }
        public DbSet<ModelFile> ModelFile { get; set; }
        public DbSet<ModelRltModel> ModelRltModel { get; set; }
        public DbSet<ModelTerminal> ModelTerminal { get; set; }


        // Repair
        public DbSet<RepairGroup> RepairGroup { get; set; }
        public DbSet<RepairItem> RepairItem { get; set; }
        public DbSet<RepairItemRltComponentCategory> RepairItemRltComponentCategory { get; set; }
        public DbSet<RepairItemRltOrganizationType> RepairItemRltOrganizationType { get; set; }
        public DbSet<RepairTestItem> RepairTestItem { get; set; }

        //InfluenceRange
        public DbSet<InfluenceRange> InfluenceRange { get; set; }
        //WorkAttention
        public DbSet<WorkAttention> WorkAttention { get; set; }


        public DbSet<BasePrice> BasePrice { get; set; }
        public DbSet<ComponentCategoryRltMaterial> ComponentCategoryRltMaterial { get; set; }
        public DbSet<ComponentCategoryRltQuota> ComponentCategoryRltQuota { get; set; }
        public DbSet<ComputerCode> ComputerCode { get; set; }
        public DbSet<ProductCategoryRltMaterial> ProductCategoryRltMaterial { get; set; }
        public DbSet<ProductCategoryRltQuota> ProductCategoryRltQuota { get; set; }
        public DbSet<Quota> Quota { get; set; }
        public DbSet<QuotaCategory> QuotaCategory { get; set; }
        public DbSet<QuotaItem> QuotaItem { get; set; }
        public DbSet<IndividualProject> IndividualProject { get; set; }
        public DbSet<ProcessTemplate> ProcessTemplate { get; set; }
        public DbSet<ProjectItem> ProjectItem { get; set; }
        public DbSet<ProjectItemRltComponentCategory> ProjectItemRltComponentCategory { get; set; }
        public DbSet<ProjectItemRltIndividualProject> ProjectItemRltIndividualProject { get; set; }
        public DbSet<ProjectItemRltProcessTemplate> ProjectItemRltProcessTemplate { get; set; }
        public DbSet<ProjectItemRltProductCategory> ProjectItemRltProductCategory { get; set; }

        public StdBasicDbContext(DbContextOptions<StdBasicDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureStdBasic();
        }
    }
}