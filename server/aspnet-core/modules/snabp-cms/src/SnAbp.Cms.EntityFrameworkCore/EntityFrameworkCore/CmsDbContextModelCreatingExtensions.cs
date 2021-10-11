using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Cms.Entities;
using SnAbp.File.Settings;
using Volo.Abp;

namespace SnAbp.Cms.EntityFrameworkCore
{
    public static class CmsDbContextModelCreatingExtensions
    {
        public static void ConfigureCms(
            this ModelBuilder builder,
            Action<CmsModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new CmsModelBuilderConfigurationOptions(
                CmsDbProperties.DbTablePrefix,
                CmsDbProperties.DbSchema
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



            builder.Entity<File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(File), FileSettings.DbSchema);
            });

            builder.Entity<Category>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Category), options.Schema);
            });
            builder.Entity<Article>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Article), options.Schema);
            });
            builder.Entity<ArticleAccessory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ArticleAccessory), options.Schema);
            });
            builder.Entity<ArticleCarousel>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ArticleCarousel), options.Schema);
            });
            builder.Entity<CategoryRltArticle>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(CategoryRltArticle), options.Schema);
            });
        }
    }
}
