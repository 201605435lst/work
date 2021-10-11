using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Exam.Entities;
//using SnAbp.Exam.Entities;
using Volo.Abp;

namespace SnAbp.Exam.EntityFrameworkCore
{
    public static class ExamDbContextModelCreatingExtensions
    {
        public static void ConfigureExam(
            this ModelBuilder builder,
            Action<ExamModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ExamModelBuilderConfigurationOptions(
                ExamDbProperties.DbTablePrefix,
                ExamDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);


            builder.Entity<AnswerOption>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(AnswerOption), options.Schema);
            });

            builder.Entity<Category>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Category), options.Schema);
            });

            builder.Entity<ExamPaper>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ExamPaper), options.Schema);
            });

            builder.Entity<ExamPaperRltQuestion>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ExamPaperRltQuestion), options.Schema);
            });

            builder.Entity<ExamPaperTemplate>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ExamPaperTemplate), options.Schema);
            });

            builder.Entity<ExamPaperTemplateConfig>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ExamPaperTemplateConfig), options.Schema);
            });

            builder.Entity<KnowledgePoint>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(KnowledgePoint), options.Schema);
            });

            builder.Entity<KnowledgePointRltCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(KnowledgePointRltCategory), options.Schema);
            });

            builder.Entity<Question>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Question), options.Schema);
            });

            builder.Entity<QuestionRltKnowledgePoint>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(QuestionRltKnowledgePoint), options.Schema);
            });
            builder.Entity<QuestionRltCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(QuestionRltCategory), options.Schema);
            });


        }
    }
}