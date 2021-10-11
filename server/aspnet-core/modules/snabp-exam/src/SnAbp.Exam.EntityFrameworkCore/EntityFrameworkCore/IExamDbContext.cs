using Microsoft.EntityFrameworkCore;

using SnAbp.EntityFrameworkCore;
using SnAbp.Exam.Entities;
using Volo.Abp.Data;

namespace SnAbp.Exam.EntityFrameworkCore
{
    [ConnectionStringName(ExamDbProperties.ConnectionStringName)]
    public interface IExamDbContext : IEfCoreDbContext
    {
        DbSet<AnswerOption> AnswerOption { get; set; }
        DbSet<ExamPaper> ExamPaper { get; set; }
        DbSet<ExamPaperRltQuestion> ExamPaperRltQuestion { get; set; }
        DbSet<Category> Category { get; set; }
        DbSet<ExamPaperTemplate> ExamPaperTemplate { get; set; }
        DbSet<ExamPaperTemplateConfig> ExamPaperTemplateConfig { get; set; }
        DbSet<KnowledgePoint> KnowledgePoint { get; set; }
        DbSet<KnowledgePointRltCategory> KnowledgePointRltCategory { get; set; }
        DbSet<Question> Question { get; set; }
        DbSet<QuestionRltKnowledgePoint> QuestionRltKnowledgePoint { get; set; }
        DbSet<QuestionRltCategory> QuestionRltCategory { get; set; }
    }

    
}