using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using SnAbp.CostManagement.Entities;
using SnAbp.File.Settings;
using SnAbp.Identity;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.CostManagement.EntityFrameworkCore
{
    public static class CostManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureCostManagement(
            this ModelBuilder builder,
            Action<CostManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new CostManagementModelBuilderConfigurationOptions(
                CostManagementDbProperties.DbTablePrefix,
                CostManagementDbProperties.DbSchema
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
            #region 当前模块
            builder.Entity<PeopleCost>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(PeopleCost), options.Schema);
                builder.ConfigureFullAudited();
            });
            builder.Entity<CostOther>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(CostOther), options.Schema);
                builder.ConfigureFullAudited();
            });
            builder.Entity<MoneyList>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(MoneyList), options.Schema);
                builder.ConfigureFullAudited();
            });
            builder.Entity<Contract>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(Contract), options.Schema);
                builder.ConfigureFullAudited();
            });
            builder.Entity<ContractRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ContractRltFile), options.Schema);
                b.ConfigureByConvention();
            });
            #endregion
            #region 依赖模块
            builder.Entity<DataDictionary>(b =>
            {
                b.ToTable(SnAbpIdentityDbProperties.DbTablePrefix + "DataDictionary", SnAbpIdentityDbProperties.DbSchema);
            });
            builder.Entity<File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(File), FileSettings.DbSchema);
            });
            #endregion
        }
    }
}