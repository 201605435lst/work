using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Identity;
using SnAbp.Regulation.Entities;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Regulation.EntityFrameworkCore
{
    public static class RegulationDbContextModelCreatingExtensions
    {
        public static void ConfigureRegulation(
            this ModelBuilder builder,
            Action<RegulationModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new RegulationModelBuilderConfigurationOptions(
                RegulationDbProperties.DbTablePrefix,
                RegulationDbProperties.DbSchema
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
            builder.Entity<Institution>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Institution), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<Label>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Label), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<InstitutionRltAuthority>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(InstitutionRltAuthority), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<InstitutionRltEdition>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(InstitutionRltEdition), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<InstitutionRltLabel>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(InstitutionRltLabel), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<InstitutionRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(InstitutionRltFile), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<InstitutionRltFlow>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(InstitutionRltFlow), options.Schema);
                b.ConfigureByConvention();
            });
            #endregion

            #region  依赖模块
            builder.Entity<Organization>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(Organization), SystemSettings.DbSchema);
            });
            #endregion
        }
    }
}