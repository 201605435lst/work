using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore.Modeling;
using SnAbp.FileApprove.Entities;
using Volo.Abp;

namespace SnAbp.FileApprove.EntityFrameworkCore
{
    public static class FileApproveDbContextModelCreatingExtensions
    {
        public static void ConfigureFileApprove(
            this ModelBuilder builder,
            Action<FileApproveModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new FileApproveModelBuilderConfigurationOptions(
                FileApproveDbProperties.DbTablePrefix,
                FileApproveDbProperties.DbSchema
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
            builder.Entity<FileApprove>(b => b.ToTable(options.TablePrefix + nameof(FileApprove), options.Schema).ConfigureByConvention());
            builder.Entity<FileApproveRltFlow>(b => b.ToTable(options.TablePrefix + nameof(FileApproveRltFlow), options.Schema).ConfigureByConvention());

        }
    }
}