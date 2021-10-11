using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SnAbp.Exam.EntityFrameworkCore
{
    public class ExamHttpApiHostMigrationsDbContext : AbpDbContext<ExamHttpApiHostMigrationsDbContext>
    {
        public ExamHttpApiHostMigrationsDbContext(DbContextOptions<ExamHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureExam();
        }
    }
}
