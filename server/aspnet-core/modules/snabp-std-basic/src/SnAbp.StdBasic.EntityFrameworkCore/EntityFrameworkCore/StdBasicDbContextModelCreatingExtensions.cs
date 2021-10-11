using System;

using Microsoft.EntityFrameworkCore;
using SnAbp.Common.Entities;
using SnAbp.Common.Settings;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Identity.Settings;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Settings;

using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.StdBasic.EntityFrameworkCore
{
    public static class StdBasicDbContextModelCreatingExtensions
    {
        public static void ConfigureStdBasic(
            this ModelBuilder builder,
            Action<StdBasicModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new StdBasicModelBuilderConfigurationOptions(
                StdBasicDbProperties.DbTablePrefix,
                StdBasicDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<SnAbp.File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(File), FileSettings.DbSchema);
            });
            // Category
            builder.Entity<ComponentCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ComponentCategory), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<ComponentCategoryRltProductCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ComponentCategoryRltProductCategory), options.Schema);
            });
            builder.Entity<ComponentCategoryRltMVDProperty>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ComponentCategoryRltMVDProperty), options.Schema);
            });
            builder.Entity<ProductCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ProductCategory), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<ProductCategoryRltMVDProperty>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ProductCategoryRltMVDProperty), options.Schema);
            });

            // Manufacture
            builder.Entity<Manufacturer>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Manufacturer), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<EquipmentControlType>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EquipmentControlType), options.Schema);
                b.ConfigureFullAudited();
            });

            // Model
            // Model Block
            builder.Entity<Block>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Block), options.Schema);
            });
            builder.Entity<BlockCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(BlockCategory), options.Schema);
            });
            builder.Entity<ModelRltBlock>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ModelRltBlock), options.Schema);
            });

            // Model ModelMVD
            builder.Entity<ModelRltMVDProperty>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ModelRltMVDProperty), options.Schema);
            });
            builder.Entity<MVDProperty>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(MVDProperty), options.Schema);
            });
            builder.Entity<MVDCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(MVDCategory), options.Schema);
            });

            builder.Entity<RevitConnector>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(RevitConnector), options.Schema);
            });
            builder.Entity<Model>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Model), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<ModelFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ModelFile), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<ModelRltModel>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ModelRltModel), options.Schema);
            });
            builder.Entity<ModelTerminal>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ModelTerminal), options.Schema);
            });

            // Repair
            builder.Entity<RepairGroup>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(RepairGroup), options.Schema);
            });
            builder.Entity<RepairItem>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(RepairItem), options.Schema);
            });
            builder.Entity<RepairTestItem>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(RepairTestItem), options.Schema);
            });
            builder.Entity<RepairItemRltComponentCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(RepairItemRltComponentCategory), options.Schema);
            });
            builder.Entity<RepairItemRltOrganizationType>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(RepairItemRltOrganizationType), options.Schema);
            });
            builder.Entity<SnAbp.Identity.DataDictionary>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(SnAbp.Identity.DataDictionary), SystemSettings.DbSchema);
            });

            //InfluenceRange
            builder.Entity<InfluenceRange>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(InfluenceRange), options.Schema);
            });
            //WorkAttention
            builder.Entity<WorkAttention>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkAttention), StdBasicSettings.DbSchema);
            });
            // quota
            builder.Entity<BasePrice>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(BasePrice), StdBasicSettings.DbSchema);
            });

            // Area
            builder.Entity<Area>(b =>
            {
                b.ToTable(CommonSettings.DbTablePrefix + nameof(Area), CommonSettings.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<ComponentCategoryRltMaterial>(b => b.ToTable(options.TablePrefix + nameof(ComponentCategoryRltMaterial), StdBasicSettings.DbSchema));
            builder.Entity<ComponentCategoryRltQuota>(b => b.ToTable(options.TablePrefix + nameof(ComponentCategoryRltQuota), StdBasicSettings.DbSchema));
            builder.Entity<ComputerCode>(b => b.ToTable(options.TablePrefix + nameof(ComputerCode), StdBasicSettings.DbSchema));
            builder.Entity<ProductCategoryRltMaterial>(b => b.ToTable(options.TablePrefix + nameof(ProductCategoryRltMaterial), StdBasicSettings.DbSchema));
            builder.Entity<ProductCategoryRltQuota>(b => b.ToTable(options.TablePrefix + nameof(ProductCategoryRltQuota), StdBasicSettings.DbSchema));
            builder.Entity<Quota>(b => b.ToTable(options.TablePrefix + nameof(Quota), StdBasicSettings.DbSchema));
            builder.Entity<QuotaCategory>(b => b.ToTable(options.TablePrefix + nameof(QuotaCategory), StdBasicSettings.DbSchema));
            builder.Entity<QuotaItem>(b => b.ToTable(options.TablePrefix + nameof(QuotaItem), StdBasicSettings.DbSchema));
            builder.Entity<IndividualProject>(b => b.ToTable(options.TablePrefix + nameof(IndividualProject), StdBasicSettings.DbSchema));
            builder.Entity<ProcessTemplate>(b => b.ToTable(options.TablePrefix + nameof(ProcessTemplate), StdBasicSettings.DbSchema));
            builder.Entity<ProjectItem>(b => b.ToTable(options.TablePrefix + nameof(ProjectItem), StdBasicSettings.DbSchema));
            builder.Entity<ProjectItemRltComponentCategory>(b => b.ToTable(options.TablePrefix + nameof(ProjectItemRltComponentCategory), StdBasicSettings.DbSchema));
            builder.Entity<ProjectItemRltIndividualProject>(b => b.ToTable(options.TablePrefix + nameof(ProjectItemRltIndividualProject), StdBasicSettings.DbSchema));
            builder.Entity<ProjectItemRltProcessTemplate>(b => b.ToTable(options.TablePrefix + nameof(ProjectItemRltProcessTemplate), StdBasicSettings.DbSchema));
            builder.Entity<ProjectItemRltProductCategory>(b => b.ToTable(options.TablePrefix + nameof(ProjectItemRltProductCategory), StdBasicSettings.DbSchema));
        }
    }
}