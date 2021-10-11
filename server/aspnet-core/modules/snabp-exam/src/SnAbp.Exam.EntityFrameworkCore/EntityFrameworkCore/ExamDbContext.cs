using Microsoft.EntityFrameworkCore;

using SnAbp.EntityFrameworkCore;
using SnAbp.Exam.Entities;
//using SnAbp.Exam.Entities;
using Volo.Abp.Data;

namespace SnAbp.Exam.EntityFrameworkCore
{
    [ConnectionStringName(ExamDbProperties.ConnectionStringName)]
    public class ExamDbContext : AbpDbContext<ExamDbContext>, IExamDbContext
    {
        public DbSet<AnswerOption> AnswerOption { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ExamPaper> ExamPaper { get; set; }
        public DbSet<ExamPaperRltQuestion> ExamPaperRltQuestion { get; set; }
        public DbSet<ExamPaperTemplate> ExamPaperTemplate { get; set; }
        public DbSet<ExamPaperTemplateConfig> ExamPaperTemplateConfig { get; set; }
        public DbSet<KnowledgePoint> KnowledgePoint { get; set; }
        public DbSet<KnowledgePointRltCategory> KnowledgePointRltCategory { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<QuestionRltKnowledgePoint> QuestionRltKnowledgePoint { get; set; }
        public DbSet<QuestionRltCategory> QuestionRltCategory { get; set; }


        public ExamDbContext(DbContextOptions<ExamDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureExam();
        }
    }
}