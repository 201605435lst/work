using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Basic.Entities;
using SnAbp.Identity;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Basic.EntityFrameworkCore
{
    public static class BasicDbContextModelCreatingExtensions
    {
        public static void ConfigureBasic(
            this ModelBuilder builder,
            Action<BasicModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BasicModelBuilderConfigurationOptions(
                BasicDbProperties.DbTablePrefix,
                BasicDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
                b.ConfigureFullAuditedAggregateRoot();
            
                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */


            builder.Entity<Organization>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(Organization), options.Schema);
            });

            builder.Entity<DataDictionary>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(DataDictionary), options.Schema);
            });

            builder.Entity<Railway>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Railway), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<StationRltRailway>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(StationRltRailway), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<Station>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Station), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<StationRltOrganization>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(StationRltOrganization), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<InstallationSite>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(InstallationSite), options.Schema);
                b.HasIndex(x => x.CodeName).IsUnique();
                b.ConfigureFullAudited();
            });
            builder.Entity<RailwayRltOrganization>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(RailwayRltOrganization), options.Schema);
                //b.HasOne(x => x.Railway).WithOne().OnDelete(DeleteBehavior.SetNull);
                //b.HasOne(x => x.Organization).WithOne().OnDelete(DeleteBehavior.SetNull);
                //b.ConfigureByConvention();
            });
        }
    }
}