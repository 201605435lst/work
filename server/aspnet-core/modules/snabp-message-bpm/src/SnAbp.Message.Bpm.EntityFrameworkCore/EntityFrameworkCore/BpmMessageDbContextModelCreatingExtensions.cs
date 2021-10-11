using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Message.Bpm.Entities;
using Volo.Abp;

namespace SnAbp.Message.Bpm.EntityFrameworkCore
{
    public static class BpmMessageDbContextModelCreatingExtensions
    {
        public static void ConfigureMessageBpm(
            this ModelBuilder builder,
            Action<BpmMessageModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BpmMessageModelBuilderConfigurationOptions(
                BpmDbProperties.DbTablePrefix,
                BpmDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<BpmRltMessage>(b=>
            {
                b.ToTable(options.TablePrefix + "BpmRltMessage", options.Schema);
            });

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
        }
    }
}