using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SnAbp.Bpm.Entities;
//using SnAbp.Bpm.Entities;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Bpm.EntityFrameworkCore
{
    public static class BpmDbContextModelCreatingExtensions
    {
        public static void ConfigureBpm(
            this ModelBuilder builder,
            Action<BpmModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BpmModelBuilderConfigurationOptions(
                BpmDbProperties.DbTablePrefix,
                BpmDbProperties.DbSchema
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

            builder.Entity<WorkflowTemplate>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkflowTemplate), options.Schema);
                b.HasIndex(x => x.Key).IsUnique();
                b.ConfigureFullAudited();
            });

            builder.Entity<WorkflowTemplateRltMember>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkflowTemplateRltMember), options.Schema);
            });

            builder.Entity<FormTemplate>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FormTemplate), options.Schema);

                b.Property(x => x.FormItems).HasColumnType("jsonb").HasConversion(
                     formItems => JsonConvert.SerializeObject(
                         formItems,
                         new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                     formItems => JsonConvert.DeserializeObject<string>(
                         formItems,
                         new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                     );
                b.Property(x => x.Config).HasColumnType("jsonb").HasConversion(
                   config => JsonConvert.SerializeObject(
                       config,
                       new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                   config => JsonConvert.DeserializeObject<string>(
                       config,
                       new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                   );
                b.ConfigureAudited();
            });

            builder.Entity<FlowTemplate>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FlowTemplate), options.Schema);
                b.ConfigureAudited();
            });

            builder.Entity<FlowTemplateNode>(b =>
            {
               
                b.ToTable(options.TablePrefix + nameof(FlowTemplateNode), options.Schema);
                b.Property(x => x.FormItemPermisstions).HasColumnType("jsonb").HasConversion(
               permisstions => JsonConvert.SerializeObject(
                   permisstions,
                   new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
               permisstions => JsonConvert.DeserializeObject<List<FlowNodeFormItemPermisstion>>(
                   permisstions,
                   new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
               );
                
            });

            builder.Entity<FlowTemplateNodeRltMember>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FlowTemplateNodeRltMember), options.Schema);

            });

            builder.Entity<FlowTemplateStep>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FlowTemplateStep), options.Schema);

            });

            builder.Entity<WorkflowData>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkflowData), options.Schema);
                b.Property(x => x.FormValues).HasColumnType("jsonb").HasConversion(
                    formValues => JsonConvert.SerializeObject(
                        formValues,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    formValues => JsonConvert.DeserializeObject<string>(
                        formValues,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                    );
                b.ConfigureAudited();
            });

            builder.Entity<Workflow>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Workflow), options.Schema);
                b.ConfigureAudited();
            });

            builder.Entity<WorkflowStateRltMember>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkflowStateRltMember), options.Schema);
                b.ConfigureAudited();
            });
            builder.Entity<WorkflowTemplateGroup>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(WorkflowTemplateGroup), options.Schema);
            });
        }
    }
}