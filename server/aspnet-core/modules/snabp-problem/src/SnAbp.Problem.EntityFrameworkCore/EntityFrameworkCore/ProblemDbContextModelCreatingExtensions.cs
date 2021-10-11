using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Problem.Entities;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Problem.EntityFrameworkCore
{
    public static class ProblemDbContextModelCreatingExtensions
    {
        public static void ConfigureProblem(
            this ModelBuilder builder,
            Action<ProblemModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ProblemModelBuilderConfigurationOptions(
                ProblemDbProperties.DbTablePrefix,
                ProblemDbProperties.DbSchema
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

            builder.Entity<Entities.Problem>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Problem), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<ProblemCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ProblemCategory), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<ProblemRltProblemCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ProblemRltProblemCategory), options.Schema);
            });
        }
    }
}