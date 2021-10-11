using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Settings;
using SnAbp.Technology.Entities;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Technology.EntityFrameworkCore
{
    public static class TechnologyDbContextModelCreatingExtensions
    {
        public static void ConfigureTechnology(
            this ModelBuilder builder,
            Action<TechnologyModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new TechnologyModelBuilderConfigurationOptions(
                TechnologyDbProperties.DbTablePrefix,
                TechnologyDbProperties.DbSchema
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
            builder.Entity<ConstructInterface>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ConstructInterface), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<ConstructInterfaceInfo>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ConstructInterfaceInfo), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<ConstructInterfaceInfoRltMarkFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ConstructInterfaceInfoRltMarkFile), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<Disclose>(b =>
            {
                b.ToTable(options.TablePrefix + "Disclose", options.Schema);
            });
            builder.Entity<Material>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Material), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<MaterialPlan>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(MaterialPlan), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<MaterialPlanFlowInfo>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(MaterialPlanFlowInfo), options.Schema);
                b.ConfigureByConvention();
            });  
            builder.Entity<MaterialPlanRltMaterial>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(MaterialPlanRltMaterial), options.Schema);
                b.ConfigureByConvention();
            });
            #endregion

        }
    }
}