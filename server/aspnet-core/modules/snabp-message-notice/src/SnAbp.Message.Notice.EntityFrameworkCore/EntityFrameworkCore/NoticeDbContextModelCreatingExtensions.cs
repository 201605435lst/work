using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace SnAbp.Message.Notice.EntityFrameworkCore
{
    public static class NoticeDbContextModelCreatingExtensions
    {
        public static void ConfigureNotice(
            this ModelBuilder builder,
            Action<NoticeModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new NoticeModelBuilderConfigurationOptions(
                NoticeDbProperties.DbTablePrefix,
                NoticeDbProperties.DbSchema
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

            builder.Entity<Entities.Notice>(b =>
            {
                b.ToTable(options.TablePrefix + "Notice", options.Schema);
            });
        }
    }
}